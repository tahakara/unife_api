namespace Buisness.Features.CQRS.Base.Generic.Response.FailResponse
{
    /// <summary>
    /// Represents a failed response for a CQRS operation.
    /// </summary>
    /// <typeparam name="T">The type of the response data.</typeparam>
    public class BaseResponseFail<T> : BaseResponse<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseResponseFail{T}"/> class.
        /// </summary>
        public BaseResponseFail() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseResponseFail{T}"/> class with a message and status code.
        /// </summary>
        /// <param name="message">The failure message.</param>
        /// <param name="statusCode">The HTTP status code.</param>
        public BaseResponseFail(string message, int statusCode) : base(false, message, statusCode)
        {
        }
    }
}