using O24OpenAPI.Core.Extensions;

namespace O24OpenAPI.Core.Caching;

public class CachingKey
{
    public const string SessionTemplate = "session:{0}";
    public const string ServiceTemplate = "service:{0}:{1}";
    public const string EntityTemplate = "entity:{0}:{1}";
    public const string RefreshTokenTemplate = "refresh_token:{0}:{1}";

    public static CacheKey SessionKey(string token) =>
        new(SessionTemplate, new object[] { token.Hash() });

    public static CacheKey SessionKeyNoHash(string token) =>
       new(SessionTemplate, new object[] { token });

    public static CacheKey EntityKey<T>(params string[] values) =>
        new(EntityTemplate, new object[] { typeof(T).Name, string.Join(":", values) });

    public static CacheKey ChannelRolesKey(int roleId) =>
        new(EntityTemplate, new object[] { "CTH", $"roleid:{roleId}" });
}
