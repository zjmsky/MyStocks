
using MyStocksCMOV.Objects;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyStocksCMOV.Views {
				[XamlCompilation(XamlCompilationOptions.Compile)]
				public partial class GraphPage : ContentPage {

								public const float minHeightPercentage = 0.5f;
								public const float axisExtensionPercentage = 0.02f;
								public const float arrowOffset = 7;
								public const float textScalePercentage = 0.7f;
								public const float textPaddingPercentage = 0.1f;

								public SKColor[] colors = {
												SKColors.Blue,
												SKColors.Chocolate,
												SKColors.Red,
												SKColors.Orange,
												SKColors.Yellow,
												SKColors.Green,
												SKColors.Pink,
												SKColors.Purple,
												SKColors.MediumVioletRed,
												SKColors.Black
								};

								Graph graph;

								public GraphPage()
								{
												this.InitializeComponent();
												graph = new Graph();
												
								}

								void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
								{
												SKImageInfo info = args.Info;
												SKSurface surface = args.Surface;
												SKCanvas canvas = surface.Canvas;

												canvas.Clear();

												float graphMinHeightPercentage = minHeightPercentage;
												if (info.Width > info.Height)
																graphMinHeightPercentage = Graph.graphTopPaddingPercentage;

												graph.CalculatePoints(graphMinHeightPercentage * 100, info.Width, info.Height);


												List<List<SKPoint>> graphLines = graph.GetGraphLines();
												List<List<SKPoint>> graphLinesFill = graph.GetGraphLinesFill();
												List<List<SKPoint>> graphHelperLines = graph.GetGraphHelperLines();

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
																paint.Color = this.colors[i % colors.Length];
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


												float bottomY = (1 - graphMinHeightPercentage) * info.Height;
												float topY = (Graph.graphTopPaddingPercentage - axisExtensionPercentage) * info.Height;
												float leftX = Graph.graphLeftPaddingPercentage * info.Width;
												float rightX = (Graph.graphLeftPaddingPercentage + Graph.graphWidthPercentage + axisExtensionPercentage) * info.Width;

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

												List<Graph.GraphValue> graphValues = graph.GetGraphValues();

												SKPaint textPaint = new SKPaint {
																Color = SKColors.Blue,
																IsAntialias = true,
												};

												float textWidth = textPaint.MeasureText("22.22");
												textPaint.TextSize = Graph.graphLeftPaddingPercentage * info.Width * textScalePercentage * textPaint.TextSize / textWidth;
												SKRect textBounds = new SKRect();
												textPaint.MeasureText("22.22", ref textBounds);

												float xText = info.Width * (Graph.graphLeftPaddingPercentage + Graph.graphLeftPaddingPercentage * textPaddingPercentage )  / 2 - textBounds.MidX;


												foreach (Graph.GraphValue graphValue in graphValues) {
																canvas.DrawText(graphValue.value, xText, (float)graphValue.rightAnchor.Y - textBounds.MidY, textPaint);
												}


								}
				}
}
