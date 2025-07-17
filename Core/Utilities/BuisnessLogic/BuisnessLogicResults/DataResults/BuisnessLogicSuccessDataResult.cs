namespace Core.Utilities.BuisnessLogic.BuisnessLogicResults.DataResults
{
    public class BuisnessLogicSuccessDataResult<T> : BuisnessLogicDataResult<T>
    {
        public BuisnessLogicSuccessDataResult(T data, int statusCode, string message) : base(data, true, statusCode, message)
        {
        }

        public BuisnessLogicSuccessDataResult(T data, int statusCode) : base(data, true, statusCode)
        {
        }

        public BuisnessLogicSuccessDataResult(int statusCode, string message) : base(default, true, statusCode, message)
        {
        }

        public BuisnessLogicSuccessDataResult(int statusCode) : base(default, true, statusCode)
        {
        }
    }
}
