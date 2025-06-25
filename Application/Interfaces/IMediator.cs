namespace Application.Interfaces
{
    public interface IMediator
    {
        Task Send<TCommand>(TCommand command, CancellationToken cancellationToken) where TCommand : ICommand;

        Task<TResult> Send<TCommand, TResult>(TCommand command, CancellationToken cancellationToken) where TCommand : ICommand<TResult>;

        Task<TResult> Query<TQuery, TResult>(TQuery query, CancellationToken cancellationToken) where TQuery : IQuery<TResult>;//Em vez de chamar de Send usamos o Query para não dar conflito
    }
}
