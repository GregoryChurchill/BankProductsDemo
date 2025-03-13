using System.Diagnostics;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Admin.Properties;
using ComboBox = System.Windows.Forms.ComboBox;
using ListView = System.Windows.Forms.ListView;
using BankingProductsData;
using BankingProductsData.Models;
using System.Text;

namespace Admin
{
    public partial class FormMain : Form
    {
        private ListViewColumnSorter lvwColumnSorter;
        private HttpClient _httpClient = new();
        private bool _formLoaded = false;
        int _searchStartIndex = 0;
        int _searchIndexOfText = 0;
        private string _cacheDirectory = string.Empty;
        private const string _cacheDirectoryDefault = @"C:";
        private BankData? _bankData = new();
        private BankDataFilteredTermDeposits? _bankDataFilteredTermDeposits = new();
        private Product _selectedProduct;
        private const string _all = "ALL";
        private const string _comparisonFile = "Comparison.md";
        private int LoadAttempts = 1;
        private const int MaxLoadAttempts = 5;

        public FormMain()
        {
            InitializeComponent();

            lvwColumnSorter = new ListViewColumnSorter();
            listViewBanks.ListViewItemSorter = lvwColumnSorter;
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            ShowApiMessage(true);

            SetFormSize();

            LoadCandidateBanksAsync();

            SetSavedSettings();

            LoadSearchDropdowns();

            SetDefaultPrompt();

            timerMain.Enabled = true;
            _formLoaded = true;
        }

        private void SetDefaultPrompt()
        {
            richTextBoxPrompt.Text = "Create a mark down (md) table which compares the main features of these two banking products (do not include ```md):";
        }

        public void AddEnumValuesToCombo<T>(ComboBox cbo)
        {
            var values = Enum.GetValues(typeof(T));

            foreach (var value in values)
            {
                var selectedEnum = (T)Enum.Parse(typeof(T), value.ToString());

                cbo.Items.Add(GetEnumMemberAttrValue(selectedEnum.GetType(), selectedEnum));
            }
        }

        private string GetEnumMemberAttrValue(Type enumType, object enumVal)
        {
            var memInfo = enumType.GetMember(enumVal.ToString());
            var attr = memInfo[0].GetCustomAttributes(false).OfType<EnumMemberAttribute>().FirstOrDefault();

            return attr != null ? attr.Value : null;
        }

        private async Task LoadCandidateBanksAsync()
        {
            var response = await _httpClient.GetAsync($"http://localhost:7071/api/GetCandidateBanks?IncludeLastUpdated=true");
            var candidateBanks = new List<CandidateBank>();

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                candidateBanks = JsonConvert.DeserializeObject<List<CandidateBank>>(json);
            }

            if (candidateBanks == null) return;

            listViewBanks.BeginUpdate();
            listViewBanks.Items.Clear();

            foreach (var candidateBank in candidateBanks)
            {
                var listViewItem = new ListViewItem(candidateBank.Name);

                listViewItem.SubItems.Add(candidateBank.LastRefreshedFormatted);
                listViewItem.SubItems.Add(DaysFromNow(candidateBank.LastRefreshed));
                listViewItem.SubItems.Add(candidateBank.LastBankUpdateFormatted);
                listViewItem.SubItems.Add(DaysFromNow(candidateBank.LastBankUpdate));
                listViewItem.SubItems.Add(candidateBank.DisplayOrder.ToString());
                listViewItem.SubItems.Add(candidateBank.Active.ToString());
                listViewItem.SubItems.Add(candidateBank.OpenBankingProductListUrl);
                listViewItem.SubItems.Add(candidateBank.Version);
                listViewItem.SubItems.Add(candidateBank.VersionMin);
                listViewItem.SubItems.Add(candidateBank.LogoURL);

                listViewBanks.Items.Add(listViewItem);
            }

            listViewBanks.EndUpdate();
        }

