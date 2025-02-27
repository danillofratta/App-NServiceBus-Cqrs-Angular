using Newtonsoft.Json;
using StackExchange.Redis;

namespace Shared.Infrasctructure.Orm.Service
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IDatabase _database;

        public RedisCacheService(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            var json = JsonConvert.SerializeObject(value);
            await _database.StringSetAsync(key, json, expiry);
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var json = await _database.StringGetAsync(key);
            return json.IsNullOrEmpty ? default : JsonConvert.DeserializeObject<T>(json);
        }

        public async Task RemoveAsync(string key)
        {
            await _database.KeyDeleteAsync(key);
        }

        public async Task AddToSetAsync(string key, string value)
        {
            await _database.SetAddAsync(key, value);
        }

        public async Task RemoveFromSetAsync(string key, string value)
        {
            await _database.SetRemoveAsync(key, value);
        }

        public async Task<string[]> GetSetMembersAsync(string key)
        {
            var members = await _database.SetMembersAsync(key);
            return members.Select(m => m.ToString()).ToArray();
        }
    }

}
