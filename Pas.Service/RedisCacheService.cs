using Newtonsoft.Json;
using Pas.Service.Interface;
using StackExchange.Redis;
using System;

namespace Pas.Service
{
    public class RedisCacheService : ICacheService
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private const int TIME_OUT = 60;
        public RedisCacheService(IConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
        }

        public string GetCacheValue(string key)
        {
            var redisDb = _connectionMultiplexer.GetDatabase();
            return redisDb.StringGet(key);
        }

        public T GetCacheValue<T>(string key)
        {
            var redisDb = _connectionMultiplexer.GetDatabase();

            var json = redisDb.StringGet(key);
            if (string.IsNullOrEmpty(json))
                return default;

            return JsonConvert.DeserializeObject<T>(json);
        }

        public void SetCacheValue(string key, string value)
        {
            var redisDb = _connectionMultiplexer.GetDatabase();
            redisDb.StringSet(key, value);
            redisDb.KeyExpire(key, DateTime.Now.AddMinutes(TIME_OUT));
        }

        public void SetCacheValue<T>(string key, T item)
        {
            var json = JsonConvert.SerializeObject(item);

            var redisDb = _connectionMultiplexer.GetDatabase();
            redisDb.StringSet(key, json);
            redisDb.KeyExpire(key, DateTime.Now.AddMinutes(TIME_OUT));
        }
    }
}
