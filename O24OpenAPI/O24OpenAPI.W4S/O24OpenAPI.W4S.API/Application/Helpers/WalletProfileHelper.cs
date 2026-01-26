namespace O24OpenAPI.W4S.API.Application.Helpers
{
    /// <summary>
    /// Defines the <see cref="WalletProfileHelper" />
    /// </summary>
    public static class WalletProfileHelper
    {
        /// <summary>
        /// The GenerateWalletProfileCode
        /// </summary>
        /// <param name="walletType">The walletType<see cref="string"/></param>
        /// <param name="classification">The classification<see cref="string"/></param>
        /// <returns>The <see cref="string"/></returns>
        public static string GenerateWalletProfileCode(string walletType, string classification)
        {
            string date = DateTime.UtcNow.ToString("yyyyMMdd");

            long ms = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            string last3 = (ms % 1000).ToString("D2");

            string rand = Random.Shared.Next(0, 1000).ToString("D2");

            string suffix = last3 + rand;

            return $"{date}{walletType}{classification}{suffix}";
        }

        /// <summary>
        /// The GenerateCategoryCode
        /// </summary>
        /// <param name="groupCode">The groupCode<see cref="string"/></param>
        /// <returns>The <see cref="string"/></returns>
        public static string GenerateCategoryCode(string groupCode)
        {
            string date = DateTime.UtcNow.ToString("yyyyMMdd");

            long ms = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            string last3 = (ms % 1000).ToString("D2");

            string rand = Random.Shared.Next(0, 1000).ToString("D2");

            string suffix = last3 + rand;

            return $"{date}{groupCode}{suffix}";
        }
    }
}
