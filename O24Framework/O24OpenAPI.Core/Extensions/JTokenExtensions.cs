using Newtonsoft.Json.Linq;

namespace O24OpenAPI.Core.Extensions;

/// <summary>
/// The token extensions class
/// </summary>
public static class JTokenExtensions
{
    /// <summary>
    /// Is the empty or null using the specified token
    /// </summary>
    /// <param name="token">The token</param>
    /// <returns>The bool</returns>
    public static bool IsEmptyOrNull(this JToken token)
    {
        return (token == null)
            || (token.Type == JTokenType.Array && !token.HasValues)
            || (token.Type == JTokenType.Object && !token.HasValues)
            || (token.Type == JTokenType.String && string.IsNullOrWhiteSpace(token.ToString()))
            || (token.Type == JTokenType.Null)
            || (token.Type == JTokenType.Undefined);
    }
}
