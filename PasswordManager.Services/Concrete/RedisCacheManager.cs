using PasswordManager.Services.Abstract;
using StackExchange.Redis;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordManager.Services.Concrete
{
    public class RedisCacheManager : IRedisCacheService
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly IDatabase _db;
        private TimeSpan ExpireTime => TimeSpan.FromDays(1);

        public RedisCacheManager(IConnectionMultiplexer redis)
        {
            _redis = redis;
            _db = _redis.GetDatabase();
        }

        public async Task DeleteAsync(string key)
        {
            await _db.KeyDeleteAsync(key);
        }

        public async Task<string> GetAsync(string key)
        {
            return await _db.StringGetAsync(key);
        }

        public bool Any()
        {
            var server = _redis.GetServer(_redis.GetEndPoints().First());
            var keys = server.Keys(pattern: "*").ToList();
            return keys.Count > 0;
        }

        public async Task<bool> SetAsync(string key, string value)
        {
            return await _db.StringSetAsync(key, value, ExpireTime);
        }
    }
}
