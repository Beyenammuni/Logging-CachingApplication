using Logging_CachingApplication.Common.Interfaces;
using StackExchange.Redis;
using System.Text.Json;

namespace Logging_CachingInfrastructure.Data
{
    public class RedisService : IRedisService
    {
        private readonly IDatabase _database;

        public RedisService(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }
        // C#
        public async Task SetAsync<T>(
       string key,
       T value,
       TimeSpan? expiry = null)
        {
            var json = JsonSerializer.Serialize(value);

            if (expiry.HasValue)
            {
                await _database.StringSetAsync(
                    key,
                    json,
                    new Expiration(expiry.Value)
                );
            }
            else
            {
                await _database.StringSetAsync(
                    key,
                    json
                );
            }
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            try
            {
                var value = await _database.StringGetAsync(key);
                if (value.IsNullOrEmpty) return default;

                // If caller requested a raw string, return the stored Redis value directly.
                if (typeof(T) == typeof(string))
                {
                    return (T)(object)value.ToString();
                }

                return JsonSerializer.Deserialize<T>(value.ToString());
            }
            catch (RedisConnectionException)
            {
                // log and treat as cache miss
                return default;
            }
        }
        public async Task<long> DecrimentAsync(string key)
        {
           return await _database.StringDecrementAsync(key);
        }

        public async Task<bool> DeleteAsync(string key)
        {
           return await _database.KeyDeleteAsync(key);
        }

        public async Task<bool> ExistsAsync(string key)
        {
           return await _database.KeyExistsAsync(key);
        }

        public async Task<bool> ExpireAsync(string key, TimeSpan expiry)
        {
           return await _database.KeyExpireAsync(key, expiry);
        }

     

        public async Task<TimeSpan?> GetTimeToLiveAsync(string key)
        {
           return await _database.KeyTimeToLiveAsync(key);
        }

        public async Task<long> IncrementAsync(string value)
        {
            return await _database.StringIncrementAsync(value);
        }

      
    }
}
