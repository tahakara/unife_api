using Core.ObjectStorage.Base;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ObjectStorage
{
    public class UnifeObjectStorageConnectionFactory : ObjectStorageConnectionFactoryBase
    {
        private IConnectionMultiplexer? _connectionMultiplexer;
        private readonly object _lock = new object();

        public UnifeObjectStorageConnectionFactory(
            IConfiguration configuration, 
            ILogger<ObjectStorageConnectionFactoryBase> logger) 
            : base(configuration, logger, "UnifeObjectStorageConnectionString", "unife-container")
        {
        }

        protected override IObjectStorageConnection CreateConnectionInstance(string connectionString)
        {
            try
            {
                _logger.LogInformation("Creating Redis connection instance");
                
                lock (_lock)
                {
                    if (_connectionMultiplexer == null || !_connectionMultiplexer.IsConnected)
                    {
                        var connectionOptions = ParseConnectionString(connectionString);
                        _connectionMultiplexer = ConnectionMultiplexer.Connect(connectionOptions);
                        
                        _logger.LogInformation("Redis connection multiplexer created successfully");
                        
                        // Connection events
                        _connectionMultiplexer.ConnectionFailed += OnConnectionFailed;
                        _connectionMultiplexer.ConnectionRestored += OnConnectionRestored;
                        _connectionMultiplexer.ErrorMessage += OnErrorMessage;
                    }
                }

                var database = _connectionMultiplexer.GetDatabase();
                var redisConnection = new RedisObjectStorageConnection(
                    _connectionMultiplexer, 
                    database, 
                    GetContainerName());

                _logger.LogInformation("Redis object storage connection created successfully");
                return redisConnection;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create Redis connection instance");
                throw;
            }
        }

        protected override async Task<IObjectStorageConnection> CreateConnectionInstanceAsync(string connectionString)
        {
            try
            {
                _logger.LogInformation("Creating Redis connection instance asynchronously");
                
                if (_connectionMultiplexer == null || !_connectionMultiplexer.IsConnected)
                {
                    var connectionOptions = ParseConnectionString(connectionString);
                    _connectionMultiplexer = await ConnectionMultiplexer.ConnectAsync(connectionOptions);
                    
                    _logger.LogInformation("Redis connection multiplexer created asynchronously");
                    
                    // Connection events
                    _connectionMultiplexer.ConnectionFailed += OnConnectionFailed;
                    _connectionMultiplexer.ConnectionRestored += OnConnectionRestored;
                    _connectionMultiplexer.ErrorMessage += OnErrorMessage;
                }

                var database = _connectionMultiplexer.GetDatabase();
                var redisConnection = new RedisObjectStorageConnection(
                    _connectionMultiplexer, 
                    database, 
                    GetContainerName());

                _logger.LogInformation("Redis object storage connection created asynchronously");
                return redisConnection;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create Redis connection instance asynchronously");
                throw;
            }
        }

        protected override async Task<bool> TestConnectionImplementationAsync(IObjectStorageConnection connection)
        {
            try
            {
                _logger.LogInformation("Testing Redis connection");

                if (connection is RedisObjectStorageConnection redisConnection)
                {
                    if (!redisConnection.IsConnected)
                    {
                        _logger.LogWarning("Redis connection is not connected");
                        return false;
                    }

                    // Use the interface method for testing
                    var result = await connection.TestConnectionAsync();
                    _logger.LogInformation("Redis connection test completed. Result: {Result}", result);
                    
                    return result;
                }

                _logger.LogWarning("Invalid connection object type for Redis test");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Redis connection test failed");
                return false;
            }
        }

        #region Private Methods

        private ConfigurationOptions ParseConnectionString(string connectionString)
        {
            var options = ConfigurationOptions.Parse(connectionString);
            
            // Default ayarlar
            options.AbortOnConnectFail = false;
            options.ConnectTimeout = 10000; // 10 saniye
            options.SyncTimeout = 5000;     // 5 saniye
            options.AsyncTimeout = 5000;    // 5 saniye
            options.KeepAlive = 60;         // 60 saniye
            options.ClientName = "UnifeObjectStorage";
            
            return options;
        }

        private void OnConnectionFailed(object sender, ConnectionFailedEventArgs e)
        {
            _logger.LogError(e.Exception, "Redis connection failed - EndPoint: {EndPoint}, ConnectionType: {ConnectionType}", 
                e.EndPoint, e.ConnectionType);
        }

        private void OnConnectionRestored(object sender, ConnectionFailedEventArgs e)
        {
            _logger.LogInformation("Redis connection restored - EndPoint: {EndPoint}, ConnectionType: {ConnectionType}", 
                e.EndPoint, e.ConnectionType);
        }

        private void OnErrorMessage(object sender, RedisErrorEventArgs e)
        {
            _logger.LogError("Redis error - EndPoint: {EndPoint}, Message: {Message}", 
                e.EndPoint, e.Message);
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                try
                {
                    if (_connectionMultiplexer != null)
                    {
                        _connectionMultiplexer.ConnectionFailed -= OnConnectionFailed;
                        _connectionMultiplexer.ConnectionRestored -= OnConnectionRestored;
                        _connectionMultiplexer.ErrorMessage -= OnErrorMessage;
                        
                        _connectionMultiplexer.Dispose();
                        _connectionMultiplexer = null;
                        
                        _logger.LogInformation("Redis connection multiplexer disposed");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error disposing Redis connection multiplexer");
                }
            }
            base.Dispose(disposing);
        }
    }
}
