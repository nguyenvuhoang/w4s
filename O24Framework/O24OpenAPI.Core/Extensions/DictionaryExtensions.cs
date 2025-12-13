namespace O24OpenAPI.Core.Extensions;

/// <summary>
/// The dictionary extensions class
/// </summary>
public static class DictionaryExtensions
{
    /// <summary>
    /// Gets the string value using the specified dic
    /// </summary>
    /// <param name="dic">The dic</param>
    /// <param name="key">The key</param>
    /// <param name="defaultValue">The default value</param>
    /// <returns>The default value</returns>
    public static string GetStringValue(
        this Dictionary<string, object> dic,
        string key,
        string defaultValue = ""
    )
    {
        return dic.TryGetValue(key, out var value)
            ? value?.ToString() ?? defaultValue
            : defaultValue;
    }
}
