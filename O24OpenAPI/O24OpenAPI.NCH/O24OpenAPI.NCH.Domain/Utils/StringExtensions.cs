using Newtonsoft.Json.Linq;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.NCH.Config;
using System.Globalization;
using System.Text.RegularExpressions;

namespace O24OpenAPI.NCH.API.Application.Utils;

/// <summary>
///
/// </summary>
public static partial class StringExtensions
{
    private static string _defaultLanguage;

    private static bool LooksLikeIndexedFormat(string s) => CharacterRegex().IsMatch(s ?? "");

    private static readonly Regex NamedTokenRegex = AlphabetRegex();
    private static string DefaultLanguage
    {
        get
        {
            if (_defaultLanguage == null)
            {
                var nchSetting = EngineContext.Current.Resolve<O24NCHSetting>();
                _defaultLanguage = nchSetting.DefaultLanguage;
            }
            return _defaultLanguage;
        }
    }

    public static string GetLangValue(this string input, string lang = null)
    {
        if (string.IsNullOrEmpty(lang))
        {
            lang = "en";
        }

        if (input.TryParse(out JObject ob))
        {
            var langValue = ob[lang];
            if (langValue.IsEmptyOrNull())
            {
                langValue = ob[DefaultLanguage];
                if (langValue.IsEmptyOrNull())
                {
                    return string.Empty;
                }
            }
            return langValue.ToString();
        }

        return string.Empty;
    }

    public static string GetMessage(
        this string input,
        Dictionary<string, object> parameters,
        string language = "en",
        IFormatProvider formatProvider = null
    )
    {
        if (string.IsNullOrWhiteSpace(input))
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

            if (parameters == null || parameters.Count == 0)
            {
                return langValue;
            }

            if (LooksLikeIndexedFormat(langValue))
            {
                return string.Format(
                    formatProvider ?? CultureInfo.InvariantCulture,
                    langValue,
                    [.. parameters.Values]
                );
            }

            var provider = formatProvider ?? CultureInfo.InvariantCulture;

            string result = NamedTokenRegex.Replace(
                langValue,
                m =>
                {
                    var key = m.Groups["key"].Value;
                    var fmt = m.Groups["fmt"]?.Value;

                    if (!parameters.TryGetValue(key, out var raw) || raw is null)
                    {
                        return "";
                    }

                    if (fmt?.Length > 0 && raw is IFormattable formattable)
                    {
                        return formattable.ToString(fmt, provider) ?? "";
                    }

                    if (raw is IFormattable f)
                    {
                        return f.ToString(null, provider) ?? "";
                    }

                    return raw.ToString() ?? "";
                }
            );

            return result;
        }
        catch
        {
            return string.Empty;
        }
    }

    public static string AutoFormatBullets(string text)
    {
        var lines = text.Split('\n')
            .Select(l => l.Trim())
            .Where(l => !string.IsNullOrWhiteSpace(l))
            .Select(l => "- " + l);

        return string.Join("\n", lines);
    }


    [GeneratedRegex(@"\{[0-9]+(?:[:}])")]
    private static partial Regex CharacterRegex();

    [GeneratedRegex(@"\{(?<key>[A-Za-z0-9_]+)(?::(?<fmt>[^}]+))?\}", RegexOptions.Compiled)]
    private static partial Regex AlphabetRegex();
}

