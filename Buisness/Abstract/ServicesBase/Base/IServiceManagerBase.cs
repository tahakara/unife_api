using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.Abstract.ServicesBase.Base
{
    public interface IServiceManagerBase
    {
        // Transaction Management
        Task<TResult> ExecuteInTransactionAsync<TResult>(Func<Task<TResult>> operation);
        Task ExecuteInTransactionAsync(Func<Task> operation);
        
        // Validation
        Task ValidateAsync<T>(T dto) where T : class;
        
        // Audit/Logging
        Task LogOperationAsync(string operation, object? data = null);
        Task LogErrorAsync(string operation, Exception exception, object? data = null);
        
        // Common Business Operations
        Task<bool> ExistsAsync(Guid id);
        Task<bool> IsActiveAsync(Guid id);
    }
}
