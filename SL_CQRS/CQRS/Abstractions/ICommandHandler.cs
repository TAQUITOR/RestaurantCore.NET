namespace SL_CQRS.CQRS.Abstractions
{
    public interface ICommandHandler<TCommand, TResult>
    {

        TResult Handle(TCommand command);

    }


    public interface ICommandHandler<TCommand> { 
        
        void Handle(TCommand command);
    }

}