        private void buttonRefreshCache_Click(object sender, EventArgs e)
        {
            if (listViewBanks.CheckedItems.Count == 0)
                MessageBox.Show(@"Please select at least 1 bank to refresh", @"Bank Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                RefreshCache();
        }

        private void ShowCacheProgress(int value, int maximumValue)
        {


            Application.DoEvents();
        }

        private void AddProgressLine(string message, int value, int maxValue, Color color, bool extraSpace = false)
        {
            progressBarCache.Minimum = 0;
            progressBarCache.Maximum = maxValue;
            progressBarCache.Value = value;

            richTextBoxCachResults.AppendText(message);

            if (richTextBoxCachResults.TextLength >= message.Length)
            {
                richTextBoxCachResults.SelectionStart = richTextBoxCachResults.TextLength - message.Length;
                richTextBoxCachResults.SelectionLength = message.Length;
                richTextBoxCachResults.SelectionColor = color;
            }

            richTextBoxCachResults.AppendText(Environment.NewLine);

            if (extraSpace) richTextBoxCachResults.AppendText(Environment.NewLine);

            richTextBoxCachResults.ScrollToCaret();

            Application.DoEvents();
        }

        private async Task RefreshCache()
        {
            if (listViewBanks.CheckedItems.Count == 0)
                return;

            try
            {
                ShowApiMessage(true);

                richTextBoxCachResults.Text = "";

                var httpConnector = new HttpConnector();

                Stopwatch stopwatchTotal = new();
                stopwatchTotal.Start();

                AddProgressLine($"Started", 0, listViewBanks.CheckedItems.Count, Color.Black);

                int counter = 0;
                int successful = 0;
                int failed = 0;

                foreach (ListViewItem checkedItem in listViewBanks.CheckedItems)
                {
                    counter++;

                    ShowCacheProgress(counter, listViewBanks.CheckedItems.Count);

                    var banks = new List<string>{checkedItem.Text};
                    var json = JsonConvert.SerializeObject(banks);

                    Stopwatch stopwatch = new();
                    stopwatch.Start();

                    var response = await httpConnector.PostAsync($"http://localhost:7071/api/RefreshCache?CacheDirectory={_cacheDirectory}", json);

                    stopwatch.Stop();
                    TimeSpan ts = stopwatch.Elapsed;

                    string elapsedTime = string.Format("{0} seconds", ts.Seconds);

                    if (response.IsSuccessStatusCode)
                    {
                        successful++;
                        AddProgressLine($"{checkedItem.Text} [{elapsedTime}]", counter, listViewBanks.CheckedItems.Count, Color.Green);
                    }
                    else
                    {
                        failed++;
                        var error = await response.Content.ReadAsStringAsync();
                        AddProgressLine($"{error} [{elapsedTime}]", counter, listViewBanks.CheckedItems.Count, Color.Red);
                    }
                }

                stopwatchTotal.Stop();
                TimeSpan tsTotal= stopwatchTotal.Elapsed;

                string elapsedTimeTotal = $"{tsTotal.Minutes} mins {tsTotal.Seconds} secs";

                AddProgressLine($"Finished [{elapsedTimeTotal}]", counter, listViewBanks.CheckedItems.Count, Color.Black);
                AddProgressLine($"{successful} successful", counter, listViewBanks.CheckedItems.Count, Color.Green);
                AddProgressLine($"{failed} failed", counter, listViewBanks.CheckedItems.Count, Color.Red);
                AddProgressLine($"{counter} total", counter, listViewBanks.CheckedItems.Count, Color.Black);

                ShowCacheProgress(0, listViewBanks.CheckedItems.Count);

                ShowApiMessage(false);
            }
            catch (Exception ex)
            {
                AddProgressLine($"Unexpected Error: {ex.Message} {ex.InnerException?.Message}", 0, 0, Color.Red);

                ShowError(ex.Message);

                ShowApiMessage(false);
            }
        }

        private void ShowApiMessage(bool show)
        {
            toolStripStatusLabelMessage.Text = show ? "Calling API..." : "";

            tabControlMain.Enabled = !show;

            richTextBoxError.Visible = false;
        }

        private void buttonGetProducts_Click(object sender, EventArgs e)
        {
            LoadProducts();
        }

        private async Task LoadProducts()
        {
            try
            {
                ShowApiMessage(true);

                ResetControls();

                await GetProductData();

                if (_bankData != null && _bankData?.data.Count > 0)
                {
                    ShowAllProductsInListView();

                    ShowProductCounts();

                    ShowApiMessage(false);
                }
                else
                {
                    ShowApiMessage(false);

                    MessageBox.Show(@$"No local cache file found at {_cacheDirectory} or the directory is incorrect. Please re-generate the cache.", @"No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                ShowApiMessage(false);

                ShowError(ex.Message);
            }
        }

        private void ResetControls()
        {
            listViewFilteredProducts.Items.Clear();
            listViewProductValues.Items.Clear();

            richTextBoxPromptResults.Text = string.Empty;

            buttonCompare.Enabled = false;
        }

        private async Task GetProductData()
        {
            var filter = string.Empty;

            if (comboBoxProductCategory.Text != "ALL")
            {
                filter = $"&ProductCategory={comboBoxProductCategory.Text}";
            }

            var response = await _httpClient.GetAsync($"http://localhost:7071/api/GetProducts?CacheDirectory={_cacheDirectory}{filter}");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                _bankData = JsonConvert.DeserializeObject<BankData>(json);
            }
        }

        private void ShowAllProductsInListView(string? searchString = null)
        {
            var bankCount = 0;
            var productCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            if (searchString != null)
                Settings.Default.LastSearch = searchString;

            listViewProducts.Items.Clear();
            listViewProducts.BeginUpdate();

            richTextBoxData.Clear();

            if (_bankData is { data.Count: > 0 })
            {
                foreach (var bank in _bankData.data)
                {
                    var atLeastOneProductShown = false;

                    foreach (var product in bank.Products)
                    {
                        var show = false;

                        if (string.IsNullOrEmpty(searchString))
                        {
                            show = true;
                        }
                        else
                        {
                            var allData = JsonConvert.SerializeObject(product);

                            if (allData.IndexOf(searchString, StringComparison.Ordinal) != -1)
                            {
                                show = true;
                            }
                        }

                        if (!show)
                            continue;

                        atLeastOneProductShown = true;

                        var listViewItem = new ListViewItem(bank.Name);

                        listViewItem.SubItems.Add(product.listData.Name);
                        listViewItem.SubItems.Add(product.listData.ProductCategory.ToString());
                        listViewItem.SubItems.Add(product.detailData.DepositRates == null || product.detailData.DepositRates.Count == 0 ? "Lending" : "Deposit");
                        listViewItem.SubItems.Add(product.ID);
                        listViewItem.SubItems.Add(bank.LastRefreshed.ToString());

                        listViewItem.Tag = product.ID;

                        listViewProducts.Items.Add(listViewItem);

                        productCount++;
                    }

                    if (atLeastOneProductShown)
                        bankCount++;
                }
            }

            if (listViewProducts.Items.Count > 0)
            {
                listViewProducts.Items[0].Selected = true;
                listViewProducts.FocusedItem = listViewProducts.Items[0];
            }

            listViewProducts.EndUpdate();

            toolStripStatusLabelBankCount.Text = $@"Bank Count: {bankCount}";
            toolStripStatusLabelProductCount.Text = $@"Product Count: {productCount}";

            Cursor.Current = Cursors.WaitCursor;
        }

        private void SaveFormSettings()
        {
            switch (WindowState)
            {
                case FormWindowState.Maximized:
                    Settings.Default.FormMainMaximized = true;
                    break;
                case FormWindowState.Normal:
                    Settings.Default.FormMainLocation = Location;
                    Settings.Default.FormMainSize = Size;
                    Settings.Default.FormMainMaximized = false;
                    break;
            }

            Settings.Default.Save();
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveFormSettings();
        }

        private void SetSavedSettings()
        {
            tabControlMain.SelectedIndex = Settings.Default.LastMainTabIndex;

            if (Settings.Default.SplitterPositionCache != 0)
                splitContainerCache.SplitterDistance = Settings.Default.SplitterPositionCache;

            if (Settings.Default.SplitterPositionProducts != 0)
                splitContainerProducts.SplitterDistance = Settings.Default.SplitterPositionProducts;

            if (Settings.Default.SplitterPositionProductDetails != 0)
                splitContainerProductDetails.SplitterDistance = Settings.Default.SplitterPositionProductDetails;

            if (Settings.Default.SplitterPositionProductProperties != 0)
                splitContainerProductProperties.SplitterDistance = Settings.Default.SplitterPositionProductProperties;

            if (Settings.Default.SplitterPositionProductCounts != 0)
                splitContainerProductCounts.SplitterDistance = Settings.Default.SplitterPositionProductCounts;

            if (Settings.Default.SplitterPositionProductComparison != 0)
                splitContainerProductComparison.SplitterDistance = Settings.Default.SplitterPositionProductComparison;

            if (string.IsNullOrEmpty(Settings.Default.CacheDirectory) && Directory.Exists(_cacheDirectoryDefault))
            {
                _cacheDirectory = _cacheDirectoryDefault;
            }
            else
            {
                _cacheDirectory = Settings.Default.CacheDirectory;
            }

            Settings.Default.CacheDirectory = _cacheDirectory;

            textBoxSearchProducts.Text = Settings.Default.LastSearch;

            checkBoxShowCompareResults.Checked = Settings.Default.ShowCompareResults;
        }

        private void SetFormSize()
        {
            // Set the form size based on the saved settings
            if (Settings.Default.FormMainLocation.IsEmpty) return;

            if (Settings.Default.FormMainMaximized)
            {
                WindowState = FormWindowState.Maximized;
            }
            else
            {
                Location = Settings.Default.FormMainLocation;
                Size = Settings.Default.FormMainSize;
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void ShowProductJson()
        {
            if (_bankData != null && (listViewProducts.Items.Count == 0 || _bankData.data.Count == 0))
                return;

            if (listViewProducts.SelectedItems.Count == 0)
            {
                richTextBoxData.Text = "";
                return;
            }

            // The Tag contains the ID
            var key = listViewProducts.SelectedItems[0].Tag;

            _selectedProduct = null;

            if (_bankData?.data != null)
            {
                foreach (var bank in _bankData?.data)
                {
                    foreach (var product in bank.Products)
                    {
                        if (product.ID == key)
                        {
                            _selectedProduct = product;
                            richTextBoxData.Text = JsonConvert.SerializeObject(product, Formatting.Indented);
                        }

                    }
                }
            }

            HighlightSearchString();
        }

        private void HighlightSearchString()
        {
            var startIndex = 0;

            if (textBoxSearchProducts.Text.Length > 0)
                startIndex = FindMyText(textBoxSearchProducts.Text.Trim(), _searchStartIndex,
                    richTextBoxData.Text.Length);

            if (startIndex < 0)
                return;

            richTextBoxData.SelectionBackColor = Color.Yellow;

            var endIndex = textBoxSearchProducts.Text.Length;

            richTextBoxData.Select(startIndex, endIndex);
            _searchStartIndex = startIndex + endIndex;
            richTextBoxData.ScrollToCaret();
        }

        public int FindMyText(string txtToSearch, int searchStart, int searchEnd)
        {
            if (searchStart > 0 && searchEnd > 0 && _searchIndexOfText >= 0)
                richTextBoxData.Undo();

            var retVal = -1;

            if (searchStart < 0 || _searchIndexOfText < 0)
                return retVal;

            if (searchEnd <= searchStart && searchEnd != -1)
                return retVal;

            _searchIndexOfText = richTextBoxData.Find(txtToSearch, searchStart, searchEnd, RichTextBoxFinds.None);

            if (_searchIndexOfText != -1)
                retVal = _searchIndexOfText;

            return retVal;
        }

        private void tabControlMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            Settings.Default.LastMainTabIndex = tabControlMain.SelectedIndex;
        }

        private void buttonSearchProducts_Click(object sender, EventArgs e)
        {
            ShowAllProductsInListView(textBoxSearchProducts.Text);
        }

        private void linkLabelResetProducts_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ShowAllProductsInListView();
        }

        private void richTextBoxData_TextChanged(object sender, EventArgs e)
        {
            _searchStartIndex = 0;
            _searchIndexOfText = 0;
        }

        private void textBoxSearchProducts_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ShowAllProductsInListView(textBoxSearchProducts.Text);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowAboutForm();
        }

        private void ShowAboutForm()
        {
            // Show the About Form as a dialogue
            var formAbout = new FormAbout();

            if (formAbout.ShowDialog(this) == DialogResult.OK)
                formAbout.Dispose();
        }

        private void toolStripButtonSettings_Click(object sender, EventArgs e)
        {
            ShowSettingsForm();
        }

        private void ShowSettingsForm()
        {
            // Initialise the settings form with the saved values
            var formSettings = new FormSettings
            {
                textBoxCacheDirectory = { Text = Settings.Default.CacheDirectory },
            };

            if (formSettings.ShowDialog(this) == DialogResult.OK)
            {
                // The user has closed the form and requested the settings to be saved
                _cacheDirectory = formSettings.textBoxCacheDirectory.Text;
                Settings.Default.CacheDirectory = formSettings.textBoxCacheDirectory.Text;
            }

            formSettings.Dispose();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowSettingsForm();
        }

        private void linkLabelOpenCacheFolder_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (!Directory.Exists(_cacheDirectory))
                MessageBox.Show($@"Directory ""{_cacheDirectory}"" does not exist!", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
                Process.Start("explorer.exe", _cacheDirectory);
        }

        private void listViewMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowProductJson();
        }

