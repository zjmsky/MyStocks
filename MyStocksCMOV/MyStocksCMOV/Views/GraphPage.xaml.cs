
using MyStocksCMOV.Models;
using MyStocksCMOV.Objects;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyStocksCMOV.Views {
				[XamlCompilation(XamlCompilationOptions.Compile)]
				public partial class GraphPage : ContentPage {


								public const float axisExtensionPercentage = 0.02f;
								public const float arrowOffset = 7;
								public const float textScalePercentage = 0.7f;
								public const float textPaddingPercentage = 0.1f;

								private float graphWidthPercentage = Graph.defaultGraphWidthPercentage;
								private float graphMinHeightPercentage = Graph.defaultGraphMinHeightPercentage;


								Graph graph;
								private StocksViewModel stocksViewModel;

								public GraphPage()
								{
												this.InitializeComponent();
												this.graph = new Graph();


												this.stocksViewModel = new StocksViewModel();
												this.GetUserCompanies();
												this.BindingContext = this.stocksViewModel;

								}

								private void GetUserCompanies()
								{
												//Get from local storage later
												List<Stock> companies = new List<Stock>() {
																new Stock("Apple", "APL", SKColors.Blue),
																new Stock("Microsoft", "MCRST", SKColors.Pink),
																new Stock("Google", "GGL", SKColors.Brown)
												};

												companies[0].AddStock(new List<double>() {
																5, 10, 15, 10, 50, 40, 30, 20, 25, 28, 31, 59, 12, 65, 5, 10, 12
												});
												companies[1].AddStock(new List<double>() {
																10, 25, 20, 30, 50, 60, 100, 20, 30, 59, 20, 23, 24, 30, 40, 25, 30
												});
												companies[2].AddStock(new List<double>() {
																0, 15, 35, 10, 20, 50, 30, 40, 50, 40, 40, 40, 45, 25, 28, 32, 35
												});

												bool enable = true;

												foreach (Stock company in companies) {
																graph.activeStocks.Add(company, enable);
																this.stocksViewModel.Stocks.Add(new StockCell(company.companyCode, company.companyName, company.stock, enable));
																enable = false;

												}


								}

								void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
								{
												SKImageInfo info = args.Info;
												SKSurface surface = args.Surface;
												SKCanvas canvas = surface.Canvas;

												canvas.Clear();


												if (info.Width > info.Height)
																this.graphMinHeightPercentage = Graph.graphTopPaddingPercentage;
												else
																this.graphMinHeightPercentage = Graph.defaultGraphMinHeightPercentage;

												DateTime startDate = new DateTime(2018, 11, 1);
												DateTime endDate = new DateTime(2018, 12, 8);

												bool success = this.graph.CalculatePoints(Graph.defaultGraphWidthPercentage, this.graphMinHeightPercentage * 100, info.Width, info.Height, startDate, endDate);


												List<List<SKPoint>> graphLines = this.graph.GetGraphLines();
												List<List<SKPoint>> graphLinesFill = this.graph.GetGraphLinesFill();
												List<List<SKPoint>> graphHelperLines = this.graph.GetGraphHelperLines();

												SKPaint paint = new SKPaint {
																Color = SKColors.LightGray.WithAlpha(0xAA),
																StrokeWidth = 1,
																StrokeCap = SKStrokeCap.Square,
																Style = SKPaintStyle.Stroke,
																IsAntialias = true
												};
												foreach (List<SKPoint> graphHelperLine in graphHelperLines)
																canvas.DrawPoints(SKPointMode.Lines, graphHelperLine.ToArray(), paint);

												paint.StrokeCap = SKStrokeCap.Round;

												for (int i = 0; i < graphLines.Count; ++i) {
																paint.Color = graph.colorList[i];
																paint.Style = SKPaintStyle.Stroke;
																paint.StrokeWidth = 2;
																canvas.DrawPoints(SKPointMode.Polygon, graphLines[i].ToArray(), paint);
																paint.StrokeWidth = 6;
																canvas.DrawPoints(SKPointMode.Points, graphLines[i].ToArray(), paint);
																paint.Color = paint.Color.WithAlpha(0x50);
																paint.Style = SKPaintStyle.Fill;
																SKPath path = new SKPath();
																path.AddPoly(graphLinesFill[i].ToArray());
																canvas.DrawPath(path, paint);
												}

												paint.StrokeWidth = 4;
												paint.Color = SKColors.Black;


												float bottomY = ( 1 - this.graphMinHeightPercentage ) * info.Height;
												float topY = ( Graph.graphTopPaddingPercentage - axisExtensionPercentage ) * info.Height;
												float leftX = Graph.graphLeftPaddingPercentage * info.Width;
												float rightX = ( Graph.graphLeftPaddingPercentage + this.graphWidthPercentage + axisExtensionPercentage ) * info.Width;

												List<SKPoint> horizontalAxis = new List<SKPoint> {

																//Axis
																new SKPoint(leftX, bottomY),
																new SKPoint(rightX, bottomY),


																//Arrow
																new SKPoint(rightX, bottomY),
																new SKPoint(rightX - arrowOffset, bottomY - arrowOffset),

																new SKPoint(rightX, bottomY),
																new SKPoint(rightX - arrowOffset, bottomY + arrowOffset),


												};
												List<SKPoint> verticalAxis = new List<SKPoint> {
																
																//Axis
																new SKPoint(leftX, bottomY),
																new SKPoint(leftX, topY),

																//Arrow

																new SKPoint(leftX, topY),
																new SKPoint(leftX - arrowOffset, topY + arrowOffset),

																new SKPoint(leftX, topY),
																new SKPoint(leftX + arrowOffset, topY + arrowOffset),
												};

												canvas.DrawPoints(SKPointMode.Lines, horizontalAxis.ToArray(), paint);
												canvas.DrawPoints(SKPointMode.Lines, verticalAxis.ToArray(), paint);
												if (success) {
																List<Graph.GraphValue> graphValues = this.graph.GetGraphValues();
																List<Graph.GraphValue> xValues = this.graph.GetXValues();

																SKPaint textPaint = new SKPaint {
																				Color = SKColors.Blue,
																				IsAntialias = true,
																};

																float textWidth = textPaint.MeasureText("22.22");
																textPaint.TextSize = Graph.graphLeftPaddingPercentage * info.Width * textScalePercentage * textPaint.TextSize / textWidth;
																SKRect textBounds = new SKRect();

																foreach (Graph.GraphValue graphValue in graphValues) {
																				textPaint.MeasureText(graphValue.Value, ref textBounds);

																				float xText = info.Width * ( Graph.graphLeftPaddingPercentage + Graph.graphLeftPaddingPercentage * textPaddingPercentage ) / 2 - textBounds.MidX;

																				canvas.DrawText(graphValue.Value, xText, (float) graphValue.anchor.Y - textBounds.MidY, textPaint);
																}

																foreach (Graph.GraphValue xValue in xValues) {
																				textPaint.MeasureText(xValue.Value, ref textBounds);
																				float yText = bottomY + textBounds.Height + 10;
																				canvas.DrawText(xValue.Value, (float) xValue.anchor.X - textBounds.MidX, yText, textPaint);
																}
												}
								}

								private void SwitchCell_OnChanged(object sender, ToggledEventArgs e)
								{
												SwitchCell switchedCell = (SwitchCell) sender;

												string companyCellText = switchedCell.Text;

												foreach (Stock company in graph.activeStocks.Keys) {
																string companyText = company.companyCode + " - " + company.companyName + ": " + company.stock;


																if (companyText.Equals(companyCellText)) {
																				graph.activeStocks[company] = switchedCell.On;
																				break;
																}
												}

												this.canvasView.InvalidateSurface();
								}
				}
}
