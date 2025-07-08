namespace Core.Utilities.BuisnessLogic.BuisnessLogicResults.Base
{
    public interface IBuisnessLogicDataResult<T> : IBuisnessLogicResult
    {
        T Data { get; }
    }
}
