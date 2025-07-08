using DataAccess.Context;
using DataAccess.Database;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Data;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/database")]
    public class DatabaseController : ControllerBase
    {
        private readonly UnifeConnectionFactory _connectionFactory;
        private readonly ILogger<DatabaseController> _logger;

        public DatabaseController(UnifeConnectionFactory connectionFactory, ILogger<DatabaseController> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        /// <summary>
        /// Tests database connection
        /// </summary>
        [HttpGet("test-connection")]
        public async Task<IActionResult> TestConnection()
        {
            try
            {
                using var context = _connectionFactory.CreateContext();
                await context.Database.OpenConnectionAsync();

                var connectionString = context.Database.GetConnectionString();
                var maskedConnectionString = MaskConnectionString(connectionString);

                return Ok(new
                {
                    success = true,
                    message = "Database connection successful",
                    connectionString = maskedConnectionString,
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database connection failed");
                return StatusCode(500, new
                {
                    success = false,
                    message = "Database connection failed",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        /// <summary>
        /// Executes SELECT 42 AS agent query to test database functionality
        /// </summary>
        [HttpGet("test-query")]
        public async Task<IActionResult> TestQuery()
        {
            try
            {
                using var context = _connectionFactory.CreateContext();
                var connection = context.Database.GetDbConnection();
                await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = "SELECT 42 AS agent;";
                command.CommandTimeout = 30; // 30 seconds timeout

                var results = new List<Dictionary<string, object>>();
                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    var row = new Dictionary<string, object>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        row[reader.GetName(i)] = reader.GetValue(i) ?? DBNull.Value;
                    }
                    results.Add(row);
                }

                return Ok(new
                {
                    success = true,
                    message = "Query executed successfully",
                    query = "SELECT 42 AS agent;",
                    rowCount = results.Count,
                    data = results,
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Test query failed");
                return StatusCode(500, new
                {
                    success = false,
                    message = "Test query failed",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        /// <summary>
        /// Lists all tables in the database
        /// </summary>
        [HttpGet("tables")]
        public async Task<IActionResult> ListTables()
        {
            try
            {
                using var context = _connectionFactory.CreateContext();

                var tablesQuery = @"
                    SELECT 
                        schemaname as schema_name,
                        tablename as table_name,
                        tableowner as owner,
                        tablespace,
                        hasindexes as has_indexes,
                        hasrules as has_rules,
                        hastriggers as has_triggers
                    FROM pg_tables 
                    WHERE schemaname = 'public' 
                    ORDER BY tablename";

                var connection = context.Database.GetDbConnection();
                await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = tablesQuery;

                var tables = new List<object>();
                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    tables.Add(new
                    {
                        schemaName = reader["schema_name"].ToString(),
                        tableName = reader["table_name"].ToString(),
                        owner = reader["owner"].ToString(),
                        tableSpace = reader["tablespace"]?.ToString(),
                        hasIndexes = (bool)reader["has_indexes"],
                        hasRules = (bool)reader["has_rules"],
                        hasTriggers = (bool)reader["has_triggers"]
                    });
                }

                return Ok(new
                {
                    success = true,
                    tablesCount = tables.Count,
                    tables = tables,
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to list tables");
                return StatusCode(500, new
                {
                    success = false,
                    message = "Failed to list tables",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        /// <summary>
        /// Creates missing tables in the database
        /// </summary>
        [HttpPost("create-tables")]
        public async Task<IActionResult> CreateTables()
        {
            try
            {
                using var context = _connectionFactory.CreateContext();

                // Check if database exists and create if not
                var created = await context.Database.EnsureCreatedAsync();

                if (!created)
                {
                    // If database already exists, check for pending migrations
                    var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
                    var appliedMigrations = await context.Database.GetAppliedMigrationsAsync();

                    if (pendingMigrations.Any())
                    {
                        await context.Database.MigrateAsync();

                        return Ok(new
                        {
                            success = true,
                            message = "Pending migrations applied successfully",
                            appliedMigrations = appliedMigrations.Count(),
                            pendingMigrations = pendingMigrations.Count(),
                            migrations = pendingMigrations.ToList(),
                            timestamp = DateTime.UtcNow
                        });
                    }
                    else
                    {
                        return Ok(new
                        {
                            success = true,
                            message = "Database and all tables already exist",
                            appliedMigrations = appliedMigrations.Count(),
                            pendingMigrations = 0,
                            timestamp = DateTime.UtcNow
                        });
                    }
                }
                else
                {
                    return Ok(new
                    {
                        success = true,
                        message = "Database and tables created successfully",
                        timestamp = DateTime.UtcNow
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create tables");
                return StatusCode(500, new
                {
                    success = false,
                    message = "Failed to create tables",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        /// <summary>
        /// Drops all tables from the database
        /// </summary>
        [HttpDelete("drop-all-tables")]
        public async Task<IActionResult> DropAllTables()
        {
            try
            {
                using var context = _connectionFactory.CreateContext();

                // Get all table names first
                var tablesQuery = @"
                    SELECT tablename 
                    FROM pg_tables 
                    WHERE schemaname = 'public' 
                    ORDER BY tablename";

                var connection = context.Database.GetDbConnection();
                await connection.OpenAsync();

                var tableNames = new List<string>();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = tablesQuery;
                    using var reader = await command.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        tableNames.Add(reader["tablename"].ToString());
                    }
                }

                if (tableNames.Count == 0)
                {
                    return Ok(new
                    {
                        success = true,
                        message = "No tables found to drop",
                        droppedTables = 0,
                        timestamp = DateTime.UtcNow
                    });
                }

                // Disable foreign key checks and drop tables
                using (var transaction = await connection.BeginTransactionAsync())
                {
                    try
                    {
                        // Drop all tables with CASCADE to handle dependencies
                        foreach (var tableName in tableNames)
                        {
                            using var dropCommand = connection.CreateCommand();
                            dropCommand.Transaction = transaction;
                            dropCommand.CommandText = $"DROP TABLE IF EXISTS \"{tableName}\" CASCADE";
                            await dropCommand.ExecuteNonQueryAsync();
                        }

                        await transaction.CommitAsync();
                    }
                    catch
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }

                return Ok(new
                {
                    success = true,
                    message = "All tables dropped successfully",
                    droppedTables = tableNames.Count,
                    tableNames = tableNames,
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to drop tables");
                return StatusCode(500, new
                {
                    success = false,
                    message = "Failed to drop tables",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        /// <summary>
        /// Gets database schema information
        /// </summary>
        [HttpGet("schema")]
        public async Task<IActionResult> GetDatabaseSchema()
        {
            try
            {
                using var context = _connectionFactory.CreateContext();
                var connection = context.Database.GetDbConnection();
                await connection.OpenAsync();

                var schemaQuery = @"
                    SELECT 
                        t.table_name,
                        c.column_name,
                        c.data_type,
                        c.is_nullable,
                        c.column_default,
                        c.character_maximum_length,
                        c.numeric_precision,
                        c.numeric_scale,
                        tc.constraint_type
                    FROM information_schema.tables t
                    LEFT JOIN information_schema.columns c ON t.table_name = c.table_name
                    LEFT JOIN information_schema.key_column_usage kcu ON c.table_name = kcu.table_name AND c.column_name = kcu.column_name
                    LEFT JOIN information_schema.table_constraints tc ON kcu.constraint_name = tc.constraint_name
                    WHERE t.table_schema = 'public' AND t.table_type = 'BASE TABLE'
                    ORDER BY t.table_name, c.ordinal_position";

                using var command = connection.CreateCommand();
                command.CommandText = schemaQuery;

                var schema = new Dictionary<string, List<object>>();
                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    var tableName = reader["table_name"].ToString();
                    var columnInfo = new
                    {
                        columnName = reader["column_name"].ToString(),
                        dataType = reader["data_type"].ToString(),
                        isNullable = reader["is_nullable"].ToString(),
                        columnDefault = reader["column_default"]?.ToString(),
                        maxLength = reader["character_maximum_length"] as int?,
                        precision = reader["numeric_precision"] as int?,
                        scale = reader["numeric_scale"] as int?,
                        constraintType = reader["constraint_type"]?.ToString()
                    };

                    if (!schema.ContainsKey(tableName))
                    {
                        schema[tableName] = new List<object>();
                    }
                    schema[tableName].Add(columnInfo);
                }

                return Ok(new
                {
                    success = true,
                    message = "Database schema retrieved successfully",
                    tablesCount = schema.Keys.Count,
                    schema = schema,
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get database schema");
                return StatusCode(500, new
                {
                    success = false,
                    message = "Failed to get database schema",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        /// <summary>
        /// Executes a raw SQL query (GET method for SELECT queries)
        /// </summary>
        [HttpPost("execute-query")]
        public async Task<IActionResult> ExecuteQuery([FromBody] ExecuteQueryRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Query))
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Query cannot be empty",
                        timestamp = DateTime.UtcNow
                    });
                }

                // Basic security check - only allow SELECT statements
                var trimmedQuery = request.Query.Trim().ToUpper();
                if (!trimmedQuery.StartsWith("select"))
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Only SELECT queries are allowed for security reasons",
                        timestamp = DateTime.UtcNow
                    });
                }

                using var context = _connectionFactory.CreateContext();
                var connection = context.Database.GetDbConnection();
                await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = request.Query;
                command.CommandTimeout = 30; // 30 seconds timeout

                var results = new List<Dictionary<string, object>>();
                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    var row = new Dictionary<string, object>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        row[reader.GetName(i)] = reader.GetValue(i) ?? DBNull.Value;
                    }
                    results.Add(row);
                }

                return Ok(new
                {
                    success = true,
                    message = "Query executed successfully",
                    query = request.Query,
                    rowCount = results.Count,
                    data = results,
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to execute query: {Query}", request.Query);
                return StatusCode(500, new
                {
                    success = false,
                    message = "Failed to execute query",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        private static string MaskConnectionString(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                return connectionString;

            // Mask password in connection string
            var parts = connectionString.Split(';');
            for (int i = 0; i < parts.Length; i++)
            {
                if (parts[i].ToUpper().Contains("password"))
                {
                    var keyValue = parts[i].Split('=');
                    if (keyValue.Length == 2)
                    {
                        parts[i] = $"{keyValue[0]}=***";
                    }
                }
            }
            return string.Join(';', parts);
        }
    }

    public class ExecuteQueryRequest
    {
        public string Query { get; set; } = string.Empty;
    }
}