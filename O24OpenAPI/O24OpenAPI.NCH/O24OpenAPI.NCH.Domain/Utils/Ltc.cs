using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace O24OpenAPI.NCH.API.Application.Utils;

public static class LTC
{
    private static byte[]? key;
    private static Aes? aesAlg;

    public static void SetKey(string privateKey)
    {
        using (SHA1 sha = SHA1.Create())
        {
            key = Encoding.UTF8.GetBytes(privateKey);
            key = sha.ComputeHash(key);
            key = key.Take(16).ToArray();

            aesAlg = Aes.Create();
            aesAlg.Key = key;
            aesAlg.Mode = CipherMode.ECB;
            aesAlg.Padding = PaddingMode.PKCS7;
        }
    }

    public static string Encrypt(string dataString, string privateKey)
    {
        Stopwatch stopwatch = new();
        stopwatch.Start();
        try
        {
            Console.WriteLine($"Starting execution Encrypt at {DateTime.Now}");
            SetKey(privateKey);
            ICryptoTransform encryptor = aesAlg.CreateEncryptor();
            byte[] inputBytes = Encoding.UTF8.GetBytes(dataString);
            byte[] encrypted = encryptor.TransformFinalBlock(inputBytes, 0, inputBytes.Length);
            Console.WriteLine("Result Encrypt by Class: " + Convert.ToBase64String(encrypted));

            stopwatch.Stop();
            Console.WriteLine($"Completed execution of Encrypt in {stopwatch.ElapsedMilliseconds}ms");
            return Convert.ToBase64String(encrypted);
        }
        catch (Exception e)
        {
            Console.WriteLine("Error while encrypting: " + e.ToString());
            return null;
        }
    }

    public static string Decrypt(string decryptString, string privateKey)
    {
        try
        {
            SetKey(privateKey);
            ICryptoTransform decryptor = aesAlg.CreateDecryptor();
            byte[] cipherBytes = Convert.FromBase64String(decryptString);
            byte[] decrypted = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
            return Encoding.UTF8.GetString(decrypted);
        }
        catch (Exception e)
        {
            Console.WriteLine("Error while decrypting: " + e.ToString());
            return null;
        }
    }

}
