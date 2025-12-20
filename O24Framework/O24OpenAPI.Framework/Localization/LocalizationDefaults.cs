using O24OpenAPI.Core.Caching;
using O24OpenAPI.Core.Domain.Localization;

namespace O24OpenAPI.Framework.Localization;

/// <summary>
/// The localization defaults class
/// </summary>
public static class LocalizationDefaults
{
    /// <summary>
    /// Gets the value of the locale strign resources by name cache key
    /// </summary>
    public static CacheKey LocaleStringResourcesByNameCacheKey
    {
        get
        {
            return new CacheKey(
                "neptune.localestringresource.byname.{0}-{1}",
                new string[2]
                {
                    LocalizationDefaults.LocaleStringResourcesByNamePrefix,
                    O24OpenAPIEntityCacheDefaults<LocaleStringResource>.Prefix,
                }
            );
        }
    }

    /// <summary>
    /// Gets the value of the locale string resources by name prefix
    /// </summary>
    public static string LocaleStringResourcesByNamePrefix
    {
        get => new PrefixKey("neptune.localestringresource.byname.{0}").Key;
    }
}
