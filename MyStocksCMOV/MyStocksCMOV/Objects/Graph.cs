using SkiaSharp;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace MyStocksCMOV.Objects {
				class Graph {
								public const int NUM_HELPER_LINES = 10;

								List<List<Point>> lines = new List<List<Point>>();
								List<List<SKPoint>> graphLines = new List<List<SKPoint>>();
								List<List<SKPoint>> graphLinesFill = new List<List<SKPoint>>();
								List<List<SKPoint>> graphHelperLines = new List<List<SKPoint>>();


								public Graph(List<List<Point>> lines)
								{
												this.lines = lines;
								}

								//Only for testing purposes
								public Graph()
								{
												List<Point> line = new List<Point> {
																new Point(0, 100),
																new Point(0, 120),
																new Point(0, 130),
																new Point(0, 115),
																new Point(0, 105),
																new Point(0, 100),
																new Point(0, 250),
																new Point(0, 200)
												};


												List<Point> line2 = new List<Point> {
																new Point(0, 20),
																new Point(0, 35),
																new Point(0, 30),
																new Point(0, 45),
																new Point(0, 50),
																new Point(0, 25),
																new Point(0, 30),
																new Point(0, 40)
												};

												List<Point> line3 = new List<Point> {
																new Point(0, 350),
																new Point(0, 365),
																new Point(0, 360),
																new Point(0, 385),
																new Point(0, 400),
																new Point(0, 420),
																new Point(0, 405),
																new Point(0, 410)
												};

												lines.Add(line);
												lines.Add(line2);
												lines.Add(line3);


								}



								public void CalculatePoints(double minHeightPercentage, int width, int height)
								{

												double minValue = double.PositiveInfinity;
												double maxValue = double.NegativeInfinity;
												int numPoints = this.lines[0].Count - 1;


												foreach (List<Point> line in this.lines) {
																foreach (Point point in line) {
																				minValue = Math.Min(point.Y, minValue);
																				maxValue = Math.Max(point.Y, maxValue);
																}
												}


												double xInc = 0.95 * width / numPoints;
												double yInc = ( 95 - minHeightPercentage ) / 100 * height / NUM_HELPER_LINES;
												double scale = ( maxValue - minValue ) / ( 95 - minHeightPercentage );
												double currentX = 0.025f * width;

												double minY = height - ( minHeightPercentage / 100 * height );

												SKPoint startPoint = new SKPoint((float)currentX, (float) minY);
												SKPoint endPoint = new SKPoint(0.975f * width, (float) minY);
												

												foreach (List<Point> line in this.lines) {
																List<SKPoint> graphLine = new List<SKPoint>();
																List<SKPoint> graphLineFill = new List<SKPoint>();
																graphLineFill.Add(startPoint);
																foreach (Point point in line) {
																				double y = height - ((point.Y - minValue)/ scale + minHeightPercentage) / 100  * height;
																				double x = currentX;
																				currentX += xInc;

																				SKPoint newPoint = new SKPoint((float) x, (float) y);

																				graphLine.Add(newPoint);
																				graphLineFill.Add(newPoint);

																}
																currentX = 0.025f * width;
																graphLines.Add(graphLine);
																graphLineFill.Add(endPoint);
																graphLineFill.Add(startPoint);
																graphLinesFill.Add(graphLineFill);
												}

								}

								public List<List<SKPoint>> GetGraphLines()
								{
												return graphLines;
								}

								public List<List<SKPoint>> GetGraphHelperLines()
								{
												return graphHelperLines;
								}

								public List<List<SKPoint>> GetGraphLinesFill()
								{
												return graphLinesFill;
								}

				}
}
