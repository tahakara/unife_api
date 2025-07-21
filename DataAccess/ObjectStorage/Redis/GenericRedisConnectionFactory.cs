using Core.ObjectStorage.Base;
using Core.ObjectStorage.Base.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Core.ObjectStorage.Redis
{
    public class GenericRedisConnectionFactory : ObjectStorageConnectionFactoryBase
    {
        private readonly ConcurrentDictionary<RedisStorageType, IConnectionMultiplexer> _connectionMultiplexers;
        private readonly object _lock = new object();
        private readonly RedisStorageType _storageType;

        public GenericRedisConnectionFactory(
            IConfiguration configuration,
            ILogger<ObjectStorageConnectionFactoryBase> logger,
            RedisStorageType storageType = RedisStorageType.Cache)
            : base(configuration, logger, "UnifeObjectStorageConnectionString", GetContainerName(storageType))
        {
            _connectionMultiplexers = new ConcurrentDictionary<RedisStorageType, IConnectionMultiplexer>();
            _storageType = storageType;
        }

        private static string GetContainerName(RedisStorageType storageType)
        {
            return storageType switch
            {
                RedisStorageType.Cache => "unife-cache-container",
                RedisStorageType.Session => "unife-session-container",
                _ => "unife-default-container"
            };
        }

        protected override IObjectStorageConnection CreateConnectionInstance(string connectionString)
        {
            try
            {
                _logger.LogInformation("Creating Redis connection instance for storage type: {StorageType}", _storageType);

                lock (_lock)
                {
                    if (!_connectionMultiplexers.TryGetValue(_storageType, out var connectionMultiplexer) || 
                        !connectionMultiplexer.IsConnected)
                    {
                        var connectionOptions = ParseConnectionString(connectionString);
                        connectionMultiplexer = ConnectionMultiplexer.Connect(connectionOptions);

                        _logger.LogInformation("Redis connection multiplexer created for storage type: {StorageType}", _storageType);

                        // Connection events
                        connectionMultiplexer.ConnectionFailed += OnConnectionFailed;
                        connectionMultiplexer.ConnectionRestored += OnConnectionRestored;
                        connectionMultiplexer.ErrorMessage += OnErrorMessage;

                        _connectionMultiplexers.AddOrUpdate(_storageType, connectionMultiplexer, (key, old) => connectionMultiplexer);
                    }

                    var databaseNumber = RedisStorageConfiguration.GetDatabaseNumber(_storageType);
                    var database = connectionMultiplexer.GetDatabase(databaseNumber);
                    var redisConnection = new RedisObjectStorageConnection(
                        connectionMultiplexer,
                        database,
                        GetContainerName(),
                        _storageType);

                    _logger.LogInformation("Redis object storage connection created for storage type: {StorageType}, Database: {Database}", 
                        _storageType, databaseNumber);
                    return redisConnection;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create Redis connection instance for storage type: {StorageType}", _storageType);
                throw;
            }
        }

        protected override async Task<IObjectStorageConnection> CreateConnectionInstanceAsync(string connectionString)
        {
            try
            {
                _logger.LogInformation("Creating Redis connection instance asynchronously for storage type: {StorageType}", _storageType);

                if (!_connectionMultiplexers.TryGetValue(_storageType, out var connectionMultiplexer) || 
                    !connectionMultiplexer.IsConnected)
                {
                    var connectionOptions = ParseConnectionString(connectionString);
                    connectionMultiplexer = await ConnectionMultiplexer.ConnectAsync(connectionOptions);

                    _logger.LogInformation("Redis connection multiplexer created asynchronously for storage type: {StorageType}", _storageType);

                    // Connection events
                    connectionMultiplexer.ConnectionFailed += OnConnectionFailed;
                    connectionMultiplexer.ConnectionRestored += OnConnectionRestored;
                    connectionMultiplexer.ErrorMessage += OnErrorMessage;

                    _connectionMultiplexers.AddOrUpdate(_storageType, connectionMultiplexer, (key, old) => connectionMultiplexer);
                }

                var databaseNumber = RedisStorageConfiguration.GetDatabaseNumber(_storageType);
                var database = connectionMultiplexer.GetDatabase(databaseNumber);
                var redisConnection = new RedisObjectStorageConnection(
                    connectionMultiplexer,
                    database,
                    GetContainerName(),
                    _storageType);

                _logger.LogInformation("Redis object storage connection created asynchronously for storage type: {StorageType}, Database: {Database}", 
                    _storageType, databaseNumber);
                return redisConnection;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create Redis connection instance asynchronously for storage type: {StorageType}", _storageType);
                throw;
            }
        }

        protected override async Task<bool> TestConnectionImplementationAsync(IObjectStorageConnection connection)
        {
            try
            {
                _logger.LogInformation("Testing Redis connection for storage type: {StorageType}", _storageType);

                if (connection is RedisObjectStorageConnection redisConnection)
                {
                    if (!redisConnection.IsConnected)
                    {
                        _logger.LogWarning("Redis connection is not connected for storage type: {StorageType}", _storageType);
                        return false;
                    }

                    var result = await connection.TestConnectionAsync();
                    _logger.LogInformation("Redis connection test completed for storage type: {StorageType}. Result: {Result}", 
                        _storageType, result);

                    return result;
                }

                _logger.LogWarning("Invalid connection object type for Redis test for storage type: {StorageType}", _storageType);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Redis connection test failed for storage type: {StorageType}", _storageType);
                return false;
            }
        }

        #region Private Methods

        private ConfigurationOptions ParseConnectionString(string connectionString)
        {
            var options = ConfigurationOptions.Parse(connectionString);

            // Default settings
            options.AbortOnConnectFail = false;
            options.ConnectTimeout = 10000; // 10 seconds
            options.SyncTimeout = 5000;     // 5 seconds
            options.AsyncTimeout = 5000;    // 5 seconds
            options.KeepAlive = 60;         // 60 seconds
            options.ClientName = RedisStorageConfiguration.GetClientName(_storageType);

            return options;
        }

        private void OnConnectionFailed(object sender, ConnectionFailedEventArgs e)
        {
            _logger.LogError(e.Exception, "Redis connection failed for storage type: {StorageType} - EndPoint: {EndPoint}, ConnectionType: {ConnectionType}",
                _storageType, e.EndPoint, e.ConnectionType);
        }

        private void OnConnectionRestored(object sender, ConnectionFailedEventArgs e)
        {
            _logger.LogInformation("Redis connection restored for storage type: {StorageType} - EndPoint: {EndPoint}, ConnectionType: {ConnectionType}",
                _storageType, e.EndPoint, e.ConnectionType);
        }

        private void OnErrorMessage(object sender, RedisErrorEventArgs e)
        {
            _logger.LogError("Redis error for storage type: {StorageType} - EndPoint: {EndPoint}, Message: {Message}",
                _storageType, e.EndPoint, e.Message);
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                try
                {
                    foreach (var kvp in _connectionMultiplexers)
                    {
                        var connectionMultiplexer = kvp.Value;
                        if (connectionMultiplexer != null)
                        {
                            connectionMultiplexer.ConnectionFailed -= OnConnectionFailed;
                            connectionMultiplexer.ConnectionRestored -= OnConnectionRestored;
                            connectionMultiplexer.ErrorMessage -= OnErrorMessage;

                            connectionMultiplexer.Dispose();

                            _logger.LogInformation("Redis connection multiplexer disposed for storage type: {StorageType}", kvp.Key);
                        }
                    }
                    _connectionMultiplexers.Clear();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error disposing Redis connection multiplexers");
                }
            }
            base.Dispose(disposing);
        }
    }
}