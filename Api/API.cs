using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using BankingProductsData.Models;
using File = System.IO.File;
using BankingProductsData;
using Azure.Storage.Blobs;
using System.Text.RegularExpressions;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage;

namespace BankingProductsApi
{
    public static class Api
    {
        // Keys
        private const string ConnectionString = "HIDDEN";
        private const string GeminiApiKey = "HIDDEN";

        private const string BanksDataFileNameAdmin = "BankData.json";
        private const string BanksDataFileNameWebsiteTermDeposits = "TERM_DEPOSITS.json";
        private const string PageSize = "200";
        private const string BlobContainerName = "bank-products";
        private const string _all = "ALL";

        private enum StorageType
        {
            LocalFile,
            AzureBlobStorage
        }

        [FunctionName("AddUser")]
        public static async Task<IActionResult> RunAddUser([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req, ILogger log)
        {
            try
            {
                var userJson = new StreamReader(req.Body).ReadToEnd();
                var user = JsonConvert.DeserializeObject<User>(userJson);

                await AddUserAsync(user);

                return new OkObjectResult(true);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message == "Conflict" ? "A user with this email address already exists" : ex.Message);
            }
        }

        [FunctionName("GetUsers")]
        public static async Task<IActionResult> RunGetUsers([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req, ILogger log)
        {
            var users = await GetUsersAsync();

            return new OkObjectResult(users);
        }

        private static async Task<List<User>> GetUsersAsync()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("User");

            TableQuery<User> query = new TableQuery<User>().Where(TableQuery.GenerateFilterConditionForBool("Active", QueryComparisons.Equal, true));

            var users = new List<User>();

            var result = await table.ExecuteQuerySegmentedAsync(query, null);

            foreach (User user in result)
            {
                users.Add(new User(firstName: user.FirstName, lastName: user.LastName, password: user.Password, email: user.Email, mobile: user.Mobile, active: user.Active, joinedOn: user.JoinedOn));
            }

            return users;
        }

        private static async Task AddUserAsync(User user)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("User");
            await table.CreateIfNotExistsAsync();
            TableOperation insertOperation = TableOperation.Insert(user);

            await table.ExecuteAsync(insertOperation);
        }

        private static async Task<List<User>> UsersGetAllAsync()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("User");

