using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestApi.Models;

namespace TestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdditionController : ControllerBase
    {
        private readonly ILogger<AdditionController> _logger;

        public AdditionController(ILogger<AdditionController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "NoParameters")]
        [ProducesResponseType(200, Type = typeof(AdditionResult))]
        public AdditionResult Get()
        {
            return new AdditionResult { Result = 42 };
        }

        [HttpGet("{operand1}/{operand2}", Name = "QueryStringParameters")]
        [ProducesResponseType(200, Type = typeof(AdditionResult))]
        [ProducesResponseType(400)]
        public IActionResult GetFromQueryString(int operand1, int operand2)
        {
            return Ok(new AdditionResult
                { Result = operand1 + operand2 }
            );
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(AdditionResult))]
        [ProducesResponseType(400)]
        public IActionResult GetFromBody([FromBody] AdditionParameters? additionParameters)
        {
            if (additionParameters == null)
            {
                return BadRequest();
            }
            else
            {
                return Ok(
                    new AdditionResult
                    { Result = additionParameters.Operand1 + additionParameters.Operand2 }
                    );
            }
            
        }


    }
}
