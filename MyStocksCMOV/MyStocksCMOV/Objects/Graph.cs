using SkiaSharp;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace MyStocksCMOV.Objects {
				class Graph {

								public enum TimeDisplay {
												day, month, year
								}

								public const float graphLeftPaddingPercentage = 0.075f;
								public const float graphTopPaddingPercentage = 0.05f;
								public const float defaultGraphWidthPercentage = 0.9f;
								public const float defaultGraphMinHeightPercentage = 0.2f;
								public const float graphHeightPercentage = 0.95f;

								public const int NUM_HELPER_LINES = 8;
								private TimeDisplay timeDisplay;
								public Dictionary<Stock, bool> activeStocks = new Dictionary<Stock, bool>();


								public struct GraphValue {
												public Point anchor;

												public string Value { get; set; }
								}

								public readonly List<SKColor> colorList = new List<SKColor>();
								List<List<double>> lines = new List<List<double>>();
								List<List<SKPoint>> graphLines = new List<List<SKPoint>>();
								List<List<SKPoint>> graphLinesFill = new List<List<SKPoint>>();
								List<List<SKPoint>> graphHelperLines = new List<List<SKPoint>>();
								List<GraphValue> graphValues = new List<GraphValue>();
								List<GraphValue> xAxisValues = new List<GraphValue>();

								public Graph(List<List<double>> lines)
								{
												this.lines = lines;
								}

								//Only for testing purposes
								public Graph()
								{ 

								}

								private void Clear()
								{
												this.graphLines.Clear();
												this.graphLinesFill.Clear();
												this.graphHelperLines.Clear();
												this.graphValues.Clear();
												this.xAxisValues.Clear();
												this.lines.Clear();
												this.colorList.Clear();
								}


								public bool CalculatePoints(float graphWidthPercentage, double minHeightPercentage, int width, int height, DateTime startDate, DateTime endDate)
								{
												this.Clear();

												foreach (Stock company in activeStocks.Keys)
																if (activeStocks[company]) {
																				colorList.Add(company.color);
																				lines.Add(company.stockHistory);
																}

												if (lines.Count == 0)
																return false;

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
												double scale = ( maxValue - minValue ) / ( ( graphHeightPercentage * 100 ) - minHeightPercentage );
												double currentX = graphLeftPaddingPercentage * width;

												double minY = height - ( minHeightPercentage / 100 * height );

												SKPoint startPoint = new SKPoint((float) currentX, (float) minY);
												SKPoint endPoint = new SKPoint(( graphWidthPercentage + graphLeftPaddingPercentage ) * width, (float) minY);

												foreach (List<double> line in this.lines) {
																List<SKPoint> graphLine = new List<SKPoint>();
																List<SKPoint> graphLineFill = new List<SKPoint>();
																graphLineFill.Add(startPoint);
																foreach (double pointY in line) {
																				double y = minY - ( ( pointY - minValue ) / scale / 100 * height );

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
												string[] xValues = this.CalculateXValues(startDate, endDate);


												for (int i = 0; i <= NUM_HELPER_LINES; ++i) {
																double x = graphLeftPaddingPercentage * width + ( width * graphWidthPercentage / NUM_HELPER_LINES * i );
																List<SKPoint> helperLineVertical = new List<SKPoint> {
																				new SKPoint((float) x, (float) minY),
																				new SKPoint((float) x, graphTopPaddingPercentage * height)
																};

																double y = minY - ( height * ( graphHeightPercentage - minHeightPercentage / 100 ) / NUM_HELPER_LINES * i );
																List<SKPoint> helperLineHorizontal = new List<SKPoint> {
																				new SKPoint((float)currentX, (float) y),
																				new SKPoint((graphLeftPaddingPercentage + graphWidthPercentage) * width, (float) y)
																};

																float value = (float) minValue + valueInc * i;

																this.graphValues.Add(new GraphValue() {
																				Value = value.ToString("0.00"),
																				anchor = new Point(graphLeftPaddingPercentage * width, y)
																});

																if (i != NUM_HELPER_LINES)
																				this.xAxisValues.Add(new GraphValue() {
																								Value = xValues[i],
																								anchor = new Point(x, minY)
																				});


																this.graphHelperLines.Add(helperLineHorizontal);
																this.graphHelperLines.Add(helperLineVertical);

												}
												return true;

								}

								private string[] CalculateXValues(DateTime dateTime1, DateTime dateTime2)
								{
												float timeInterval = (float) ( dateTime2 - dateTime1 ).TotalDays / Graph.NUM_HELPER_LINES;

												string[] values = new string[Graph.NUM_HELPER_LINES];
												DateTime curDate = dateTime1;

												if (timeInterval >= 365)
																timeDisplay = TimeDisplay.year;
												else if (timeInterval >= 30.5)
																timeDisplay = TimeDisplay.month;
												else
																timeDisplay = TimeDisplay.day;

												for (int i = 0; i < Graph.NUM_HELPER_LINES; ++i) {
																if (timeDisplay == TimeDisplay.day)
																				values[i] = curDate.Day + "/" + curDate.Month;
																else if (timeDisplay == TimeDisplay.month)
																				values[i] = curDate.ToString("MMMM");
																else
																				values[i] = curDate.Year.ToString();
																curDate = curDate.AddDays(timeInterval);
												}

												return values;
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
												return this.graphValues;
								}

								public List<GraphValue> GetXValues()
								{
												return this.xAxisValues;
								}

				}
}
