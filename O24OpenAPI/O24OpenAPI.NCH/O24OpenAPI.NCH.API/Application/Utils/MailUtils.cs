using System.Text.RegularExpressions;

namespace O24OpenAPI.NCH.API.Application.Utils;

public static class MailUtils
{
    public static bool IsValidEmail(this string email)
    {
        string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        Regex regex = new(pattern);

        return regex.IsMatch(email);
    }
}
