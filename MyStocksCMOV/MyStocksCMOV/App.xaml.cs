﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MyStocksCMOV.Views;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace MyStocksCMOV {
				public partial class App : Application {


								public App()
								{
												MainPage = new MainPage();
								}

								protected override void OnStart()
								{
												// Handle when your app starts
								}

								protected override void OnSleep()
								{
												// Handle when your app sleeps
								}

								protected override void OnResume()
								{
												// Handle when your app resumes
								}
				}
}
