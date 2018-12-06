
using MyStocksCMOV.Objects;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyStocksCMOV.Views {
				[XamlCompilation(XamlCompilationOptions.Compile)]
				public partial class GraphPage : ContentPage {

								public SKColor[] colors = {
												SKColors.Blue,
												SKColors.BurlyWood,
												SKColors.Red,
												SKColors.Orange,
												SKColors.Yellow,
												SKColors.Green,
												SKColors.Pink,
												SKColors.Purple,
												SKColors.MediumVioletRed,
												SKColors.Black
								};

								public GraphPage()
								{
												this.InitializeComponent();
								}

								void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
								{
												SKImageInfo info = args.Info;
												SKSurface surface = args.Surface;
												SKCanvas canvas = surface.Canvas;

												canvas.Clear();


												SKPaint paint = new SKPaint {
																StrokeWidth = 5,
																StrokeCap = SKStrokeCap.Round,
																IsAntialias = true
												};

												Graph graph = new Graph();
												graph.CalculatePoints(50.0, info.Width, info.Height);

												List<List<SKPoint>> graphLines = graph.GetGraphLines();
												List<List<SKPoint>> graphLinesFill = graph.GetGraphLinesFill();
												List<List<SKPoint>> graphHelperLines = graph.GetGraphHelperLines();

												System.Diagnostics.Debug.WriteLine(graphLines.Count);
												System.Diagnostics.Debug.WriteLine(graphLinesFill.Count);


												for (int i = 0; i < graphLines.Count; ++i) {
																paint.Color = colors[i % 10];
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
												paint.Color = SKColors.HotPink;
												foreach (List<SKPoint> graphHelperLine in graphHelperLines)
																canvas.DrawPoints(SKPointMode.Points, graphHelperLine.ToArray(), paint);
								}
				}
}
	