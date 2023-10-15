using Microsoft.Extensions.Caching.Memory;
using System.Collections;

namespace 内存缓存.MemoryCacheHelper;

public class MemoryCacheHelper : IMemoryCacheHelper
{
    private readonly IMemoryCache memoryCache;
    public MemoryCacheHelper(IMemoryCache memoryCache)
    {
        this.memoryCache = memoryCache;
    }
    private static void ValidateValueType<TResult>()
    {
        Type typeResult = typeof(TResult);
        if (typeResult.IsGenericType)//如果是泛型类型，获取泛型类型定义
        {
            typeResult = typeResult.GetGenericTypeDefinition();
        }
        if (typeResult == typeof(IEnumerable<>) ||
            typeResult == typeof(IEnumerable) ||
            typeResult == typeof(IAsyncEnumerable<TResult>) ||
            typeResult == typeof(IQueryable<TResult>) ||
            typeResult == typeof(IQueryable))
        {
            throw new InvalidOperationException($"please use List<T> or T[] instead.");
        }
    }

    private static void InitCacheEntry(ICacheEntry entry, int baseExpireSeconds)
    {
        double sec = Random.Shared.Next(baseExpireSeconds, baseExpireSeconds * 2);
        TimeSpan expiration = TimeSpan.FromSeconds(sec);
        entry.AbsoluteExpirationRelativeToNow = expiration;
    }
    public async Task<TResult?> GetOrCreateAsync<TResult>(string cacheKey, Func<ICacheEntry, Task<TResult?>> valueFactory,
        int baseExpireSeconds = 60)
    {
        ValidateValueType<TResult>();
        if (!memoryCache.TryGetValue(cacheKey, out TResult result))
        {
            using ICacheEntry entry = memoryCache.CreateEntry(cacheKey);
            InitCacheEntry(entry, baseExpireSeconds);
            result = await valueFactory(entry);
            entry.Value = result;
        }
        return result;
    }

    public TResult? GetOrCreate<TResult>(string cacheKey, Func<ICacheEntry, TResult?> valueFactory, int expireSeconds = 60)
    {
        throw new NotImplementedException();
    }

    public void Remove(string cacheKey)
    {
        throw new NotImplementedException();
    }
}
