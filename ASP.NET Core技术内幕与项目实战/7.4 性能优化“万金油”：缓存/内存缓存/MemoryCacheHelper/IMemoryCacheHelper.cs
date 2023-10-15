﻿using Microsoft.Extensions.Caching.Memory;

namespace 内存缓存.MemoryCacheHelper;

public interface IMemoryCacheHelper
{
    TResult? GetOrCreate<TResult>(string cacheKey,
     Func<ICacheEntry, TResult?> valueFactory, int expireSeconds = 60);

    Task<TResult?> GetOrCreateAsync<TResult>(string cacheKey,
     Func<ICacheEntry, Task<TResult?>> valueFactory, int expireSeconds = 60);

    void Remove(string cacheKey);
}