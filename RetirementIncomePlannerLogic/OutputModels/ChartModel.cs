using LiveChartsCore;
using LiveChartsCore.Drawing;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.SKCharts;
using LiveChartsCore.SkiaSharpView.VisualElements;
using LiveChartsCore.SkiaSharpView.Drawing;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using LiveChartsCore.Measure;
using LiveChartsCore.Kernel.Sketches;
using System.Globalization;

namespace RetirementIncomePlannerLogic
{    
    public class ChartModel
    {
        private static readonly CultureInfo _culture = CultureInfo.CreateSpecificCulture("en-GB");
        //private static readonly NumberStyles numberStyle = NumberStyles.Number | NumberStyles.AllowCurrencySymbol;

        private static Func<double, string> LabellerGBPCurrency => (double value) => string.Create(_culture, $"{value:C2}");


        public string Title { get; set; } = "Retirement Income Planner";
        public bool IsChartBuilt { get; set; } = false;
        public string LegendPosition { get; } = "Bottom";
        public List<ISeries> SeriesCollection { get; set; } = new List<ISeries>();

        public List<Axis> XAxisCollection { get; set; } =
            new List<Axis>
            {
                new Axis { MinLimit=-0.75, MinStep=1, ForceStepToMin=true }
            };

        public List<Axis> YAxisCollection { get; set; } =
            new List<Axis> {
                new Axis { Position=LiveChartsCore.Measure.AxisPosition.Start, Labeler=LabellerGBPCurrency, MinLimit=-0.75 },
                new Axis { Position=LiveChartsCore.Measure.AxisPosition.End, ShowSeparatorLines=false, Labeler = LabellerGBPCurrency, MinLimit=-0.75 }
            };

        
        public void BuildChart(YearRowModel[] dataForChart)
        {
            PensionChartColorModel colorModel = new PensionChartColorModel();            
            BuildChart(dataForChart, colorModel);
        }

