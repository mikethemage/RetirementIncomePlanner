using System;
using System.Collections.Generic;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.SKCharts;
using RetirementIncomePlannerLibrary;

namespace RetirementIncomePlannerTestHarness
{
    internal class Program
    {
        static void Main(string[] args)
        {            
            IncomePlannerModel incomePlanner = new IncomePlannerModel();

            incomePlanner.PotMethod = PotMethodEnum.Individual;
            incomePlanner.ClientList[0].CurrentAge.Text = "56";
            incomePlanner.ClientList[0].EmploymentList[0].Salary.Text = "16,800";
            incomePlanner.ClientList[0].EmploymentList[0].RetirementAge.Text = "60";
            incomePlanner.ClientList[0].PensionList[0].PensionAmount.Text = "9,600";
            incomePlanner.ClientList[0].PensionList[0].PensionAge.Text = "60";
            incomePlanner.ClientList[0].PensionList[1].PensionAmount.Text = "10,490";
            incomePlanner.ClientList[0].PensionList[1].PensionAge.Text = "60";
            incomePlanner.ClientList[0].IndividualPot.PotAmount.Text = "60,000";
            incomePlanner.ClientList[0].IndividualPot.InvestmentGrowth.Text = "0.03";
            incomePlanner.ClientList[0].RetirementIncomeLevel.Text = "25,000";

            DataForGraph dataForGraph = incomePlanner.GetData();

            List<ISeries> seriesList = new List<ISeries>();

            for (int i = 0; i < dataForGraph.seriesCount; i++)
            {
                Console.WriteLine(dataForGraph.seriesHeaders[i]);
                if (dataForGraph.seriesHeaders[i] == "Total Fund Value")
                {
                    seriesList.Add(new LineSeries<decimal> { Values = dataForGraph.seriesValues[i].ToArray() });
                }
                else
                {
                    seriesList.Add(new StackedColumnSeries<decimal> { Values = dataForGraph.seriesValues[i].ToArray() });
                }
            }
            var cartesianChart = new SKCartesianChart
            {
                Width = 900,
                Height = 600,
                Series = seriesList
            };
            cartesianChart.SaveImage("cartesianChart.png");            

        }
    }
}
