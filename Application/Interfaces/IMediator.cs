using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IMediator
    {
        Task Send<TCommand>(TCommand command) where TCommand : ICommand;

        Task<TResult> Send<TCommand, TResult>(TCommand command) where TCommand : ICommand<TResult>;
    }
}
