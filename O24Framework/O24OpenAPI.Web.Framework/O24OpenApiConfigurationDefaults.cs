using O24OpenAPI.Core.Caching;
using O24OpenAPI.Core.Domain.Configuration;

namespace O24OpenAPI.Web.Framework;

/// <summary>
/// The 24 open api configuration defaults class
/// </summary>
public static class O24OpenApiConfigurationDefaults
{
    /// <summary>
    /// Gets the value of the settings all as dictionary cache key
    /// </summary>
    public static CacheKey SettingsAllAsDictionaryCacheKey
    {
        get
        {
            return new CacheKey(
                new PrefixKey("jits.neptune.core.domain.configuration.setting.all.dictionary.").Key,
                [O24OpenAPIEntityCacheDefaults<Setting>.Prefix]
            );
        }
    }
}
