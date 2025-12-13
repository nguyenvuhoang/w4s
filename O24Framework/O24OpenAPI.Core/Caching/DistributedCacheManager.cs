using System.Collections.Concurrent;
using Newtonsoft.Json;
using O24OpenAPI.Core.Configuration;
using StackExchange.Redis;

namespace O24OpenAPI.Core.Caching;

/// <summary>
/// The distributed cache manager class
/// </summary>
/// <seealso cref="CacheKeyService"/>
/// <seealso cref="ILocker"/>
/// <seealso cref="IStaticCacheManager"/>
/// <seealso cref="IDisposable"/>
public class DistributedCacheManager : CacheKeyService, ILocker, IStaticCacheManager, IDisposable
{
    private readonly IConnectionMultiplexer _redis;
    private readonly IDatabase _distributedCache;
    private static readonly ConcurrentDictionary<string, byte> _keys;
    private readonly CacheConfig _config;

    static DistributedCacheManager()
    {
        _keys = new ConcurrentDictionary<string, byte>();
    }

    public DistributedCacheManager(AppSettings appSettings, IConnectionMultiplexer redis)
        : base(appSettings)
    {
        _redis = redis ?? throw new ArgumentNullException(nameof(redis));
        _distributedCache = _redis.GetDatabase();
        _config = appSettings.Get<CacheConfig>();
    }

    public async Task Set(CacheKey key, object data)
    {
        ArgumentNullException.ThrowIfNull(key);

        if (key.CacheTime <= 0 || data == null)
        {
            return;
        }

        TimeSpan? expiration = key.IsForever ? null : TimeSpan.FromMinutes(key.CacheTime);

        await _distributedCache.StringSetAsync(
            key.Key,
            JsonConvert.SerializeObject(data),
            expiration
        );

        _keys.TryAdd(key.Key, 1);
    }

    public void SetFieldsHash(string key, HashEntry hashData, int cacheTime = 0)
    {
        _distributedCache.HashSet((RedisKey)key, hashData.Name, hashData.Value);
        _distributedCache.KeyExpire(
            key,
            TimeSpan.FromSeconds(cacheTime <= 0 ? _config.DefaultCacheTime : cacheTime)
        );
        _keys.TryAdd(key, 1);
    }

    public T? GetFieldHash<T>(string key, string field)
        where T : class
    {
        RedisValue redisValue = _distributedCache.HashGet((RedisKey)key, field);
        if (redisValue.IsNull)
        {
            return null;
        }

        return JsonConvert.DeserializeObject<T>(redisValue.ToString());
    }

    /// <summary>
    /// Removes the hash using the specified key
    /// </summary>
    /// <param name="key">The key</param>
    public async Task RemoveHash(string key)
    {
        await _redis.GetDatabase().KeyDeleteAsync((RedisKey)key);
        _keys.TryRemove(key, out _);
    }

    /// <summary>
    /// Disposes this instance
    /// </summary>
    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Gets the key
    /// </summary>
    /// <typeparam name="T">The </typeparam>
    /// <param name="key">The key</param>
    /// <param name="acquire">The acquire</param>
    /// <returns>The result</returns>
    public async Task<T?> Get<T>(CacheKey key, Func<Task<T>> acquire)
    {
        T result = await acquire();
        if (result != null)
        {
            await Set(key, result);
        }

        return result;
    }

    /// <summary>
    /// Gets the key
    /// </summary>
    /// <typeparam name="T">The </typeparam>
    /// <param name="key">The key</param>
    /// <param name="acquire">The acquire</param>
    /// <returns>The result</returns>
    public async Task<T?> Get<T>(CacheKey key, Func<T> acquire)
    {
        T result = acquire();
        if (result != null)
        {
            await Set(key, result);
        }

        return result;
    }

    /// <summary>
    /// Gets the key
    /// </summary>
    /// <typeparam name="T">The </typeparam>
    /// <param name="key">The key</param>
    /// <returns>A task containing the</returns>
    public async Task<T?> Get<T>(CacheKey key)
    {
        var (isSet, item) = await TryGetItem<T>(key);
        if (isSet)
        {
            return item;
        }

        return default;
    }

    /// <summary>
    /// Gets the key
    /// </summary>
    /// <typeparam name="T">The </typeparam>
    /// <param name="key">The key</param>
    /// <param name="defaultValue">The default value</param>
    /// <returns>A task containing the</returns>
    public async Task<T?> Get<T>(CacheKey key, T defaultValue)
    {
        RedisValue redisValue = await _distributedCache.StringGetAsync((RedisKey)key.Key);
        if (string.IsNullOrEmpty(redisValue))
        {
            return defaultValue;
        }

        return JsonConvert.DeserializeObject<T>(redisValue.ToString());
    }

