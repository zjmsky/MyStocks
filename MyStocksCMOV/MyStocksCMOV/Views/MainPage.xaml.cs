﻿using System;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;

namespace MyStocksCMOV.Views {
				[XamlCompilation(XamlCompilationOptions.Compile)]
				public partial class MainPage : Xamarin.Forms.TabbedPage {
								public MainPage()
								{
												InitializeComponent();

												On<Xamarin.Forms.PlatformConfiguration.Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
								}
				}
}