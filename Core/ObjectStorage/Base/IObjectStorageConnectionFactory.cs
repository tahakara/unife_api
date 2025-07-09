using System;
using System.Threading.Tasks;

namespace Core.ObjectStorage.Base
{
    public interface IObjectStorageConnectionFactory : IDisposable
    {
        /// <summary>
        /// Creates a new object storage connection asynchronously
        /// </summary>
        /// <returns>Object storage connection instance</returns>
        Task<IObjectStorageConnection> CreateConnectionAsync();

        /// <summary>
        /// Creates a new object storage connection
        /// </summary>
        /// <returns>Object storage connection instance</returns>
        IObjectStorageConnection CreateConnection();

        /// <summary>
        /// Gets the connection string for object storage
        /// </summary>
        /// <returns>Connection string</returns>
        string GetConnectionString();

        /// <summary>
        /// Tests the connection to object storage
        /// </summary>
        /// <returns>True if connection is successful</returns>
        Task<bool> TestConnectionAsync();
    }
}
