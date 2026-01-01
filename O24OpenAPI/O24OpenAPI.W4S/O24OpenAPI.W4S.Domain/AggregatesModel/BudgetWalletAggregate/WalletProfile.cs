using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;

[Auditable]
public partial class WalletProfile : BaseEntity
{
    /// <summary>
    /// Unique wallet identifier
    /// </summary>
    public Guid WalletId { get; private set; }
    /// <summary>
    /// Contract owner identifier
    /// </summary>
    public string ContractNumber { get; set; }

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
    public char Status { get; private set; }

    public WalletProfile() { }

    public WalletProfile(
        Guid walletId,
        string contractNumber,
        string userCode,
        string walletName,
        string walletType,
        string defaultCurrency
    )
    {
        WalletId = walletId;
        ContractNumber = contractNumber;
        UserCode = userCode;
        WalletName = walletName;
        WalletType = walletType;
        DefaultCurrency = defaultCurrency;

        Status = 'A';
    }

    public void ChangeName(string walletName)
    {
        WalletName = walletName;
    }

    public void ChangeStatus(char status)
    {
        Status = status;
    }

    public void ChangeDefaultCurrency(string currency)
    {
        DefaultCurrency = currency;
    }

    public static WalletProfile Create(
        Guid walletId,
        string contractNumber,
        string userCode,
        string walletName,
        string walletType,
        string defaultCurrency

    )
    {
        return new WalletProfile
        {
            WalletId = walletId,
            ContractNumber = contractNumber,
            UserCode = userCode,
            WalletName = walletName,
            WalletType = walletType,
            DefaultCurrency = defaultCurrency,
            Status = 'A'
        };
    }

}
