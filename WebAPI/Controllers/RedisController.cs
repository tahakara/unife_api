using Core.ObjectStorage.Base;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/redis")]
    public class RedisController : ControllerBase
    {
        private readonly IObjectStorageConnectionFactory _connectionFactory;
        private readonly ILogger<RedisController> _logger;

        public RedisController(IObjectStorageConnectionFactory connectionFactory, ILogger<RedisController> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        /// <summary>
        /// Tests Redis connection
        /// </summary>
        [HttpGet("test-connection")]
        public async Task<IActionResult> TestConnection()
        {
            try
            {
                var connectionResult = await _connectionFactory.TestConnectionAsync();
                
                return Ok(new
                {
                    success = connectionResult,
                    message = connectionResult ? "Redis connection successful" : "Redis connection failed",
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Redis connection test failed");
                return StatusCode(500, new
                {
                    success = false,
                    message = "Redis connection test failed",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        /// <summary>
        /// Tests Redis connection with PING
        /// </summary>
        [HttpGet("ping")]
        public async Task<IActionResult> Ping()
        {
            try
            {
                using var connection = await _connectionFactory.CreateConnectionAsync();
                var pingResult = await connection.TestConnectionAsync();
                
                return Ok(new
                {
                    success = pingResult,
                    message = pingResult ? "Redis PING successful" : "Redis PING failed",
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Redis PING failed");
                return StatusCode(500, new
                {
                    success = false,
                    message = "Redis PING failed",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        /// <summary>
        /// Gets database size (number of keys)
        /// </summary>
        [HttpGet("db-size")]
        public async Task<IActionResult> GetDatabaseSize()
        {
            try
            {
                using var connection = await _connectionFactory.CreateConnectionAsync();
                var dbSize = await connection.GetDatabaseSizeAsync();
                
                return Ok(new
                {
                    success = true,
                    message = "Database size retrieved successfully",
                    keyCount = dbSize,
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get Redis database size");
                return StatusCode(500, new
                {
                    success = false,
                    message = "Failed to get Redis database size",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        /// <summary>
        /// Lists all keys in Redis with optional pattern matching
        /// </summary>
        [HttpGet("keys")]
        public async Task<IActionResult> GetKeys([FromQuery] string pattern = "*")
        {
            try
            {
                using var connection = await _connectionFactory.CreateConnectionAsync();
                var keys = await connection.GetKeysAsync(pattern);
                
                return Ok(new
                {
                    success = true,
                    message = "Redis keys retrieved successfully",
                    pattern = pattern,
                    count = keys.Count,
                    keys = keys,
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get Redis keys");
                return StatusCode(500, new
                {
                    success = false,
                    message = "Failed to get Redis keys",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        /// <summary>
        /// Gets a value from Redis by key
        /// </summary>
        [HttpGet("get/{key}")]
        public async Task<IActionResult> GetValue(string key)
        {
            try
            {
                using var connection = await _connectionFactory.CreateConnectionAsync();
                var value = await connection.GetStringAsync(key);
                var exists = await connection.ExistsAsync(key);
                
                if (exists && value != null)
                {
                    return Ok(new
                    {
                        success = true,
                        message = "Value retrieved successfully",
                        key = key,
                        value = value,
                        exists = exists,
                        timestamp = DateTime.UtcNow
                    });
                }
                else
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Key not found",
                        key = key,
                        exists = exists,
                        timestamp = DateTime.UtcNow
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get value from Redis for key: {Key}", key);
                return StatusCode(500, new
                {
                    success = false,
                    message = "Failed to get value from Redis",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        /// <summary>
        /// Sets a value in Redis
        /// </summary>
        [HttpPost("set")]
        public async Task<IActionResult> SetValue([FromBody] SetValueRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Key))
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Key cannot be empty",
                        timestamp = DateTime.UtcNow
                    });
                }

                using var connection = await _connectionFactory.CreateConnectionAsync();
                
                var expiry = request.ExpirationSeconds.HasValue 
                    ? TimeSpan.FromSeconds(request.ExpirationSeconds.Value) 
                    : (TimeSpan?)null;
                
                var result = await connection.SetStringAsync(request.Key, request.Value ?? string.Empty, expiry);
                
                return Ok(new
                {
                    success = result,
                    message = result ? "Value set successfully" : "Failed to set value",
                    key = request.Key,
                    value = request.Value,
                    expirationSeconds = request.ExpirationSeconds,
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to set value in Redis for key: {Key}", request.Key);
                return StatusCode(500, new
                {
                    success = false,
                    message = "Failed to set value in Redis",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        /// <summary>
        /// Deletes a key from Redis
        /// </summary>
        [HttpDelete("delete/{key}")]
        public async Task<IActionResult> DeleteKey(string key)
        {
            try
            {
                using var connection = await _connectionFactory.CreateConnectionAsync();
                var result = await connection.DeleteAsync(key);
                
                return Ok(new
                {
                    success = result,
                    message = result ? "Key deleted successfully" : "Key not found or already deleted",
                    key = key,
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete key from Redis: {Key}", key);
                return StatusCode(500, new
                {
                    success = false,
                    message = "Failed to delete key from Redis",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        /// <summary>
        /// Flushes all keys from the current database
        /// </summary>
        [HttpPost("flush-db")]
        public async Task<IActionResult> FlushDatabase()
        {
            try
            {
                using var connection = await _connectionFactory.CreateConnectionAsync();
                var result = await connection.FlushAllAsync();
                
                return Ok(new
                {
                    success = result,
                    message = result ? "Database flushed successfully" : "Failed to flush database",
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to flush Redis database");
                return StatusCode(500, new
                {
                    success = false,
                    message = "Failed to flush Redis database",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        /// <summary>
        /// Sets a JSON object in Redis
        /// </summary>
        [HttpPost("set-json")]
        public async Task<IActionResult> SetJsonValue([FromBody] SetJsonRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Key))
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Key cannot be empty",
                        timestamp = DateTime.UtcNow
                    });
                }

                using var connection = await _connectionFactory.CreateConnectionAsync();
                
                var expiry = request.ExpirationSeconds.HasValue 
                    ? TimeSpan.FromSeconds(request.ExpirationSeconds.Value) 
                    : (TimeSpan?)null;
                
                var serializedValue = System.Text.Json.JsonSerializer.Serialize(request.Value);
                var result = await connection.SetStringAsync(request.Key, serializedValue, expiry);
                
                return Ok(new
                {
                    success = result,
                    message = result ? "JSON value set successfully" : "Failed to set JSON value",
                    key = request.Key,
                    value = request.Value,
                    expirationSeconds = request.ExpirationSeconds,
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to set JSON value in Redis for key: {Key}", request.Key);
                return StatusCode(500, new
                {
                    success = false,
                    message = "Failed to set JSON value in Redis",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        /// <summary>
        /// Gets a JSON object from Redis by key
        /// </summary>
        [HttpGet("get-json/{key}")]
        public async Task<IActionResult> GetJsonValue(string key)
        {
            try
            {
                using var connection = await _connectionFactory.CreateConnectionAsync();
                var value = await connection.GetStringAsync(key);
                var exists = await connection.ExistsAsync(key);
                
                if (exists && value != null)
                {
                    object? jsonValue = null;
                    try
                    {
                        jsonValue = System.Text.Json.JsonSerializer.Deserialize<object>(value);
                    }
                    catch
                    {
                        jsonValue = value; // If not JSON, return as string
                    }
                    
                    return Ok(new
                    {
                        success = true,
                        message = "JSON value retrieved successfully",
                        key = key,
                        value = jsonValue,
                        exists = exists,
                        timestamp = DateTime.UtcNow
                    });
                }
                else
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Key not found",
                        key = key,
                        exists = exists,
                        timestamp = DateTime.UtcNow
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get JSON value from Redis for key: {Key}", key);
                return StatusCode(500, new
                {
                    success = false,
                    message = "Failed to get JSON value from Redis",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        /// <summary>
        /// Gets batch values from Redis
        /// </summary>
        [HttpPost("get-batch")]
        public async Task<IActionResult> GetBatch([FromBody] GetBatchRequest request)
        {
            try
            {
                if (request.Keys == null || !request.Keys.Any())
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Keys cannot be empty",
                        timestamp = DateTime.UtcNow
                    });
                }

                using var connection = await _connectionFactory.CreateConnectionAsync();
                var result = await connection.GetBatchAsync(request.Keys);
                
                return Ok(new
                {
                    success = true,
                    message = "Batch values retrieved successfully",
                    count = result.Count,
                    data = result,
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get batch values from Redis");
                return StatusCode(500, new
                {
                    success = false,
                    message = "Failed to get batch values from Redis",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        /// <summary>
        /// Sets batch values in Redis
        /// </summary>
        [HttpPost("set-batch")]
        public async Task<IActionResult> SetBatch([FromBody] SetBatchRequest request)
        {
            try
            {
                if (request.KeyValuePairs == null || !request.KeyValuePairs.Any())
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "KeyValuePairs cannot be empty",
                        timestamp = DateTime.UtcNow
                    });
                }

                using var connection = await _connectionFactory.CreateConnectionAsync();
                
                var expiry = request.ExpirationSeconds.HasValue 
                    ? TimeSpan.FromSeconds(request.ExpirationSeconds.Value) 
                    : (TimeSpan?)null;
                
                var result = await connection.SetBatchAsync(request.KeyValuePairs, expiry);
                
                return Ok(new
                {
                    success = result,
                    message = result ? "Batch values set successfully" : "Failed to set batch values",
                    count = request.KeyValuePairs.Count,
                    expirationSeconds = request.ExpirationSeconds,
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to set batch values in Redis");
                return StatusCode(500, new
                {
                    success = false,
                    message = "Failed to set batch values in Redis",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        /// <summary>
        /// Deletes multiple keys from Redis
        /// </summary>
        [HttpPost("delete-batch")]
        public async Task<IActionResult> DeleteBatch([FromBody] DeleteBatchRequest request)
        {
            try
            {
                if (request.Keys == null || !request.Keys.Any())
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Keys cannot be empty",
                        timestamp = DateTime.UtcNow
                    });
                }

                using var connection = await _connectionFactory.CreateConnectionAsync();
                var deletedCount = await connection.DeleteBatchAsync(request.Keys);
                
                return Ok(new
                {
                    success = deletedCount > 0,
                    message = $"{deletedCount} keys deleted successfully",
                    deletedCount = deletedCount,
                    requestedCount = request.Keys.Count(),
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete batch keys from Redis");
                return StatusCode(500, new
                {
                    success = false,
                    message = "Failed to delete batch keys from Redis",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        /// <summary>
        /// Sets expiry for a key
        /// </summary>
        [HttpPost("set-expiry")]
        public async Task<IActionResult> SetExpiry([FromBody] SetExpiryRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Key))
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Key cannot be empty",
                        timestamp = DateTime.UtcNow
                    });
                }

                using var connection = await _connectionFactory.CreateConnectionAsync();
                var expiry = TimeSpan.FromSeconds(request.ExpirationSeconds);
                var result = await connection.SetExpiryAsync(request.Key, expiry);
                
                return Ok(new
                {
                    success = result,
                    message = result ? "Expiry set successfully" : "Failed to set expiry",
                    key = request.Key,
                    expirationSeconds = request.ExpirationSeconds,
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to set expiry for key: {Key}", request.Key);
                return StatusCode(500, new
                {
                    success = false,
                    message = "Failed to set expiry",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        /// <summary>
        /// Gets time to live for a key
        /// </summary>
        [HttpGet("get-ttl/{key}")]
        public async Task<IActionResult> GetTimeToLive(string key)
        {
            try
            {
                using var connection = await _connectionFactory.CreateConnectionAsync();
                var ttl = await connection.GetTimeToLiveAsync(key);
                
                return Ok(new
                {
                    success = true,
                    message = "TTL retrieved successfully",
                    key = key,
                    ttl = ttl?.TotalSeconds,
                    hasExpiry = ttl.HasValue,
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get TTL for key: {Key}", key);
                return StatusCode(500, new
                {
                    success = false,
                    message = "Failed to get TTL",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        /// <summary>
        /// Gets connection information
        /// </summary>
        [HttpGet("connection-info")]
        public async Task<IActionResult> GetConnectionInfo()
        {
            try
            {
                using var connection = await _connectionFactory.CreateConnectionAsync();
                
                var info = new
                {
                    isConnected = connection.IsConnected,
                    containerName = connection.ContainerName,
                    createdAt = connection.CreatedAt,
                    connectionType = "Redis"
                };
                
                return Ok(new
                {
                    success = true,
                    message = "Connection info retrieved successfully",
                    connectionInfo = info,
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get connection info");
                return StatusCode(500, new
                {
                    success = false,
                    message = "Failed to get connection info",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }
    }

    #region Request Models
    
    public class SetValueRequest
    {
        public string Key { get; set; } = string.Empty;
        public string? Value { get; set; }
        public int? ExpirationSeconds { get; set; }
    }

    public class SetJsonRequest
    {
        public string Key { get; set; } = string.Empty;
        public object? Value { get; set; }
        public int? ExpirationSeconds { get; set; }
    }

    public class GetBatchRequest
    {
        public IEnumerable<string> Keys { get; set; } = Enumerable.Empty<string>();
    }

    public class SetBatchRequest
    {
        public Dictionary<string, string> KeyValuePairs { get; set; } = new();
        public int? ExpirationSeconds { get; set; }
    }

    public class DeleteBatchRequest
    {
        public IEnumerable<string> Keys { get; set; } = Enumerable.Empty<string>();
    }

    public class SetExpiryRequest
    {
        public string Key { get; set; } = string.Empty;
        public int ExpirationSeconds { get; set; }
    }

    #endregion                                                                                                      
}