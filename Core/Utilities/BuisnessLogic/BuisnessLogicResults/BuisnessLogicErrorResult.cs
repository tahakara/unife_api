namespace Core.Utilities.BuisnessLogic.BuisnessLogicResults
{
    public sealed record BuisnessLogicErrorResult : BuisnessLogicResult
    {
        // Mesaj + özel status code
        public BuisnessLogicErrorResult(string message, int statusCode = 400) 
            : base(false, statusCode, message)
        {
        }

        // Sadece özel status code
        public BuisnessLogicErrorResult(int statusCode = 400) 
            : base(false, statusCode)
        {
        }

        // Sadece mesaj (varsayılan 400)
        public BuisnessLogicErrorResult() 
            : base(false, 400)
        {
        }
    }
}
