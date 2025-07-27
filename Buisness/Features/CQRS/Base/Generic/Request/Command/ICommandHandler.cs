using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.Features.CQRS.Base.Generic.Request.Command
{
    /// <summary>
    /// Handles a CQRS command of type <typeparamref name="TCommand"/> and returns a response of type <typeparamref name="TResponse"/>.
    /// </summary>
    /// <typeparam name="TCommand">The type of the command to handle.</typeparam>
    /// <typeparam name="TResponse">The type of the response returned by the handler.</typeparam>
    public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
    {
    }

    /// <summary>
    /// Handles a CQRS command of type <typeparamref name="TCommand"/> that does not return a value.
    /// </summary>
    /// <typeparam name="TCommand">The type of the command to handle.</typeparam>
    public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand>
        where TCommand : ICommand
    {
    }
}