        private void ShowProductPropertyValuesTermDeposits()
        {
            if (string.IsNullOrEmpty(_selectedProduct.ID))
                return;

            listViewProductValues.Items.Clear();
            listViewProductValues.BeginUpdate();

            if (_bankDataFilteredTermDeposits?.TermDeposits != null)
            {
                foreach (var filteredTermDeposit in _bankDataFilteredTermDeposits.TermDeposits)
                {
                    if (filteredTermDeposit.ID != _selectedProduct.ID)
                        continue;

                    var allTermRates = string.Empty;

                    foreach (var termRate in filteredTermDeposit.TermRates)
                    {
                        var termRates = $"{termRate.Term} at {termRate.Percent} for {termRate.Percent}";

                        if (string.IsNullOrEmpty(allTermRates))
                            allTermRates = termRates;
                        else
                            allTermRates = $"{allTermRates}, {termRates}";
                    }

                    AddProductPropertyValue(listViewProductValues, "Category", filteredTermDeposit.Category);
                    AddProductPropertyValue(listViewProductValues, "Name", filteredTermDeposit.ProductName);
                    AddProductPropertyValue(listViewProductValues, "Description", filteredTermDeposit.Description);
                    AddProductPropertyValue(listViewProductValues, "ID", filteredTermDeposit.ID);
                    AddProductPropertyValue(listViewProductValues, "Highest Rate", filteredTermDeposit.HighestRateFormatted);
                    AddProductPropertyValue(listViewProductValues, "All rates", allTermRates);
                    AddProductPropertyValue(listViewProductValues, "Minimum Deposit", filteredTermDeposit.MinimumDeposit);
                    AddProductPropertyValue(listViewProductValues, "Maximum Deposit", filteredTermDeposit.MaximumDeposit);
                    AddProductPropertyValue(listViewProductValues, "Min-Max Deposit", filteredTermDeposit.MinimumToMaximumDepositFormatted);
                    AddProductPropertyValue(listViewProductValues, "Apply URL", filteredTermDeposit.ApplyUrl);
                    AddProductPropertyValue(listViewProductValues, "Last Refreshed", filteredTermDeposit.LastRefreshed.ToString());
                }
            }

            listViewProductValues.EndUpdate();
        }

