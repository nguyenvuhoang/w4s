using System.Globalization;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Web.CMS.Configuration;

namespace O24OpenAPI.Web.CMS.Utils;

/// <summary>
///
/// </summary>
public static class StringExtensions
{
    private static string _defaultLanguage;

    private static string DefaultLanguage
    {
        get
        {
            if (_defaultLanguage == null)
            {
                var cmsSetting = EngineContext.Current.Resolve<CMSSetting>();
                _defaultLanguage = cmsSetting.DefaultLanguage;
            }
            return _defaultLanguage;
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static string CapitalizerFirstLetter(this string s)
    {
        return char.ToUpper(s[0]) + s.Substring(1);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string CapitalizerFirstLetterRemoveUnderScore(this string str)
    {
        var rs = "";
        foreach (var s in str.Split("_"))
        {
            rs += char.ToUpper(s[0]) + s.Substring(1);
        }
        return rs;
    }

    /// <summary>
    /// Returns a value contain in string
    /// </summary>
    /// <param name="value"></param>
    /// <param name="values"></param>
    /// <returns></returns>
    public static bool In(this string value, string[] values)
    {
        return values.Contains(value);
    }

    /// <summary>
    /// Write line value console
    /// </summary>
    /// <param name="value"></param>
    public static void Print(this string value)
    {
        System.Console.WriteLine(value);
    }

    /// <summary>
    /// Convert string to title case (text sample -> Text Sample)
    /// </summary>
    /// <param name="str"></param>
    /// <param name="culTureInfo"></param>
    /// <returns></returns>
    public static string ToTitleCase(this string str, string culTureInfo = "en-US")
    {
        string strReturn = str;
        System.Globalization.TextInfo info = new System.Globalization.CultureInfo(
            culTureInfo,
            false
        ).TextInfo;
        return info.ToTitleCase(strReturn);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="str"></param>
    /// <param name="culTureInfo"></param>
    /// <returns></returns>
    public static string ToTitleCaseRemoveUnderScore(this string str, string culTureInfo = "en-US")
    {
        string strReturn = str;
        System.Globalization.TextInfo info = new System.Globalization.CultureInfo(
            culTureInfo,
            false
        ).TextInfo;
        return info.ToTitleCase(strReturn).Replace("_", "");
    }

    /// <summary>
    /// Convert string to Underscore case (Snack case)
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string ToUnderscoreCase(this string str)
    {
        return string.Concat(
                str.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())
            )
            .ToLower();
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="str"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static T MapToModel<T>(this string str)
    {
        try
        {
            return System.Text.Json.JsonSerializer.Deserialize<T>(str);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="st"></param>
    /// <returns></returns>
    public static bool IsNumeric(this string st)
    {
        return int.TryParse(st, out int n);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="st"></param>
    /// <returns></returns>
    public static string RemoveWhiteSpaceAndUpper(this string st)
    {
        return st.ToUpper().Replace(" ", "").Trim();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="input"></param>
    /// <param name="mask"></param>
    /// <returns></returns>
    public static string ApplyMaskAccountNumber(this string input, string mask)
    {
        if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(mask))
        {
            return input;
        }

        var result = new char[mask.Length];
        int inputIndex = 0;

        for (int i = 0; i < mask.Length; i++)
        {
            if (mask[i] == '_' && inputIndex < input.Length)
            {
                result[i] = input[inputIndex++];
            }
            else
            {
                result[i] = mask[i];
            }
        }

        return new string(result);
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public static DateTime ToDateTime(
        this string input,
        string format,
        DateTime? defaultValue = null
    )
    {
        try
        {
            return DateTime.ParseExact(
                input,
                format,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None
            );
        }
        catch (FormatException)
        {
            return defaultValue ?? DateTime.MinValue;
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="input"></param>
    public static void LogError(this string input)
    {
        input.LogWithColor(ConsoleColor.Red);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="input"></param>
    /// <param name="color"></param>
    public static void LogWithColor(this string input, ConsoleColor color)
    {
        var previousColor = Console.ForegroundColor;

        try
        {
            Console.ForegroundColor = color;
            Console.WriteLine(input);
        }
        finally
        {
            Console.ForegroundColor = previousColor;
        }
    }

    public static Dictionary<string, object> ToDictionary(this string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return new Dictionary<string, object>();
        }
        return JsonConvert.DeserializeObject<Dictionary<string, object>>(input);
    }

    public static string GetMessage(
        this string input,
        Dictionary<string, object> parameters,
        string language = "en"
    )
    {
        if (string.IsNullOrEmpty(input))
        {
            return string.Empty;
        }

        try
        {
            var langValue = input.GetLangValue(language);
            if (string.IsNullOrEmpty(langValue))
            {
                return string.Empty;
            }

            if (parameters != null && parameters.Count != 0)
            {
                return string.Format(langValue, parameters.Values.ToArray());
            }
            return langValue;
        }
        catch
        {
            return string.Empty;
        }
    }

    public static string GetLangValueDefault(
        this string input,
        string defaultValue,
        string lang = null
    )
    {
        try
        {
            if (string.IsNullOrEmpty(lang))
            {
                lang = Utils.GetRequestLanguage();
            }

            if (input.TryParse(out JObject ob))
            {
                var langValue = ob[lang];
                if (langValue.IsEmptyOrNull())
                {
                    langValue = ob[DefaultLanguage];
                    if (langValue.IsEmptyOrNull())
                    {
                        return defaultValue;
                    }
                }
                return langValue.ToString();
            }

            return defaultValue;
        }
        catch
        {
            return defaultValue;
        }
    }

    public static bool TryParse<T>(this string s, out T result)
    {
        result = default;

        if (string.IsNullOrWhiteSpace(s))
        {
            return false;
        }

        try
        {
            if (typeof(T) == typeof(JObject))
            {
                var jObject = JObject.Parse(s);
                result = (T)(object)jObject;
                return true;
            }
            else if (typeof(T) == typeof(JArray))
            {
                var jArray = JArray.Parse(s);
                result = (T)(object)jArray;
                return true;
            }
            else
            {
                result = JsonConvert.DeserializeObject<T>(s);
                return true;
            }
        }
        catch
        {
            return false;
        }
    }

    public static JObject ToObjectSuccess(this string stringObject)
    {
        return new JObject { ["status"] = stringObject };
    }

    public static string ToLowerCase(this string s)
    {
        if (string.IsNullOrEmpty(s))
        {
            return string.Empty;
        }

        return Regex.Replace(s, @"\s+", "").ToLower();
    }
}

/// <summary>
/// OptionFormatString
/// </summary>
public enum OptionFormatString
{
    /// <summary>
    /// Default
    /// </summary>
    Default = 0,

    /// <summary>
    /// UnderscoreCase (Snack case)
    /// </summary>
    UnderscoreCase = 1,

    /// <summary>
    /// TitleCase
    /// </summary>
    TitleCase = 2,
}
