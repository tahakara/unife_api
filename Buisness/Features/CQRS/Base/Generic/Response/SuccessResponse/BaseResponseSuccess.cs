namespace Buisness.Features.CQRS.Base.Generic.Response.SuccessResponse
{
    /// <summary>
    /// Represents a successful response for a CQRS operation.
    /// </summary>
    /// <typeparam name="T">The type of the response data.</typeparam>
    public class BaseResponseSuccess<T> : BaseResponse<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseResponseSuccess{T}"/> class.
        /// </summary>
        public BaseResponseSuccess() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseResponseSuccess{T}"/> class with a message and status code.
        /// </summary>
        /// <param name="message">The success message.</param>
        /// <param name="statusCode">The HTTP status code.</param>
        public BaseResponseSuccess(string message, int statusCode) : base(true, message, statusCode)
        {
        }
    }
}