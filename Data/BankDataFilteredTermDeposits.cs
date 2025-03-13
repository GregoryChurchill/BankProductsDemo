using BankingProductsData.Models;
using System;
using System.Collections.Generic;

namespace BankingProductsData
{
    public class BankDataFilteredTermDeposits
    {
        public List<TermDeposit> TermDeposits;
        public int TermPeriodsSelected;
        public int CountTotal;
        public int CountFilteredTotal;
        public int CountFiltered;
        public List<string> Debug;
    }

    public class TermDeposit
    {
        public string BankName { get; set; }
        public string BankNameDisplay { get; set; }
        public string BankLogoUrl { get; set; }
        public string Category { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string ID { get; set; }
        public List<BankingProductDepositRate> MatchedDepositRates { get; set; }
        public decimal HighestRate { get; set; }
        public string HighestTermFormatted { get; set; }
        public string HighestRateFormatted { get; set; }
        public List<TermRate> TermRates { get; set; }
        public string MinimumDeposit { get; set; }
        public string MaximumDeposit { get; set; }
        public string MinimumToMaximumDepositFormatted { get; set; }
        public string ApplyUrl { get; set; }
        public DateTime LastRefreshed { get; set; }
    }
}
