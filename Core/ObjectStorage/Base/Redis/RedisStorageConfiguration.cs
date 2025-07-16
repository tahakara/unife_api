namespace Core.ObjectStorage.Base.Redis
{
    /// <summary>
    /// Redis storage configuration with database mapping
    /// </summary>
    public static class RedisStorageConfiguration
    {
        /// <summary>
        /// Gets the Redis database number for the specified storage type
        /// </summary>
        /// <param name="storageType">Storage type</param>
        /// <returns>Redis database number</returns>
        public static int GetDatabaseNumber(RedisStorageType storageType)
        {
            return storageType switch
            {
                RedisStorageType.Cache => 0,
                RedisStorageType.Session => 1,
                RedisStorageType.VerificationCode => 2,
                _ => throw new ArgumentException($"Unknown storage type: {storageType}", nameof(storageType))
            };
        }

        /// <summary>
        /// Gets the default expiration time for the specified storage type
        /// </summary>
        /// <param name="storageType">Storage type</param>
        /// <returns>Default expiration time</returns>
        public static TimeSpan GetDefaultExpiration(RedisStorageType storageType)
        {
            return storageType switch
            {
                RedisStorageType.Cache => TimeSpan.FromMinutes(15),
                RedisStorageType.Session => TimeSpan.FromHours(24),
                RedisStorageType.VerificationCode => TimeSpan.FromMinutes(5),
                _ => TimeSpan.FromMinutes(15)
            };
        }

        /// <summary>
        /// Gets the key prefix for the specified storage type
        /// </summary>
        /// <param name="storageType">Storage type</param>
        /// <returns>Key prefix</returns>
        public static string GetKeyPrefix(RedisStorageType storageType)
        {
            return storageType switch
            {
                RedisStorageType.Cache => "cache",
                RedisStorageType.Session => "session",
                RedisStorageType.VerificationCode => "verification",
                _ => "default"
            };
        }

        /// <summary>
        /// Gets the connection client name for the specified storage type
        /// </summary>
        /// <param name="storageType">Storage type</param>
        /// <returns>Client name</returns>
        public static string GetClientName(RedisStorageType storageType)
        {
            return storageType switch
            {
                RedisStorageType.Cache => "UnifeCache",
                RedisStorageType.Session => "UnifeSession",
                RedisStorageType.VerificationCode => "UnifeVerification",
                _ => "UnifeDefault"
            };
        }
    }
}