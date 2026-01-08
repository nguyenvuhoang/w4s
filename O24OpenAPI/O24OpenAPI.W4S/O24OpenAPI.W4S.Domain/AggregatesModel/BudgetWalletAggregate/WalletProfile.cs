using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Core.Domain;
using O24OpenAPI.W4S.Domain.Constants;

namespace O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;

[Auditable]
public partial class WalletProfile : BaseEntity
{
    /// <summary>
    /// Unique wallet identifier
    /// </summary>
    public string? WalletProfileCode { get; private set; }

    /// <summary>
    /// Contract owner identifier
    /// </summary>
    public string? ContractNumber { get; set; }

    /// <summary>
    /// Owner of the wallet
    /// </summary>
    public string UserCode { get; private set; } = string.Empty;

    /// <summary>
    /// Wallet display name
    /// </summary>
    public string WalletName { get; private set; } = default!;

    /// <summary>
    /// Wallet type: PERSONAL | SHARED | BUSINESS | DEFI | FIAT
    /// </summary>
    public string WalletType { get; private set; } = default!;

    /// <summary>
    /// Default currency (VND, USD, LAK, USDT...)
    /// </summary>
    public string DefaultCurrency { get; private set; } = default!;

    /// <summary>
    /// Wallet status: A = Active, I = Inactive, C = Closed
    /// </summary>
    public string? Status { get; private set; }
    public string? Icon { get; set; }
    public string? Color { get; set; }

    public WalletProfile() { }

    public WalletProfile(
        string? walletProfileCode,
        string contractNumber,
        string userCode,
        string walletName,
        string walletType,
        string defaultCurrency
    )
    {
        WalletProfileCode = walletProfileCode;
        ContractNumber = contractNumber;
        UserCode = userCode;
        WalletName = walletName;
        WalletType = walletType;
        DefaultCurrency = defaultCurrency;

        Status = WalletProfileStatus.Active;
    }

    public void ChangeName(string walletName)
    {
        WalletName = walletName;
    }

    public void ChangeStatus(string status)
    {
        Status = status;
    }

    public void ChangeDefaultCurrency(string currency)
    {
        DefaultCurrency = currency;
    }

    public static WalletProfile Create(
        string? walletProfileCode,
        string contractNumber,
        string userCode,
        string walletName,
        string walletType,
        string defaultCurrency
    )
    {
        return new WalletProfile
        {
            WalletProfileCode = walletProfileCode,
            ContractNumber = contractNumber,
            UserCode = userCode,
            WalletName = walletName,
            WalletType = walletType,
            DefaultCurrency = defaultCurrency,
            Status = WalletProfileStatus.Active,
        };
    }
}
