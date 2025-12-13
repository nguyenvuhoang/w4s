using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using O24OpenAPI.Web.Framework.Extensions;
using O24OpenAPI.Web.Framework.Utils;
using System.Security.Cryptography;

namespace O24OpenAPI.ControlHub.Utils;

/// <summary>
/// The password utils class
/// </summary>
public class PasswordUtils
{
    /// <summary>
    /// Hashes the password using the specified password
    /// </summary>
    /// <param name="password">The password</param>
    /// <returns>The string hash string salt</returns>
    public static (string hash, string salt) HashPassword(string password)
    {
        byte[] salt = new byte[16];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        byte[] hash = KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 32
        );

        return (Convert.ToBase64String(hash), Convert.ToBase64String(salt));
    }

    /// <summary>
    /// Verifies the password using the specified password
    /// </summary>
    /// <param name="password">The password</param>
    /// <param name="storedHash">The stored hash</param>
    /// <param name="storedSalt">The stored salt</param>
    /// <returns>The bool</returns>
    public static bool VerifyPassword(string usercode, string password, string storedHash, string storedSalt)
    {
        if (string.IsNullOrEmpty(usercode) || string.IsNullOrEmpty(password) ||
            string.IsNullOrEmpty(storedHash) || string.IsNullOrEmpty(storedSalt))
        {
            return false;
        }
        try
        {
            string passwordDecrypted = O9Encrypt.Decrypt(password);
            int lastUnderscoreIndex = passwordDecrypted.LastIndexOf('_');
            if (lastUnderscoreIndex == -1)
            {
                throw new FormatException("Invalid password format. No underscore found.");
            }

            string newPassword = passwordDecrypted[(lastUnderscoreIndex + 1)..];
            string hashPassword = O9Encrypt.sha_sha256(newPassword, usercode);

            return hashPassword.Equals(storedHash, StringComparison.OrdinalIgnoreCase);
        }
        catch (Exception ex)
        {
            _ = ex.LogErrorAsync();
            throw new Exception("Invalid password format. Please check your input.");
        }
    }


    /// <summary>
    /// Hashes the pass using the specified password
    /// </summary>
    /// <param name="password">The password</param>
    /// <returns>The string</returns>
    public static string HashPass(string password)
    {
        byte[] salt = new byte[16];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        byte[] hash = KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 32
        );
        return $"{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
    }

    /// <summary>
    /// Verifies the password using the specified password
    /// </summary>
    /// <param name="password">The password</param>
    /// <param name="storedHash">The stored hash</param>
    /// <returns>The bool</returns>
    public static bool VerifyPassword(string password, string storedHash)
    {
        var parts = storedHash.Split('.');
        if (parts.Length != 2)
        {
            return false;
        }

        byte[] salt = Convert.FromBase64String(parts[0]);
        byte[] storedHashBytes = Convert.FromBase64String(parts[1]);

        byte[] hash = KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 32
        );

        return CryptographicOperations.FixedTimeEquals(hash, storedHashBytes);
    }

    /// <summary>
    /// GenerateRandomPassword
    /// </summary>
    /// <param name="length"></param>
    /// <returns></returns>
    public static string GenerateRandomPassword(int length)
    {
        const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^*";
        var invalidXmlChars = new[] { '&', '<', '>', '"', '\'' };

        var safeChars = new string(chars.Where(c => !invalidXmlChars.Contains(c)).ToArray());

        var random = new Random();
        return new string(Enumerable.Repeat(safeChars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }


    /// <summary>
    /// Generate Random Salt
    /// </summary>
    /// <param name="size"></param>
    /// <returns></returns>
    public static string GenerateRandomSalt(int size = 16)
    {
        byte[] saltBytes = RandomNumberGenerator.GetBytes(size);
        return Convert.ToBase64String(saltBytes);
    }
}
