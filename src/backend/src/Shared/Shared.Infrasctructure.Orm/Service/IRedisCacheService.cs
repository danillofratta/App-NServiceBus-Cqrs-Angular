using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Infrasctructure.Orm.Service
{
    public interface IRedisCacheService
    {
        Task SetAsync<T>(string key, T value, TimeSpan? expiry = null);
        Task<T> GetAsync<T>(string key);
        Task RemoveAsync(string key);
        Task AddToSetAsync(string key, string value);
        Task RemoveFromSetAsync(string key, string value);
        Task<string[]> GetSetMembersAsync(string key);
    }
}
