using System.Globalization;
using System.Text.RegularExpressions;

namespace O24OpenAPI.NCH.API.Application.Utils;

public static partial class NotificationUtils
{
    public static string GetMessageNotification(
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
            string langValue = input.GetLangValue(language);
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

            IFormatProvider provider = formatProvider ?? CultureInfo.InvariantCulture;

            string result = NamedTokenRegex.Replace(
                langValue,
                m =>
                {
                    string key = m.Groups["key"].Value;
                    string fmt = m.Groups["fmt"]?.Value;

                    if (!parameters.TryGetValue(key, out object raw) || raw is null)
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

    private static bool LooksLikeIndexedFormat(string s) => CharacterRegex().IsMatch(s ?? "");

    [GeneratedRegex(@"\{[0-9]+(?:[:}])")]
    private static partial Regex CharacterRegex();

    private static readonly Regex NamedTokenRegex = AlphabetRegex();

    [GeneratedRegex(@"\{(?<key>[A-Za-z0-9_]+)(?::(?<fmt>[^}]+))?\}", RegexOptions.Compiled)]
    private static partial Regex AlphabetRegex();
}
