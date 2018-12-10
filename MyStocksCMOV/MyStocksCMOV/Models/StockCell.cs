using System;
using System.Collections.Generic;
using System.Text;

namespace MyStocksCMOV.Objects
{
    class StockCell
    {
								public string CompanyText { get; }
								public string CompanyName { get; }
								public bool On { get; set; }

								public StockCell(string companyCode, string companyName, float stockValue, bool on)
								{
												this.CompanyName = companyName;
												this.CompanyText = companyCode + " - " + companyName + ": " + stockValue;
												this.On = on;
								}
				}
}
