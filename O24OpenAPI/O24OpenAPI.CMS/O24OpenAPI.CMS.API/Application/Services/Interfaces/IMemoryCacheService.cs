namespace O24OpenAPI.CMS.API.Application.Services.Interfaces;

public interface IMemoryCacheService
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="key"></param>
    void ResetCache(string key);

    /// <summary>
    ///
    /// </summary>
    /// <param name="key"></param>
    /// <typeparam name="TItem"></typeparam>
    /// <returns></returns>
    TItem GetCache<TItem>(string key);

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="cacheKey"></param>
    /// <param name="factory"></param>
    /// <param name="absoluteExpirationRelativeToNow"></param>
    /// <param name="slidingExpiration"></param>
    /// <returns></returns>
    Task<T> GetOrCreateAsync<T>(
        string cacheKey,
        Func<Task<T>> factory,
        TimeSpan absoluteExpirationRelativeToNow = default,
        TimeSpan slidingExpiration = default
    );

    /// <summary>
    /// Gets the or create using the specified cache key
    /// </summary>
    /// <typeparam name="T">The </typeparam>
    /// <param name="cacheKey">The cache key</param>
    /// <param name="factory">The factory</param>
    /// <param name="absoluteExpirationRelativeToNow">The absolute expiration relative to now</param>
    /// <param name="slidingExpiration">The sliding expiration</param>
    /// <returns>The</returns>
    T GetOrCreate<T>(
        string cacheKey,
        Func<T> factory,
        TimeSpan absoluteExpirationRelativeToNow = default,
        TimeSpan slidingExpiration = default
    );
}
