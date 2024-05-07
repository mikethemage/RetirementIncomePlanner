using LiveChartsCore.Drawing;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.SKCharts;
using LiveChartsCore.SkiaSharpView.VisualElements;
using SkiaSharp;
using System.Numerics;
using System.Reflection;
using System.Text;
using LiveChartsCore;
using System.Globalization;
using RetirementIncomePlannerLogic.InputModels;
using CustomChartLegendSample;

namespace RetirementIncomePlannerLogic
{
    public class PensionCalcs
    {
        private static readonly CultureInfo _culture = CultureInfo.CreateSpecificCulture("en-GB");
        //private static readonly NumberStyles numberStyle = NumberStyles.Number | NumberStyles.AllowCurrencySymbol;        

        public static SKTypeface GetTypeface(string fullFontName)
        {
            SKTypeface result;
            var assembly = Assembly.GetExecutingAssembly();
            var stream = assembly.GetManifestResourceStream("RetirementIncomePlannerLogic.Fonts." + fullFontName + ".ttf");
            result = SKTypeface.FromStream(stream);
            return result;
        }

        public static void BuildReportAndSaveToFile(DataInputModel inputModel, ChartModel chartModel, string FileName)
        {
            // create the document
            using var stream = SKFileWStream.OpenStream(FileName);

            if (stream == null)
            {
                throw new IOException($"Error saving to file: {FileName}");
            }
            BuildReportFromStream(inputModel, chartModel, stream);
            stream.Dispose(); //probably not needed due to using statement
        }

        public static MemoryStream BuildReportAndReturnStream(DataInputModel inputModel, ChartModel chartModel)
        {
            var stream = new MemoryStream();
            using var wstream = new SKManagedWStream(stream);
            if (stream == null || wstream==null)
            {
                throw new IOException($"Error creating memory stream");
            }
            BuildReportFromStream(inputModel, chartModel, wstream);
            wstream.Flush();
            stream.Position = 0;
            return stream;
        }

        public static Stream ChartImageToStream(ChartModel chartModel)
        {
            ImageSizeModel imageSizeModel = new ImageSizeModel();
            return ChartImageToStream(chartModel, imageSizeModel);
        }

        public static Stream ChartImageToStream(ChartModel chartModel, ImageSizeModel imageSizeModel)
        {            
            // Create an SKPictureRecorder object
            var recorder = new SKPictureRecorder();
            var sourceCanvas = recorder.BeginRecording(new SKRect(0, 0, imageSizeModel.Width,
                imageSizeModel.Height));
            // Draw something on the canvas
            sourceCanvas.Clear(SKColors.White);

            // draw on the canvas ...           
            var cartesianChart = new SKCartesianChart
            {
                Width = imageSizeModel.Width,
                Height = imageSizeModel.Height,

                Series = chartModel.SeriesCollection,

                LegendPosition = LiveChartsCore.Measure.LegendPosition.Bottom,

                Background = SKColors.White,
                XAxes = chartModel.XAxisCollection,
                YAxes = chartModel.YAxisCollection
            };
            cartesianChart.SaveImage(sourceCanvas);
            // End recording and get the SKPicture object
            var picture = recorder.EndRecording();

            var image = SKImage.FromPicture(picture, new SKSizeI { 
                Width = imageSizeModel.Width, 
                Height = imageSizeModel.Height});

            var data = image.Encode(SKEncodedImageFormat.Png, 100);            

            return data.AsStream(true);
        }

