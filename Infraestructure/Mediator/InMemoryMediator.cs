using Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Infraestructure.Mediator
{
    public class InMemoryMediator : IMediator
    {
        private readonly IServiceProvider _serviceProvider;

        public InMemoryMediator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task Send<TCommand>(TCommand command) where TCommand : ICommand
        {
            // Resolve o handler para o tipo de comando específico
            var handler = _serviceProvider.GetService<ICommandHandler<TCommand>>();

            if (handler == null)
            {
                throw new InvalidOperationException($"No handler found for command {typeof(TCommand).Name}");
            }

            await handler.Handle(command);
        }

        public async Task<TResult> Send<TCommand, TResult>(TCommand command) where TCommand : ICommand<TResult>
        {
            // Resolve o handler para o tipo de comando e tipo de retorno específicos
            var handler = _serviceProvider.GetService<ICommandHandler<TCommand, TResult>>();

            if (handler == null)
            {
                throw new InvalidOperationException($"No handler found for command {typeof(TCommand).Name} with result {typeof(TResult).Name}");
            }

            return await handler.Handle(command);
        }

    }
}
