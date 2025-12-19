using System.Globalization;

namespace O24OpenAPI.CMS.API.Application.Utils;

public static class NumberParser
{
    private static readonly CultureInfo CustomCulture;

    static NumberParser()
    {
        CustomCulture = new CultureInfo("en-US");
        CustomCulture.NumberFormat.NumberDecimalSeparator = ".";
        CustomCulture.NumberFormat.NumberGroupSeparator = ",";
    }

    /// <summary>
    ///
    /// </summary>
    public static bool TryParseDouble(string input, out double result)
    {
        return double.TryParse(input, NumberStyles.Number, CustomCulture, out result);
    }

    /// <summary>
    ///
    /// </summary>
    public static string ToString(double value)
    {
        return value.ToString(CustomCulture);
    }
}
