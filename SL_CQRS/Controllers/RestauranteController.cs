using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SL_CQRS.CQRS.Restaurante.Queries.GetAll;

namespace SL_CQRS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestauranteController : ControllerBase
    {

        private readonly IMediator _mediator;

        public RestauranteController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll() {

            ML.Result result = await _mediator.Send(new GetAllRestauranteQuery());
            if (result.Correct) {
                return Ok(result);
            }
            else {
                return BadRequest(result);
            }

        }



    }
}
