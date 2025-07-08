namespace Core.Utilities.BuisnessLogic.BuisnessLogicResults
{
    public class BuisnessLogicErrorDataResult<T> : BuisnessLogicDataResult<T>
    {
        // Data + mesaj + özel status code
        public BuisnessLogicErrorDataResult(T data, string message, int statusCode = 400) 
            : base(data, false, statusCode, message)
        {
        }

        // Data + özel status code
        public BuisnessLogicErrorDataResult(T data, int statusCode = 400) 
            : base(data, false, statusCode)
        {
        }

        // Mesaj + özel status code (data yok)
        public BuisnessLogicErrorDataResult(string message, int statusCode = 400) 
            : base(default, false, statusCode, message)
        {
        }

        // Sadece özel status code (varsayılan 400)
        public BuisnessLogicErrorDataResult(int statusCode = 400) 
            : base(default, false, statusCode)
        {
        }
    }
}
