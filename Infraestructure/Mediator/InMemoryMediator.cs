using Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;// Para IServiceProvider, GetRequiredService
using System.Reflection;// Para Type.MakeGenericType, MethodInfo.Invoke

namespace Infraestructure.Mediator
{
    public class InMemoryMediator : IMediator
    {
        private readonly IServiceProvider _serviceProvider;

        public InMemoryMediator(IServiceProvider serviceProvider)  =>  _serviceProvider = serviceProvider;

        // Método para Comandos sem retorno
        public async Task Send<TCommand>(TCommand command) where TCommand : ICommand
        {
            Type handlerType = typeof(ICommandHandler<>).MakeGenericType(command.GetType());
            // Usa GetRequiredService(Type) para resolver dinamicamente e garantir que não será nulo
            object handler = _serviceProvider.GetRequiredService(handlerType);

            MethodInfo handleMethod = handlerType.GetMethod(nameof(ICommandHandler<TCommand>.Handle));

            if (handleMethod == null)
            {
                throw new InvalidOperationException($"Handle method not found on handler for command {typeof(TCommand).Name}.");
            }

            await (Task)handleMethod.Invoke(handler, new object[] { command });
        }

        // Método para Comandos com retorno
        public async Task<TResult> Send<TCommand, TResult>(TCommand command) where TCommand : ICommand<TResult>
        {
            Type handlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResult));
            // Usa GetRequiredService(Type)
            object handler = _serviceProvider.GetRequiredService(handlerType);

            MethodInfo handleMethod = handlerType.GetMethod(nameof(ICommandHandler<TCommand, TResult>.Handle));

            if (handleMethod == null)
            {
                throw new InvalidOperationException($"Handle method not found on handler for command {typeof(TCommand).Name} with result {typeof(TResult).Name}.");
            }

            return await (Task<TResult>)handleMethod.Invoke(handler, new object[] { command });
        }

        // Método para Queries (sempre com retorno)
        // Implementa o método 'Query' da interface IMediator (nome diferente para evitar conflito)
        public async Task<TResult> Query<TQuery, TResult>(TQuery query) where TQuery : IQuery<TResult>
        {
            Type handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
            // Usa GetRequiredService(Type)
            object handler = _serviceProvider.GetRequiredService(handlerType);

            // O 'nameof' aqui buscará o método 'Handle' da interface IQueryHandler<TQuery, TResult>
            MethodInfo handleMethod = handlerType.GetMethod(nameof(IQueryHandler<TQuery, TResult>.Handle));

            if (handleMethod == null)
            {
                throw new InvalidOperationException($"Handle method not found on handler for query {typeof(TQuery).Name} with result {typeof(TResult).Name}.");
            }

            return await (Task<TResult>)handleMethod.Invoke(handler, new object[] { query });
        }
    }
}
