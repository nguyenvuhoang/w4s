namespace O24OpenAPI.Core.Utils;

/// <summary>
/// The guild utils class
/// </summary>
public class GuildUtils
{
    /// <summary>
    /// Gets the new string guild
    /// </summary>
    /// <returns>The string</returns>
    public static string GetNewStringGuild()
    {
        return Guid.NewGuid().ToString();
    }
}
