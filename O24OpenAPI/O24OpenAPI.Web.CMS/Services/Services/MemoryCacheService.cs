using Microsoft.Extensions.Caching.Memory;
using O24OpenAPI.Web.CMS.Services.Interfaces;

namespace O24OpenAPI.Web.CMS.Services.Services;

public class MemoryCacheService : IMemoryCacheService
{
    /// <summary>
    /// The memory cache
    /// </summary>
    private readonly IMemoryCache _memoryCache;

    /// <summary>
    /// 
    /// </summary>
    public MemoryCacheService(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    /// <summary>
    /// Set a cache entry.
    /// </summary>
    public void SetCache<TItem>(string key, TItem value, MemoryCacheEntryOptions options)
    {
        _memoryCache.Set(key, value, options);
    }

    /// <summary>
    /// Get a cache entry.
    /// </summary>
    public TItem GetCache<TItem>(string key)
    {
        return _memoryCache.TryGetValue(key, out TItem value) ? value : default;
    }

    /// <summary>
    /// Remove a cache entry.
    /// </summary>
    public void ResetCache(string key)
    {
        _memoryCache.Remove(key);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="cacheKey"></param>
    /// <param name="factory"></param>
    /// <param name="absoluteExpirationRelativeToNow"></param>
    /// <param name="slidingExpiration"></param>
    /// <returns></returns>
    public async Task<T> GetOrCreateAsync<T>(string cacheKey, Func<Task<T>> factory, TimeSpan absoluteExpirationRelativeToNow = default, TimeSpan slidingExpiration = default)
    {
        if (absoluteExpirationRelativeToNow == default)
        {
            absoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60);
        }

        if (slidingExpiration == default)
        {
            slidingExpiration = TimeSpan.FromMinutes(10);
        }

        if (!_memoryCache.TryGetValue(cacheKey, out T cacheEntry))
        {
            cacheEntry = await factory();
            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = absoluteExpirationRelativeToNow,
                SlidingExpiration = slidingExpiration
            };

            SetCache(cacheKey, cacheEntry, cacheEntryOptions);
        }

        return cacheEntry;
    }
    
    
    /// <summary>
    /// Gets the or create using the specified cache key
    /// </summary>
    /// <typeparam name="T">The </typeparam>
    /// <param name="cacheKey">The cache key</param>
    /// <param name="factory">The factory</param>
    /// <param name="absoluteExpirationRelativeToNow">The absolute expiration relative to now</param>
    /// <param name="slidingExpiration">The sliding expiration</param>
    /// <returns>The cache entry</returns>
    public  T GetOrCreate<T>(string cacheKey, Func<T> factory, TimeSpan absoluteExpirationRelativeToNow = default, TimeSpan slidingExpiration = default)
    {
        if (absoluteExpirationRelativeToNow == default)
        {
            absoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60);
        }

        if (slidingExpiration == default)
        {
            slidingExpiration = TimeSpan.FromMinutes(10);
        }

        if (!_memoryCache.TryGetValue(cacheKey, out T cacheEntry))
        {
            cacheEntry =  factory();
            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = absoluteExpirationRelativeToNow,
                SlidingExpiration = slidingExpiration
            };

            SetCache(cacheKey, cacheEntry, cacheEntryOptions);
        }

        return cacheEntry;
    }
}