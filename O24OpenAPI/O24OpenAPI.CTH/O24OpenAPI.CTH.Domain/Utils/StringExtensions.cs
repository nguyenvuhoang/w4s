using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Utils.O9;
using QRCoder;

namespace O24OpenAPI.CTH.API.Application.Utils;

/// <summary>
/// The string extensions class
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Tries the parse using the specified s
    /// </summary>
    /// <typeparam name="T">The </typeparam>
    /// <param name="s">The </param>
    /// <param name="result">The result</param>
    /// <returns>The bool</returns>
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

    /// <summary>
    /// Gets the lang value using the specified input
    /// </summary>
    /// <param name="input">The input</param>
    /// <param name="lang">The lang</param>
    /// <returns>The string</returns>
    public static string GetLangValue(this string input, string lang = "en")
    {
        if (input.TryParse(out JObject ob))
        {
            var langValue = ob[lang];
            if (langValue.IsEmptyOrNull())
            {
                langValue = ob["en"];
                if (langValue.IsEmptyOrNull())
                {
                    return string.Empty;
                }
            }
            return langValue.ToString();
        }

        return string.Empty;
    }

    public static string TryGetLabelFromJson(string json, string lang)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            return "";
        }

        try
        {
            var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            return dict.TryGetValue(lang, out var value) ? value : "";
        }
        catch
        {
            return "";
        }
    }

    public static byte[] GenerateQRCodeBytes(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            throw new ArgumentException("Content cannot be null or empty.", nameof(content));
        }

        using var qrGenerator = new QRCodeGenerator();
        var qrData = qrGenerator.CreateQrCode(content, QRCodeGenerator.ECCLevel.Q);
        var qrCode = new PngByteQRCode(qrData);
        return qrCode.GetGraphic(20);
    }
}
