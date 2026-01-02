
using System.Text.Json;
namespace O24OpenAPI.W4S.API.Application.Helpers
{
    public static class LocalizationHelper
    {
        /// <summary>
        /// Get localized value from JSON string
        /// </summary>
        /// <param name="json"></param>
        /// <param name="language"></param>
        /// <param name="fallbackLanguage"></param>
        /// <returns></returns>
        public static string GetLocalizedValue(
            string json,
            string language,
            string fallbackLanguage = "en"
        )
        {
            if (string.IsNullOrWhiteSpace(json))
                return string.Empty;

            try
            {
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                if (!string.IsNullOrWhiteSpace(language) &&
                    root.TryGetProperty(language, out var langProp))
                {
                    return langProp.GetString() ?? string.Empty;
                }

                if (root.TryGetProperty(fallbackLanguage, out var fallbackProp))
                {
                    return fallbackProp.GetString() ?? string.Empty;
                }
            }
            catch
            {
                return json;
            }

            return string.Empty;
        }
    }

}
