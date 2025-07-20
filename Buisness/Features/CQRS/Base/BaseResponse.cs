namespace Buisness.Features.CQRS.Base
{
    public class BaseResponseFail<T> : BaseResponse<T>
    {
        public BaseResponseFail() : base()
        {
        }

        public BaseResponseFail(string message, int statusCode) : base(false, message, statusCode)
        {
        }
    }
    public class BaseResponseFailWithErrors<T> : BaseResponseFail<T>
    {
        public BaseResponseFailWithErrors() : base()
        {
        }

        public BaseResponseFailWithErrors(string message, int statusCode) : base(message, statusCode)
        {
        }

        public BaseResponseFailWithErrors(List<string>? errors, string message, int statusCode) : base(message, statusCode)
        {

        }
        public List<string> Errors { get; set; } = new();
    }

    public class BaseResponseSuccess<T> : BaseResponse<T>
    {
        public BaseResponseSuccess() : base()
        {
        }

        public BaseResponseSuccess(string message, int statusCode) : base(true, message, statusCode)
        {
        }
    }

    public class BaseResponseSuccessWithData<T> : BaseResponseSuccess<T>
    {
        public BaseResponseSuccessWithData() : base()
        {
        }

        public BaseResponseSuccessWithData(string message, int statusCode) : base(message, statusCode)
        {
        }

        public BaseResponseSuccessWithData(T data, string message, int statusCode) : base(message, statusCode)
        {
            Data = data;
        }
        public T? Data { get; set; }
    }



    public class BaseResponse<T>
    {
        public BaseResponse()
        {
        }

        public BaseResponse(bool isSuccess, string message, int statusCode)
        {
            IsSuccess = isSuccess;
            Message = message;
            StatusCode = statusCode;
        }

        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public int StatusCode { get; set; } = 200;

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