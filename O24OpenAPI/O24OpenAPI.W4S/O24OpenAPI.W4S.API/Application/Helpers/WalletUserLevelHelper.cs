using O24OpenAPI.W4S.Domain.AggregatesModel.WalletMasterAggregate;

namespace O24OpenAPI.W4S.API.Application.Helpers
{
    public static class WalletUserLevelHelper
    {
        public static WalletUserLevel? Parse(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return WalletUserLevel.L0;

            var normalized = value.Trim().ToUpperInvariant();

            return normalized switch
            {
                "0" or "L0" => WalletUserLevel.L0,
                "1" or "L1" => WalletUserLevel.L1,
                "2" or "L2" => WalletUserLevel.L2,
                "3" or "L3" => WalletUserLevel.L3,

                _ => throw new ArgumentException(
                    $"Unsupported wallet user level: '{value}'"
                )
            };
        }
    }
}
