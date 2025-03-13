using System;

namespace BankingProductsData;

public partial class CandidateBank
{
    public CandidateBank(string name, string nameDisplay, string openBankingBaseUrl, string logoUrl, bool useLogoFile, string minVersion, string version, int displayOrder, bool active)
    {
        Name = name;
        NameDisplay = nameDisplay;
        OpenBankingBaseUrl = openBankingBaseUrl;
        OpenBankingProductListUrl = $"{openBankingBaseUrl}/cds-au/v1/banking/products";
        LogoURL = logoUrl;
        UseLogoFile = useLogoFile;
        Version = version;
        VersionMin = minVersion;
        DisplayOrder = displayOrder;
        Active = active;
    }

    public bool Refresh { get; set; }
    public string Name { get; set; }
    public string NameDisplay { get; set; }
    public string OpenBankingBaseUrl { get; set; }
    public string OpenBankingProductListUrl { get; set; }
    public DateTime? LastRefreshed { get; set; }
    public string LastRefreshedFormatted { get; set; }
    public DateTime? LastBankUpdate { get; set; }
    public string LastBankUpdateFormatted { get; set; }
    public string LogoURL { get; set; }
    public bool UseLogoFile { get; set; }
    public string Version { get; set; }
    public string VersionMin { get; set; }
    public int DisplayOrder { get; set; }
    public bool Active { get; set; }
}