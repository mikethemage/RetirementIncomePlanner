using LiveChartsCore.Drawing;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.SKCharts;
using LiveChartsCore.SkiaSharpView.VisualElements;
using SkiaSharp;

namespace RetirementIncomePlannerLogic
{
    public class PensionCalcs
    {

        public static void BuildReport(DataInputModel inputModel, ChartModel chartModel)
        {
            // create the document
            using var stream = SKFileWStream.OpenStream(@"C:\Users\Mike\Documents\TestRIOutputDocument.pdf");
            var defaulDPI = 72F;

            var width = 8.27F * defaulDPI; // A4 width in inches
            var height = 11.69F * defaulDPI; // A4 height in inches

            var metadata = new SKDocumentPdfMetadata { RasterDpi = defaulDPI }; // change the DPI to 96

            using var document = SKDocument.CreatePdf(stream, metadata);

            // get the canvas from the page
            // begin a new page with the specified dimensions
            var destCanvas = document.BeginPage(width, height);

            // draw on the canvas ...           
            var cartesianChart = new SKCartesianChart
            {
                Width = (int)((8.27F - 2F) * defaulDPI * 3F),
                Height = (int)(4.54F * defaulDPI * 3F),

                Series = chartModel.SeriesCollection,
                Title = new LabelVisual
                {
                    Text = "Retirement Income Planner",
                    TextSize = 30,
                    Padding = new Padding(15),
                    Paint = new SolidColorPaint(0xff303030)
                },
                LegendPosition = LiveChartsCore.Measure.LegendPosition.Bottom,

                Background = SKColors.White,
                XAxes = chartModel.XAxisCollection,
                YAxes = chartModel.YAxisCollection
            };

            // Create an SKPictureRecorder object
            var recorder = new SKPictureRecorder();

            // Begin recording
            var canvas = recorder.BeginRecording(new SKRect(0, 0, (int)((8.27F - 2F) * defaulDPI * 3F),
                (int)(4.54F * defaulDPI * 3F)));

            // Draw something on the canvas
            canvas.Clear(SKColors.White);

            cartesianChart.SaveImage(canvas);

            // End recording and get the SKPicture object
            var picture = recorder.EndRecording();

            SKMatrix matrix = SKMatrix.CreateScaleTranslation(1F / 3F, 1F / 3F, 1F * defaulDPI, 1F * defaulDPI);

            destCanvas.DrawPicture(picture, ref matrix);

            var nextPosition = (4.54F + 1F + 1F) * defaulDPI;
            //Write inputModel here:


            document.EndPage();
            document.Close();
            stream.Dispose();
        }


        public static YearRowModel[] RunPensionCalcs(DataInputModel inputViewModel)
        {
            YearRowModel[] output = new YearRowModel[inputViewModel.NumberOfYears + 1];
            for (int i = 0; i < output.Length; i++)
            {
                output[i] = new YearRowModel
                {
                    Year = i
                };

                if (i == 0)
                {
                    output[i].IndexationMultiplier = 1M;
                }
                else
                {
                    output[i].IndexationMultiplier = (1M + inputViewModel.Indexation) * output[i - 1].IndexationMultiplier;
                }

                foreach (var clientViewModel in inputViewModel.Clients)
                {
                    ClientRowModel clientRowToAdd = new ClientRowModel
                    {
                        ClientNumber = clientViewModel.ClientNumber,
                        Age = i + clientViewModel.Age
                    };

                    //if (clientViewModel.StatePensionAmount != null && clientViewModel.StatePensionAge != null)
                    //{
                    if (clientRowToAdd.Age >= clientViewModel.StatePensionAge)
                    {
                        clientRowToAdd.StatePension = clientViewModel.StatePensionAmount * output[i].IndexationMultiplier;
                    }
                    //}

                    if (clientViewModel.OtherPensionDetails != null)
                    {
                        if (clientRowToAdd.Age >= clientViewModel.OtherPensionDetails.Age)
                        {
                            clientRowToAdd.OtherPension = clientViewModel.OtherPensionDetails.Amount * output[i].IndexationMultiplier;
                        }
                    }

                    if (clientViewModel.SalaryDetails != null)
                    {
                        if (clientViewModel.SalaryDetails.PartialRetirementDetails != null)
                        {
                            if (clientRowToAdd.Age < clientViewModel.SalaryDetails.PartialRetirementDetails.Age)
                            {
                                clientRowToAdd.Salary = clientViewModel.SalaryDetails.FullSalaryAmount;
                            }
                            else if (clientRowToAdd.Age < clientViewModel.RetirementAge)
                            {
                                clientRowToAdd.Salary = clientViewModel.SalaryDetails.PartialRetirementDetails.Amount;
                            }
                            else
                            {
                                clientRowToAdd.Salary = 0M;
                            }
                        }
                        else if (clientRowToAdd.Age < clientViewModel.RetirementAge)
                        {
                            clientRowToAdd.Salary = clientViewModel.SalaryDetails.FullSalaryAmount;
                        }
                        else
                        {
                            clientRowToAdd.Salary = 0M;
                        }

                        clientRowToAdd.Salary *= output[i].IndexationMultiplier;
                    }

                    if (clientViewModel.OtherIncome != null)
                    {
                        clientRowToAdd.OtherIncome = (clientViewModel.OtherIncome ?? 0) * output[i].IndexationMultiplier;
                    }

                    foreach (AgeAmountInputModel item in clientViewModel.AdhocTransactions)
                    {
                        if (item.Age == clientRowToAdd.Age)
                        {
                            clientRowToAdd.Contribution = item.Amount;
                        }
                    }


                    output[i].Clients.Add(clientRowToAdd);

                }

                decimal totalRequiredDrawdown = 0M;
                decimal totalClientContributions = 0M;

                foreach (ClientRowModel client in output[i].Clients)
                {
                    ClientInputModel? clientViewModel = inputViewModel.Clients.Where(x => x.ClientNumber == client.ClientNumber).FirstOrDefault();
                    if (clientViewModel != null)
                    {
                        decimal requiredDrawdownForClient = 0M;

                        requiredDrawdownForClient = (clientViewModel.RetirementIncomeLevel * output[i].IndexationMultiplier)
                            -
                            (client.StatePension + client.OtherPension + client.Salary + client.OtherIncome)
                            ;


                        totalRequiredDrawdown += requiredDrawdownForClient;

                        totalClientContributions += client.Contribution;
                    }
                }

                totalRequiredDrawdown = Math.Max(totalRequiredDrawdown, 0);

                output[i].TotalRequiredDrawdown = totalRequiredDrawdown;

                if (i == 0)
                {
                    output[i].FundBeforeDrawdown = inputViewModel.RetirementPot;
                }
                else
                {
                    output[i].FundBeforeDrawdown = output[i - 1].TotalFundValue;
                }

                output[i].FundBeforeDrawdown += totalClientContributions / 2;

                output[i].FundBeforeDrawdown *= (1M + inputViewModel.InvestmentGrowth);

                output[i].FundBeforeDrawdown += totalClientContributions / 2;

                output[i].TotalDrawdown = Math.Min(output[i].TotalRequiredDrawdown, output[i].FundBeforeDrawdown);

                output[i].TotalFundValue = Math.Max(output[i].FundBeforeDrawdown - output[i].TotalDrawdown, 0M);

            }

            return output;
        }
    }
}