        private void AddProductPropertyValue(ListView listView, string name, string? value)
        {
            var listViewItem = new ListViewItem(name);
            listViewItem.SubItems.Add(value);
            listView.Items.Add(listViewItem);
        }

        private void timerMain_Tick(object sender, EventArgs e)
        {
            if (listViewProducts.Items.Count > 0 || LoadAttempts == MaxLoadAttempts)
            {
                timerMain.Enabled = false;

                return;
            }

            LoadAttempts = LoadAttempts + 1;
            LoadCandidateBanksAsync();
            LoadProducts();
        }

        private void LoadSearchDropdowns()
        {
            comboBoxProductCategory.Items.Add("ALL");
            AddEnumValuesToCombo<BankingProductCategory>(comboBoxProductCategory);

            comboBoxProductCategory.SelectedIndex = Settings.Default.LastProductCategoryIndex;

            checkedDepositListBoxTerm.Items.Add("1 month");
            checkedDepositListBoxTerm.Items.Add("2 months");
            checkedDepositListBoxTerm.Items.Add("3 months", true);
            checkedDepositListBoxTerm.Items.Add("4 months");
            checkedDepositListBoxTerm.Items.Add("5 months");
            checkedDepositListBoxTerm.Items.Add("6 months");
            checkedDepositListBoxTerm.Items.Add("7 months");
            checkedDepositListBoxTerm.Items.Add("8 months");
            checkedDepositListBoxTerm.Items.Add("9 months");
            checkedDepositListBoxTerm.Items.Add("10 months");
            checkedDepositListBoxTerm.Items.Add("11 months");
            checkedDepositListBoxTerm.Items.Add("12 months");
            checkedDepositListBoxTerm.Items.Add("13 months");
            checkedDepositListBoxTerm.Items.Add("14 months");
            checkedDepositListBoxTerm.Items.Add("15 months");
            checkedDepositListBoxTerm.Items.Add("16 months");
            checkedDepositListBoxTerm.Items.Add("17 months");
            checkedDepositListBoxTerm.Items.Add("18 months");
            checkedDepositListBoxTerm.Items.Add("19 months");
            checkedDepositListBoxTerm.Items.Add("20 months");
            checkedDepositListBoxTerm.Items.Add("21 months");
            checkedDepositListBoxTerm.Items.Add("22 months");
            checkedDepositListBoxTerm.Items.Add("23 months");
            checkedDepositListBoxTerm.Items.Add("24 months");
            checkedDepositListBoxTerm.Items.Add("36 months");
            checkedDepositListBoxTerm.Items.Add("48 months");
            checkedDepositListBoxTerm.Items.Add("60 months");
        }

