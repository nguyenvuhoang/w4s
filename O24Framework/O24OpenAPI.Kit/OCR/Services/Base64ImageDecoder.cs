using System.Text.RegularExpressions;

public static class Base64ImageDecoder
{
    public static byte[] DecodeToBytes(string base64)
    {
        if (string.IsNullOrWhiteSpace(base64))
            throw new ArgumentException("Base64 is empty");

        var s = base64.Trim();

        // data:image/png;base64,xxxx
        var commaIndex = s.IndexOf(',');
        if (s.StartsWith("data:", StringComparison.OrdinalIgnoreCase) && commaIndex >= 0)
            s = s[(commaIndex + 1)..];

        // remove ALL whitespace (space/tab/newline)
        s = Regex.Replace(s, @"\s+", "");

        // base64url -> base64
        s = s.Replace('-', '+').Replace('_', '/');

        // pad nếu thiếu '='
        var mod = s.Length % 4;
        if (mod == 2) s += "==";
        else if (mod == 3) s += "=";
        else if (mod == 1) throw new FormatException("Invalid base64 length (likely truncated).");

        // validate + decode
        try
        {
            return Convert.FromBase64String(s);

        }
        catch (FormatException ex)
        {
            throw new FormatException("Invalid base64 format. (Possible truncated/illegal characters)", ex);
        }
    }
}
