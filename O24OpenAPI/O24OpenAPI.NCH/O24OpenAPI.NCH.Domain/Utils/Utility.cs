using MimeKit;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace O24OpenAPI.NCH.API.Application.Utils;

public class Utility
{
    public static string EncryptOTP(string otp)
    {
        using SHA256 sha256 = SHA256.Create();
        byte[] bytes = Encoding.UTF8.GetBytes(otp);
        byte[] hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }


    public static string ReplaceData(string para, Dictionary<string, object> data)
    {
        if (para == null || data == null)
        {
            return para;
        }

        string pattern = "@\\{([^\\{]*)\\}";

        foreach (Match match in Regex.Matches(para, pattern))
        {
            if (match.Success && match.Groups.Count > 0)
            {
                var text = match.Groups[1].Value;
                try
                {
                    var value = JObject.Parse(System.Text.Json.JsonSerializer.Serialize(data));
                    if (value.SelectToken(text) != null)
                    {
                        para = para.Replace(
                            match.Groups[0].Value,
                            value.SelectToken(text).ToString()
                        );
                    }
                }
                catch (System.Exception ex)
                {
                    // TODO
                    System.Console.WriteLine(
                        "CANT FIND "
                            + text
                            + " IN DATA SAMPLE "
                            + System.Text.Json.JsonSerializer.Serialize(data)
                            + ex.StackTrace
                    );
                }
            }
        }

        return para;
    }

    public static MimePart ConvertBase64ToMimeEntity(
           string base64String,
           string contentType,
           string contentId
       )
    {
        byte[] fileBytes = Convert.FromBase64String(base64String);
        var memoryStream = new MemoryStream(fileBytes);
        var mimePart = new MimePart(contentType)
        {
            Content = new MimeContent(memoryStream),
            ContentDisposition = new ContentDisposition(ContentDisposition.Inline),
            ContentTransferEncoding = ContentEncoding.Base64,
            ContentId = contentId,
        };

        return mimePart;
    }

    public static string FormatAmount(decimal amount, string currencyCode)
    {
        if (string.Equals(currencyCode, "LAK", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(currencyCode, "VND", StringComparison.OrdinalIgnoreCase))
        {
            return $"{amount:N0} {currencyCode}";
        }

        return $"{amount:N2} {currencyCode}";
    }

    public static string ReplaceTokens(string template, object data)
    {
        if (data is string s)
        {
            data = JsonConvert.DeserializeObject<Dictionary<string, object>>(s);
        }

        var dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(JsonConvert.SerializeObject(data)) ?? [];
        foreach (var kv in dict)
        {
            template = template.Replace($"{{{kv.Key}}}", kv.Value?.ToString() ?? "");
        }
        return template;
    }
}
