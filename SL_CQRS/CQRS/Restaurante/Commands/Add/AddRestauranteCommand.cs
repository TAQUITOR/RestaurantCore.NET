
using MediatR;


namespace SL_CQRS.CQRS.Restaurante.Commands.Add
{
    public class AddRestauranteCommand : IRequest<ML.Result>
    {
        public string? Nombre { get; set; }
        public string? Slogan { get; set; }
        public string? Descripcion { get; set; }

    }
}
