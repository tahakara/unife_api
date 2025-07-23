using Core.Utilities.BuisnessLogic.BuisnessLogicResults.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.Services.EntityRepositoryServices.Base
{
    public interface IServiceManagerBase
    {
        // Transaction Management
        Task<TResult> ExecuteInTransactionAsync<TResult>(Func<Task<TResult>> operation);
        Task ExecuteInTransactionAsync(Func<Task> operation);

        // Validation
        Task<IBuisnessLogicResult> ValidateAsync<T>(T dto) where T : class;

        // Mapping
        Task<IBuisnessLogicResult> MapToDtoAsync<TSource, TDestination>(TSource source, TDestination target)
            where TSource : class, new()
            where TDestination : class, new();

        // Audit/Logging
        Task LogOperationAsync(string operation, object? data = null);
        Task LogWarningAsync(string operation, string message, object? data = null);
        Task LogDebugAsync(string operation, object? data = null);
        Task LogErrorAsync(string operation, Exception exception, object? data = null);
        
        // Common Business Operations
        Task<bool> ExistsAsync(Guid id);
        Task<bool> IsActiveAsync(Guid id);
    }
}
