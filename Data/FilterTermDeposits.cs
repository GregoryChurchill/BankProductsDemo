using System.Collections.Generic;

namespace BankingProductsData;

public class FilterTermDeposits
{
    public FilterTermDeposits(string bankName, string productCategory, List<string> terms, string depositAmount, string sortByIndex)
    {
        BankName = bankName;
        ProductCategory = productCategory;
        Terms = terms;
        DepositAmount = depositAmount;
        SortByIndex = sortByIndex;
    }

    public string BankName { get; set; }
    public string ProductCategory { get; set; }
    public List<string> Terms { get; set; }
    public string DepositAmount { get; set; }
    public string SortByIndex { get; set; }
}