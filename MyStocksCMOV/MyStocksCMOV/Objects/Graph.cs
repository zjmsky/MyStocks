using SkiaSharp;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace MyStocksCMOV.Objects {
				class Graph {



								public const float graphLeftPaddingPercentage = 0.075f;
								public const float graphTopPaddingPercentage = 0.05f;
								public const float graphWidthPercentage = 0.9f;
								public const float graphHeightPercentage = 0.95f;

								public const int NUM_HELPER_LINES = 8;


								public struct GraphValue{
												public Point rightAnchor;
												public string value;

								}

								readonly List<List<double>> lines = new List<List<double>>();
								readonly List<List<SKPoint>> graphLines = new List<List<SKPoint>>();
								readonly List<List<SKPoint>> graphLinesFill = new List<List<SKPoint>>();
								readonly List<List<SKPoint>> graphHelperLines = new List<List<SKPoint>>();
								readonly List<GraphValue> graphValues = new List<GraphValue>();

								public Graph(List<List<double>> lines)
								{
												this.lines = lines;
								}

								//Only for testing purposes
								public Graph()
								{
												List<double> line = new List<double> {
																15, 19,46,21,44, 0,
																48,13,19,11,23,
																50,52,23,54,32,
36,18,48,45,40,
55,48,43,28,43,
13,5,21,52,5,
35,12,22,22,23,
44,35,44,26,18,
33,16,17,52,48,
16,28,32,5,47,
34,34,23,17,12,
28,48,34,36,21,
27,5,55,12,42,
14,42,27,45,5,
25,31,47,25,45,
12,29,33,29,54,
53,33,50,40,32,
38,34,41,21,32,
47,21,40,7,55,
15,19,48,7,33
												};

												List<double> line2 = new List<double> {
																49,36,8,51,36,
50,7,52,25,50, 1,
27,9,32,51,43,
10,14,33,34,21,
38,33,34,53,13,
5,47,46,47,52,
44,22,17,34,12,
21,34,34,17,52,
15,14,50,46,40,
6,34,21,28,33,
49,53,21,30,18,
55,32,15,37,42,
48,24,54,43,49,
9,20,42,34,46,
17,15,28,11,24,
18,48,49,17,22,
14,20,45,25,21,
18,35,14,26,50,
24,22,36,46,9,
49,13,52,37,52
												};

												this.lines.Add(line);
												this.lines.Add(line2);

								}




								public void CalculatePoints(double minHeightPercentage, int width, int height)
								{

												double minValue = double.PositiveInfinity;
												double maxValue = double.NegativeInfinity;
												int numPoints = this.lines[0].Count - 1;


												foreach (List<double> line in this.lines) {
																foreach (double pointY in line) {
																				minValue = Math.Min(pointY, minValue);
																				maxValue = Math.Max(pointY, maxValue);
																}
												}


												double xInc = graphWidthPercentage * width / numPoints;
												double yInc = ( 1 - graphTopPaddingPercentage - minHeightPercentage / 100 ) * height / NUM_HELPER_LINES;
												double scale = ( maxValue - minValue ) / ( (graphHeightPercentage * 100) - minHeightPercentage);
												double currentX = graphLeftPaddingPercentage * width;

												double minY = height - ( minHeightPercentage / 100 * height );

												SKPoint startPoint = new SKPoint((float) currentX, (float) minY);
												SKPoint endPoint = new SKPoint((graphWidthPercentage + graphLeftPaddingPercentage) * width, (float) minY);

												foreach (List<double> line in this.lines) {
																List<SKPoint> graphLine = new List<SKPoint>();
																List<SKPoint> graphLineFill = new List<SKPoint>();
																graphLineFill.Add(startPoint);
																foreach (double pointY in line) {
																				double y = minY - ( ( pointY - minValue ) / scale / 100 * height);

																				double x = currentX;
																				currentX += xInc;

																				SKPoint newPoint = new SKPoint((float) x, (float) y);

																				graphLine.Add(newPoint);
																				graphLineFill.Add(newPoint);

																}
																currentX = graphLeftPaddingPercentage * width;
																this.graphLines.Add(graphLine);
																graphLineFill.Add(endPoint);
																graphLineFill.Add(startPoint);
																this.graphLinesFill.Add(graphLineFill);
												}

												float valueInc = (float) ( maxValue - minValue ) / NUM_HELPER_LINES;

												for (int i = 0; i <= NUM_HELPER_LINES; ++i) {
																double x = graphLeftPaddingPercentage * width + ( width * graphWidthPercentage / NUM_HELPER_LINES * i );
																List<SKPoint> helperLineVertical = new List<SKPoint> {
																				new SKPoint((float) x, (float) minY),
																				new SKPoint((float) x, graphTopPaddingPercentage * height)
																};

																double y = minY - ( (graphHeightPercentage - graphTopPaddingPercentage) * height * ( 1 - minHeightPercentage / 100 ) / NUM_HELPER_LINES * i );
																List<SKPoint> helperLineHorizontal = new List<SKPoint> {
																				new SKPoint((float)currentX, (float) y),
																				new SKPoint((graphLeftPaddingPercentage + graphWidthPercentage) * width, (float) y)
																};

																float value = (float) minValue + valueInc * i;

																graphValues.Add(new GraphValue() {
																				value = value.ToString("0.00"),
																				rightAnchor = new Point(graphLeftPaddingPercentage * width, y)
																});


																this.graphHelperLines.Add(helperLineHorizontal);
																this.graphHelperLines.Add(helperLineVertical);
												}

								}

								public List<List<SKPoint>> GetGraphLines()
								{
												return this.graphLines;
								}

								public List<List<SKPoint>> GetGraphHelperLines()
								{
												return this.graphHelperLines;
								}

								public List<List<SKPoint>> GetGraphLinesFill()
								{
												return this.graphLinesFill;
								}

								public List<GraphValue> GetGraphValues()
								{
												return graphValues;
								}

				}
}