        public static void BuildReportFromStream(DataInputModel inputModel, ChartModel chartModel, SKWStream stream)
        {
            const float defaulDPI = 72F;

            const float pageWidthInInches = 8.27F;
            const float pageHeightInInches = 11.69F;

            const float chartHeightInInches = 5.00F;

            const float pageWidthInPixels = pageWidthInInches * defaulDPI; // A4 width in inches
            const float pageHeightInPixels = pageHeightInInches * defaulDPI; // A4 height in inches

            const float sideMarginInInches = 1F;
            const float topMarginInInches = 1F;

            const float scaleFactor = 3F;

            var metadata = new SKDocumentPdfMetadata { RasterDpi = defaulDPI }; // change the DPI to 96

            using var document = SKDocument.CreatePdf(stream, metadata);

            // get the canvas from the page
            // begin a new page with the specified dimensions
            var destCanvas = document.BeginPage(pageWidthInPixels, pageHeightInPixels);

            var boldFont = GetTypeface("OpenSans-Bold"); // SKTypeface.FromFamilyName("Arial", SKFontStyleWeight.Bold, SKFontStyleWidth.Normal, SKFontStyleSlant.Upright);
            var normalFont = GetTypeface("OpenSans-Regular");// SKTypeface.FromFamilyName("Arial", SKFontStyleWeight.Normal, SKFontStyleWidth.Normal, SKFontStyleSlant.Upright);

            SolidColorPaint labelsPaint = new SolidColorPaint { Color = SKColors.Black, SKTypeface = normalFont };	

			chartModel.XAxisCollection[0].LabelsPaint = labelsPaint;
            chartModel.YAxisCollection[0].LabelsPaint = labelsPaint;
            chartModel.YAxisCollection[1].LabelsPaint = labelsPaint;            

            // draw on the canvas ...           
            var cartesianChart = new SKCartesianChart
            {
                Width = (int)((pageWidthInInches - (sideMarginInInches * 2F)) * defaulDPI * scaleFactor),
                Height = (int)(chartHeightInInches * defaulDPI * scaleFactor) - 100,       //scale factor of 3

                Series = chartModel.SeriesCollection,
				LegendPosition = LiveChartsCore.Measure.LegendPosition.Hidden,				

				Background = SKColors.White,
                XAxes = chartModel.XAxisCollection,
                YAxes = chartModel.YAxisCollection
            };


			// Create an SKPictureRecorder object
			var recorder = new SKPictureRecorder();

            // Begin recording
            var sourceCanvas = recorder.BeginRecording(new SKRect(0, 0, (int)((pageWidthInInches - (sideMarginInInches * 2F)) * defaulDPI * scaleFactor),
                (int)(chartHeightInInches * defaulDPI * scaleFactor)));                   //Source canvas has scale factor of 3

            // Draw something on the canvas
            sourceCanvas.Clear(SKColors.White);      
            
            CustomChartLegend.DrawLegend(sourceCanvas, cartesianChart);  //Draw custom legend

            cartesianChart.SaveImage(sourceCanvas);                 //Save chart to source canvas

			

			// End recording and get the SKPicture object
			var picture = recorder.EndRecording();

            //Draw the chart:
            SKMatrix matrix = SKMatrix.CreateScaleTranslation(1F / scaleFactor, 1F / scaleFactor, sideMarginInInches * defaulDPI, (topMarginInInches * defaulDPI) + 20F);  //Create scale matrix to shrink by factor of 3,
                                                                                                                          // Place chart at sidemargin from sides of page, topmargin + 20 pixels from top of page (20 points should be enough for Title to appear above)

            destCanvas.DrawPicture(picture, ref matrix);         //Draw source canvas to dest canvas, applying scale matrix  

            //then draw the title:
            SKPaint chartTitlePaint = new SKPaint
            {
                Color = SKColors.Black,
                Typeface = boldFont,  
                TextAlign = SKTextAlign.Center,
                TextSize = 12.0F
            };

            destCanvas.DrawText("Retirement Income Planner", pageWidthInPixels / 2, topMarginInInches * defaulDPI, chartTitlePaint);  //Draw title above the chart, at 1 inch from top of page

            //Write inputModel here:
            var nextPosition = ((chartHeightInInches + topMarginInInches) * defaulDPI) + 10F;  //Next position is chart height + topmargin + 10 pixels  (are we clippling into chart area by 10 pixels????)
            var leftTextPos = sideMarginInInches * defaulDPI;
            var textLineHeight = 0.12F * defaulDPI;

            SKPaint paint = new()
            {
                Color = SKColors.Black,
                TextSize = 8.0F,
                Typeface = boldFont
            };

            destCanvas.DrawText("Client Data Entry", leftTextPos, nextPosition, paint);
            nextPosition += textLineHeight * 2;

            paint.TextSize = 6.0F;

            var intputDataDisplayWidth = 100F;

            paint.Typeface = normalFont;
            destCanvas.DrawText("Number of years for projection:", leftTextPos, nextPosition, paint);

            destCanvas.DrawText(string.Create(_culture, $"{inputModel.NumberOfYears}"), leftTextPos + intputDataDisplayWidth, nextPosition, paint);

            nextPosition += textLineHeight;

            destCanvas.DrawText("Number of clients:", leftTextPos, nextPosition, paint);

            destCanvas.DrawText($"{inputModel.NumberOfClients}", leftTextPos + intputDataDisplayWidth, nextPosition, paint);

            nextPosition += textLineHeight * 2;

            destCanvas.DrawText("Indexation:", leftTextPos, nextPosition, paint);

            destCanvas.DrawText($"{inputModel.Indexation:P}", leftTextPos + intputDataDisplayWidth, nextPosition, paint);

            nextPosition += textLineHeight;

            destCanvas.DrawText("Retirement Pot:", leftTextPos, nextPosition, paint);

            destCanvas.DrawText(string.Create(_culture, $"{inputModel.RetirementPot:C}"), leftTextPos + intputDataDisplayWidth, nextPosition, paint);

            nextPosition += textLineHeight;

            destCanvas.DrawText("Investment Growth:", leftTextPos, nextPosition, paint);

            destCanvas.DrawText($"{inputModel.InvestmentGrowth:P}", leftTextPos + intputDataDisplayWidth, nextPosition, paint);
            nextPosition += textLineHeight;

            nextPosition += 0.2F * defaulDPI;

            var clientAreaWidth = (pageWidthInPixels - (leftTextPos * 2)) / inputModel.NumberOfClients;

            var clientAreaTop = nextPosition;

            for (int i = 0; i < inputModel.NumberOfClients; i++)
            {
                //Todo: draw box around clients
                nextPosition = clientAreaTop;

                var leftClientTextPos = leftTextPos + (clientAreaWidth * i) + 20F;

                paint.TextSize = 8.0f;
                paint.Typeface = boldFont;

                string clientName = $"Client {inputModel.Clients[i].ClientNumber}";

                if (!string.IsNullOrWhiteSpace(inputModel.Clients[i].ClientName))
                {
                    clientName = inputModel.Clients[i].ClientName!;
                }

                destCanvas.DrawText(clientName, leftClientTextPos, nextPosition, paint);
                
                nextPosition += textLineHeight * 2;
                paint.TextSize = 6.0f;
                paint.Typeface = normalFont;

                destCanvas.DrawText("Age: ", leftClientTextPos, nextPosition, paint);
                destCanvas.DrawText($"{inputModel.Clients[i].Age}", leftClientTextPos + intputDataDisplayWidth, nextPosition, paint);
                nextPosition += textLineHeight;

                if (inputModel.Clients[i].SalaryDetails != null)
                {
                    destCanvas.DrawText("Salary: ", leftClientTextPos, nextPosition, paint);
                    destCanvas.DrawText(string.Create(_culture, $"{inputModel.Clients[i].SalaryDetails!.FullSalaryAmount:C}"), leftClientTextPos + intputDataDisplayWidth, nextPosition, paint);
                    nextPosition += textLineHeight;

                    if (inputModel.Clients[i].SalaryDetails!.PartialRetirementDetails != null)
                    {
                        destCanvas.DrawText("Partial Retirement Age: ", leftClientTextPos, nextPosition, paint);
                        destCanvas.DrawText($"{inputModel.Clients[i].SalaryDetails!.PartialRetirementDetails!.Age}", leftClientTextPos + intputDataDisplayWidth, nextPosition, paint);
                        nextPosition += textLineHeight;

                        destCanvas.DrawText("Partial Retirement Salary: ", leftClientTextPos, nextPosition, paint);
                        destCanvas.DrawText(string.Create(_culture, $"{inputModel.Clients[i].SalaryDetails!.PartialRetirementDetails!.Amount:C}"), leftClientTextPos + intputDataDisplayWidth, nextPosition, paint);
                        nextPosition += textLineHeight;
                    }
                }                

                destCanvas.DrawText("Retirement Age: ", leftClientTextPos, nextPosition, paint);
                destCanvas.DrawText($"{inputModel.Clients[i].RetirementAge}", leftClientTextPos + intputDataDisplayWidth, nextPosition, paint);
                nextPosition += textLineHeight;

                destCanvas.DrawText("State Pension Amount: ", leftClientTextPos, nextPosition, paint);
                destCanvas.DrawText(string.Create(_culture, $"{inputModel.Clients[i].StatePensionAmount:C}"), leftClientTextPos + intputDataDisplayWidth, nextPosition, paint);
                nextPosition += textLineHeight;

                destCanvas.DrawText("State Pension Age: ", leftClientTextPos, nextPosition, paint);
                destCanvas.DrawText($"{inputModel.Clients[i].StatePensionAge}", leftClientTextPos + intputDataDisplayWidth, nextPosition, paint);
                nextPosition += textLineHeight;

                if(inputModel.Clients[i].OtherPensionDetails!=null)
                {
                    destCanvas.DrawText("Other Pensions: ", leftClientTextPos, nextPosition, paint);
                    destCanvas.DrawText(string.Create(_culture, $"{inputModel.Clients[i].OtherPensionDetails!.Amount:C}"), leftClientTextPos + intputDataDisplayWidth, nextPosition, paint);
                    nextPosition += textLineHeight;

                    destCanvas.DrawText("Other Pension Age: ", leftClientTextPos, nextPosition, paint);
                    destCanvas.DrawText($"{inputModel.Clients[i].OtherPensionDetails!.Age}", leftClientTextPos + intputDataDisplayWidth, nextPosition, paint);
                    nextPosition += textLineHeight;
                }

                if (inputModel.Clients[i].OtherIncome != null)
                {
                    destCanvas.DrawText("Other Income: ", leftClientTextPos, nextPosition, paint);
                    destCanvas.DrawText(string.Create(_culture, $"{inputModel.Clients[i].OtherIncome:C}"), leftClientTextPos + intputDataDisplayWidth, nextPosition, paint);
                    nextPosition += textLineHeight;
                }

                destCanvas.DrawText("Retirement Income Level: ", leftClientTextPos, nextPosition, paint);
                destCanvas.DrawText(string.Create(_culture, $"{inputModel.Clients[i].RetirementIncomeLevel:C}"), leftClientTextPos + intputDataDisplayWidth, nextPosition, paint);
                nextPosition += textLineHeight * 2;


                if (inputModel.Clients[i].AdhocTransactions.Count > 0)
                {
                    destCanvas.DrawText("Ad-hoc Contributions/Withdrawals: ", leftClientTextPos, nextPosition, paint);

                    var leftAdhocTextPos = leftClientTextPos + 100F;

                    destCanvas.DrawText("Age", leftAdhocTextPos, nextPosition, paint);
                    destCanvas.DrawText("Amount", leftAdhocTextPos+25F, nextPosition, paint);
                    nextPosition += textLineHeight;

                    foreach (AgeAmountInputModel adhocItem in inputModel.Clients[i].AdhocTransactions)
                    {
                        destCanvas.DrawText($"{adhocItem.Age}", leftAdhocTextPos, nextPosition, paint);

                        destCanvas.DrawText(string.Create(_culture, $"{adhocItem.Amount:C2}"), leftAdhocTextPos+25F, nextPosition, paint);
                        nextPosition += textLineHeight;
                    }
                }               
            }

            //end of text code
            document.EndPage();
            document.Close();            
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

                    
                    if (clientRowToAdd.Age >= clientViewModel.StatePensionAge)
                    {
                        clientRowToAdd.StatePension = clientViewModel.StatePensionAmount * output[i].IndexationMultiplier;
                    }
                    

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
                            (client.StatePension + client.OtherPension + client.Salary + client.OtherIncome);

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
