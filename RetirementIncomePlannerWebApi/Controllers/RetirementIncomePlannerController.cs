using Microsoft.AspNetCore.Mvc;
using RetirementIncomePlannerLogic;
using RetirementIncomePlannerLogic.InputModels;
using RetirementIncomePlannerWebApi.Models;
using System.IO;
using System.Net;
using System.Net.Http.Headers;

namespace RetirementIncomePlannerWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RetirementIncomePlannerController : ControllerBase
{
    private readonly ILogger<RetirementIncomePlannerController> _logger;

    public RetirementIncomePlannerController(ILogger<RetirementIncomePlannerController> logger)
    {
        _logger = logger;
    }

    [Route(nameof(GetDefaultColorScheme))]
    [HttpGet]
    [ProducesResponseType(typeof(PensionChartColorModel), 200, "application/json")]
    public IActionResult GetDefaultColorScheme()
    {
        return Ok(new PensionChartColorModel());
    }

    [Route(nameof(RequestOutputData))]
    [HttpPost]
    [ProducesResponseType(typeof(YearRowModel[]), 200, "application/json")]
    [ProducesResponseType(400)]
    public IActionResult RequestOutputData([FromBody] RequestModel requestModel)
    {
        string? errorCheck = CheckInputForErrorsAndFixClientNumbers(requestModel.PensionInputData);
        if (errorCheck != null)
        {
            return BadRequest(errorCheck);
        }       

        return Ok(PensionCalcs.RunPensionCalcs(requestModel.PensionInputData!));
    }

    [Route(nameof(RequestChartImage))]
    [HttpPost]
    [ProducesResponseType(typeof(FileContentResult), 200, "image/png")]
    [ProducesResponseType(400)]
    public IActionResult RequestChartImage([FromBody] RequestWithSizeInputConfigModel requestModel)
    {
        string? errorCheck = CheckInputForErrorsAndFixClientNumbers(requestModel.PensionInputData);
        if (errorCheck != null)
        {
            return BadRequest(errorCheck);
        }
                
        if (requestModel.ImageSize == null)
        {
            requestModel.ImageSize = new ImageSizeModel { Height = 0, Width = 0 };
        }

        PensionChartColorModel pensionChartColorModel = requestModel.PensionChartColors!;
        if (pensionChartColorModel == null)
        {
            pensionChartColorModel = new PensionChartColorModel();
        }
        else
        {
            if(!pensionChartColorModel.ValidateColors())
            {
                return BadRequest("Invalid color selection, colors must be in hex format i.e.: #FFFFFF.");
            }
        }

        YearRowModel[] outputModel = PensionCalcs.RunPensionCalcs(requestModel.PensionInputData!);
        ChartModel chartModel= new ChartModel();
        chartModel.BuildChart(outputModel, pensionChartColorModel);
        var stream = PensionCalcs.ChartImageToStream(chartModel);

        //The framework will dispose of the stream used in this case when the response is completed. If a using statement is used, the stream will be disposed before the response has been sent and result in an exception or corrupt response.
        return File(stream, "image/png");
    }


    [Route(nameof(RequestReportPDF))]
    [HttpPost]
    [ProducesResponseType(typeof(FileContentResult),200, "application/pdf")]
    [ProducesResponseType(400)]
    public IActionResult RequestReportPDF([FromBody] RequestWithInputConfigModel requestModel)
    {
        string? errorCheck = CheckInputForErrorsAndFixClientNumbers(requestModel.PensionInputData);
        if (errorCheck != null)
        {
            return BadRequest(errorCheck);
        }

        PensionChartColorModel pensionChartColorModel = requestModel.PensionChartColors!;
        if (pensionChartColorModel == null)
        {
            pensionChartColorModel = new PensionChartColorModel();
        }
        else
        {
            if (!pensionChartColorModel.ValidateColors())
            {
                return BadRequest("Invalid color selection, colors must be in hex format i.e.: #FFFFFF.");
            }
        }

        YearRowModel[] outputModel = PensionCalcs.RunPensionCalcs(requestModel.PensionInputData!);
        ChartModel chartModel = new ChartModel();
        chartModel.BuildChart(outputModel, pensionChartColorModel);        

        var stream = PensionCalcs.BuildReportAndReturnStream(requestModel.PensionInputData!, chartModel);


        //The framework will dispose of the stream used in this case when the response is completed. If a using statement is used, the stream will be disposed before the response has been sent and result in an exception or corrupt response.
        return File(stream, "application/pdf", "Retirement Income Report.pdf");
    }

    private static string? CheckInputForErrorsAndFixClientNumbers(DataInputModel? inputModel)
    {
        if (inputModel == null)
        {
            return "No data provided.";
        }

        if (inputModel.NumberOfClients < 1 || inputModel.NumberOfClients > 2)
        {
            return $"Invalid number of clients: {inputModel.NumberOfClients}.";
        }

        //Fixup Client numbers if needed:
        for (int i = 1; i <= inputModel.NumberOfClients; i++)
        {
            inputModel.Clients[i - 1].ClientNumber = i;
        }

        return null;
    }

}
