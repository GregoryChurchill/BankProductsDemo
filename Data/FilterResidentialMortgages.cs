namespace BankingProductsData;

public class FilterResidentialMortgages
{
    public FilterResidentialMortgages(string productCategory, double propertyValuation, double loanAmount, double depositAmount, string lendingRateTpe, string repaymentType, string loanPurpose)
    {
        ProductCategory = productCategory;
        PropertyValuation = propertyValuation;
        LoanAmount = loanAmount;
        DepositAmount = depositAmount;
        LendingRateTpe = lendingRateTpe;
        RepaymentType = repaymentType;
        LoanPurpose = loanPurpose;
    }

    public string ProductCategory { get; set; }
    public double PropertyValuation { get; set; }
    public double LoanAmount { get; set; }
    public double DepositAmount { get; set; }
    public string LendingRateTpe { get; set; }
    public string RepaymentType { get; set; }
    public string LoanPurpose { get; set; }

    public double LVR
    {
        get
        {
            var lvr = LoanAmount / PropertyValuation * 100;

            return lvr;
        }
    }
}