using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ObjectStorage.Base
{
    public abstract class ObjectStorageConnectionFactoryBase : IObjectStorageConnectionFactory, IDisposable
    {
        protected readonly IConfiguration _configuration;
        protected readonly ILogger<ObjectStorageConnectionFactoryBase> _logger;
        protected readonly string _connectionStringKey;
        protected readonly string _containerName;

        protected ObjectStorageConnectionFactoryBase(
            IConfiguration configuration, 
            ILogger<ObjectStorageConnectionFactoryBase> logger,
            string connectionStringKey, 
            string containerName)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _connectionStringKey = connectionStringKey ?? throw new ArgumentNullException(nameof(connectionStringKey));
            _containerName = containerName ?? throw new ArgumentNullException(nameof(containerName));
        }

        public virtual async Task<IObjectStorageConnection> CreateConnectionAsync()
        {
            try
            {
                _logger.LogDebug("Creating object storage connection for container: {ContainerName}", _containerName);
                
                var connectionString = GetConnectionString();
                var connection = await CreateConnectionInstanceAsync(connectionString);
                
                _logger.LogDebug("Object storage connection created successfully");
                return connection;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create object storage connection");
                throw;
            }
        }

        public virtual IObjectStorageConnection CreateConnection()
        {
            try
            {
                _logger.LogDebug("Creating object storage connection for container: {ContainerName}", _containerName);
                
                var connectionString = GetConnectionString();
                var connection = CreateConnectionInstance(connectionString);
                
                _logger.LogDebug("Object storage connection created successfully");
                return connection;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create object storage connection");
                throw;
            }
        }

        public virtual string GetConnectionString()
        {
            var connectionString = _configuration.GetConnectionString(_connectionStringKey);
            
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException($"Connection string '{_connectionStringKey}' not found in configuration.");
            }
            
            return connectionString;
        }

        public virtual async Task<bool> TestConnectionAsync()
        {
            try
            {
                _logger.LogDebug("Testing object storage connection");
                
                using var connection = await CreateConnectionAsync();
                var result = await TestConnectionImplementationAsync(connection);
                
                _logger.LogDebug("Connection test result: {Result}", result);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Connection test failed");
                return false;
            }
        }

        // Abstract methods for concrete implementations
        protected abstract IObjectStorageConnection CreateConnectionInstance(string connectionString);
        protected abstract Task<IObjectStorageConnection> CreateConnectionInstanceAsync(string connectionString);
        protected abstract Task<bool> TestConnectionImplementationAsync(IObjectStorageConnection connection);

        protected string GetContainerName() => _containerName;

        public virtual void Dispose()
        {
            _logger.LogDebug("Disposing object storage connection factory");
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _logger.LogDebug("Object storage connection factory disposed");
            }
        }
    }
}
