namespace O24OpenAPI.Core.Extensions;

/// <summary>
/// The date time extensions class
/// </summary>
public static class DateTimeExtensions
{
    /// <summary>
    /// Returns the date time offset using the specified unix time milliseconds
    /// </summary>
    /// <param name="unixTimeMilliseconds">The unix time milliseconds</param>
    /// <returns>The date time offset</returns>
    public static DateTimeOffset ToDateTimeOffset(this long unixTimeMilliseconds)
    {
        var dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(unixTimeMilliseconds);
        return dateTimeOffset;
    }
}
