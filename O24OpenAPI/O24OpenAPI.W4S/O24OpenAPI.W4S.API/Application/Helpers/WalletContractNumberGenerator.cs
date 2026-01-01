using System.Security.Cryptography;

namespace O24OpenAPI.W4S.API.Application.Helpers
{
    public static class WalletContractNumberGenerator
    {
        public static string Generate(
            WalletContractKind kind,
            string systemCode = "O24",
            string module = "WAL",
            int nodeId = 0
        )
        {
            var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");

            Span<byte> buffer = stackalloc byte[1];
            RandomNumberGenerator.Fill(buffer);

            var entropy = buffer[0].ToString("X2");

            return $"{systemCode}{module}{(char)kind}{timestamp}{nodeId}{entropy}";
        }
    }

    public enum WalletContractKind
    {
        Personal = 'P',
        Agent = 'A',
        Merchant = 'M',
        DeFi = 'D'
    }
}
