using MediatR;

namespace SL_CQRS.CQRS.Restaurante.Commands.Add
{
    public class AddRestauranteHandler : IRequestHandler<AddRestauranteCommand , ML.Result>
    {

        private readonly DL.RestaurantedbcoreContext _context;

        public AddRestauranteHandler(DL.RestaurantedbcoreContext context) => _context = context;

        public async Task<ML.Result> Handle(AddRestauranteCommand request , CancellationToken cancellationToken) 
        {
        
            var result = new ML.Result();

            try
            {

                var Restaurante = new DL.Restaurante() { 
                    
                    Nombre = request.Nombre,
                    Descripcion = request.Descripcion,
                    Slogan = request.Slogan,
                };

                _context.Restaurantes.Add(Restaurante);
                var filasAfectadas = await _context.SaveChangesAsync(cancellationToken);

                if (filasAfectadas > 0)
                {

                    result.Correct = true;
                }
                else {
                    result.Correct = false;
                    result.ErrorMessage = "Ocurrio un error al registrar restaurante";
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
