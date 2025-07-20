using Core.Utilities.BuisnessLogic.BuisnessLogicResults.Base;

namespace Core.Utilities.BuisnessLogic.BuisnessLogicResults.DataResults
{
    public record BuisnessLogicDataResult<T> : BuisnessLogicResult, IBuisnessLogicDataResult<T>
    {
        // Tam constructor
        public BuisnessLogicDataResult(T data, bool success, int statusCode, string? message = null) 
            : base(success, statusCode, message)
        {
            Data = data;
        }

        // Varsayılan status code ile
        public BuisnessLogicDataResult(T data, bool success, string? message = null) 
            : base(success, success ? 200 : 400, message)
        {
            Data = data;
        }

        public T Data { get; }
    }
}
