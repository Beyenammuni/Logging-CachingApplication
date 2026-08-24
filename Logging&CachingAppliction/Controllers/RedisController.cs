using Logging_CachingApplication.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace Logging_CachingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RedisController : ControllerBase
    {
        private readonly IRedisService _redis;

        public RedisController(IRedisService redisService)
        {
            _redis = redisService;
        }
        [HttpPost("set")]
        public async Task<IActionResult> Set(string key, string value)
        {
            await _redis.SetAsync(key, value);

            return Ok(new
            {
                Message = "Value stored successfully",
                Key = key,
                Value = value
            });
        }
        [HttpGet("get")]
        public async Task<IActionResult> Get(string key)
        {
            var value = await _redis.GetAsync<string>(key);

            if (value is null)
            {
                return NotFound($"Key '{key}' not found");
            }

            return Ok(new
            {
                Key = key,
                Value = value
            });
        }
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(
    string key)
        {
            var deleted =
                await _redis.DeleteAsync(key);

            if (!deleted)
            {
                return NotFound(
                    $"Key '{key}' not found");
            }

            return Ok(new
            {
                Message = "Key deleted successfully"
            });
        }
        [HttpGet("exists")]
        public async Task<IActionResult> Exists(
    string key)
        {
            var exists =
                await _redis.ExistsAsync(key);

            return Ok(new
            {
                Key = key,
                Exists = exists
            });
        }
        [HttpPost("expire")]
        public async Task<IActionResult> Expire(string key,int seconds)
        {
            var result =
                await _redis.ExpireAsync(
                    key,
                    TimeSpan.FromSeconds(seconds)
                );

            if (!result)
            {
                return NotFound(
                    "Key not found");
            }

            return Ok(new
            {
                Message = "Expiration set successfully",
                Key = key,
                Seconds = seconds
            });
        }
        [HttpPost("increment")]
        public async Task<IActionResult> Increment(
    string key)
        {
            var value =
                await _redis.IncrementAsync(key);

            return Ok(new
            {
                Key = key,
                Value = value
            });
        }
        [HttpGet("ttl")]
        public async Task<IActionResult> TTL(
    string key)
        {
            var ttl =
                await _redis.GetTimeToLiveAsync(key);

            if (ttl is null)
            {
                return NotFound(
                    "Key does not exist or has no expiration");
            }

            return Ok(new
            {
                Key = key,
                RemainingSeconds =
                    ttl.Value.TotalSeconds
            });
        }

    }
}
