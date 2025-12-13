using System.Collections.Concurrent;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using O24OpenAPI.Core.Configuration;
using StackExchange.Redis;

namespace O24OpenAPI.Core.Caching;

public class MemoryCacheManager(AppSettings appSettings, IMemoryCache memoryCache)
    : CacheKeyService(appSettings),
        ILocker,
        IStaticCacheManager,
        IMemoryCacheService,
        IDisposable
{
    private bool _disposed;
    private readonly IMemoryCache _memoryCache = memoryCache;
    private static readonly ConcurrentDictionary<string, CancellationTokenSource> _prefixes = new();
    private static CancellationTokenSource _clearToken = new();

    private static MemoryCacheEntryOptions PrepareEntryOptions(CacheKey key)
    {
        MemoryCacheEntryOptions options = new()
        {
            AbsoluteExpirationRelativeToNow = new TimeSpan?(TimeSpan.FromMinutes(key.CacheTime)),
        };
        options.AddExpirationToken(new CancellationChangeToken(_clearToken.Token));
        foreach (string key1 in key.Prefixes.ToList<string>())
        {
            CancellationTokenSource orAdd = _prefixes.GetOrAdd(key1, new CancellationTokenSource());
            options.AddExpirationToken(new CancellationChangeToken(orAdd.Token));
        }
        return options;
    }

    /// <summary>
    /// Removes the cache key
    /// </summary>
    /// <param name="cacheKey">The cache key</param>
    /// <param name="cacheKeyParameters">The cache key parameters</param>
    public Task Remove(CacheKey cacheKey, params object[] cacheKeyParameters)
    {
        cacheKey = PrepareKey(cacheKey, cacheKeyParameters);
        _memoryCache.Remove(cacheKey.Key);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(CacheKey cacheKey)
    {
        _memoryCache.Remove(cacheKey.Key);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Sets the key
    /// </summary>
    /// <param name="key">The key</param>
    /// <param name="data">The data</param>
    public Task Set(CacheKey key, object data)
    {
        ArgumentNullException.ThrowIfNull(key);
        if (key.CacheTime <= 0 || data == null)
        {
            return Task.CompletedTask;
        }
        _memoryCache.Set(key.Key, data, PrepareEntryOptions(key));
        return Task.CompletedTask;
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
        ArgumentNullException.ThrowIfNull(key);
        CacheKey cacheKey = key;
        if (cacheKey.CacheTime <= 0)
        {
            T obj = await acquire();
            return obj;
        }
        if (_memoryCache.TryGetValue<T>(key.Key, out T? result))
        {
            return result;
        }

        T obj1 = await acquire();
        result = obj1;
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
        CacheKey cacheKey = key;
        if ((cacheKey != null ? cacheKey.CacheTime : 0) <= 0)
        {
            return acquire();
        }

        T? result = _memoryCache.GetOrCreate<T>(
            key.Key,
            entry =>
            {
                entry.SetOptions(PrepareEntryOptions(key));
                return acquire();
            }
        );
        if (result == null)
        {
            await Remove(key, []);
        }
        return result;
    }

    /// <summary>
    /// Gets the key
    /// </summary>
    /// <typeparam name="T">The </typeparam>
    /// <param name="key">The key</param>
    /// <param name="defaultValue">The default value</param>
    /// <returns>The obj</returns>
    public async Task<T?> Get<T>(CacheKey key, T defaultValue)
    {
        Task<T>? value = _memoryCache.Get<Lazy<Task<T>>>(key.Key)?.Value;
        try
        {
            T obj1;
            if (value != null)
            {
                T? obj2 = await value;
                obj1 = obj2;
                obj2 = default;
            }
            else
            {
                obj1 = defaultValue;
            }

            return obj1;
        }
        catch (Exception)
        {
            await Remove(key, Array.Empty<object>());
            throw;
        }
    }

    /// <summary>
    /// Gets the key
    /// </summary>
    /// <typeparam name="T">The </typeparam>
    /// <param name="key">The key</param>
    /// <returns>A task containing the</returns>
    public async Task<T?> Get<T>(CacheKey key)
    {
        await Task.CompletedTask;
        var (isSet, item) = TryGetItem<T>(key);
        if (isSet)
        {
            return item;
        }

        return default;
    }

    /// <summary>
    /// <summary>
    /// Gets the key
    /// </summary>
    /// <param name="key">The key</param>
    /// <returns>The obj</returns>
    public async Task<object?> Get(CacheKey key)
    {
        object? entry = _memoryCache.Get(key.Key);
        if (entry == null)
        {
            return null;
        }

        try
        {
            if (!(entry.GetType().GetProperty("Value")?.GetValue(entry) is Task task))
            {
                return null;
            }

            await task;
            return task.GetType().GetProperty("Result")?.GetValue(task);
        }
        catch (Exception)
        {
            await Remove(key, Array.Empty<object>());
            throw;
        }
    }

    /// <summary>
    /// Describes whether this instance perform action with lock
    /// </summary>
    /// <param name="key">The key</param>
    /// <param name="expirationTime">The expiration time</param>
    /// <param name="action">The action</param>
    /// <returns>The bool</returns>
    public bool PerformActionWithLock(string key, TimeSpan expirationTime, Action action)
    {
        if (_memoryCache.TryGetValue(key, out object? _))
        {
            return false;
        }

        try
        {
            _memoryCache.Set<string>(key, key, expirationTime);
            action();
            return true;
        }
        finally
        {
            _memoryCache.Remove(key);
        }
    }

    /// <summary>
    /// Removes the by prefix using the specified prefix
    /// </summary>
    /// <param name="prefix">The prefix</param>
    /// <param name="prefixParameters">The prefix parameters</param>
    public Task RemoveByPrefix(string prefix, params object[] prefixParameters)
    {
        prefix = PrepareKeyPrefix(prefix, prefixParameters);
        _prefixes.TryRemove(prefix, out CancellationTokenSource? cancellationTokenSource);
        cancellationTokenSource?.Cancel();
        cancellationTokenSource?.Dispose();
        return Task.CompletedTask;
    }

    /// <summary>
    /// Clears this instance
    /// </summary>
    public Task Clear()
    {
        _clearToken.Cancel();
        _clearToken.Dispose();
        _clearToken = new CancellationTokenSource();
        foreach (string key in _prefixes.Keys.ToList<string>())
        {
            _prefixes.TryRemove(key, out CancellationTokenSource? cancellationTokenSource);
            cancellationTokenSource?.Dispose();
        }
        return Task.CompletedTask;
    }

    /// <summary>
    /// Sets the fields hash using the specified key
    /// </summary>
    /// <param name="key">The key</param>
    /// <param name="hashData">The hash data</param>
    /// <param name="cacheTime">The cache time</param>
    /// <exception cref="NotImplementedException"></exception>
    public void SetFieldsHash(string key, HashEntry hashData, int cacheTime = 0)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Gets the field hash using the specified key
    /// </summary>
    /// <typeparam name="T">The </typeparam>
    /// <param name="key">The key</param>
    /// <param name="field">The field</param>
    /// <exception cref="NotImplementedException"></exception>
    /// <returns>The</returns>
    public T GetFieldHash<T>(string key, string field)
        where T : class
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Removes the hash using the specified key
    /// </summary>
    /// <param name="key">The key</param>
    /// <exception cref="NotImplementedException"></exception>
    public Task RemoveHash(string key)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Disposes this instance
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disposes the disposing
    /// </summary>
    /// <param name="disposing">The disposing</param>
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            _memoryCache.Dispose();
        }

        _disposed = true;
    }

    /// <summary>
    /// Tries the get item using the specified key
    /// </summary>
    /// <typeparam name="T">The </typeparam>
    /// <param name="key">The key</param>
    /// <returns>The bool is set item</returns>
    private (bool isSet, T? item) TryGetItem<T>(CacheKey key)
    {
        object? entry = _memoryCache.Get(key.Key);
        if (entry == null)
        {
            return (false, default(T));
        }

        T item = (T)entry;
        return (true, item);
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
        var (isSet, item) = TryGetItem<T>(key);
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
    public Task ClearAll()
    {
        return Clear();
    }

    public async Task<T?> GetOrSet<T>(string key, Func<Task<T>> acquire)
    {
        object? entry = _memoryCache.Get(key);
        if (entry != null)
        {
            return (T?)entry;
        }

        var result = await acquire();
        if (result != null)
        {
            _memoryCache.Set(key, result, TimeSpan.FromMinutes(60));
        }

        return result;
    }
}
