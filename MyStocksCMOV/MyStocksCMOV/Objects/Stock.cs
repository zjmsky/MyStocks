using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyStocksCMOV.Objects
{
    class Stock
    {
								public readonly string companyName;
								public readonly string companyCode;
								public readonly float stock;
								public readonly SKColor color;

								public readonly List<double> stockHistory = new List<double>();

								public Stock(string companyName, string companyCode, SKColor color)
								{
												this.companyName = companyName;
												this.companyCode = companyCode;
												this.color = color;

												stock = 0;
								}

								public void AddStock(double stock)
								{
												stockHistory.Add(stock);
								}

								public void AddStock(List<double> stocks)
								{
												stockHistory.AddRange(stocks);
								}

				}
}
