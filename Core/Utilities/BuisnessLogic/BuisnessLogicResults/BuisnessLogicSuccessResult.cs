namespace Core.Utilities.BuisnessLogic.BuisnessLogicResults
{
    public sealed record BuisnessLogicSuccessResult : BuisnessLogicResult
    {
        // Mesaj + özel status code
        public BuisnessLogicSuccessResult(string message, int statusCode = 200) 
            : base(true, statusCode, message)
        {
        }

        // Sadece özel status code
        public BuisnessLogicSuccessResult(int statusCode = 200) 
            : base(true, statusCode)
        {
        }

        // Sadece mesaj (varsayılan 200)
        public BuisnessLogicSuccessResult() 
            : base(true, 200)
        {
        }
    }
}
