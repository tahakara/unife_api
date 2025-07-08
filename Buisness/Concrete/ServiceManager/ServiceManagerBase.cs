using Buisness.Abstract.ServicesBase.Base;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Buisness.Concrete.ServiceManager
{
    public abstract class ServiceManagerBase : IServiceManagerBase
    {
        protected readonly ILogger<ServiceManagerBase> _logger;
        protected readonly IServiceProvider _serviceProvider;

        protected ServiceManagerBase(ILogger<ServiceManagerBase> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public virtual async Task ValidateAsync<T>(T dto) where T : class
        {
            var validator = _serviceProvider.GetService<IValidator<T>>();
            if (validator != null)
            {
                var validationResult = await validator.ValidateAsync(dto);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    throw new ValidationException($"Validation failed: {errors}");
                }
            }
        }

        public virtual async Task LogOperationAsync(string operation, object? data = null)
        {
            _logger.LogInformation("Operation: {Operation}, Data: {@Data}", operation, data);
            await Task.CompletedTask;
        }

        public virtual async Task LogErrorAsync(string operation, Exception exception, object? data = null)
        {
            _logger.LogError(exception, "Error in operation: {Operation}, Data: {@Data}", operation, data);
            await Task.CompletedTask;
        }

        public virtual async Task<bool> ExistsAsync(Guid id)
        {
            await Task.CompletedTask;
            throw new NotImplementedException("ExistsAsync must be implemented by derived classes");
        }

        public virtual async Task<bool> IsActiveAsync(Guid id)
        {
            await Task.CompletedTask;
            throw new NotImplementedException("IsActiveAsync must be implemented by derived classes");
        }

        public virtual async Task<TResult> ExecuteInTransactionAsync<TResult>(Func<Task<TResult>> operation)
        {
            // Basit implementation, gerçek transaction sonra eklenecek
            try
            {
                await LogOperationAsync("Transaction.Start");
                var result = await operation();
                await LogOperationAsync("Transaction.Commit");
                return result;
            }
            catch (Exception ex)
            {
                await LogErrorAsync("Transaction.Rollback", ex);
                throw;
            }
        }

        public virtual async Task ExecuteInTransactionAsync(Func<Task> operation)
        {
            // Basit implementation, gerçek transaction sonra eklenecek
            try
            {
                await LogOperationAsync("Transaction.Start");
                await operation();
                await LogOperationAsync("Transaction.Commit");
            }
            catch (Exception ex)
            {
                await LogErrorAsync("Transaction.Rollback", ex);
                throw;
            }
        }
    }
}
