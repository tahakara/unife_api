namespace Buisness.Features.CQRS.Base.Generic.Response.FailResponse
{
    /// <summary>
    /// Represents a failed response with error details for a CQRS operation.
    /// </summary>
    /// <typeparam name="T">The type of the response data.</typeparam>
    public class BaseResponseFailWithErrors<T> : BaseResponseFail<T>
    {
        /// <summary>
        /// Gets or sets the list of error messages.
        /// </summary>
        public List<string> Errors { get; set; } = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseResponseFailWithErrors{T}"/> class.
        /// </summary>
        public BaseResponseFailWithErrors() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseResponseFailWithErrors{T}"/> class with a message and status code.
        /// </summary>
        /// <param name="message">The failure message.</param>
        /// <param name="statusCode">The HTTP status code.</param>
        public BaseResponseFailWithErrors(string message, int statusCode) : base(message, statusCode)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseResponseFailWithErrors{T}"/> class with errors, a message, and status code.
        /// </summary>
        /// <param name="errors">A list of error messages.</param>
        /// <param name="message">The failure message.</param>
        /// <param name="statusCode">The HTTP status code.</param>
        public BaseResponseFailWithErrors(List<string>? errors, string message, int statusCode) : base(message, statusCode)
        {

        }
    }
}