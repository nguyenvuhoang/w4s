namespace O24OpenAPI.Core;

/// <summary>
/// The utility class
/// </summary>
public static class Utility
{
    /// <summary>
    /// Coalesces the values
    /// </summary>
    /// <typeparam name="T">The </typeparam>
    /// <param name="values">The values</param>
    /// <returns>The</returns>
    public static T Coalesce<T>(params T[] values)
        where T : class
    {
        foreach (var value in values)
        {
            if (value != null)
            {
                return value;
            }
        }
        return null;
    }

    /// <summary>
    /// Coalesces the values
    /// </summary>
    /// <typeparam name="T">The </typeparam>
    /// <param name="values">The values</param>
    /// <returns>The</returns>
    public static T? Coalesce<T>(params T?[] values)
        where T : struct
    {
        foreach (var value in values)
        {
            if (value.HasValue)
            {
                return value.Value;
            }
        }
        return null;
    }
}
