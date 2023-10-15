using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace 分布式缓存.DistributedCacheHelper;

public class DistributedCacheHelper : IDistributedCacheHelper
{
    private readonly IDistributedCache distCache;
    public DistributedCacheHelper(IDistributedCache distCache)
    {
        this.distCache = distCache;
    }
    private static DistributedCacheEntryOptions CreateOptions(int baseExpireSeconds)
    {
        double sec = Random.Shared.Next(baseExpireSeconds, baseExpireSeconds * 2);
        TimeSpan expiration = TimeSpan.FromSeconds(sec);
        var options = new DistributedCacheEntryOptions();
        options.AbsoluteExpirationRelativeToNow = expiration;
        return options;
    }

    public TResult? GetOrCreate<TResult>(string cacheKey, Func<DistributedCacheEntryOptions, TResult?> valueFactory, int expireSeconds = 60)
    {
        throw new NotImplementedException();
    }

    public async Task<TResult?> GetOrCreateAsync<TResult>(string cacheKey,
        Func<DistributedCacheEntryOptions, Task<TResult?>> valueFactory, int expireSeconds)
    {
        string jsonStr = await distCache.GetStringAsync(cacheKey);
        if (string.IsNullOrEmpty(jsonStr))
        {
            var options = CreateOptions(expireSeconds);

            TResult? result = await valueFactory(options);

            string jsonOfResult = JsonSerializer.Serialize(result, typeof(TResult));
            await distCache.SetStringAsync(cacheKey, jsonOfResult, options);

            return result;
        }
        else
        {
            await distCache.RefreshAsync(cacheKey);
            return JsonSerializer.Deserialize<TResult>(jsonStr)!;
        }
    }

    public void Remove(string cacheKey)
    {
        throw new NotImplementedException();
    }

    public Task RemoveAsync(string cacheKey)
    {
        throw new NotImplementedException();
    }
}