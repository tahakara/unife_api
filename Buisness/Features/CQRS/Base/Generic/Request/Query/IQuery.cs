using MediatR;

namespace Buisness.Features.CQRS.Base.Generic.Request.Query
{
    /// <summary>
    /// Represents a CQRS query that returns a response of type <typeparamref name="TResponse"/>.
    /// </summary>
    /// <typeparam name="TResponse">The type of the response returned by the query.</typeparam>
    public interface IQuery<out TResponse> : IRequest<TResponse>
    {
    }
}