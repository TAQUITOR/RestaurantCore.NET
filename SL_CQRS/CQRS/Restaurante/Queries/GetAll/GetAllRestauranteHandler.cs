using SL_CQRS.CQRS.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace SL_CQRS.CQRS.Restaurante.Queries.GetAll
{
    public sealed class GetAllRestauranteHandler : IRequestHandler<GetAllRestauranteQuery,ML.Result>
    {

        private readonly DL.RestaurantedbcoreContext _context;
        public GetAllRestauranteHandler(DL.RestaurantedbcoreContext context) => _context = context;


        public async Task<ML.Result> Handle(GetAllRestauranteQuery request , CancellationToken cancellationToken)
        {

            var result = new ML.Result() { Objects = new List<object>() };

            try {

                var response = await _context.Restaurantes.ToListAsync(cancellationToken);

                if (response != null)
                {

                    foreach (var item in response)
                    {

                        ML.Restaurante restaurante = new();

                        restaurante.IdRestaurante = item.IdRestaurante;
                        restaurante.Nombre = item.Nombre;
                        restaurante.Descripcion = item.Descripcion;


                        result.Objects.Add(restaurante);

                    }

                    result.Correct = true;
                }
                else { 
                    result.Correct = false;
                    result.ErrorMessage = "Ocurrio un problema al obeter restaurantes";
                }
            
            }
            catch (Exception ex) 
            {
                result.Correct = false;
                result.ErrorMessage  = ex.Message;
                result.Ex = ex;
            }


            return result;

        }


    }
}
