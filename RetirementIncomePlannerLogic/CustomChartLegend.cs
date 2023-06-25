using LiveChartsCore;
using LiveChartsCore.Drawing;
using LiveChartsCore.Kernel;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Drawing;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.SKCharts;
using RetirementIncomePlannerLogic;
using SkiaSharp;

namespace CustomChartLegendSample
{
	public class CustomChartLegend
	{
		public static void DrawLegend(SKCanvas canvas, SKCartesianChart chart)
		{
			var legendPaint = new SKPaint()
			{
				TextSize = 18,
				IsAntialias = true,
				TextAlign = SKTextAlign.Left,
				Typeface = PensionCalcs.GetTypeface("OpenSans-Regular")				
			};

			const int legendPadding = 8;

			var _labels = chart.Series.Select(x => x.Name).ToArray();

			var legendMargin = (72 * 3) / 2; //40;

			int circlePadding = 16;

			List<int> rowWidths = new List<int>();
			int currentRowWidth = 0;
			for (var i = 0; i < _labels.Length; i++)
			{
				int nextWidth = (int)legendPaint.MeasureText(_labels[i]) + legendPadding * 4 + circlePadding;
				if (currentRowWidth + nextWidth > chart.Width - (2 * legendMargin))
				{
					rowWidths.Add(currentRowWidth);
					currentRowWidth = nextWidth;
				}
				else
				{
					currentRowWidth += nextWidth;
				}
			}
			rowWidths.Add(currentRowWidth);


			var legendHeight = (rowWidths.Count * legendPadding * 3) + legendPadding * 3;			

			chart.Height -= legendHeight;

			var legendY = chart.Height;

			int currentRow = 0;

			bool useMaxLegendWidth = true;

			var legendX = 0;// legendMargin;

			if(useMaxLegendWidth)
			{
				legendX += ((chart.Width - rowWidths.Max()) / 2);
			}
			else
			{
				legendX+= ((chart.Width - rowWidths[currentRow]) / 2);
			}			 

			for (var i = 0; i < _labels.Length; i++)
			{
				var labelX = legendX;

				legendX += (int)legendPaint.MeasureText(_labels[i]) + legendPadding * 4 + circlePadding;

				if (legendX > (chart.Width - legendMargin))
				{
					currentRow++;
					labelX = 0; // legendMargin;

					if (useMaxLegendWidth)
					{
						labelX += ((chart.Width - rowWidths.Max()) / 2);
					}
					else
					{
						labelX += ((chart.Width - rowWidths[currentRow]) / 2);
					}

					legendX = labelX + (int)legendPaint.MeasureText(_labels[i]) + legendPadding * 4 + circlePadding;

					legendY += legendPadding * 3;
				}

				var labelY = legendY + legendPadding * 3;

				SKColor sKColor;
				if (chart.Series.Where(x => x.Name == _labels[i]).First() is StackedColumnSeries<decimal> lineSeries && lineSeries.Fill != null)
				{
					SolidColorPaint temp = (SolidColorPaint)(lineSeries.Fill);
					if (temp != null)
					{
						sKColor = temp.Color;
					}
					else
					{
						sKColor = SKColors.Black;
					}
				}
				else
				{
					sKColor = SKColors.Black;
				}

				canvas.DrawCircle(labelX, labelY, 7, new SKPaint
				{
					IsAntialias = true,
					Style = SKPaintStyle.Fill,
					Color = sKColor
				});

				canvas.DrawText(_labels[i], labelX + circlePadding, labelY + 5, legendPaint);
			}
		}
	}
}