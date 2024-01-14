﻿using AsyncKeyedLock;
using Microsoft.Extensions.Caching.Distributed;
using SharedKernel.Application.Caching;
using SharedKernel.Application.Serializers;

namespace SharedKernel.Infrastructure.Caching;

/// <summary>  </summary>
public class DistributedCacheHelper : ICacheHelper
{
    private readonly IDistributedCache _distributedCache;
    private readonly IBinarySerializer _binarySerializer;
    private static readonly AsyncNonKeyedLocker Lock = new(1);

    /// <summary>  </summary>
    public DistributedCacheHelper(
        IDistributedCache distributedCache,
        IBinarySerializer binarySerializer)
    {
        _distributedCache = distributedCache;
        _binarySerializer = binarySerializer;
    }

    /// <summary>  </summary>
    public async Task<T?> GetAsync<T>(string key) where T : notnull
    {
        var value = await _distributedCache.GetAsync(key);

        return value == default || value.Length == 0 ? default : _binarySerializer.Deserialize<T>(value);
    }

    /// <summary>  </summary>
    public async Task SetAsync<T>(string key, T value, TimeSpan? timeSpan = default) where T : notnull
    {
        using (await Lock.LockAsync())
        {
            await _distributedCache.SetAsync(key, _binarySerializer.Serialize(value),
                new DistributedCacheEntryOptions());
        }
    }

    /// <summary>  </summary>
    public async Task<T?> GetOrCreateAsync<T>(string key, Func<Task<T>> generator, TimeSpan? timeSpan = null)
        where T : notnull
    {
        using (await Lock.LockAsync())
        {
            var value = await _distributedCache.GetAsync(key);

            if (value != default && value.Length != 0)
                return _binarySerializer.Deserialize<T>(value);


            var valueToCache = await generator();
            await _distributedCache.SetAsync(key, _binarySerializer.Serialize(valueToCache),
                new DistributedCacheEntryOptions());

            return valueToCache;
        }
    }

    /// <summary>  </summary>
    public void Remove(string key)
    {
        _distributedCache.Remove(key);
    }
}
