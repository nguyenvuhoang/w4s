namespace O24OpenAPI.CMS.API.Application.Utils
{
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// Defines the <see cref="PkceUtil" />
    /// </summary>
    public static class PkceUtil
    {
        /// <summary>
        /// The Base64UrlEncode
        /// </summary>
        /// <param name="input">The input<see cref="byte[]"/></param>
        /// <returns>The <see cref="string"/></returns>
        private static string Base64UrlEncode(byte[] input)
        {
            return Convert.ToBase64String(input)
                .Replace("+", "-")
                .Replace("/", "_")
                .Replace("=", "");
        }

        /// <summary>
        /// Generate a high-entropy code_verifier (43–128 chars, RFC 7636)
        /// </summary>
        /// <param name="length">The length<see cref="int"/></param>
        /// <returns>The <see cref="string"/></returns>
        public static string GenerateCodeVerifier(int length = 64)
        {
            if (length < 43 || length > 128)
                throw new ArgumentOutOfRangeException(nameof(length), "PKCE verifier length must be between 43 and 128");

            var bytes = RandomNumberGenerator.GetBytes(length);
            return Base64UrlEncode(bytes);
        }

        /// <summary>
        /// Generate code_challenge from code_verifier using S256
        /// </summary>
        /// <param name="codeVerifier">The codeVerifier<see cref="string"/></param>
        /// <returns>The <see cref="string"/></returns>
        public static string GenerateCodeChallenge(string codeVerifier)
        {
            var bytes = Encoding.ASCII.GetBytes(codeVerifier);
            var hash = SHA256.HashData(bytes);

            return Base64UrlEncode(hash);
        }

        /// <summary>
        /// The GenerateState
        /// </summary>
        /// <returns>The <see cref="string"/></returns>
        public static string GenerateState()
        {
            var bytes = RandomNumberGenerator.GetBytes(32);
            return Base64UrlEncode(bytes);
        }
    }

}
