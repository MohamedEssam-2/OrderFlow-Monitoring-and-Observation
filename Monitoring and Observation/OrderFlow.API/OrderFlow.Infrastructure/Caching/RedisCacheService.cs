using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using OrderFlow.Application;

namespace OrderFlow.Infrastructure.Caching
{
    public class RedisCacheService(IDistributedCache _cache) : ICacheService
    {
        public async Task<T?> GetAsync<T>(string key,CancellationToken cancellationToken = default)
        {
            var json = await _cache.GetStringAsync(key,cancellationToken);
            if (json is null)
                return default;
            return JsonSerializer.Deserialize<T>(json);
        }


        public async Task SetAsync<T>(string key,T value,TimeSpan expiration,CancellationToken cancellationToken = default)
        {
            var json = JsonSerializer.Serialize(value);
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration
            };
            await _cache.SetStringAsync(key,json,options,cancellationToken);
        }

        public async Task RemoveAsync(string key,CancellationToken cancellationToken = default)
        {
            await _cache.RemoveAsync(key,cancellationToken);
        }
    }
}
