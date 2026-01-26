using System.Security.Cryptography;
using System.Text;

namespace O24OpenAPI.CTH.API.Application.Utils;

public static class OtpCryptoUtil
{

    public static string EncryptSmartOTP(string key, string otp)
    {
        string SecretKey = key;
        var plainText = $"{key}|{otp}";

        using var aes = Aes.Create();
        var keyBytes = Encoding.UTF8.GetBytes(SecretKey.PadRight(32).Substring(0, 32));
        aes.Key = keyBytes;
        aes.IV = new byte[16];

        using var encryptor = aes.CreateEncryptor();
        var plainBytes = Encoding.UTF8.GetBytes(plainText);
        var encrypted = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

        return Convert.ToBase64String(encrypted);
    }

    public static (string key, string otp)? DecryptSmartOTP(string key, string encryptedText)
    {
        try
        {
            using var aes = Aes.Create();
            var keyBytes = Encoding.UTF8.GetBytes(key.PadRight(32).Substring(0, 32));
            aes.Key = keyBytes;
            aes.IV = new byte[16];

            using var decryptor = aes.CreateDecryptor();
            var encryptedBytes = Convert.FromBase64String(encryptedText);
            var decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
            var decrypted = Encoding.UTF8.GetString(decryptedBytes);

            var parts = decrypted.Split('|');
            if (parts.Length == 2)
            {
                return (parts[0], parts[1]);
            }

            return null;
        }
        catch
        {
            return null;
        }
    }

    public static string ComputeOTP(byte[] key, long counter)
    {
        var counterBytes = BitConverter.GetBytes(counter);
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(counterBytes);
        }

        using var hmac = new HMACSHA1(key);
        var hash = hmac.ComputeHash(counterBytes);

        int offset = hash[hash.Length - 1] & 0x0F;
        int binaryCode =
            ((hash[offset] & 0x7f) << 24) |
            ((hash[offset + 1] & 0xff) << 16) |
            ((hash[offset + 2] & 0xff) << 8) |
            (hash[offset + 3] & 0xff);

        return (binaryCode % 100_000_000).ToString().PadLeft(8, '0');
    }

}