            TableQuery<User> query = new TableQuery<User>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "greg@somewhere.com"));

            var users = new List<User>();

            foreach (User user in await table.ExecuteQuerySegmentedAsync(query, null))
            {
                users.Add(new User(firstName: user.FirstName, lastName: user.LastName, password: user.Password, email: user.Email, mobile: user.Mobile, active: user.Active, joinedOn: user.JoinedOn));
            }

            return users;
        }

        [FunctionName("GetCandidateBanks")]
        public static async Task<IActionResult> RunGetCandidateBanks([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req, ILogger log)
        {
            var candidateBanks = GetCandidateBanks();

            var includeLastUpdated = bool.Parse(req.Query["IncludeLastupdated"]);

            if (!includeLastUpdated)
                return new OkObjectResult(candidateBanks);

            var allBankData = await ReadAllBankData(null);

            foreach (var candidateBank in candidateBanks)
            {
                var matchedBank = allBankData.data.FirstOrDefault(bank => bank.Name == candidateBank.Name);
                if (matchedBank != null)
                {
                    candidateBank.LastRefreshed = matchedBank.LastRefreshed;
                    candidateBank.LastRefreshedFormatted = matchedBank.LastRefreshed.ToString("dd/MM/yyyy h:mm tt");

                    DateTime? maximumLastUpdatedDate = null;

                    foreach (var product in matchedBank.Products)
                    {
                        if (product.listData.ProductCategory != BankingProductCategory.TERMDEPOSITS)
                            continue;

                        if (string.IsNullOrEmpty(product.listData.LastUpdated) || product.listData.LastUpdated.Length < 19)
                            continue;

                        DateTime? dateTimeUtc = null;
                        var upToSeconds = product.listData.LastUpdated.Substring(0, 19);    // Strip off the milliseconds

                        try
                        {
                            dateTimeUtc = DateTime.ParseExact(upToSeconds, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
                        }
                        catch (Exception ex)
                        {
                            Debug.Print($"{matchedBank.Name}: {product.listData.Name}: {product.listData.LastUpdated}: {ex.Message}");

                            continue;
                        }
                        if (maximumLastUpdatedDate == null || dateTimeUtc > maximumLastUpdatedDate)
                            maximumLastUpdatedDate = dateTimeUtc;
                    }

                    candidateBank.LastBankUpdate = maximumLastUpdatedDate;
                    if (maximumLastUpdatedDate != null)
                        candidateBank.LastBankUpdateFormatted = maximumLastUpdatedDate?.ToString("dd/MM/yyyy h:mm tt");
                }
            }

            return new OkObjectResult(candidateBanks);
        }

        [FunctionName("GetProducts")]
        public static async Task<IActionResult> RunGetProducts([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req, ILogger log)
        {
            var allBankData = await ReadAllBankData(req.Query["CacheDirectory"]);

            allBankData.data = allBankData.data.OrderBy(bank => bank.Name).ToList();

            var filterByProductCategory = req.Query["ProductCategory"].ToString();

            if (!string.IsNullOrEmpty(filterByProductCategory))
            {
                var filteredBankData = new BankData { data = new List<Bank>() };

                foreach (var bank in allBankData.data)
                {
                    var filteredBankProducts = new List<Product>();

                    foreach (var product in bank.Products)
                    {
                        if (MatchProductCategory(filterByProductCategory, product.listData.ProductCategory))
                        {
                            filteredBankProducts.Add(product);
                        }
                    }

                    bank.Products.Clear();

                    bank.Products.AddRange(filteredBankProducts);

                    filteredBankData.data.Add(bank);
                }

                return new OkObjectResult(filteredBankData);
            }

            return new OkObjectResult(allBankData);
        }

        [FunctionName("DeleteCache")]
        public static async Task<IActionResult> RunDeleteCache([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req, ILogger log)
        {
            var cacheDirectory = req.Query["CacheDirectory"];

            if (Directory.Exists(cacheDirectory))
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(cacheDirectory);

                foreach (FileInfo file in directoryInfo.GetFiles())
                {
                    if (file.Extension == ".json")
                        file.Delete();
                }
            }

            await DeleteBlobStorageDataAsync(BanksDataFileNameAdmin);
            await DeleteBlobStorageDataAsync(BanksDataFileNameWebsiteTermDeposits);

            return new OkObjectResult(true);
        }

        private static async Task<BankData> ReadAllBankData(string cacheDirectory)
        {
            var allBankData = new BankData { data = new List<Bank>() };
            var json = string.Empty;
            var localFilePath = $"{cacheDirectory}\\{BanksDataFileNameAdmin}";

            if (File.Exists(localFilePath))
            {
                json = await File.ReadAllTextAsync(localFilePath);
            }
            else
            {
                BlobServiceClient blobServiceClient = new(ConnectionString);
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(BlobContainerName);
                BlobClient blobClient = containerClient.GetBlobClient(BanksDataFileNameWebsiteTermDeposits);

                if (await blobClient.ExistsAsync())
                {
                    var response = await blobClient.DownloadAsync();
                    using var streamReader = new StreamReader(response.Value.Content);

                    while (!streamReader.EndOfStream)
                    {
                        json = await streamReader.ReadLineAsync();
                    }
                }
            }

            if (!string.IsNullOrEmpty(json))
                allBankData = JsonConvert.DeserializeObject<BankData>(json);

            return allBankData;
        }

        private static async Task DeleteBlobStorageDataAsync(string fileName)
        {
            BlobServiceClient blobServiceClient = new(ConnectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(BlobContainerName);
            await containerClient.GetBlobClient(fileName).DeleteIfExistsAsync();
        }

        private static async Task SaveBlobStorageDataAsync(string fileName, string blobContents)
        {
            BlobServiceClient blobServiceClient = new(ConnectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(BlobContainerName);
            BlobClient blobClient = containerClient.GetBlobClient(fileName);

            await blobClient.UploadAsync(BinaryData.FromString(blobContents), overwrite: true);
        }

        [FunctionName("FilterTermDeposits")]
        public static async Task<IActionResult> RunFilterTermDeposits([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req, ILogger log)
        {
            var allBankData = await ReadAllBankData(req.Query["CacheDirectory"]);
            var showAll = req.Query["ShowAll"] == "True";
            var showRemaining = req.Query["ShowRemaining"] == "True";
            var filterJson = new StreamReader(req.Body).ReadToEnd();
            var filter = JsonConvert.DeserializeObject<FilterTermDeposits>(filterJson);
            var countTotal = 0;
            var countFilteredTotal = 0;
            var countFiltered = 0;

            if (allBankData.data == null || allBankData.data.Count == 0)
                return new NotFoundResult();

            if (allBankData is not { data.Count: > 0 })
                return new OkObjectResult(allBankData);

            var bankFilteredDataTermDeposits = new BankDataFilteredTermDeposits { TermDeposits = new List<TermDeposit>(), TermPeriodsSelected = filter.Terms.Count, Debug = new List<string>() };

            foreach (var bank in allBankData.data)
            {
                if (filter.BankName != null && bank.Name != filter.BankName && bank.NameDisplay != filter.BankName)
                    continue;

                foreach (var product in bank.Products)
                {
                    if (bank.Name == "Bank of Sydney" && product.listData.Name == "Online Term Deposit")
                        Debug.Print("here");

                    bool found;

                    var matchedDepositRates = new List<BankingProductDepositRate>();
                    var productCategoryMatch = MatchProductCategory(filter.ProductCategory.ToString(CultureInfo.InvariantCulture), product.listData.ProductCategory);
                    var depositAmountMatch = MatchDepositAmount(filter.DepositAmount, product.detailData.Constraints);
                    var termsMatch = MatchTerms(filter.Terms, product.detailData.DepositRates, ref matchedDepositRates);

                    //if (product.ID == "544ca08a-57ec-45a3-85ef-0a398646795a") // ING constraint text rather than number
                    //    Debug.Print("here");

                    //if (product.ID == "TD001MBLTDA001") // Macquarie 60M
                    //    Debug.Print("here");

                    //if (product.ID == "4740c27d-eb72-375d-2d25-ad256ed37b1e") // ANZ Business Notice Term Deposit
                    //    Debug.Print("here");  //TODO this will have ?? below

                    found = productCategoryMatch && depositAmountMatch && termsMatch;

                    countTotal++;

                    if (found || showAll)
                    {
                        countFilteredTotal++;

                        var termRates = new List<TermRate>();
                        decimal highestRate = 0;
                        var highestTerm = "";
                        var minimumDeposit = "";
                        var maximumDeposit = "";
                        var minToMaxDeposit = "";

                        if (product.listData.Name == "JPY Term Deposit")
                            Debug.Print("here");

                        if (product?.detailData?.Constraints != null)
                        {
                            foreach (var constraint in product.detailData.Constraints)
                            {
                                if (constraint.ConstraintType == BankingProductConstraint.ConstraintTypeEnum.MINBALANCE)
                                    minimumDeposit = StringToAmount(constraint.AdditionalValue);
                                else if (constraint.ConstraintType == BankingProductConstraint.ConstraintTypeEnum.MAXBALANCE)
                                    maximumDeposit = StringToAmount(constraint.AdditionalValue);
                            }

                            if (minimumDeposit == "")
                                Debug.Print("No min deposit");
                        }

                        if (!string.IsNullOrEmpty(minimumDeposit))
                        {
                            minToMaxDeposit = $"${minimumDeposit}+";

                            if (!string.IsNullOrEmpty(maximumDeposit))
                                minToMaxDeposit = $"${minimumDeposit} - ${maximumDeposit}";
                        }

                        foreach (var depositRate in matchedDepositRates)
                        {
                            var depositRateNoTrailingZeros = RemoveTrailingZeros(depositRate.Rate);
                            var termRatePercentage = FormatAsPercentage(depositRateNoTrailingZeros);
                            var term = $"{depositRate.AdditionalValue.Replace("P", "").Replace("M", "")}";
                            var termFormatted = term == "1" ? $"{term} month" : $"{term} months";

                            decimal rate = StringToDecimal(depositRateNoTrailingZeros);

                            // Create a variable with the highest rate to sort by
                            if (rate > highestRate)
                            {
                                highestRate = rate;
                                highestTerm = termFormatted;
                            }

                            var interestFrequency = string.Empty;

                            if (depositRate.ApplicationFrequency == depositRate.AdditionalValue)
                                interestFrequency = "At Maturity";
                            else if (depositRate.ApplicationFrequency == "P1M" || depositRate.ApplicationFrequency == "P4W")
                                interestFrequency = "Monthly";
                            else if (depositRate.ApplicationFrequency == "P3M")
                                interestFrequency = "Quarterly";
                            else if (depositRate.ApplicationFrequency == "P6M")
                                interestFrequency = "Semi-Annually";
                            else if (depositRate.ApplicationFrequency == "P12M" || depositRate.ApplicationFrequency == "P1Y")
                                interestFrequency = "Annually";
                            else
                                interestFrequency = "??";

                            termRates.Add(new TermRate { Term = termFormatted, Percent = termRatePercentage, Frequency = interestFrequency });
                        }

                        var foundTD = new TermDeposit
                        {
                            BankName = bank.Name,
                            BankNameDisplay = bank.NameDisplay,
                            BankLogoUrl = bank.LogoURL,
                            Category = GetEnumMemberAttrValue(product.listData.ProductCategory.GetType(), product.listData.ProductCategory),
                            ProductName = product.listData.Name,
                            Description = product.listData.Description,
                            ID = product.ID,
                            MatchedDepositRates = matchedDepositRates,
                            TermRates = termRates,
                            HighestRate = highestRate,
                            HighestTermFormatted = highestTerm,
                            HighestRateFormatted = FormatAsPercentage(highestRate.ToString()),
                            MinimumDeposit = minimumDeposit,
                            MaximumDeposit = maximumDeposit,
                            MinimumToMaximumDepositFormatted = minToMaxDeposit,
                            ApplyUrl = product.listData.ApplicationUri,
                            LastRefreshed = bank.LastRefreshed
                        };

                        bankFilteredDataTermDeposits.TermDeposits.Add(foundTD);
                    }
                }
            }

            // 1 - "Interest Rate - highest to lowest",
            // 2 - "Interest Rate - lowest to highest"
            // 3 - "Provider - A to Z"
            // 4 - "Provider - Z to A"
            // 5 - "Last Refereshed"
            switch (filter.SortByIndex)
            {
                case "":
                case "1":
                    {
                        bankFilteredDataTermDeposits.TermDeposits = bankFilteredDataTermDeposits.TermDeposits.OrderByDescending(td => td.HighestRate).ThenByDescending(td => td.LastRefreshed).ToList();
                        break;
                    }
                case "2":
                    {
                        bankFilteredDataTermDeposits.TermDeposits = bankFilteredDataTermDeposits.TermDeposits.OrderBy(td => td.HighestRate).ThenBy(td => td.LastRefreshed).ToList();
                        break;
                    }
                case "3":
                    {
                        bankFilteredDataTermDeposits.TermDeposits = bankFilteredDataTermDeposits.TermDeposits.OrderBy(td => td.BankName).ThenByDescending(td => td.LastRefreshed).ToList();
                        break;
                    }
                case "4":
                    {
                        bankFilteredDataTermDeposits.TermDeposits = bankFilteredDataTermDeposits.TermDeposits.OrderByDescending(td => td.BankName).ThenBy(td => td.LastRefreshed).ToList();
                        break;
                    }
                case "5":
                    {
                        bankFilteredDataTermDeposits.TermDeposits = bankFilteredDataTermDeposits.TermDeposits.OrderByDescending(td => td.LastRefreshed).ToList();
                        break;
                    }
            }

            if (showRemaining)
            {
                bankFilteredDataTermDeposits.CountFiltered = countFilteredTotal;
            }
            else
            {
                bankFilteredDataTermDeposits.CountFiltered = countFilteredTotal >= 10 ? 10 : countFilteredTotal;

                if (!showAll)
                    bankFilteredDataTermDeposits.TermDeposits = bankFilteredDataTermDeposits.TermDeposits.Take(10).ToList();
            }

            bankFilteredDataTermDeposits.CountFilteredTotal = countFilteredTotal;
            bankFilteredDataTermDeposits.CountTotal = countTotal;

            return new OkObjectResult(bankFilteredDataTermDeposits);
        }

        public static string FormatAsPercentage(string decimalString)
        {
            if (string.IsNullOrEmpty(decimalString))
            {
                return string.Empty;
            }

            if (decimal.TryParse(decimalString, out decimal value))
            {
                // Multiply by 100 to convert to percentage and format with two decimal places
                return (value * 100).ToString("N2") + "%";
            }
            else
            {
                return "Invalid input";
            }
        }

        public static string RemoveTrailingZeros(string numberString)
        {
            if (string.IsNullOrEmpty(numberString))
            {
                return numberString;
            }

            int index = numberString.IndexOf('.');
            if (index == -1)
            {
                return numberString; // No decimal point, no trailing zeros to remove
            }

            while (numberString.EndsWith("0"))
            {
                numberString = numberString.Substring(0, numberString.Length - 1);
            }

            if (numberString.EndsWith("."))
            {
                numberString = numberString.Substring(0, numberString.Length - 1);
            }

            return numberString;
        }

        public static decimal StringToDecimal(string input)
        {
            if (decimal.TryParse(input, out decimal result))
            {
                return result;
            }
            else
            {
                return 0;
            }
        }

        public static string StringToAmount(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            double number;
            if (!double.TryParse(input, out number))
            {
                return input;
            }

            return number.ToString("N0");
        }

        private static bool MatchDepositAmount(string depositAmount, List<BankingProductConstraint> constraints)
        {
            if (string.IsNullOrEmpty(depositAmount))
                return false;

            if (constraints == null || constraints.Count == 0)
                return true;

            bool match = false;

            var depositAmountDouble = ConvertToDouble(depositAmount);
            double minimumBalanceDouble = 0;

            foreach (var constraint in constraints)
            {
                if (constraint.ConstraintType == BankingProductConstraint.ConstraintTypeEnum.MINBALANCE)
                {
                    minimumBalanceDouble = ConvertToDouble(constraint.AdditionalValue, true);
                }
            }

            if (depositAmountDouble >= minimumBalanceDouble)
                return true;

            return match;
        }

        private static double ConvertToDouble(string input, bool attemptToExtractNumberFromString = false)
        {
            if (!IsValidNumber(input))
            {
                if (attemptToExtractNumberFromString)
                    return ExtractNumberFromString(input);

                return 0;
            }

            return Convert.ToDouble(input);
        }

        public static double ExtractNumberFromString(string input)
        {
            if (string.IsNullOrEmpty(input))
                return 0.0;

            // ING Product ID
            // constraint.AdditionalValue = "A minimum combined balance of $50,000 is required across any Business Optimiser and Business Term Deposit accounts held in your business entity's name."
            // so return 50000
            // Regular expression to match numbers, including commas and decimal points
            Regex regex = new Regex(@"[-+]?[0-9]+(?:,[0-9]+)*(\.[0-9]+)?");
            Match match = regex.Match(input);

            if (match.Success)
            {
                string numberString = match.Value.Replace(",", "");
                double number;

                if (double.TryParse(numberString, out number))
                {
                    return number;
                }
            }

            return 0.0;
        }

        public static bool IsValidNumber(string input)
        {
            if (string.IsNullOrEmpty(input))
                return false;

            double number;
            return double.TryParse(input, out number);
        }

        private static bool MatchTerms(List<string> terms, List<BankingProductDepositRate> depositRates, ref List<BankingProductDepositRate> matchedRates)
        {
            if (terms == null || terms.Count == 0)
                return true;

            if (depositRates == null || depositRates.Count == 0)
                return false;

            bool found = false;

            foreach (var term in terms)
            {
                var code = $"P{term.Replace(" months", "").Replace(" month", "")}M";

                foreach (var depositRate in depositRates)
                {
                    //if (depositRate.ApplicationFrequency == code)
                    if (depositRate.AdditionalValue == code)
                    {
                        matchedRates.Add(depositRate);

                        found = true;
                    }
                }
            }

            return found;
        }

        [FunctionName("RefreshCache")]
        public static async Task<IActionResult> RunRefreshCache([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var cacheDirectory = req.Query["CacheDirectory"];
            var storageType = cacheDirectory.ToString() == string.Empty ? StorageType.AzureBlobStorage : StorageType.LocalFile;
            var currentBank = "";
            var currentProductId = "";
            var errorList = new List<string>();

            try
            {
                var requestBody = new StreamReader(req.Body);
                var body = await requestBody.ReadToEndAsync();
                var banksToRefresh = JsonConvert.DeserializeObject<List<string>>(body);
                var candidateBanks = GetCandidateBanks();
                var allRefreshedBanks = new BankData { data = new List<Bank>() };

                foreach (var candidateBank in candidateBanks)
                {
                    foreach (var bankToRefresh in banksToRefresh.Where(bankToRefresh => candidateBank.Name == bankToRefresh))
                    {
                        candidateBank.Refresh = true;

                        break;
                    }

                    if (!candidateBank.Refresh)
                        continue;

                    currentBank = candidateBank.Name;

                    // Get the products list
                    var client = new HttpConnector();
                    var bank = new Bank(candidateBank.Name, candidateBank.NameDisplay, candidateBank.LogoURL, DateTime.Now, new List<Product>());
                    var response = await client.GetProductsAsync($"{candidateBank.OpenBankingProductListUrl}?page-size={PageSize}", candidateBank.Version, candidateBank.VersionMin);
                    var individualBankToSaveToFile = new BankData { data = new List<Bank>() };
                    var successfulList = false;
                    var atLeastOneSuccessfulProduct = false;

                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        var products = JsonConvert.DeserializeObject<ResponseBankingProductListV2>(json);

                        if (products?.Data?.Products == null)
                        {
                            errorList.Add($"{currentBank}: [No list data returned]");
                            continue;
                        }

                        successfulList = true;

                        foreach (var product in products.Data.Products)
                        {
                            if (product.ProductCategory != BankingProductCategory.TERMDEPOSITS && storageType == StorageType.AzureBlobStorage)
                                continue;

                            currentProductId = product.ProductId;

                            if (string.IsNullOrEmpty(product.ProductId))
                            {
                                // MOVE Bank
                                continue;
                            }

                            if (currentBank == "Newcastle Permanent Building Society" && product.ProductId == "HL_realdealIO")
                            {
                                // minimumValue is a required property for BankingProductRateTierV3 and cannot be null
                                continue;
                            }

                            // Get the product details
                            response = await client.GetProductsAsync($"{candidateBank.OpenBankingProductListUrl}/{product.ProductId}", candidateBank.Version, candidateBank.VersionMin);

                            json = await response.Content.ReadAsStringAsync();

                            if (response.IsSuccessStatusCode)
                            {
                                var productDetail = JsonConvert.DeserializeObject<ResponseBankingProductByIdV4>(json);

                                // Combine the product list data and the product detail data into a single product object
                                var combinedProduct = new Product(product.ProductId, product, productDetail.Data);

                                bank.Products.Add(combinedProduct);

                                atLeastOneSuccessfulProduct = true;
                            }
                            else
                            {
                                // 	Australian Mutual Bank LTD: AMB_Home_Loan_Offset
                                if (json.IndexOf("Open Banking Product is inactive") != -1)
                                    continue;

                                // Cairns bank: L25_PLUS_HOME_LOAN_OWNER_OCCUPIED_P&I_1YR_FIX_LVR_80-90
                                if (json.IndexOf("A potentially dangerous Request.Path value was detected from the client") != -1)
                                    continue;

                                // AMP Bank
                                if (json.IndexOf("Service Unavailable") != -1)
                                    continue;

                                errorList.Add($"{currentBank} {currentProductId}: [{response.ReasonPhrase}]");
                            }
                        }

                        individualBankToSaveToFile.data.Add(bank);
                        allRefreshedBanks.data.Add(bank);
                    }
                    else
                    {
                        errorList.Add($"{currentBank}: [{response.ReasonPhrase}]");
                    }

                    if (successfulList && atLeastOneSuccessfulProduct)
                    {
                        var serialisedData = JsonConvert.SerializeObject(individualBankToSaveToFile);

                        // Save the individual bank data to seperate files locally for debugging
                        if (storageType == StorageType.LocalFile)
                        {
                            var cacheFileName = BanksDataFileNameAdmin.Replace(".json", $"_{candidateBank.Name}.json");
                            var cacheFile = $"{cacheDirectory}\\{cacheFileName}";

                            await File.WriteAllTextAsync(cacheFile, serialisedData);
                        }
                    }
                }

                // Stitch all data together in a single object
                var bankDataCombined = new BankData { data = new List<Bank>() };
                var allBankData = await ReadAllBankData(req.Query["CacheDirectory"]);

                // First add the refreshed banks
                if (allRefreshedBanks?.data != null)
                {
                    foreach (var refreshedBank in allRefreshedBanks.data)
                    {
                        bankDataCombined.data.Add(refreshedBank);
                    }
                }

                // Now add the remaining existing
                if (allBankData?.data != null)
                {
                    foreach (var existingBank in allBankData.data)
                    {
                        var refreshedBank = allRefreshedBanks.data.FirstOrDefault(x => x.Name.Equals(existingBank.Name));

                        if (refreshedBank == null)
                            bankDataCombined.data.Add(existingBank);
                    }
                }

                var serialisedUpdatedBankData = JsonConvert.SerializeObject(bankDataCombined);

                // Only for local storage
                if (storageType == StorageType.LocalFile)
                {
                    var cacheFileAll = $"{cacheDirectory}\\{BanksDataFileNameAdmin}";
                    File.Delete(cacheFileAll);
                    await File.WriteAllTextAsync(cacheFileAll, serialisedUpdatedBankData);
                }

                // Save combined data to Blob Storage as a backup
                await SaveBlobStorageDataAsync(BanksDataFileNameAdmin, serialisedUpdatedBankData);

                // Build Product Category Data
                var TERM_DEPOSITS = new BankData { data = new List<Bank>() };
                //var TRANS_AND_SAVINGS_ACCOUNTS = new BankData { data = new List<Bank>() };

                foreach (var bank in bankDataCombined.data)
                {
                    //TRANS_AND_SAVINGS_ACCOUNTS.data.Add(new Bank(bank.Name, bank.LastRefreshed, new List<Product>()));
                    TERM_DEPOSITS.data.Add(new Bank(bank.Name, bank.NameDisplay, bank.LogoURL, bank.LastRefreshed, new List<Product>()));

                    foreach (var product in bank.Products)
                    {
                        //if (MatchProductCategory("TRANS_AND_SAVINGS_ACCOUNTS", product.listData.ProductCategory))
                        //    TRANS_AND_SAVINGS_ACCOUNTS.data.Where(x => x.Name == bank.Name).FirstOrDefault().Products.Add(product);

                        if (MatchProductCategory("TERM_DEPOSITS", product.listData.ProductCategory))
                            TERM_DEPOSITS.data.Where(x => x.Name == bank.Name).FirstOrDefault().Products.Add(product);
                    }
                }

                //await SaveCategoryDataAsync(storageType, cacheDirectory, BanksDataFileNameWebsiteTransactionAndSavingss, TRANS_AND_SAVINGS_ACCOUNTS);
                await SaveCategoryDataAsync(storageType, cacheDirectory, BanksDataFileNameWebsiteTermDeposits, TERM_DEPOSITS);

                if (errorList.Count > 0)
                    return new BadRequestObjectResult($"{string.Join("; ", errorList)}");

                return new OkObjectResult(allRefreshedBanks);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"{currentBank} {currentProductId}: {ex.Message}");
            }
        }

        private static async Task SaveCategoryDataAsync(StorageType storageType, string cacheDirectory, string fileName, BankData data)
        {
            var serialisedBankData = JsonConvert.SerializeObject(data);

            if (storageType == StorageType.LocalFile)
            {
                var filePath = $"{cacheDirectory}\\{fileName}";
                File.Delete(filePath);
                await File.WriteAllTextAsync(filePath, serialisedBankData);
            }

            await SaveBlobStorageDataAsync($"{fileName}", serialisedBankData);
        }

        [FunctionName("CompareProducts")]
        public static async Task<IActionResult> RunGetComparison([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req, ILogger log)
        {
            try
            {
                var requestBody = new StreamReader(req.Body);
                var body = await requestBody.ReadToEndAsync();
                var productCategory = req.Query["ProductCategory"];
                var prompt = req.Query["Prompt"];
                var product1 = string.Empty;
                var product2 = string.Empty;

                if (string.IsNullOrEmpty(prompt))
                    prompt = $"Compare:";

                if (productCategory == "TERM_DEPOSITS")
                {
                    var products = JsonConvert.DeserializeObject<List<TermDeposit>>(body);

                    if (products.Count != 2)
                        return new BadRequestObjectResult("2 Products required");

                    product1 = JsonConvert.SerializeObject(products[0]);
                    product2 = JsonConvert.SerializeObject(products[1]);
                }

                //if (productCategory == "TRANS_AND_SAVINGS_ACCOUNTS")
                //{
                //    var products = JsonConvert.DeserializeObject<List<TransactionAndSaving>>(body);

                //    if (products.Count != 2)
                //        return new BadRequestObjectResult("2 Products required");

                //    product1 = JsonConvert.SerializeObject(products[0]);
                //    product2 = JsonConvert.SerializeObject(products[1]);  
                //}

                prompt = $"{prompt} {product1} and {product2}";

                string responseJson = await CallGeminiAPI(prompt);

                if (responseJson != null)
                {
                    var responseData = JsonConvert.DeserializeObject<GemimiResponse>(responseJson);
                    string generatedText = responseData.candidates[0].content.parts[0].text;

                    return new OkObjectResult(generatedText);
                }

                return new BadRequestObjectResult("Error");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        static async Task<string> CallGeminiAPI(string textPrompt)
        {
            const string endpoint = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={GeminiApiKey}";

            //const string apiKey = "AIzaSyAJkeXwLCpBH5xoC6Vs2aomPv05rRjveD4";
            //const string endpoint = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-pro:generateContent?key={apiKey}";

            try
            {
                HttpClient client = new();

                var request = new GeminiRequest { contents = new List<RequestContent>(), safety_settings = new List<RequestSafetySettings>() };
                request.contents.Add(new RequestContent { parts = new List<RequestPart>(), role = "user" });
                request.contents[0].parts.Add(new RequestPart { text = textPrompt });
                request.safety_settings.Add(new RequestSafetySettings { category = "HARM_CATEGORY_SEXUALLY_EXPLICIT", threshold = "BLOCK_LOW_AND_ABOVE" });

                var requestJson = JsonConvert.SerializeObject(request);

                HttpResponseMessage response = await client.PostAsync(endpoint, new StringContent(requestJson, Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    string responseJson = await response.Content.ReadAsStringAsync();
                    return responseJson;
                }
                else
                {
                    Console.WriteLine("API call failed with status code: {0}", response.StatusCode);
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: {0}", ex.Message);
                return null;
            }
        }

        private static bool MatchLoanAmount(string loanAmount, List<BankingProductRateTierV3> lendingRateTiers, ref decimal? loanAmountMinimum, ref decimal? loanAmountMaximum)
        {
            if (lendingRateTiers == null || lendingRateTiers.Count == 0)
                return false;

            foreach (var tier in lendingRateTiers)
            {
                if (tier.UnitOfMeasure != BankingProductRateTierV3.UnitOfMeasureEnum.DOLLAR)
                    continue;

                if (tier.MinimumValue <= decimal.Parse(loanAmount) && tier.MaximumValue >= decimal.Parse(loanAmount))
                {
                    loanAmountMinimum = tier.MinimumValue;
                    loanAmountMaximum = tier.MaximumValue;

                    return true;
                }
            }

            return false;
        }

        private static bool MatchProductCategory(string stringValue, BankingProductCategory enumValue)
        {
            if (string.IsNullOrEmpty(stringValue) || stringValue == _all)
                return true;

            var result = ValidEnumByValue<BankingProductCategory>(stringValue);

            return enumValue == (BankingProductCategory)result;
        }

        public static object ValidEnumByValue<T>(string input)
        {
            var values = Enum.GetValues(typeof(T));

            foreach (var value in values)
            {
                var selectedEnum = (T)Enum.Parse(typeof(T), value.ToString());

                if (GetEnumMemberAttrValue(selectedEnum.GetType(), selectedEnum) == input)
                    return selectedEnum;
            }

            return null;
        }

        private static string GetEnumMemberAttrValue(Type enumType, object enumVal)
        {
            var memInfo = enumType.GetMember(enumVal.ToString() ?? string.Empty);
            var attr = memInfo[0].GetCustomAttributes(false).OfType<EnumMemberAttribute>().FirstOrDefault();

            return attr != null ? attr.Value : null;
        }

        private static List<CandidateBank> GetCandidateBanks()
        {
            var banks = new List<CandidateBank>
            {
                            new(name: "ANZ", nameDisplay: "ANZ", openBankingBaseUrl: "https://api.anz", logoUrl: "https://www.anz.com.au/content/dam/anzcomau/logos/anz/ANZ-MB-Logo-3rd-Party-RGB.png", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 2, active: false),
                            new(name: "CommBank", nameDisplay: "CommBank", openBankingBaseUrl: "https://api.commbank.com.au/public", logoUrl: "https://www.commbank.com.au/content/dam/commbank-assets/cba-stacked.jpg", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 2, active: false),
                            new(name: "NATIONAL AUSTRALIA BANK", nameDisplay: "NAB", openBankingBaseUrl: "https://openbank.api.nab.com.au", logoUrl: "https://www.nab.com.au/etc/designs/nabrwd/clientlibs/images/logo.png", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 2, active: false),
                            new(name: "Westpac", nameDisplay: "Westpac", openBankingBaseUrl: "https://digital-api.westpac.com.au", logoUrl: "https://banking.westpac.com.au/wbc/banking/Themes/Default/Desktop/WBC/Core/Images/logo_white_bg.png.ce5c4c19ec61b56796f0e218fc8329c558421fd8.png", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 2, active: true),
                new(name: "AMP Bank", nameDisplay: "AMP Bank", openBankingBaseUrl: "https://api.cdr-api.amp.com.au", logoUrl: "https://www.amp.com.au/content/dam/amp-au/data/icons/amp-logo-reversed.svg", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "ANZ Plus", nameDisplay: "ANZ Plus", openBankingBaseUrl: "https://cdr.apix.anz", logoUrl: "https://assets.anz.com/is/image/anz/ANZPlus-Logo-Blue?$ResponsiveImage$&fmt=png-alpha", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
//new(name: "AWA Alliance Bank", nameDisplay: "AWA Alliance Bank", openBankingBaseUrl: "https://api.cdr.awaalliancebank.com.au", logoUrl: "https://www.cdr.gov.au/sites/default/files/2022-08/No-logo-available-2.png", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "Adelaide Bank", nameDisplay: "Adelaide Bank", openBankingBaseUrl: "https://api.cdr.adelaidebank.com.au", logoUrl: "https://www.cdr.gov.au/sites/default/files/2022-08/No-logo-available-2.png", useLogoFile: true, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "Alex Bank", nameDisplay: "Alex Bank", openBankingBaseUrl: "https://public.cdr.alex.bank", logoUrl: "https://www.alex.bank/assets/alex-bank-logo.png", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "Arab Bank Australia Limited", nameDisplay: "Arab Bank Australia Limited", openBankingBaseUrl: "https://public.cdr.arabbank.com.au", logoUrl: "https://www.arabbank.com.au/themes/arabbank/images/abal-logo.svg", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
//new(name: "Aussie Home Loans", nameDisplay: "Aussie Home Loans", openBankingBaseUrl: "https://aussie.openportal.com.au", logoUrl: "https://aussie.openportal.com.au/assets/bfs/applications/digital/ahl/images/aussiehomeloans-logo.png", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "Australian Military Bank", nameDisplay: "Australian Military Bank", openBankingBaseUrl: "https://public.open.australianmilitarybank.com.au", logoUrl: "https://www.australianmilitarybank.com.au/sites/default/files/amb_logo_478.png", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "Australian Mutual Bank LTD", nameDisplay: "Australian Mutual Bank LTD", openBankingBaseUrl: "https://internetbanking.australianmutual.bank/openbanking", logoUrl: "https://internetbanking.australianmutual.bank/openbanking/logo.png", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "Australian Unity Bank", nameDisplay: "Australian Unity Bank", openBankingBaseUrl: "https://open-banking.australianunity.com.au", logoUrl: "https://www.australianunity.com.au/-/media/rebrandcorporate/logos/au-180years-logo.svg", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "Auswide Bank Ltd", nameDisplay: "Auswide Bank Ltd", openBankingBaseUrl: "https://api.auswidebank.com.au/openbanking", logoUrl: "https://community.auswidebank.com.au/resources/images/templates/shared/auswide-bank-logo.svg", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "BCU Bank", nameDisplay: "BCU Bank", openBankingBaseUrl: "https://public.cdr-api.bcu.com.au", logoUrl: "https://broker.pnbank.info/XnIMb18a", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "BHCCU", nameDisplay: "BHCCU", openBankingBaseUrl: "https://public.cdr-api.bhccu.com.au", logoUrl: "https://images.squarespace-cdn.com/content/v1/63fd48114c119156c9e26d0d/1677543488227-M9WUNS33BA2VYN703E8M/BHCCU_50-Year_Logo_Combo_large.png?format=1500w", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "BOQ Specialist", nameDisplay: "BOQ Specialist", openBankingBaseUrl: "https://api.cds.boqspecialist.com.au", logoUrl: "https://www.boqspecialist.com.au/content/dam/boq-specialist/logos/BOQSpecialist_H_CMYK.png", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "Bank Australia", nameDisplay: "Bank Australia", openBankingBaseUrl: "https://public.cdr-api.bankaust.com.au", logoUrl: "https://www.bankaust.com.au/globalassets/assets/imagery/logos/bank-australia/ba_horizontal_logo.png", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "Bank First", nameDisplay: "Bank First", openBankingBaseUrl: "https://public.cdr.bankfirst.com.au", logoUrl: "https://cdn.intelligencebank.com/au/share/e3Gq6M/oGkXA/Y01wk/original/Bank+First+Logo+Horizontal+Navy+RGB", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
//new(name: "Bank of China", nameDisplay: "Bank of China", openBankingBaseUrl: "https://api-gateway.bankofchina.com.au", logoUrl: "https://boc-resources.s3.ap-southeast-2.amazonaws.com/images/BOC+logo.JPG", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
//new(name: "Bank of Heritage Isle", nameDisplay: "Bank of Heritage Isle", openBankingBaseUrl: "https://product.api.heritageisle.com.au", logoUrl: "https://www.cdr.gov.au/sites/default/files/2022-08/No-logo-available-2.png", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "Bank of Melbourne", nameDisplay: "Bank of Melbourne", openBankingBaseUrl: "https://digital-api.bankofmelbourne.com.au", logoUrl: "https://www.bankofmelbourne.com.au/content/dam/bom/images/home/BOM-logo_1200x1200.jpg", useLogoFile: true, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "Bank of Queensland Limited", nameDisplay: "Bank of Queensland Limited", openBankingBaseUrl: "https://api.cds.boq.com.au", logoUrl: "https://www.boq.com.au/content/dam/boq/images/miscellaneous-images/boq-logo2.png", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                            new(name: "Bank of Sydney", nameDisplay: "Bank of Sydney", openBankingBaseUrl: "https://openbank.api.banksyd.com.au", logoUrl: "https://www.banksyd.com.au/globalassets/images/logos/bos-logo-edge-fix.svg", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "Bank of us", nameDisplay: "Bank of us", openBankingBaseUrl: "https://api.bankofus.com.au/OpenBanking", logoUrl: "https://api.bankofus.com.au/OpenBanking/Logo.png", useLogoFile: true, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "BankSA", nameDisplay: "BankSA", openBankingBaseUrl: "https://digital-api.banksa.com.au", logoUrl: "https://www.banksa.com.au/content/dam/bsa/images/home/BSA-logo_1200x1200.jpg", useLogoFile: true, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "BankVic", nameDisplay: "BankVic", openBankingBaseUrl: "https://ib.bankvic.com.au/openbanking", logoUrl: "https://ib.bankvic.com.au/bvib/App_Themes/BankVicResponsive/images/BankVic-logo.svg", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "Bankwest", nameDisplay: "Bankwest", openBankingBaseUrl: "https://open-api.bankwest.com.au/bwpublic", logoUrl: "https://www.bankwest.com.au/content/dam/bankwest/web-assets/images/global/logo/logo-bankwest-horizontal-charcoal.png", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
//new(name: "Bell Potter", nameDisplay: "Bell Potter", openBankingBaseUrl: "https://bellpotter.openportal.com.au", logoUrl: "https://bellpotter.openportal.com.au/assets/bfs/applications/digital/bell-potter/images/bellpotter-logo.png", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "Bendigo Bank", nameDisplay: "Bendigo Bank", openBankingBaseUrl: "https://api.cdr.bendigobank.com.au", logoUrl: "https://www.bendigobank.com.au/globalassets/globalresources/brand-logos/bendigobank-logo.png", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "Beyond Bank Australia", nameDisplay: "Beyond Bank Australia", openBankingBaseUrl: "https://public.cdr.api.beyondbank.com.au", logoUrl: "https://www.beyondbank.com.au/dam/document-repository/open-banking/beyond-bank-logo.svg", useLogoFile: true, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "Border Bank", nameDisplay: "Border Bank", openBankingBaseUrl: "https://product.api.borderbank.com.au", logoUrl: "https://www.cdr.gov.au/sites/default/files/2022-08/No-logo-available-2.png", useLogoFile: true, minVersion: "3", version: "4", displayOrder: 3, active: true),
//new(name: "CBA - CommBiz", nameDisplay: "CBA - CommBiz", openBankingBaseUrl: "https://cdr.commbiz.api.commbank.com.au/cbzpublic", logoUrl: "https://www.commbank.com.au/content/dam/commbank/commBank-logo.svg", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "Cairns bank", nameDisplay: "Cairns bank", openBankingBaseUrl: "https://openbanking.cairnsbank.com.au/OpenBanking", logoUrl: "https://cairnsbank.com.au/fileadmin/templates/assets/images/cairns-bank-logo.png", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
//new(name: "Card Services", nameDisplay: "Card Services", openBankingBaseUrl: "https://api.openbanking.cardservicesdirect.com.au", logoUrl: "https://www.cdn.citibank.com/v1/augcb/cbol/files/images/2021/logo-Card-Services.png", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "Central Murray Credit Union Limited", nameDisplay: "Central Murray Credit Union Limited", openBankingBaseUrl: "https://secure.cmcu.com.au/openbanking", logoUrl: "https://www.cmcu.com.au/images/header.png", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "Central West CUL", nameDisplay: "Central West CUL", openBankingBaseUrl: "https://ib.cwcu.com.au/openbanking", logoUrl: "https://ib.cwcu.com.au/OpenBanking/logo.png", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "Citi", nameDisplay: "Citi", openBankingBaseUrl: "https://openbanking.api.citi.com.au", logoUrl: "https://www.cdn.citibank.com/v1/augcb/cbol/files/images/2021/logo-Citi.png", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "Coastline Credit Union", nameDisplay: "Coastline Credit Union", openBankingBaseUrl: "https://public.cdr-api.coastline.com.au", logoUrl: "https://www.coastline.com.au/Themes/Coastline/Content/img/logo.png", useLogoFile: true, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "Coles Financial Services", nameDisplay: "Coles Financial Services", openBankingBaseUrl: "https://api.openbanking.secure.coles.com.au", logoUrl: "https://www.cdn.citibank.com/v1/augcb/cbol/files/images/2021/logo-Coles-Financial-Services.png", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "CommFCU", nameDisplay: "CommFCU", openBankingBaseUrl: "https://netbank.communityfirst.com.au/cf-OpenBanking", logoUrl: "https://cms.communityfirst.com.au/assets/77808f15-64e8-4ed3-b03d-4b4c49c28ee7", useLogoFile: true, minVersion: "3", version: "4", displayOrder: 3, active: true),
//new(name: "Credit Union SA", nameDisplay: "Credit Union SA", openBankingBaseUrl: "https://openbanking.api.creditunionsa.com.au", logoUrl: "https://cms.creditunionsa.com.au/assets/images/Open-Banking/cusa-logo_stacked_trans_250px.png", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "DDH Graham", nameDisplay: "DDH Graham", openBankingBaseUrl: "https://api.cds.ddhgraham.com.au", logoUrl: "https://ddhgraham.com.au/wp-content/uploads/DDH_BOQ_512.png", useLogoFile: true, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "Defence Bank", nameDisplay: "Defence Bank", openBankingBaseUrl: "https://product.defencebank.com.au", logoUrl: "https://www.defencebank.com.au/globalassets/images/logos/defence-bank/logo.svg", useLogoFile: true, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "Dnister", nameDisplay: "Dnister", openBankingBaseUrl: "https://public.cdr-api.dnister.com.au", logoUrl: "https://www.dnister.com.au/Signature_Deployment/DnisterLogo.gif", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "Easy Street", nameDisplay: "Easy Street", openBankingBaseUrl: "https://ebranch.easystreet.com.au/es-OpenBanking", logoUrl: "https://cms.easystreet.com.au/assets/19f6a052-776a-454f-b0aa-cd70ac4259e9", useLogoFile: true, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "Family First", nameDisplay: "Family First", openBankingBaseUrl: "https://public.cdr.familyfirst.com.au", logoUrl: "https://familyfirst.com.au/wp-content/themes/familyfirst/images/logo.png", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "Fire Service Credit Union", nameDisplay: "Fire Service Credit Union", openBankingBaseUrl: "https://public.cdr-api.fscu.com.au", logoUrl: "https://www.fscu.com.au/wp-content/uploads//2023/11/FSCU_logo.png", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "Firefighters Mutual Bank", nameDisplay: "Firefighters Mutual Bank", openBankingBaseUrl: "https://ob.tmbl.com.au/fmbank", logoUrl: "https://www.fmbank.com.au/-/media/fmbank/logos/logodesktop.ashx?h=104&w=576&la=en&hash=EC9F95FE23F72341C3F09A087BE117C8", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "First Option Bank", nameDisplay: "First Option Bank", openBankingBaseUrl: "https://internetbanking.firstoption.com.au/OpenBanking", logoUrl: "https://firstoptionbank.com.au/wp-content/uploads/2020/06/FO_Bank_logo_black.png", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "G&C Mutual Bank", nameDisplay: "G&C Mutual Bank", openBankingBaseUrl: "https://ibank.gcmutualbank.com.au/OpenBanking", logoUrl: "https://www.gcmutual.bank/media/3018/gc-mutual-bank-logo.png", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "Gateway Bank", nameDisplay: "Gateway Bank", openBankingBaseUrl: "https://public.cdr-api.gatewaybank.com.au", logoUrl: "https://www.gatewaybank.com.au/Client_Theme/imgs/gway-logo.png", useLogoFile: true, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "Geelong Bank", nameDisplay: "Geelong Bank", openBankingBaseUrl: "https://online.geelongbank.com.au/OpenBanking", logoUrl: "https://geelongbank.com.au/media/2007/geelongbank_logotag_horizontal.png", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "Goldfields Money", nameDisplay: "Goldfields Money", openBankingBaseUrl: "https://prd.bnk.com.au", logoUrl: "https://www.cdr.gov.au/sites/default/files/2022-08/No-logo-available-2.png", useLogoFile: true, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "Great Southern Bank", nameDisplay: "Great Southern Bank", openBankingBaseUrl: "https://api.open-banking.greatsouthernbank.com.au", logoUrl: "https://www.greatsouthernbank.com.au/media/images/global/logo.svg", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
//new(name: "Great Southern Bank Business+", nameDisplay: "Great Southern Bank Business+", openBankingBaseUrl: "https://api.open-banking.business.greatsouthernbank.com.au", logoUrl: "https://www.greatsouthernbank.com.au/media/images/global/logo.svg", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
//new(name: "Greater Bank", nameDisplay: "Greater Bank", openBankingBaseUrl: "https://public.cdr-api.greater.com.au", logoUrl: "https://www.greater.com.au/globalassets/images/logos/greaterbanklogo.jpg", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "HSBC", nameDisplay: "HSBC", openBankingBaseUrl: "https://public.ob.hsbc.com.au", logoUrl: "https://www.hsbc.com.au/content/dam/hsbc/au/images/01_HSBC_MASTERBRAND_LOGO_RGB.svg", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "HSBC Bank Australia Limited  Wholesale Banking", nameDisplay: "HSBC Bank Australia Limited  Wholesale Banking", openBankingBaseUrl: "https://public.ob.business.hsbc.com.au", logoUrl: "https://www.hsbc.com.au/content/dam/hsbc/au/images/01_HSBC_MASTERBRAND_LOGO_RGB.svg", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "Health Professionals Bank", nameDisplay: "Health Professionals Bank", openBankingBaseUrl: "https://ob.tmbl.com.au/hpbank", logoUrl: "https://www.hpbank.com.au/-/media/hpbank/logos/hpblogodesktop.ashx?h=520&w=2900&la=en&hash=FB4ACDF35B4BAB4B9AECB37A69160D21", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "Heartland", nameDisplay: "Heartland", openBankingBaseUrl: "https://api.cdr.heartlandbank.com.au", logoUrl: "https://www.heartlandbank.com.au/images/heartland-bank-logo-white-on-midnight-blue.jpg", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "Heritage Bank", nameDisplay: "Heritage Bank", openBankingBaseUrl: "https://product.api.heritage.com.au", logoUrl: "https://www.heritage.com.au/Inbound/Open-Banking/logo", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "Hiver Bank", nameDisplay: "Hiver Bank", openBankingBaseUrl: "https://ob.tmbl.com.au/hiver", logoUrl: "https://hiver.bank/Client_Theme/errors/images/hiver.jpg", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "Horizon Bank", nameDisplay: "Horizon Bank", openBankingBaseUrl: "https://onlinebanking.horizonbank.com.au/openbanking", logoUrl: "https://horizonbank.com.au/Client_Theme/imgs/horizon_logo.png", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "Hume Bank", nameDisplay: "Hume Bank", openBankingBaseUrl: "https://ibankob.humebank.com.au/OpenBanking", logoUrl: "https://ibankob.humebank.com.au/humebank.png", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "IMB Bank", nameDisplay: "IMB Bank", openBankingBaseUrl: "https://openbank.openbanking.imb.com.au", logoUrl: "https://www.imb.com.au/templates/client/images/header/IMB-logo.svg", useLogoFile: true, minVersion: "3", version: "4", displayOrder: 3, active: true),
                            new(name: "ING BANK (Australia) Ltd", nameDisplay: "ING BANK (Australia) Ltd", openBankingBaseUrl: "https://id.ob.ing.com.au", logoUrl: "https://www.ing.com.au/img/logos/ING_Primary_Logo_RGB.png", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "Illawarra Credit Union Limited", nameDisplay: "Illawarra Credit Union Limited", openBankingBaseUrl: "https://onlineteller.cu.com.au/OpenBanking", logoUrl: "https://www.illawarracu.com.au/content/themes/nucleo-icu/assets/images/logo.png", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                            new(name: "Judo Bank", nameDisplay: "Judo Bank", openBankingBaseUrl: "https://public.open.judo.bank", logoUrl: "https://cdn.unifii.net/judobank/1ddcbc50-08b1-4472-9267-85da3bb20c83.svg", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "Kogan Money Credit Cards", nameDisplay: "Kogan Money Credit Cards", openBankingBaseUrl: "https://api.openbanking.cards.koganmoney.com.au", logoUrl: "https://www.cdn.citibank.com/v1/augcb/cbol/files/images/2021/logo-Kogan-Money.png", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "Laboratories Credit Union", nameDisplay: "Laboratories Credit Union", openBankingBaseUrl: "https://internetbanking.lcu.com.au/OpenBanking", logoUrl: "https://www.lcu.com.au/templates/client/images/header/lcu-logo.gif", useLogoFile: true, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "Liberty Financial", nameDisplay: "Liberty Financial", openBankingBaseUrl: "https://services.liberty.com.au/api/data-holder-public", logoUrl: "https://d2ttwt9gu7swv4.cloudfront.net/cdr_assets/Liberty-Aero-Horizontal-RGB.jpg", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "ME Bank", nameDisplay: "ME Bank", openBankingBaseUrl: "https://public.openbank.mebank.com.au", logoUrl: "https://www.mebank.com.au/getmedia/c79ff77e-5b3b-4410-9a1b-98e98a79f025/ME-logo-600px-black-transparent.png", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "ME Bank - ME Go", nameDisplay: "ME Bank - ME Go", openBankingBaseUrl: "https://api.cds.mebank.com.au", logoUrl: "https://www.mebank.com.au/getmedia/c79ff77e-5b3b-4410-9a1b-98e98a79f025/ME-logo-600px-black-transparent.png", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "MOVE Bank", nameDisplay: "MOVE Bank", openBankingBaseUrl: "https://openbanking.movebank.com.au/OpenBanking", logoUrl: "https://movebank.com.au/media/3405/move-bank-logo-website.png", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
//new(name: "Macquarie", nameDisplay: "Macquarie", openBankingBaseUrl: "https://cdr.energymadeeasy.gov.au/macquarie", logoUrl: "https://www.cdr.gov.au/sites/default/files/2022-08/No-logo-available-2.png", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                            new(name: "Macquarie Bank Limited", nameDisplay: "Macquarie Bank Limited", openBankingBaseUrl: "https://api.macquariebank.io", logoUrl: "https://www.macquarie.com.au/assets/bfs/global/macquarie-logo_black.svg", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
//new(name: "Macquarie Credit Union", nameDisplay: "Macquarie Credit Union", openBankingBaseUrl: "https://banking.macquariecu.com.au/OpenBanking", logoUrl: "https://www.cdr.gov.au/sites/default/files/2022-08/No-logo-available-2.png", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "Maitland Mutual Limited", nameDisplay: "Maitland Mutual Limited", openBankingBaseUrl: "https://openbanking.themutual.com.au/OpenBanking", logoUrl: "https://openbanking.themutual.com.au/OpenBanking/logo.png", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
//new(name: "MyState", nameDisplay: "MyState", openBankingBaseUrl: "https://openbank.api.mystate.com.au", logoUrl: "https://mystate.com.au/wp-content/uploads/MyState_Logo_s.png", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "MyState Bank", nameDisplay: "MyState Bank", openBankingBaseUrl: "https://public.cdr.mystate.com.au", logoUrl: "https://mystate.com.au/wp-content/uploads/MyState_Logo_s.png", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "Newcastle Permanent Building Society", nameDisplay: "Newcastle Permanent Building Society", openBankingBaseUrl: "https://openbank.newcastlepermanent.com.au", logoUrl: "https://www.newcastlepermanent.com.au/-/media/images/NPBS-LOGO", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "Northern Inland Credit Union Limited", nameDisplay: "Northern Inland Credit Union Limited", openBankingBaseUrl: "https://secure.nicu.com.au/OpenBanking", logoUrl: "https://nicu.com.au/images/UserUploadedImages/11/NICUlogo.png", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "ORANGE CREDIT UNION LTD", nameDisplay: "ORANGE CREDIT UNION LTD", openBankingBaseUrl: "https://online.orangecu.com.au/openbanking", logoUrl: "https://online.orangecu.com.au/openbanking/logo.png", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "Origin Energy", nameDisplay: "Origin Energy", openBankingBaseUrl: "https://public.mydata.cdr.originenergy.com.au", logoUrl: "https://res.cloudinary.com/originenergy/image/upload/v1667947270/CDR/origin-energy-logo.png", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "P&N Bank", nameDisplay: "P&N Bank", openBankingBaseUrl: "https://public.cdr-api.pnbank.com.au", logoUrl: "https://broker.pnbank.info/Nu6HWNxl", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "PayPal Australia", nameDisplay: "PayPal Australia", openBankingBaseUrl: "https://api.paypal.com/v1/identity", logoUrl: "https://newsroom.au.paypal-corp.com/image/pp_h_rgb.jpg", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "People's Choice", nameDisplay: "People's Choice", openBankingBaseUrl: "https://ob-public.peopleschoice.com.au", logoUrl: "https://media.peopleschoice.com.au/-/media/project/peopleschoice/mainsite/images/brand/600x600_peoples_choice_banking_for_life_logo", useLogoFile: true, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "Police Bank", nameDisplay: "Police Bank", openBankingBaseUrl: "https://product.api.policebank.com.au", logoUrl: "https://www.cdr.gov.au/sites/default/files/2022-08/No-logo-available-2.png", useLogoFile: true, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "Police Credit Union Ltd", nameDisplay: "Police Credit Union Ltd", openBankingBaseUrl: "https://api.policecu.com.au/OpenBanking", logoUrl: "https://www.pcunet1.com.au/internet-banking/App_Themes/PCU_Theme/LoginLogo.png", useLogoFile: true, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "QBANK", nameDisplay: "QBANK", openBankingBaseUrl: "https://banking.qbank.com.au/openbanking", logoUrl: "https://www.qbank.com.au/media/logo/qbank-logo.svg", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "Qantas Premier Credit Cards", nameDisplay: "Qantas Premier Credit Cards", openBankingBaseUrl: "https://api.openbanking.qantasmoney.com", logoUrl: "https://assets.qantasmoney.com/logos/qantas-money-logo-sm.svg", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "Qudos Bank", nameDisplay: "Qudos Bank", openBankingBaseUrl: "https://public.cdr-api.qudosbank.com.au", logoUrl: "https://cdn.prod.website-files.com/663aabca709a328fd2c0ea2e/663b1305def411e0460bafb4_logo-60.svg", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "Queensland Country Bank", nameDisplay: "Queensland Country Bank", openBankingBaseUrl: "https://public.cdr-api.queenslandcountry.bank", logoUrl: "", useLogoFile: true, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "RACQ Bank", nameDisplay: "RACQ Bank", openBankingBaseUrl: "https://cdrbank.racq.com.au", logoUrl: "https://cdn.intelligencebank.com/au/share/0Mvj/Y0GpK/RqBJZ/original/RACQ-BANK-LOGO_HOR_RGB.png", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "RAMS Financial Group Pty Ltd", nameDisplay: "RAMS Financial Group Pty Ltd", openBankingBaseUrl: "https://digital-api.westpac.com.au/rams", logoUrl: "https://www.rams.com.au/siteassets/homepage/rams_logo.png", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "RSL Money", nameDisplay: "RSL Money", openBankingBaseUrl: "https://public.open.rslmoney.com.au", logoUrl: "https://www.rslmoney.com.au/sites/default/files/logo-rslmoney.gif", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "Rabobank", nameDisplay: "Rabobank", openBankingBaseUrl: "https://openbanking.api.rabobank.com.au/public", logoUrl: "https://www.rabobank.com.au/content/dam/ranz/ranz-website-images/rbau-images/logo/rb-logo-stacked-200x242px.png", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "Regional Australia Bank", nameDisplay: "Regional Australia Bank", openBankingBaseUrl: "https://public-data.cdr.regaustbank.io", logoUrl: "https://www.regionalaustraliabank.com.au/-/media/CommunityMutual/Images/Logo/regional-australia-bank-primary-logo.png", useLogoFile: true, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "Reliance Bank", nameDisplay: "Reliance Bank", openBankingBaseUrl: "https://ibanking.reliancebank.com.au/rel-openbanking", logoUrl: "https://ibanking.reliancebank.com.au/rel-openbanking/logo.png", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
//new(name: "Rural Bank", nameDisplay: "Rural Bank", openBankingBaseUrl: "https://api.cdr.ruralbank.com.au", logoUrl: "https://www.cdr.gov.au/sites/default/files/2022-08/No-logo-available-2.png", useLogoFile: true, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "SWSbank", nameDisplay: "SWSbank", openBankingBaseUrl: "https://online.swscu.com.au/openbanking", logoUrl: "https://www.swsbank.com.au/Client_Theme/imgs/logo-desktop.png", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "Southern Cross Credit Union", nameDisplay: "Southern Cross Credit Union", openBankingBaseUrl: "https://cdr.sccu.com.au/openbanking", logoUrl: "https://www.sccu.com.au/community/attachment/stacked-logo-cmyk-transparent/", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "St.George Bank", nameDisplay: "St.George Bank", openBankingBaseUrl: "https://digital-api.stgeorge.com.au", logoUrl: "https://www.stgeorge.com.au/content/dam/stg/images/home/STG-logo_1200x1200.jpg", useLogoFile: true, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "Summerland Bank", nameDisplay: "Summerland Bank", openBankingBaseUrl: "https://public.cdr-api.summerland.com.au", logoUrl: "https://summerland.com.au/images/iconslogos/SummerlandCULogo.png", useLogoFile: true, minVersion: "3", version: "4", displayOrder: 3, active: true),
                            new(name: "Suncorp Bank", nameDisplay: "Suncorp Bank", openBankingBaseUrl: "https://id-ob.suncorpbank.com.au", logoUrl: "https://www.suncorpbank.com.au/content/dam/suncorp/bank/images/logos/suncorp/suncorp-bank-logo-358x104.png", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "TMCU", nameDisplay: "TMCU", openBankingBaseUrl: "https://banking.transportmutual.com.au/OpenBanking", logoUrl: "https://banking.transportmutual.com.au/OpenBanking/logo.png", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "Teachers Mutual Bank", nameDisplay: "Teachers Mutual Bank", openBankingBaseUrl: "https://ob.tmbl.com.au/tmbank", logoUrl: "https://www.tmbank.com.au/-/media/global/logo/desktop/logodesktop.ashx?h=90&w=498&la=en&hash=3CB4B2D2F2C566292E329870B0AE1504", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "The Capricornian", nameDisplay: "The Capricornian", openBankingBaseUrl: "https://public.cdr.onlinebanking.capricornian.com.au", logoUrl: "https://www.capricornian.com.au/app/uploads/2021/08/logo@2x-300x111.png", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "The Mac", nameDisplay: "The Mac", openBankingBaseUrl: "https://onlinebanking.themaccu.com.au/OpenBanking", logoUrl: "https://themaccu.com.au/media/2735/logolb.png", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "Thriday", nameDisplay: "Thriday", openBankingBaseUrl: "https://public.cdr.thriday.com.au", logoUrl: "https://assets.website-files.com/61dd1b15c9c40d33f29a4235/61e502fda949f4c2ee4adeb0_Thriday%20Logo_Horizontal%20Full%20Colour-p-500.png", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "Traditional Credit Union", nameDisplay: "Traditional Credit Union", openBankingBaseUrl: "https://prd.tcu.com.au", logoUrl: "https://www.cdr.gov.au/sites/default/files/2022-08/No-logo-available-2.png", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "Tyro Payments", nameDisplay: "Tyro Payments", openBankingBaseUrl: "https://public.cdr.tyro.com", logoUrl: "https://www.tyro.com/wp-content/uploads/2021/03/logo1.png", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
//new(name: "UBank", nameDisplay: "UBank", openBankingBaseUrl: "https://public.cdr-api.86400.com.au", logoUrl: "https://www.ubank.com.au/assets/images/light/ubank-logo.svg", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "UniBank", nameDisplay: "UniBank", openBankingBaseUrl: "https://ob.tmbl.com.au/unibank", logoUrl: "https://www.unibank.com.au/-/media/unibank/global/_image_/logo/UniBank_RGB_Black_Logo_200x52px.ashx", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "Unity Bank", nameDisplay: "Unity Bank", openBankingBaseUrl: "https://ibanking.unitybank.com.au/OpenBanking", logoUrl: "https://ibanking.unitybank.com.au/OpenBanking/logo.png", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "Unloan", nameDisplay: "Unloan", openBankingBaseUrl: "https://public.api.cdr.unloan.com.au", logoUrl: "https://assets.website-files.com/5db407e82f48093c889589aa/610a26bbe635e42da8e03fe8_Unloan_logo.svg", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "Up", nameDisplay: "Up", openBankingBaseUrl: "https://api.up.com.au", logoUrl: "https://up.com.au/assets/images/logo_1000x1000.png", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "Virgin Money", nameDisplay: "Virgin Money", openBankingBaseUrl: "https://api.cds.virginmoney.com.au", logoUrl: "https://virginmoney.com.au/content/dam/virginmoney/vma-logo.gif", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "Warwick Credit Union Ltd", nameDisplay: "Warwick Credit Union Ltd", openBankingBaseUrl: "https://openbanking.wcu.com.au/OpenBanking", logoUrl: "https://www.wcu.com.au/app/uploads/2024/02/warwick-logo.png.webp", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "Wise", nameDisplay: "Wise", openBankingBaseUrl: "https://au-cdrbanking-pub.wise.com", logoUrl: "https://wise.com/public-resources/assets/logos/wise/brand_logo.svg", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "Woolworths Team Bank", nameDisplay: "Woolworths Team Bank", openBankingBaseUrl: "https://online.woolworthsteambank.com.au/OpenBanking", logoUrl: "https://woolworthsteambank.com.au/wp-content/uploads/2021/06/wp-log-top-navi-logo.png", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "bankWAW", nameDisplay: "bankWAW", openBankingBaseUrl: "https://onlinebanking.wawcu.com.au/OpenBanking", logoUrl: "https://onlinebanking.wawcu.com.au/OpenBanking/logo.png", useLogoFile: false, minVersion: "3", version: "4", displayOrder: 3, active: true),
                new(name: "gmcu", nameDisplay: "gmcu", openBankingBaseUrl: "https://secure.gmcu.com.au/OpenBanking", logoUrl: "https://gmcu.com.au/images/layout/gmcu-logo-2021.png", useLogoFile: true, minVersion: "3", version: "4", displayOrder: 3, active: true)
            };

            foreach (var bank in banks)
            {
                if (bank.UseLogoFile)
                    bank.LogoURL = $"/img/{bank.Name}.png";
            }

            banks = banks.OrderBy(x => x.DisplayOrder).ThenBy(x => x.NameDisplay).ToList();

            return banks;
        }
    }
}

//A
//Adelaide Bank
//AFG Home Loans
//AIMS
//AMO Group
//AMP
//ANZ
//Arab Bank of Australia
//Athena
//Aussie
//Australian Military Bank
//Australian Mutual Bank
//Australian Unity
//Auswide Bank
//AWA Alliance Bank Limited

//B
//Bank Australia
//Bank First
//Bank of China Limited
//Bank of Heritage Isle
//Bank of Melbourne
//Bank of Queensland
//Bank of Sydney
//Bank of us
//BankSA
//BankVic
//BankWAW
//Bankwest
//BCU
//Bendigo Bank
//Berrima District Credit Union (BDCU)
//Beyond Bank
//Bluestone
//Border Bank
//Broken Hill Community Credit Union

//C
//Cairns Bank
//Central Murray Credit Union
//Central West Credit Union
//Challenger
//Citi
//ClickLoans
//Coastline Credit Union
//Commonwealth Bank
//Community First
//Credit Union SA
//Crown Money Management

//D
//Defence Bank
//Dnister

//E
//Easy Street
//eMoney
//Express Reverse Mortgage

//F
//Family First Credit Union
//Fire Services Credit Union
//Firefighters Mutual Bank
//First Choice Credit Union
//First Option Bank
//Firstmac
//Fox Symes
//Freedom Lend
//Freedom Loans
//Funding.com.au

//G
//G&C Mutual Bank
//Gateway Bank
//Geelong Bank
//Goldfields Money
//Goulburn Murray Credit Union
//Great Southern Bank
//Greater Bank

//H
//Heartland Reverse Mortgages
//Heritage Bank
//homeloans.com.au
//Homestar
//HomeStart
//Horizon Bank
//Household Capital
//HSBC
//Hume Bank

//I
//Illawarra Credit Union
//Illawarra Home Loans
//IMB
//ING

//K
//Keystart
//Kogan Money

//L
//La Trobe Financial
//Laboratories Credit Union
//Liberty Financial
//Loan Market
//loans.com.au

//M
//Macquarie Bank
//Macquarie Credit Union
//ME
//Morgan Brooks DIRECT
//Mortgage House
//Mortgageport
//MOVE Bank
//MyState

//N
//NAB
//Nano
//Newcastle Permanent Building Society
//Northern Inland Credit Union
//Nova Alliance Bank

//O
//OneTwo
//Online Home Loans
//Orange Credit Union

//P
//P&N Bank
//Pacific Mortgage Group
//Peoples Choice Credit Union
//Pepper Money
//Police Bank
//Police Credit Union

//Q
//Qantas Money
//QBANK
//Qudos Bank
//Queensland Country Bank

//R
//RACQ Bank
//RAMS
//Reduce Home Loans
//Regional Australia Bank
//Resi
//RESIMAC

//S
//Service One Alliance Bank
//South West Slopes Credit Union Ltd
//Southern Cross Credit Union Ltd
//St.George
//Sucasa
//Summerland Credit Union
//Suncorp Bank
//Switzer Home Loans

//T
//Teachers Mutual Bank
//The Capricornian
//The Mac
//The Mutual
//Tic:Toc
//Transport Mutual Credit Union

//U
//Ubank
//UniBank
//Unity Bank
//Unloan
//Up Bank

//V
//Virgin Money
//VMG

//W
//Warwick Credit Union Ltd
//Well Money
//Westpac

//Y
//Yard
//Yellow Brick Road
