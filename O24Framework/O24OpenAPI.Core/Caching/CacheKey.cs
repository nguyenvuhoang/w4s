using O24OpenAPI.Core.Configuration;
using O24OpenAPI.Core.Infrastructure;

namespace O24OpenAPI.Core.Caching;

/// <summary>
/// The cache key class
/// </summary>
public class CacheKey
{
    /// <summary>
    /// The template
    /// </summary>
    private readonly string? _template;

    /// <summary>
    /// The prefixes
    /// </summary>
    private readonly List<string>? _prefixes;

    /// <summary>
    /// Initializes with a template string
    /// </summary>
    /// <param name="template">The key template (e.g., "{0}:data" or "user:123")</param>
    public CacheKey(string template, params object[] keyObjects)
    {
        if (string.IsNullOrWhiteSpace(template))
        {
            throw new ArgumentException("Template cannot be empty.", nameof(template));
        }

        _template = template;
        _prefixes = [];
        Key = GetFullKey(keyObjects);
    }

    /// <summary>
    /// Initializes with a list of prefixes
    /// </summary>
    /// <param name="prefixes">List of prefixes (e.g., ["app1", "user"])</param>
    public CacheKey(List<string> prefixes)
    {
        if (prefixes == null || prefixes.Count == 0)
        {
            throw new ArgumentException("Prefixes list cannot be empty.", nameof(prefixes));
        }

        _template = null;
        _prefixes = prefixes.Where(p => !string.IsNullOrEmpty(p)).ToList();
        Key = GetFullKey();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CacheKey"/> class
    /// </summary>
    /// <param name="key">The key</param>
    /// <param name="prefixes">The prefixes</param>
    public CacheKey(string key, params string[] prefixes)
    {
        Key = key;
        Prefixes.AddRange(prefixes.Where(prefix => !string.IsNullOrEmpty(prefix)));
    }

    public CacheKey(string key)
    {
        Key = key;
    }

    /// <summary>
    /// /// Gets or sets the value of the key
    /// </summary>
    public string Key { get; protected set; } = string.Empty;

    /// <summary>
    /// Gets or sets the value of the prefixes
    /// </summary>
    public List<string> Prefixes { get; protected set; } = [];

    /// <summary>
    /// /// Gets or sets the value of the cache time
    /// </summary>
    public int CacheTime { get; set; } =
        Singleton<AppSettings>.Instance.Get<CacheConfig>().DefaultCacheTime;

    /// <summary>
    /// Gets or sets the value of the is forever
    /// </summary>
    public bool IsForever { get; set; } = false;

    /// <summary>
    /// Creates the create cache key parameters
    /// </summary>
    /// <param name="createCacheKeyParameters">The create cache key parameters</param>
    /// <param name="keyObjects">The key objects</param>
    /// <returns>The cache key</returns>
    public virtual CacheKey Create(
        Func<object, object> createCacheKeyParameters,
        params object[] keyObjects
    )
    {
        CacheKey cacheKey = new(Key, [.. Prefixes]);
        if (keyObjects.Length == 0)
        {
            return cacheKey;
        }

        cacheKey.Key = string.Format(
            cacheKey.Key,
            [.. keyObjects.Select(createCacheKeyParameters)]
        );
        for (int index = 0; index < cacheKey.Prefixes.Count; ++index)
        {
            cacheKey.Prefixes[index] = string.Format(
                cacheKey.Prefixes[index],
                [.. keyObjects.Select(createCacheKeyParameters)]
            );
        }

        return cacheKey;
    }

    /// <summary>
    /// Creates a scan pattern for Redis based on the prefixes or template
    /// </summary>
    public string CreateScanPattern()
    {
        if (_template != null)
        {
            return $"{_template}:*".ToLower();
        }
        else if (_prefixes?.Count > 0)
        {
            return $"{string.Join(":", _prefixes)}:*".ToLower();
        }
        throw new InvalidOperationException("No template or prefixes provided.");
    }

    /// <summary>
    /// Gets the full key based on template or prefixes
    /// </summary>
    /// <param name="keyObjects">Optional parameters to format the template</param>
    public string GetFullKey(params object[] keyObjects)
    {
        if (_template != null)
        {
            return keyObjects.Length > 0
                ? string.Format(_template, keyObjects).ToLowerInvariant()
                : _template.ToLowerInvariant();
        }
        else if (_prefixes?.Count > 0)
        {
            var parts = new List<string>(_prefixes);
            if (keyObjects.Length > 0)
            {
                var partsToAdd = keyObjects
                    .Select(o => o?.ToString() ?? string.Empty)
                    .Where(s => !string.IsNullOrWhiteSpace(s));
                parts.AddRange(partsToAdd);
            }
            return string.Join(":", parts).ToLowerInvariant();
        }
        throw new InvalidOperationException("No template or prefixes provided.");
    }
}
