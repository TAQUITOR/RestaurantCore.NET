using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PL_MVC.Controllers
{
    public class RestauranteController : Controller
    {

        private readonly BL.Restaurante _restaurante;

        public RestauranteController(BL.Restaurante restaurante)
        {

            _restaurante = restaurante;
        }


        public IActionResult Index()
        {
            return View();
        }

        // ================== GET ALL ========================= \\

        [HttpGet]
        public IActionResult GetAll()
        {

            ML.Restaurante restaurante = new ML.Restaurante();

            //ML.Result resultGetAll = _restaurante.GetAll();
            ML.Result resultGetAll = GetAllAPI();

            if (resultGetAll.Correct)
            {

                restaurante.Restaurantes = new List<object>();
                restaurante.Restaurantes = resultGetAll.Objects;

            }


            return View(restaurante);
        }


        // ================================= FORM ===================================== \\
        [HttpGet]
        public IActionResult Form(int? IdRestaurante)
        {

            ML.Restaurante restaurante = new ML.Restaurante();

            // ======== LOAD USER INFO IF EDIT =========== \\
            if (IdRestaurante > 0)
            {

                //ML.Result resultGetById = _restaurante.GetByid(IdRestaurante.Value);
                ML.Result resultGetById = GetByIdAPI(IdRestaurante.Value);
                if (resultGetById.Correct)
                {
                    restaurante = (ML.Restaurante)resultGetById.Object;
                }
            }


            return View(restaurante);
        }


        [HttpPost]
        public IActionResult Form(ML.Restaurante restaurante, IFormFile img)
        {


            // =============== GUARDAR IMAGEN ============================= \\
            if (img != null)
            {
                using (var reader = new StreamReader(img.OpenReadStream()))
                {
                    var fileContent = reader.ReadToEnd();

                    var imagen = img.OpenReadStream();
                    using (var memoryStream = new MemoryStream())
                    {

                        imagen.CopyTo(memoryStream);
                        restaurante.Imagen = memoryStream.ToArray();
                    }

                }
            }


            if (restaurante.IdRestaurante > 0)
            {
                // ==================== UPDATE ==================== \\
                //ML.Result resultUpdate = _restaurante.Update(restaurante.IdRestaurante, restaurante);
                ML.Result resultUpdate = UpdateAPI(restaurante.IdRestaurante, restaurante);

                if (resultUpdate.Correct)
                {
                    return RedirectToAction("GetAll");
                }
            }
            else
            {
                // ================== ADD ============================//
                //ML.Result resultAdd = _restaurante.Add(restaurante);
                ML.Result resultAdd = AddAPI(restaurante);

                if (resultAdd.Correct)
                {
                    return RedirectToAction("GetAll");
                }
            }

            return View(restaurante);
        }


        [HttpGet]
        public IActionResult Delete(int IdRestaurante)
        {

            //ML.Result resultDelete = _restaurante.Delete(IdRestaurante);
            ML.Result resultDelete = DeleteAPI(IdRestaurante);
            if (resultDelete.Correct)
            {
                return RedirectToAction("GetAll");
            }

            return RedirectToAction("GetAll");
        }



        // ============= CONSUMO DE API ===================== \\

        [NonAction]
        public ML.Result AddAPI(ML.Restaurante restaurante)
        {

            ML.Result result = new ML.Result();

            try
            {
                using (var client = new HttpClient())
                {

                    var userEndPoint = "http://localhost:5037/api/Restaurante/";
                    client.BaseAddress = new Uri(userEndPoint);

                    var addTask = client.PostAsJsonAsync("Add", restaurante);
                    addTask.Wait();

                    var resultApi = addTask.Result;

                    if (resultApi.IsSuccessStatusCode)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "Ocurrio un problema al agregar usuario";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }
            return result;
        }
        [NonAction]
        public ML.Result UpdateAPI(int IdRestaurante, ML.Restaurante restaurante)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (var client = new HttpClient())
                {

                    var userEndPoint = "http://localhost:5037/api/Restaurante/";
                    client.BaseAddress = new Uri(userEndPoint);

                    var updateTask = client.PostAsJsonAsync($"Update/ {IdRestaurante}", restaurante);
                    updateTask.Wait();

                    var resultApi = updateTask.Result;

                    if (resultApi.IsSuccessStatusCode)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "Ocurrio un problema al actualizar datos";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }

            return result;
        }
        [NonAction]
        public ML.Result DeleteAPI(int IdRestaurante)
        {

            ML.Result result = new ML.Result();

            try
            {

                using (var client = new HttpClient())
                {

                    var userEndPoint = "http://localhost:5037/api/Restaurante/";
                    client.BaseAddress = new Uri(userEndPoint);

                    var deleteTask = client.DeleteAsync($"Delete/ {IdRestaurante}");
                    deleteTask.Wait();

                    var resultApi = deleteTask.Result;
                    if (resultApi.IsSuccessStatusCode)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "Ocurrio un problema al eliminar usuario";
                    }
                }

            }
            catch (Exception ex)
            {

                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }

            return result;
        }
        [NonAction]
        public ML.Result GetAllAPI()
        {
            ML.Result result = new ML.Result();
            result.Objects = new List<object>();
            try
            {
                using (var client = new HttpClient())
                {

                    var userEndPoint = "http://localhost:5037/api/Restaurante/";
                    client.BaseAddress = new Uri(userEndPoint);

                    var getAllTask = client.GetAsync("GetAll");
                    getAllTask.Wait();

                    var resultApi = getAllTask.Result;
                    if (resultApi.IsSuccessStatusCode)
                    {
                        result.Correct = true;
                        var resultTemp = getAllTask.Result.Content.ReadAsAsync<ML.Result>();

                        foreach (var item in resultTemp.Result.Objects)
                        {

                            ML.Restaurante restauranteTemp = JsonConvert.DeserializeObject<ML.Restaurante>(item.ToString());
                            result.Objects.Add(restauranteTemp);
                        }
                    }
                    else {
                        result.Correct = false;
                        result.ErrorMessage = "Ocurrio un error al obtener restaurantes";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }

            return result;
        }
        [NonAction]
        public ML.Result GetByIdAPI(int IdRestaurante)
        {

            ML.Result result = new ML.Result();

            try
            {

                using (var client = new HttpClient())
                {
                    var userEndPoint = "http://localhost:5037/api/Restaurante/";
                    client.BaseAddress = new Uri(userEndPoint);

                    var getByIdTask = client.GetAsync($"GetById/ {IdRestaurante}");
                    getByIdTask.Wait();

                    var resultApi = getByIdTask.Result;
                    if (resultApi.IsSuccessStatusCode)
                    {
                        result.Correct = true;

                        var resultTemp = getByIdTask.Result.Content.ReadAsAsync<ML.Result>();

                        ML.Restaurante restauranteTemp = JsonConvert.DeserializeObject<ML.Restaurante>(resultTemp.Result.Object.ToString());

                        result.Object = restauranteTemp;
                    }
                    else {
                        result.Correct = false;
                    }

                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }

            return result;
        }
    }
}
