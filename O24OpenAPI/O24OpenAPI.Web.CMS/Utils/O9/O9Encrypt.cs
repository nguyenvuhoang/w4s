using System.Security.Cryptography;
using System.Text;

namespace O24OpenAPI.Web.CMS.Utils;

public class O9Encrypt
{
    private static string CONSTKEY = "abhf@311";

    public static string sha256(string password)
    {
        using var sha256 = SHA256.Create();
        string empty = string.Empty;
        foreach (byte num in sha256.ComputeHash(Encoding.UTF8.GetBytes(password)))
        {
            empty += num.ToString("x2");
        }

        return empty;
    }

    public static string sha_sha256(string password, string loginName)
    {
        loginName = loginName.ToUpper();
        string str1 = string.Empty;
        string empty1 = string.Empty;
        string empty2 = string.Empty;
        string str2 = O9Encrypt.sha256(loginName + password);
        if (str2.Length > 9)
        {
            str1 = str2.Substring(6, 9).ToLower();
        }

        return O9Encrypt.sha256(str2 + str1 + loginName);
    }

    public static string AESEncrypt(string textToEncrypt)
    {
        Aes rijndaelManaged = Aes.Create();
        rijndaelManaged.Mode = CipherMode.CBC;
        rijndaelManaged.Padding = PaddingMode.PKCS7;
        rijndaelManaged.KeySize = 128;
        rijndaelManaged.BlockSize = 128;
        byte[] bytes1 = Encoding.UTF8.GetBytes(CONSTKEY);
        byte[] destinationArray = new byte[16];
        int length = bytes1.Length;
        if (length > destinationArray.Length)
        {
            length = destinationArray.Length;
        }

        Array.Copy((Array)bytes1, (Array)destinationArray, length);
        rijndaelManaged.Key = destinationArray;
        rijndaelManaged.IV = destinationArray;
        ICryptoTransform encryptor = rijndaelManaged.CreateEncryptor();
        byte[] bytes2 = Encoding.UTF8.GetBytes(textToEncrypt);
        return Convert.ToBase64String(encryptor.TransformFinalBlock(bytes2, 0, bytes2.Length));
    }

    /// <summary>
    /// Aeses the decrypt using the specified text to decrypt
    /// </summary>
    /// <param name="textToDecrypt">The text to decrypt</param>
    /// <returns>The string</returns>
    public static string AESDecrypt(string textToDecrypt)
    {
        Aes rijndaelManaged = Aes.Create();
        rijndaelManaged.Mode = CipherMode.CBC;
        rijndaelManaged.Padding = PaddingMode.PKCS7;
        rijndaelManaged.KeySize = 128;
        rijndaelManaged.BlockSize = 128;
        byte[] inputBuffer = Convert.FromBase64String(textToDecrypt);
        byte[] bytes = Encoding.UTF8.GetBytes(CONSTKEY);
        byte[] destinationArray = new byte[16];
        int length = bytes.Length;
        if (length > destinationArray.Length)
        {
            length = destinationArray.Length;
        }

        Array.Copy((Array)bytes, (Array)destinationArray, length);
        rijndaelManaged.Key = destinationArray;
        rijndaelManaged.IV = destinationArray;
        return Encoding.UTF8.GetString(
            rijndaelManaged
                .CreateDecryptor()
                .TransformFinalBlock(inputBuffer, 0, inputBuffer.Length)
        );
    }

    public static string SHA256Encrypt(string pwd)
    {
        using var crypt = SHA256.Create();
        string hash = string.Empty;

        byte[] bytes = crypt.ComputeHash(
            Encoding.UTF8.GetBytes(pwd),
            0,
            Encoding.UTF8.GetByteCount(pwd)
        );
        foreach (var item in bytes)
        {
            hash += item.ToString("x2");
        }
        return hash;
    }

    /// <summary>
    ///
    /// </summary>
    public static string Decrypt(string textToDecrypt)
    {
        using var rijndaelCipher = Aes.Create();
        rijndaelCipher.Mode = CipherMode.CBC;
        rijndaelCipher.Padding = PaddingMode.PKCS7;
        rijndaelCipher.KeySize = 0x80;
        rijndaelCipher.BlockSize = 0x80;
        byte[] encryptedData = Convert.FromBase64String(textToDecrypt);
        byte[] pwdBytes = Encoding.UTF8.GetBytes(CONSTKEY);
        byte[] keyBytes = new byte[16];
        int len = pwdBytes.Length;
        len = len > keyBytes.Length ? keyBytes.Length : pwdBytes.Length;
        Array.Copy(pwdBytes, keyBytes, len);
        rijndaelCipher.Key = keyBytes;
        rijndaelCipher.IV = keyBytes;
        byte[] plainText = rijndaelCipher
            .CreateDecryptor()
            .TransformFinalBlock(encryptedData, 0, encryptedData.Length);
        return Encoding.UTF8.GetString(plainText);
    }

    /// <summary>
    ///
    /// </summary>
    public static string Encrypt(string textToEncrypt)
    {
        using var rijndaelCipher = Aes.Create();
        rijndaelCipher.Mode = CipherMode.CBC;
        rijndaelCipher.Padding = PaddingMode.PKCS7;
        rijndaelCipher.KeySize = 0x80;
        rijndaelCipher.BlockSize = 0x80;
        byte[] pwdBytes = Encoding.UTF8.GetBytes(CONSTKEY);
        byte[] keyBytes = new byte[16];
        int len = pwdBytes.Length;
        len = len > keyBytes.Length ? keyBytes.Length : pwdBytes.Length;
        Array.Copy(pwdBytes, keyBytes, len);
        rijndaelCipher.Key = keyBytes;
        rijndaelCipher.IV = keyBytes;

        ICryptoTransform transform = rijndaelCipher.CreateEncryptor();
        byte[] plainText = Encoding.UTF8.GetBytes(textToEncrypt);

        return Convert.ToBase64String(
            transform.TransformFinalBlock(plainText, 0, plainText.Length)
        );
    }

    /// <summary>
    ///
    /// </summary>
    public static string MD5Encrypt(string pwd)
    {
        using var md5 = MD5.Create();
        byte[] result = md5.ComputeHash(Encoding.UTF8.GetBytes(pwd));
        return Convert.ToBase64String(result);
    }

    /// <summary>
    ///
    /// </summary>
    public static string GenerateRandomString()
    {
        var randomBytes = new byte[64];
        RandomNumberGenerator.Fill(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }

    /// <summary>
    ///
    /// </summary>
    public static string Base64Encode(string plainText)
    {
        var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
        return Convert.ToBase64String(plainTextBytes);
    }

    /// <summary>
    ///
    /// </summary>
    public static string Base64Decode(string base64EncodedData)
    {
        try
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }
        catch
        {
            return null;
        }
    }
}
