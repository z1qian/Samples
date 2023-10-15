using Microsoft.Extensions.Caching.Distributed;

namespace 分布式缓存.DistributedCacheHelper;

public interface IDistributedCacheHelper
{
    TResult? GetOrCreate<TResult>(string cacheKey, Func<DistributedCacheEntryOptions,
          TResult?> valueFactory, int expireSeconds = 60);
    Task<TResult?> GetOrCreateAsync<TResult>(string cacheKey,
       Func<DistributedCacheEntryOptions, Task<TResult?>> valueFactory,
             int expireSeconds);
    void Remove(string cacheKey);
    Task RemoveAsync(string cacheKey);
}