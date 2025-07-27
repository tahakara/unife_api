namespace Buisness.Features.CQRS.Base.Generic.Response
{
    /// <summary>
    /// Represents the base response for CQRS operations.
    /// </summary>
    /// <typeparam name="T">The type of the response data.</typeparam>
    public class BaseResponse<T>
    {
        /// <summary>
        /// Gets or sets a value indicating whether the operation was successful.
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Gets or sets the response message.
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the HTTP status code.
        /// </summary>
        public int StatusCode { get; set; } = 200;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseResponse{T}"/> class.
        /// </summary>
        public BaseResponse()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseResponse{T}"/> class with success state, message, and status code.
        /// </summary>
        /// <param name="isSuccess">Indicates if the operation was successful.</param>
        /// <param name="message">The response message.</param>
        /// <param name="statusCode">The HTTP status code.</param>
        public BaseResponse(bool isSuccess, string message, int statusCode)
        {
            IsSuccess = isSuccess;
            Message = message;
            StatusCode = statusCode;
        }

        /// <summary>
        /// Creates a successful response with optional data, message, and status code.
        /// </summary>
        /// <param name="data">The response data.</param>
        /// <param name="message">The success message.</param>
        /// <param name="statusCode">The HTTP status code.</param>
        /// <returns>A <see cref="BaseResponse{T}"/> representing a successful response.</returns>
        public static BaseResponse<T> Success(T? data, string message = "İşlem başarılı", int statusCode = 200)
        {
            if (data != null)
                return new BaseResponseSuccessWithData<T>
                {
                    IsSuccess = true,
                    Data = data,
                    Message = message,
                    StatusCode = statusCode
                };

            return new BaseResponseSuccess<T>
            {
                IsSuccess = true,
                Message = message,
                StatusCode = statusCode
            };
        }

        /// <summary>
        /// Creates a failed response with a message, optional errors, and status code.
        /// </summary>
        /// <param name="message">The failure message.</param>
        /// <param name="errors">A list of error messages.</param>
        /// <param name="statusCode">The HTTP status code.</param>
        /// <returns>A <see cref="BaseResponse{T}"/> representing a failed response.</returns>
        public static BaseResponse<T> Failure(string message, List<string>? errors = null, int statusCode = 400)
        {
            if (errors != null && errors.Count >= 1)
            {
                return new BaseResponseFailWithErrors<T>
                {
                    IsSuccess = false,
                    Message = message,
                    Errors = errors,
                    StatusCode = statusCode
                };
            }

            return new BaseResponseFailWithErrors<T>
            {
                IsSuccess = false,
                Message = message,
                Errors = new List<string> { message },
                StatusCode = statusCode
            };
        }
    }
}