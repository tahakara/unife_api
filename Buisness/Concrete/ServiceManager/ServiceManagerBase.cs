using AutoMapper;
using Buisness.Abstract.ServicesBase.Base;
using Core.Utilities.BuisnessLogic.BuisnessLogicResults;
using Core.Utilities.BuisnessLogic.BuisnessLogicResults.Base;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Buisness.Concrete.ServiceManager
{
    public abstract class ServiceManagerBase : IServiceManagerBase
    {
        protected readonly ILogger<ServiceManagerBase> _logger;
        protected readonly IMapper _mapper;
        protected readonly IServiceProvider _serviceProvider;

        protected ServiceManagerBase(IMapper mapper, ILogger<ServiceManagerBase> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _mapper = mapper;
            _serviceProvider = serviceProvider;
        }

        public virtual async Task<IBuisnessLogicResult> ValidateAsync<T>(T dto) where T : class
        {
            _logger.LogDebug("Validation started for {DtoType}", typeof(T).Name);

            if (dto == null)
            {
                return new BuisnessLogicErrorResult("Validation failed: DTO is null", 400);
            }

            try
            {
                var validator = _serviceProvider.GetService<IValidator<T>>();

                if (validator != null)
                {
                    var validationResult = await validator.ValidateAsync(dto);
                    if (!validationResult.IsValid)
                    {
                        var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                        _logger.LogWarning("Validation errors for {DtoType}: {Errors}", typeof(T).Name, errors);
                        return new BuisnessLogicErrorResult($"Validation failed: {errors}", 400);
                    }
                }

                _logger.LogDebug("Validation successful for {DtoType}", typeof(T).Name);
                return new BuisnessLogicSuccessResult("Validation successful", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Validation failed for {DtoType}", typeof(T).Name);
                return new BuisnessLogicErrorResult("Validation failed", 500);
            }
        }

        public virtual async Task<IBuisnessLogicResult> MapToDtoAsync<TSource, TDestination>(TSource source, TDestination target)
            where TSource : class, new()
            where TDestination : class, new()
        {
            try
            {
                _logger.LogDebug("Mapping {SourceType} to {TargetType} started",
                    typeof(TSource).Name, typeof(TDestination).Name);

                if (source == null || target == null)
                {
                    return new BuisnessLogicErrorResult("Mapping failed: null input", 400);
                }

                _mapper.Map(source, target); // AutoMapper: source -> destination

                _logger.LogDebug("Mapping {SourceType} to {TargetType} completed successfully",
                    typeof(TSource).Name, typeof(TDestination).Name);
                return new BuisnessLogicSuccessResult("Mapping successful", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Mapping failed due to exception");
                return new BuisnessLogicErrorResult("Mapping failed", 500);
            }
        }

        public virtual async Task LogOperationAsync(string operation, object? data = null)
        {
            _logger.LogInformation("Operation: {Operation}, Data: {@Data}", operation, data);
            await Task.CompletedTask;
        }

        public virtual async Task LogWarningAsync(string operation, string message, object? data = null)
        {
            _logger.LogWarning("Warning in operation: {Operation}, Message: {Message}, Data: {@Data}", operation, message, data);
            await Task.CompletedTask;
        }

        public virtual async Task LogDebugAsync(string operation, object? data = null)
        {
            _logger.LogDebug("Debugging operation: {Operation}, Data: {@Data}", operation, data);
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

        public virtual async Task<TEntity> UpdateAsync<TEntity>(TEntity entity)
        {
            // Basit implementation, gerçek update sonra eklenecek
            try
            {
                await LogOperationAsync("Update", entity);
                return entity;
            }
            catch (Exception ex)
            {
                await LogErrorAsync("Update", ex, entity);
                throw;
            }
        }
    }
}
