using System;
using System.Collections.Generic;
using BankingProductsData.Models;

namespace BankingProductsData
{
    public class BankData
    {
        public List<Bank> data;
    }

    public partial class Bank
    {
        public string Name { get; set; }
        public string NameDisplay { get; set; }
        public string LogoURL { get; set; }
        public DateTime LastRefreshed { get; set; }

        public List<Product> Products;

        public Bank(string Name, string NameDisplay, string LogoURL, DateTime LastRefreshed, List<Product> Products)
        {
            this.Name = Name;
            this.NameDisplay = NameDisplay;
            this.LogoURL = LogoURL;
            this.LastRefreshed = LastRefreshed;
            this.Products = Products;
        }
    }

    public class Product
    {
        public string ID { get; set; }
        public BankingProductV4 listData;
        public BankingProductDetailV4 detailData;

        public Product(string id, BankingProductV4 listData, BankingProductDetailV4 detailData)
        {
            this.ID = id;
            this.listData = listData;
            this.detailData = detailData;
        }
    }
}