        private void listViewFilteredProducts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewFilteredProducts.SelectedItems.Count == 0)
                return;

            if (_bankData?.data == null)
                return;

            var key = listViewFilteredProducts.SelectedItems[0].Tag;

            // Select in main list
            listViewProducts.SelectedItems.Clear();

            for (var i = 0; i < listViewProducts.Items.Count; i++)
            {
                if (listViewProducts.Items[i].Tag.ToString() != key.ToString())
                    continue;

                listViewProducts.Items[i].Selected = true;
                listViewProducts.EnsureVisible(i);

                ShowProductJson();

                break;
            }

            // Show detail
            if (comboBoxProductCategory.Text == "TERM_DEPOSITS")
                ShowProductPropertyValuesTermDeposits();
        }

        private void buttonFilterProducts_Click(object sender, EventArgs e)
        {
            ResetControls();

            if (comboBoxProductCategory.Text == "TERM_DEPOSITS")
                GetFilteredFilterTermDeposits();
        }

        private async Task GetFilteredFilterTermDeposits()
        {
            try
            {
                ShowApiMessage(true);

                await GetDataTermDeposits();

                if (_bankDataFilteredTermDeposits?.TermDeposits?.Count > 0)
                {
                    LoadListViewTermDeposits();

                    ShowApiMessage(false);
                }
                else
                {
                    ShowApiMessage(false);
                }
            }
            catch (Exception ex)
            {
                ShowApiMessage(false);

                ShowError(ex.Message);
            }
        }

        private async Task GetDataTermDeposits()
        {
            var terms = new List<string>();

            foreach (var checkedItem in checkedDepositListBoxTerm.CheckedItems)
            {
                terms.Add(checkedItem.ToString());
            }

            var filterJson = JsonConvert.SerializeObject(new FilterTermDeposits(null, comboBoxProductCategory.Text == _all ? null : comboBoxProductCategory.Text, terms, textBoxDepositDepositAmount.Text, "1"));

            var content = new StringContent(filterJson, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"http://localhost:7071/api/FilterTermDeposits?CacheDirectory={_cacheDirectory}", content);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                _bankDataFilteredTermDeposits = JsonConvert.DeserializeObject<BankDataFilteredTermDeposits>(json);
            }
            else
            {
                ShowError(response.ReasonPhrase);
            }
        }

        private void LoadListViewTermDeposits()
        {
            Cursor.Current = Cursors.WaitCursor;

            listViewProductValues.Items.Clear();
            listViewFilteredProducts.Items.Clear();
            listViewFilteredProducts.BeginUpdate();

            if (_bankDataFilteredTermDeposits is { TermDeposits.Count: > 0 })
            {
                foreach (var product in _bankDataFilteredTermDeposits.TermDeposits)
                {
                    var listViewItem = new ListViewItem(product.BankName);

                    listViewItem.SubItems.Add(product.ProductName);
                    listViewItem.SubItems.Add(product.ID);

                    listViewItem.Tag = product.ID;

                    listViewFilteredProducts.Items.Add(listViewItem);
                }
            }

            // Highlight first two to compare
            if (_bankDataFilteredTermDeposits?.TermDeposits.Count >= 1)
            {
                listViewFilteredProducts.Items[0].Checked = true;

                if (_bankDataFilteredTermDeposits?.TermDeposits.Count >= 2)
                    listViewFilteredProducts.Items[1].Checked = true;
            }

            EnableCompareButton();

            if (listViewFilteredProducts.Items.Count > 0)
            {
                richTextBoxData.Clear();

                listViewFilteredProducts.Items[0].Selected = true;
                listViewFilteredProducts.FocusedItem = listViewFilteredProducts.Items[0];
            }

            listViewFilteredProducts.EndUpdate();

            labelBankFilterCount.Text = $@"Bank Count: {_bankDataFilteredTermDeposits?.TermDeposits.Select(x => x.BankName).Distinct().Count()}";
            labelProductFilterCount.Text = $@"Product Count: {_bankDataFilteredTermDeposits?.TermDeposits.Count}";

            Cursor.Current = Cursors.WaitCursor;
        }

        private void EnableCompareButton()
        {
            var twoSelected = listViewFilteredProducts.CheckedItems.Count == 2;

            buttonCompare.Enabled = twoSelected;
            richTextBoxPrompt.Enabled = twoSelected;
            richTextBoxPromptResults.Enabled = twoSelected;
        }

        private void splitContainerCache_SplitterMoved(object sender, SplitterEventArgs e)
        {
            if (_formLoaded && splitContainerCache.SplitterDistance != 0)
                Settings.Default.SplitterPositionCache = splitContainerCache.SplitterDistance;
        }

        private void splitContainerProducts_SplitterMoved(object sender, SplitterEventArgs e)
        {
            if (_formLoaded && splitContainerProducts.SplitterDistance != 0)
                Settings.Default.SplitterPositionProducts = splitContainerProducts.SplitterDistance;
        }

        private void splitContainerProductValues_SplitterMoved(object sender, SplitterEventArgs e)
        {
            if (_formLoaded && splitContainerProductDetails.SplitterDistance != 0)
                Settings.Default.SplitterPositionProductDetails = splitContainerProductDetails.SplitterDistance;
        }

        private void splitContainerProductProperties_SplitterMoved(object sender, SplitterEventArgs e)
        {
            if (_formLoaded && splitContainerProductProperties.SplitterDistance != 0)
                Settings.Default.SplitterPositionProductProperties = splitContainerProductProperties.SplitterDistance;
        }

        private void splitContainerProductCounts_SplitterMoved(object sender, SplitterEventArgs e)
        {
            if (_formLoaded && splitContainerProductCounts.SplitterDistance != 0)
                Settings.Default.SplitterPositionProductCounts = splitContainerProductCounts.SplitterDistance;
        }

        private void splitContainerProductComparison_SplitterMoved(object sender, SplitterEventArgs e)
        {
            if (_formLoaded && splitContainerProductComparison.SplitterDistance != 0)
                Settings.Default.SplitterPositionProductComparison = splitContainerProductComparison.SplitterDistance;
        }

        private void ShowProductCounts()
        {
            if (_bankData == null)
                return;

            listViewProductCounts.Items.Clear();
            listViewProductCounts.BeginUpdate();

            foreach (var bank in _bankData.data)
            {
                if (bank.Products.Count == 0)
                    continue;

                AddEnumValuesToListView<BankingProductCategory>(listViewProductCounts, bank.Name, bank.Products);

                var listViewItem = new ListViewItem($"     {bank.Name} products");
                listViewItem.SubItems.Add(bank.Products.Count.ToString());
                listViewProductCounts.Items.Add(listViewItem);
            }

            listViewProductCounts.EndUpdate();
        }

        public void AddEnumValuesToListView<T>(ListView listView, string bank, List<Product> products)
        {
            var values = Enum.GetValues(typeof(T));

            foreach (var value in values)
            {
                var selectedEnum = (T)Enum.Parse(typeof(T), value.ToString());

                var selectedEnumWithUnderscores = GetEnumMemberAttrValue(selectedEnum.GetType(), selectedEnum);

                var listViewItem = new ListViewItem($"{bank} {selectedEnumWithUnderscores}");

                listViewItem.SubItems.Add($"{products.Count(x => x.listData.ProductCategory.ToString() == selectedEnum.ToString())}");

                listViewProductCounts.Items.Add(listViewItem);
            }
        }

        private void ShowError(string message)
        {
            if (listViewProducts.Items.Count == 0 && LoadAttempts != MaxLoadAttempts)
                return; // Don't show the error if the API is starting up

            richTextBoxError.Text = message;
            richTextBoxError.Visible = true;

            //MessageBox.Show($@"Error Message: {message}", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void buttonCompare_Click(object sender, EventArgs e)
        {
            CompareProducts();
        }

        private async Task CompareProducts()
        {
            try
            {
                ShowApiMessage(true);

                var endPoint = string.Empty;
                var requestJson = string.Empty;

                //if (comboBoxProductCategory.Text == "TRANS_AND_SAVINGS_ACCOUNTS")
                //{
                //    var product1 = _bankDataFilteredTransactionAndSavings?.TransactionAndSavings[listViewFilteredProducts.CheckedItems[0].Index];
                //    var product2 = _bankDataFilteredTransactionAndSavings?.TransactionAndSavings[listViewFilteredProducts.CheckedItems[1].Index];
                //    var products = new List<TransactionAndSavings?> { product1, product2 };

                //    requestJson = JsonConvert.SerializeObject(products);
                //}
                if (comboBoxProductCategory.Text == "TERM_DEPOSITS")
                {
                    var product1 = _bankDataFilteredTermDeposits?.TermDeposits[listViewFilteredProducts.CheckedItems[0].Index];
                    var product2 = _bankDataFilteredTermDeposits?.TermDeposits[listViewFilteredProducts.CheckedItems[1].Index];
                    var products = new List<TermDeposit?> { product1, product2 };

                    requestJson = JsonConvert.SerializeObject(products);
                }

                var httpConnector = new HttpConnector();

                var response = await httpConnector.PostAsync($"http://localhost:7071/api/CompareProducts?CacheDirectory={_cacheDirectory}&ProductCategory={comboBoxProductCategory.Text}&Prompt={richTextBoxPrompt.Text}", requestJson);

                var responseText = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    richTextBoxPromptResults.Text = responseText;

                    ShowApiMessage(false);

                    if (checkBoxShowCompareResults.Checked)
                    {
                        var path = AppDomain.CurrentDomain.BaseDirectory + @"\" + _comparisonFile;
                        System.IO.File.WriteAllText(path, responseText);

                        var p = new Process
                        {
                            StartInfo = new ProcessStartInfo(path)
                            {
                                UseShellExecute = true
                            }
                        };

                        p.Start();
                    }
                }
                else
                {
                    ShowApiMessage(false);

                    if (response.ReasonPhrase != null)
                        ShowError($"{response.ReasonPhrase}: {responseText}");
                }
            }
            catch (Exception ex)
            {
                ShowApiMessage(false);

                ShowError(ex.Message);
            }
        }

        private void listViewFilteredProducts_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            EnableCompareButton();
        }

        private void linkLabelOpenFolder_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("explorer.exe", AppDomain.CurrentDomain.BaseDirectory);
        }

        private void linkLabelSelectAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SelectCachesToRefresh(true);
        }

        private void linkLabelSelectNone_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SelectCachesToRefresh(false);
        }

        private void SelectCachesToRefresh(bool check)
        {
            if (listViewBanks.Items.Count == 0)
                return;

            listViewBanks.BeginUpdate();

            foreach (ListViewItem item in listViewBanks.Items)
                item.Checked = check;

            listViewBanks.EndUpdate();
        }

        private void checkBoxShowCompareResults_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.ShowCompareResults = checkBoxShowCompareResults.Checked;
        }

        private void comboBoxProductCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            panelTermDeposits.Visible = false;

            if (comboBoxProductCategory.Text == "TERM_DEPOSITS")
            {
                panelTermDeposits.Visible = true;
            }

            Settings.Default.LastProductCategoryIndex = comboBoxProductCategory.SelectedIndex;

            if (_formLoaded)
                LoadProducts();
        }

        private void linkLabelTermsAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SelectTerms(true);
        }

        private void linkLabelTermNone_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SelectTerms(false);
        }

        private void SelectTerms(bool check)
        {
            for (int i = 0; i < checkedDepositListBoxTerm.Items.Count; i++)
            {
                checkedDepositListBoxTerm.SetItemChecked(i, check);
            }
        }

        private void buttonDeleteCache_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dialogResult == DialogResult.Yes)
            {
                DeleteCacheAsync();
            }
        }

        private async Task DeleteCacheAsync()
        {
            try
            {
                ShowApiMessage(true);

                richTextBoxCachResults.Text = "";

                var httpConnector = new HttpConnector();

                var response = await httpConnector.PostAsync($"http://localhost:7071/api/DeleteCache?CacheDirectory={_cacheDirectory}", "");

                if (!response.IsSuccessStatusCode)
                {
                    ShowApiMessage(false);

                    var error = await response.Content.ReadAsStringAsync();

                    ShowError(error);
                }

                ShowApiMessage(false);
            }
            catch (Exception ex)
            {
                ShowApiMessage(false);

                ShowError(ex.Message);
            }
        }

        private void linkLabelReloadCandidateBanks_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LoadCandidateBanksAsync();
        }

        private async Task TestUserAdd()
        {
            var randomNumber = DateTime.Now.ToString("yyyyMMddHHmmss");
            var user = new User(firstName: "John", lastName: $"Smith {randomNumber}", password: $"password{randomNumber}", email: $"john{randomNumber}@somewhere.com", mobile: $"040100000{randomNumber}", active: true, joinedOn: DateTime.Now);
            var userJson = JsonConvert.SerializeObject(user);
            var content = new StringContent(userJson, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"http://localhost:7071/api/AddUser", content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();

                ShowError(error);
            }
        }

        private async Task TestUsersGet()
        {
            var response = await _httpClient.GetAsync($"http://localhost:7071/api/GetUsers");
            var users = new List<User>();

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                users = JsonConvert.DeserializeObject<List<User>>(json);
            }

            if (users == null) return;

            var userList = string.Empty;

            foreach (var user in users)
            {
                userList = $"{user.FirstName} {user.LastName}, {userList}";
            }

            MessageBox.Show(userList);
        }

        private static string DaysFromNow(DateTime? date)
        {
            if (date == null)
                return string.Empty;

            var days = (DateTime.Now - date.Value).Days;

            return days.ToString();
        }

        private void listViewBanks_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (lvwColumnSorter == null)
                return;

            if (e.Column == lvwColumnSorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (lvwColumnSorter.Order == SortOrder.Ascending)
                {
                    lvwColumnSorter.Order = SortOrder.Descending;
                }
                else
                {
                    lvwColumnSorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                lvwColumnSorter.SortColumn = e.Column;
                lvwColumnSorter.Order = SortOrder.Ascending;
            }

            listViewBanks.Sort();
        }

        private void buttonTestAddUser_Click(object sender, EventArgs e)
        {
            TestUserAdd();
        }

        private void buttonTestGetUsers_Click(object sender, EventArgs e)
        {
            TestUsersGet();
        }
    }
}