using System.Globalization;

namespace O24OpenAPI.W4S.API.Application.Helpers
{
    public static class IconHelper
    {
        public static string ToFaIcon(string icon)
        {
            if (string.IsNullOrWhiteSpace(icon))
                return null;

            var textInfo = CultureInfo.InvariantCulture.TextInfo;

            var parts = icon.Split('-', StringSplitOptions.RemoveEmptyEntries)
                .Select(p => textInfo.ToTitleCase(p.ToLowerInvariant()));

            return "fa" + string.Concat(parts);
        }
    }
}
