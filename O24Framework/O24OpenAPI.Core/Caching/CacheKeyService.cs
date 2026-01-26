using System.Globalization;
using System.Text;
using O24OpenAPI.Core.Configuration;
using O24OpenAPI.Core.Helper;

namespace O24OpenAPI.Core.Caching;

/// <summary>
/// The cache key service class
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="CacheKeyService"/> class
/// </remarks>
/// <param name="appSettings">The app settings</param>
public abstract class CacheKeyService(AppSettings appSettings)
{
    protected readonly AppSettings _appSettings = appSettings;
    private static string HashAlgorithm => "SHA1";

    protected virtual string PrepareKeyPrefix(string prefix, params object[] prefixParameters)
    {
        return prefixParameters == null || prefixParameters.Length == 0
            ? prefix
            : string.Format(
                prefix,
                [.. prefixParameters.Select(new Func<object, object>(CreateCacheKeyParameters))]
            );
    }

    protected virtual string CreateIdsHash(IEnumerable<int> ids)
    {
        List<int> list = [.. ids];
        return list.Count == 0
            ? string.Empty
            : HashHelper.CreateHash(
                Encoding.UTF8.GetBytes(string.Join<int>(", ", list.OrderBy<int, int>(id => id))),
                HashAlgorithm
            );
    }

    protected virtual object CreateCacheKeyParameters(object parameter)
    {
        object cacheKeyParameters = parameter switch
        {
            null => "null",
            IEnumerable<int> ids => CreateIdsHash(ids),
            IEnumerable<BaseEntity> source => CreateIdsHash(source.Select(entity => entity.Id)),
            BaseEntity baseEntity => baseEntity.Id,
            decimal num => num.ToString(CultureInfo.InvariantCulture),
            _ => parameter,
        };
        return cacheKeyParameters;
    }

    public virtual CacheKey PrepareKey(CacheKey cacheKey, params object[] cacheKeyParameters)
    {
        return cacheKey.Create(
            new Func<object, object>(CreateCacheKeyParameters),
            cacheKeyParameters
        );
    }

    public virtual CacheKey PrepareKeyForDefaultCache(
        CacheKey cacheKey,
        params object[] cacheKeyParameters
    )
    {
        CacheKey cacheKey1 = cacheKey.Create(
            new Func<object, object>(CreateCacheKeyParameters),
            cacheKeyParameters
        );
        cacheKey1.CacheTime = _appSettings.Get<CacheConfig>()?.DefaultCacheTime ?? 60;
        return cacheKey1;
    }

    public virtual CacheKey PrepareKeyForNeptuneCache(
        CacheKey cacheKey,
        params object[] cacheKeyParameters
    )
    {
        CacheKey cacheKey1 = cacheKey.Create(
            new Func<object, object>(CreateCacheKeyParameters),
            cacheKeyParameters
        );
        cacheKey1.CacheTime = _appSettings.Get<CacheConfig>()?.NeptuneCacheTime ?? 60;
        return cacheKey1;
    }

    public virtual CacheKey PrepareKeyForShortTermCache(
        CacheKey cacheKey,
        params object[] cacheKeyParameters
    )
    {
        CacheKey cacheKey1 = cacheKey.Create(
            new Func<object, object>(CreateCacheKeyParameters),
            cacheKeyParameters
        );
        cacheKey1.CacheTime = _appSettings.Get<CacheConfig>()?.ShortTermCacheTime ?? 60;
        return cacheKey1;
    }
}
