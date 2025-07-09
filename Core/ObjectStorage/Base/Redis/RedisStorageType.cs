namespace Core.ObjectStorage.Base.Redis
{
    /// <summary>
    /// Redis storage type enum to differentiate between cache and session operations
    /// </summary>
    public enum RedisStorageType
    {
        /// <summary>
        /// Cache storage type - uses Redis database 0
        /// </summary>
        Cache = 0,
        
        /// <summary>
        /// Session storage type - uses Redis database 1
        /// </summary>
        Session = 1
    }
}