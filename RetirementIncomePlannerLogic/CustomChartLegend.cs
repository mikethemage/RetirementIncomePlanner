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
                Typeface=PensionCalcs.GetTypeface("OpenSans-Regular")
                
            };

            const int legendPadding = 10;

            var _labels = chart.Series.Select(x => x.Name).ToArray();

            var legendHeight = _labels.Length * legendPadding * 2 + legendPadding * 3;
            var legendMargin = 40;
            var chartAreaHeight = chart.Height;
            var legendX = legendMargin;// chart.Width;
            var legendY = chartAreaHeight;



            for (var i = 0; i < _labels.Length; i++)
            {
                var labelX = legendX;// + legendPadding * 2;

                legendX += (int)legendPaint.MeasureText(_labels[i]) + legendPadding * 4 + 20;

                if (legendX > (chart.Width - legendMargin ))
                {
                    labelX = legendMargin;
                    legendX = legendMargin + (int)legendPaint.MeasureText(_labels[i]) + legendPadding * 4 + 20;
                    
                    legendY += legendPadding * 2;
                }

                var labelY = legendY + legendPadding * 2;// + i * legendPadding * 2;

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

                canvas.DrawText(_labels[i], labelX + 20, labelY + 5, legendPaint);
            }
        }
    }
}
