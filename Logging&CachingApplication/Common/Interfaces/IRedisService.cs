using System;
using System.Collections.Generic;
using System.Text;

namespace Logging_CachingApplication.Common.Interfaces
{
    public interface IRedisService
    {
        Task SetAsync<T>( string key,T value,TimeSpan? expiry = null);

        Task<T?> GetAsync<T>(string key);
        Task<bool> DeleteAsync(string key);
        Task<bool> ExistsAsync(string key);
        Task<long> IncrementAsync(string value);
        Task<long> DecrimentAsync(string key);

        Task<bool> ExpireAsync(string key, TimeSpan expiry);

        Task<TimeSpan?> GetTimeToLiveAsync(string key);
    }
}
