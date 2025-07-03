using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SL_ApiREST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestauranteController : ControllerBase
    {

        private readonly BL.Restaurante _restaurante;

        public RestauranteController(BL.Restaurante restaurante) {

            _restaurante = restaurante;
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult Add([FromBody] ML.Restaurante restaurante) {

            ML.Result resultAdd = _restaurante.Add(restaurante);



            if (resultAdd.Correct)
            {

                return Ok(resultAdd);
            }
            else {
                return BadRequest(resultAdd);
            }


        }

        [HttpPost]
        [Route("Update/{IdRestaurante}")]
        public IActionResult Update([FromBody] ML.Restaurante restaurante, int IdRestaurante) {

            ML.Result resultUpdate = _restaurante.Update(IdRestaurante, restaurante);

            if (resultUpdate.Correct)
            {
                return Ok(resultUpdate);
            }
            else { 
                return BadRequest(resultUpdate);
            }
        
        }


        [HttpDelete]
        [Route("Delete/{IdRestaurante}")]
        public IActionResult Delete(int IdRestaurante) {


            ML.Result resultDelete = _restaurante.Delete(IdRestaurante);
            if (resultDelete.Correct)
            {
                return Ok(resultDelete);
            }
            else { 
                return BadRequest(resultDelete);
            }
        }


        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll() { 
            
            ML.Result resultGetAll = _restaurante.GetAll();
            if (resultGetAll.Correct)
            {
                return Ok(resultGetAll);
            }
            else {
                return BadRequest(resultGetAll);
            }
        }

        [HttpGet]
        [Route("GetById/{IdRestaurante}")]

        public IActionResult GetById(int IdRestaurante) { 
            
            ML.Result resultGetById = _restaurante.GetByid(IdRestaurante);
            if (resultGetById.Correct)
            {
                return Ok(resultGetById);
            }
            else {
                return BadRequest(resultGetById);
            }
        }

    }
}
