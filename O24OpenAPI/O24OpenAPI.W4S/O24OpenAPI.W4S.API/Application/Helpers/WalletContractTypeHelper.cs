using O24OpenAPI.W4S.Domain.AggregatesModel.WalletMasterAggregate;

namespace O24OpenAPI.W4S.API.Application.Helpers;

public static class WalletContractTypeHelper
{
    public static WalletContractType Parse(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return WalletContractType.Wallet;

        var normalized = value.Trim().ToUpperInvariant();

        return normalized switch
        {
            "WALLET" => WalletContractType.Wallet,
            "WAL" => WalletContractType.Wallet,

            _ => throw new ArgumentException($"Unsupported wallet contract type: '{value}'"),
        };
    }
}
