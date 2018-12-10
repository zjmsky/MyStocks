using MyStocksCMOV.Objects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MyStocksCMOV.Models
{
    class StocksViewModel
    {
								public ObservableCollection<StockCell> Stocks { get; set; }

								public StocksViewModel()
								{
												this.Stocks = new ObservableCollection<StockCell>();
								}
				}
}
