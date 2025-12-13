namespace O24OpenAPI.Core.Utils;

/// <summary>
/// The json utils class
/// </summary>
public static class JsonUtils
{
    /// <summary>
    /// Serializes the obj
    /// </summary>
    /// <param name="obj">The obj</param>
    /// <returns>The string</returns>
    public static string Serialize(object obj)
    {
        return System.Text.Json.JsonSerializer.Serialize(obj);
    }
}
