using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;

public partial class WalletProfile : BaseEntity
{
    /// <summary>
    /// Unique wallet identifier
    /// </summary>
    public Guid WalletId { get; private set; }

    /// <summary>
    /// Owner of the wallet
    /// </summary>
    public Guid UserId { get; private set; }

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

    #region Constructor

    public WalletProfile() { }

    public WalletProfile(
        Guid walletId,
        Guid userId,
        string walletName,
        string walletType,
        string defaultCurrency
    )
    {
        WalletId = walletId;
        UserId = userId;
        WalletName = walletName;
        WalletType = walletType;
        DefaultCurrency = defaultCurrency;

        Status = 'A';
    }

    #endregion

    #region Domain Behaviors

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

    #endregion
}
