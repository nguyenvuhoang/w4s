namespace O24OpenAPI.O24ACT.Utils;

public static class StringExtensions
{
    /// <summary>
    /// Convert string to Underscore case (Snack case)
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string ToUnderscoreCase(this string str)
    {
        return string.Concat(str.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToLower();
    }
}
