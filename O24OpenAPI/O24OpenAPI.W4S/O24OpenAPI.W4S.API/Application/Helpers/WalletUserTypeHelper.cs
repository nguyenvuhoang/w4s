using O24OpenAPI.W4S.Domain.AggregatesModel.WalletMasterAggregate;

namespace O24OpenAPI.W4S.API.Application.Helpers;

public static class WalletUserTypeHelper
{
    public static WalletUserType Parse(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return WalletUserType.Individual;

        var normalized = value.Trim().ToUpperInvariant();

        return normalized switch
        {
            // Individual / Personal
            "0502" or "INDIVIDUAL" or "PERSONAL" => WalletUserType.Individual,

            // Agent
            "0601" or "AGENT" => WalletUserType.Agent,

            // Merchant
            "0701" or "MERCHANT" => WalletUserType.Merchant,

            _ => throw new ArgumentException($"Unsupported wallet user type: '{value}'"),
        };
    }
}
