namespace O24OpenAPI.EXT.API.Application.Helpers
{
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// Defines the <see cref="VNPaySignature" />
    /// </summary>
    public static class VNPaySignature
    {
        /// <summary>
        /// The Verify
        /// </summary>
        /// <param name="hashSecret">The hashSecret<see cref="string"/></param>
        /// <param name="query">The query<see cref="IDictionary{string, string}"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public static bool Verify(string hashSecret, IDictionary<string, string> query)
        {
            if (!TryGetIgnoreCase(query, "vnp_SecureHash", out var secureHash) || string.IsNullOrWhiteSpace(secureHash))
                return false;

            // loại bỏ hash fields, sort theo key ordinal
            var sorted = query
                .Where(kv =>
                    !kv.Key.Equals("vnp_SecureHash", StringComparison.OrdinalIgnoreCase) &&
                    !kv.Key.Equals("vnp_SecureHashType", StringComparison.OrdinalIgnoreCase) &&
                    !string.IsNullOrWhiteSpace(kv.Value)
                )
                .OrderBy(kv => kv.Key, StringComparer.Ordinal)
                .ToList();

            var signData = string.Join("&", sorted.Select(kv => $"{kv.Key}={Uri.EscapeDataString(kv.Value)}"));
            var expected = HmacSha512(hashSecret, signData);

            return expected.Equals(secureHash, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// The HmacSha512
        /// </summary>
        /// <param name="secret">The secret<see cref="string"/></param>
        /// <param name="data">The data<see cref="string"/></param>
        /// <returns>The <see cref="string"/></returns>
        private static string HmacSha512(string secret, string data)
        {
            var keyBytes = Encoding.UTF8.GetBytes(secret);
            var dataBytes = Encoding.UTF8.GetBytes(data);

            using var hmac = new HMACSHA512(keyBytes);
            var hashBytes = hmac.ComputeHash(dataBytes);
            return Convert.ToHexString(hashBytes).ToLowerInvariant();
        }

        /// <summary>
        /// The TryGetIgnoreCase
        /// </summary>
        /// <param name="dict">The dict<see cref="IDictionary{string, string}"/></param>
        /// <param name="key">The key<see cref="string"/></param>
        /// <param name="value">The value<see cref="string"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public static bool TryGetIgnoreCase(IDictionary<string, string> dict, string key, out string value)
        {
            foreach (var kv in dict)
            {
                if (kv.Key.Equals(key, StringComparison.OrdinalIgnoreCase))
                {
                    value = kv.Value;
                    return true;
                }
            }
            value = string.Empty;
            return false;
        }

        /// <summary>
        /// The BuildQueryString
        /// </summary>
        /// <param name="sorted">The sorted<see cref="SortedDictionary{string, string}"/></param>
        /// <returns>The <see cref="string"/></returns>
        private static string BuildQueryString(SortedDictionary<string, string> sorted)
        {
            var sb = new StringBuilder();
            foreach (var (k, v) in sorted)
            {
                if (string.IsNullOrWhiteSpace(v)) continue;
                if (sb.Length > 0) sb.Append('&');
                sb.Append(k);
                sb.Append('=');
                sb.Append(Uri.EscapeDataString(v));
            }
            return sb.ToString();
        }

        /// <summary>
        /// The CreateRequestUrl
        /// </summary>
        /// <param name="baseUrl">The baseUrl<see cref="string"/></param>
        /// <param name="sorted">The sorted<see cref="SortedDictionary{string, string}"/></param>
        /// <param name="hashSecret">The hashSecret<see cref="string"/></param>
        /// <returns>The <see cref="string"/></returns>
        public static string CreateRequestUrl(string baseUrl, SortedDictionary<string, string> sorted, string hashSecret)
        {
            var queryString = BuildQueryString(sorted);
            var secureHash = HmacSha512(hashSecret, queryString);
            var result = $"{baseUrl}?{queryString}&vnp_SecureHash={secureHash}";
            return result;
        }
    }
}