    /// <summary>
    /// Gets the key
    /// </summary>
    /// <param name="key">The key</param>
    /// <returns>A task containing the object</returns>
    public async Task<object?> Get(CacheKey key)
    {
        return await Get(key, (object?)null);
    }

    /// <summary>
    /// Removes the cache key
    /// </summary>
    /// <param name="cacheKey">The cache key</param>
    /// <param name="cacheKeyParameters">The cache key parameters</param>
    public async Task Remove(CacheKey cacheKey, params object[] cacheKeyParameters)
    {
        cacheKey = PrepareKey(cacheKey, cacheKeyParameters);
        await _distributedCache.KeyDeleteAsync((RedisKey)cacheKey.Key);
        _keys.TryRemove(cacheKey.Key, out _);
    }

    /// <summary>
    /// Removes the by prefix using the specified prefix
    /// </summary>
    /// <param name="prefix">The prefix</param>
    /// <param name="prefixParameters">The prefix parameters</param>
    public async Task RemoveByPrefix(string prefix, params object[] prefixParameters)
    {
        prefix = PrepareKeyPrefix(prefix, prefixParameters);
        var keysToRemove = _keys
            .Keys.Where(k => k.StartsWith(prefix, StringComparison.InvariantCultureIgnoreCase))
            .ToList();

        foreach (var key in keysToRemove)
        {
            await _distributedCache.KeyDeleteAsync((RedisKey)key);
            _keys.TryRemove(key, out _);
        }
    }

    /// <summary>
    /// Clears this instance
    /// </summary>
    public async Task Clear()
    {
        foreach (var key in _keys.Keys)
        {
            await _distributedCache.KeyDeleteAsync((RedisKey)key);
            _keys.TryRemove(key, out _);
        }
    }

    /// <summary>
    /// Performs the action with lock using the specified resource
    /// </summary>
    /// <param name="resource">The resource</param>
    /// <param name="expirationTime">The expiration time</param>
    /// <param name="action">The action</param>
    /// <returns>The bool</returns>
    public bool PerformActionWithLock(string resource, TimeSpan expirationTime, Action action)
    {
        if (!string.IsNullOrEmpty(_distributedCache.StringGet((RedisKey)resource)))
        {
            return false;
        }

        try
        {
            _distributedCache.StringSet((RedisKey)resource, resource);
            action();
            return true;
        }
        finally
        {
            _distributedCache.KeyDelete((RedisKey)resource);
        }
    }

    /// <summary>
    /// Gets the or set using the specified key
    /// </summary>
    /// <typeparam name="T">The </typeparam>
    /// <param name="key">The key</param>
    /// <param name="acquire">The acquire</param>
    /// <returns>The result</returns>
    public async Task<T?> GetOrSetAsync<T>(CacheKey key, Func<Task<T>> acquire)
    {
        var (isSet, item) = await TryGetItem<T>(key);
        if (isSet)
        {
            return item;
        }

        var result = await acquire();
        if (result != null)
        {
            await Set(key, result);
        }

        return result;
    }

    /// <summary>
    /// Clears the all
    /// </summary>
    public async Task ClearAll()
    {
        var server = _redis.GetServer(_redis.GetEndPoints().First());
        var keys = server.Keys(pattern: "*").ToArray();

        // Delete in batches
        const int batchSize = 1000;
        for (int i = 0; i < keys.Length; i += batchSize)
        {
            var batch = keys.Skip(i).Take(batchSize);
            var tasks = batch.Select(key => _distributedCache.KeyDeleteAsync(key));
            await Task.WhenAll(tasks);
        }

        _keys.Clear();
    }

    public async Task RemoveAsync(CacheKey cacheKey)
    {
        await _redis.GetDatabase().KeyDeleteAsync((RedisKey)cacheKey.Key);
    }

    public async Task<(bool isSet, T? item)> TryGetItem<T>(CacheKey key)
    {
        RedisValue redisValue = await _distributedCache.StringGetAsync((RedisKey)key.Key);

        if (redisValue.IsNullOrEmpty)
        {
            return (false, default);
        }
        T? item = JsonConvert.DeserializeObject<T>(redisValue!);

        return (true, item);
    }
}
