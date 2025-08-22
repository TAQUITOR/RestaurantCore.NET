namespace SL_CQRS.CQRS.Abstractions
{
    public interface IQueryHandler<TQuery, TResult>
    {

        TResult Handle(TQuery query);

    }
}
