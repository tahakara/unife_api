using Buisness.Abstract.ServicesBase.Base;

namespace Buisness.Abstract.ServicesBase
{
    public interface IUniversityTypeService : IServiceManagerBase
    {
        Task<bool> IsTypeIdExistsAsync(int typeId);
    }
}
