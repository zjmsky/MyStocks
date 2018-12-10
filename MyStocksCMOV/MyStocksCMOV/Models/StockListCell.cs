using System;
using System.Collections.Generic;
using System.Text;

namespace MyStocksCMOV.Models
{
    class StockListCell
    {
        public string CompanyCode { get; }
        public string CompanyName { get; }
        public string CompanyStockValue { get; }
        public string CompanyBalance { get; }
        public bool On { get; set; }

        public StockListCell(string companyCode, string companyName, float stockValue, float balance, bool on)
        {
            this.CompanyName = companyName;
            this.CompanyCode = companyCode;
            this.CompanyStockValue = stockValue.ToString("R");
            this.CompanyBalance = balance.ToString("R");
            this.On = on;
        }
    }
}