        public void BuildChart(YearRowModel[] dataForChart, PensionChartColorModel colorModel)
        {
            PensionChartSKColorValues pensionChartColors = new PensionChartSKColorValues(colorModel);
            
            SeriesCollection.Clear();

            XAxisCollection[0].MaxLimit = (double)dataForChart.Max(x => x.Year) + 0.75;

            if (!dataForChart.All(x => x.TotalDrawdown == 0))
            {
                SeriesCollection.Add(
                    new StackedColumnSeries<decimal>
                    {
                        Values = dataForChart.Select(x => x.TotalDrawdown).ToArray(),
                        ScalesYAt = 0,
                        Name = "Total Drawdown",
                        Fill = new SolidColorPaint { Color = pensionChartColors.TotalDrawdownColor },
                        XToolTipLabelFormatter = x => string.Create(_culture,$"{x.Context.Series.Name}: {x.PrimaryValue:C2})")
                    }
                    );
            }

            for (int i = 0; i < dataForChart[0].Clients.Count; i++)
            {
                if (!dataForChart.All(x => x.Clients[i].StatePension == 0))
                {
                    SolidColorPaint FillColor;
                    if (dataForChart[0].Clients[i].ClientNumber == 2)
                    {
                        FillColor = new SolidColorPaint { Color = pensionChartColors.StatePensionSecondaryColor };
                    }
                    else
                    {
                        FillColor = new SolidColorPaint { Color = pensionChartColors.StatePensionPrimaryColor };
                    }

                    SeriesCollection.Add(
                    new StackedColumnSeries<decimal>
                    {
                        Values = dataForChart.Select(x => x.Clients[i].StatePension).ToArray(),
                        ScalesYAt = 0,
                        Name = $"Client {dataForChart[0].Clients[i].ClientNumber} State Pension",
                        Fill = FillColor,
                        XToolTipLabelFormatter = x => string.Create(_culture,$"{x.Context.Series.Name}: {x.PrimaryValue:C2}")
                    }
                    );
                }

                if (!dataForChart.All(x => x.Clients[i].OtherPension == 0))
                {
                    SolidColorPaint FillColor;
                    if (dataForChart[0].Clients[i].ClientNumber == 2)
                    {
                        FillColor = new SolidColorPaint { Color = pensionChartColors.OtherPensionSecondaryColor };
                    }
                    else
                    {
                        FillColor = new SolidColorPaint { Color = pensionChartColors.OtherPensionPrimaryColor };
                    }

                    SeriesCollection.Add(
                    new StackedColumnSeries<decimal>
                    {
                        Values = dataForChart.Select(x => x.Clients[i].OtherPension).ToArray(),
                        ScalesYAt = 0,
                        Name = $"Client {dataForChart[0].Clients[i].ClientNumber} Other Pensions",
                        Fill = FillColor,
                        XToolTipLabelFormatter = x => string.Create(_culture, $"{x.Context.Series.Name}: {x.PrimaryValue:C2}")
                    }
                    );
                }

                if (!dataForChart.All(x => x.Clients[i].Salary == 0))
                {
                    SolidColorPaint FillColor;
                    if (dataForChart[0].Clients[i].ClientNumber == 2)
                    {
                        FillColor = new SolidColorPaint { Color = pensionChartColors.SalarySecondaryColor };
                    }
                    else
                    {
                        FillColor = new SolidColorPaint { Color = pensionChartColors.SalaryPrimaryColor };
                    }

                    SeriesCollection.Add(
                    new StackedColumnSeries<decimal>
                    {
                        Values = dataForChart.Select(x => x.Clients[i].Salary).ToArray(),
                        ScalesYAt = 0,
                        Name = $"Client {dataForChart[0].Clients[i].ClientNumber} Salary",
                        Fill = FillColor,
                        XToolTipLabelFormatter = x => string.Create(_culture,$"{x.Context.Series.Name}: {x.PrimaryValue:C2}")
                    }
                    );
                }

                if (!dataForChart.All(x => x.Clients[i].OtherIncome == 0))
                {
                    SolidColorPaint FillColor;
                    if (dataForChart[0].Clients[i].ClientNumber == 2)
                    {
                        FillColor = new SolidColorPaint { Color = pensionChartColors.OtherIncomeSecondaryColor };
                    }
                    else
                    {
                        FillColor = new SolidColorPaint { Color = pensionChartColors.OtherIncomePrimaryColor };
                    }
                    SeriesCollection.Add(
                    new StackedColumnSeries<decimal>
                    {
                        Values = dataForChart.Select(x => x.Clients[i].OtherIncome).ToArray(),
                        ScalesYAt = 0,
                        Name = $"Client {dataForChart[0].Clients[i].ClientNumber} Other Income",
                        Fill = FillColor,                        
                        XToolTipLabelFormatter = x => string.Create(_culture, $"{x.Context.Series.Name}: {x.PrimaryValue:C2}")
                    }
                    );
                }
            }

            if (!dataForChart.All(x => x.TotalFundValue == 0))
            {
                SolidColorPaint FillColor = new SolidColorPaint { Color = pensionChartColors.TotalFundValueColor };

                SeriesCollection.Add(
                    new LineSeries<decimal>
                    {
                        Values = dataForChart.Select(x => x.TotalFundValue).ToArray(),
                        ScalesYAt = 1,
                        Name = "Total Fund Value",
                        Fill = null,

                        GeometrySize = 0.0,
                        GeometryFill = FillColor,
                        Stroke = new SolidColorPaint { Color = pensionChartColors.TotalFundValueColor, StrokeThickness = 4.0F },
                        GeometryStroke = new SolidColorPaint { Color = pensionChartColors.TotalFundValueColor, StrokeThickness = 0.0F },

                        LineSmoothness = 0.0,
                        XToolTipLabelFormatter = x => string.Create(_culture,$"{x.Context.Series.Name}: {x.PrimaryValue:C2}")

                    }
                    );
            }

            IsChartBuilt = true;
        }
    }
}
