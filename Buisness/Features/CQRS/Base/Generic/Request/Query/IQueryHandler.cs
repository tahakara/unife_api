using MediatR;

namespace Buisness.Features.CQRS.Base.Generic.Request.Query
{
    /// <summary>
    /// Handles a CQRS query of type <typeparamref name="TQuery"/> and returns a response of type <typeparamref name="TResponse"/>.
    /// </summary>
    /// <typeparam name="TQuery">The type of the query to handle.</typeparam>
    /// <typeparam name="TResponse">The type of the response returned by the handler.</typeparam>
    public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
        where TQuery : IQuery<TResponse>
    {
    }
}