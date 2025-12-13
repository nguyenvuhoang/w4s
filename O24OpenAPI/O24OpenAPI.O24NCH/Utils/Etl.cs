using O24OpenAPI.Core.Logging.Helpers;
using System.Security.Cryptography;
using System.Text;

namespace O24OpenAPI.O24NCH.Utils;

public static class Etl
{
    /// <summary>
    /// Build raw sign string, then hash SHA256 (MessageDigest SHA256).
    /// </summary>
    public static string GenerateSign(
        string spId,
        string transactionId,
        string msisdn,
        string key,
        string url,
        string charset = "utf-8",
        string signType = "SAH256")
    {
        var raw = $"charset={charset}" +
                  $"&spID={spId}" +
                  $"&transactionID={transactionId}" +
                  $"&msisdn={msisdn}" +
                  $"&sign_type={signType}" +
                  $"&key={key}" +
                  $"&url={url}";


        Console.WriteLine($"ETL Sign: {raw}");
        BusinessLogHelper.Info($"ETL Sign: {raw}");

        var bytes = Encoding.UTF8.GetBytes(raw);
        var hash = SHA256.HashData(bytes);

        var sb = new StringBuilder(hash.Length * 2);
        foreach (var b in hash)
        {
            sb.Append(b.ToString("x2"));
        }

        return sb.ToString();
    }

    /// <summary>
    /// (Optional) Trả về luôn cả raw string và sign.
    /// </summary>
    public static (string Raw, string Sign) GenerateSignWithRaw(
        string spId,
        string transactionId,
        string msisdn,
        string key,
        string url,
        string charset = "utf-8",
        string signType = "SAH256")
    {
        var raw = $"charset={charset}" +
                  $"&spID={spId}" +
                  $"&transactionID={transactionId}" +
                  $"&msisdn={msisdn}" +
                  $"&sign_type={signType}" +
                  $"&key={key}" +
                  $"&url={url}";
        var bytes = Encoding.UTF8.GetBytes(raw);
        var hash = SHA256.HashData(bytes);

        var sb = new StringBuilder(hash.Length * 2);
        foreach (var b in hash)
        {
            sb.Append(b.ToString("x2"));
        }

        return (raw, sb.ToString());
    }
}
