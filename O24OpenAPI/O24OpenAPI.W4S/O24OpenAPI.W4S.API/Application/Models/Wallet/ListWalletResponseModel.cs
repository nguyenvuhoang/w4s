namespace O24OpenAPI.W4S.API.Application.Models.Wallet;

/// <summary>
/// Defines the <see cref="ListWalletResponseModel" />
/// </summary>
public class ListWalletResponseModel : BaseO24OpenAPIModel
{
    /// <summary>
    /// Gets or sets the WalletId
    /// </summary>
    public int WalletId { get; set; }

    /// <summary>
    /// Gets or sets the ContractNumber
    /// Contract owner identifier
    /// </summary>
    public string ContractNumber { get; set; }

    /// <summary>
    /// Gets or sets the UserCode
    /// Owner of the wallet
    /// </summary>
    public string UserCode { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the WalletName
    /// Wallet display name
    /// </summary>
    public string WalletName { get; set; } = default!;

    /// <summary>
    /// Gets or sets the WalletType
    /// Wallet type: PERSONAL | SHARED | BUSINESS | DEFI | FIAT
    /// </summary>
    public string WalletType { get; set; } = default!;
    public decimal? AvailableBalance { get; set; } = 0;

    /// <summary>
    /// Gets or sets the DefaultCurrency
    /// Default currency (VND, USD, LAK, USDT...)
    /// </summary>
    public string DefaultCurrency { get; set; } = default!;

    /// <summary>
    /// Gets or sets the Status
    /// Wallet status: A = Active, I = Inactive, C = Closed
    /// </summary>
    public string Status { get; set; }
    /// <summary>
    /// Icon
    /// </summary>
    public string Icon { get; set; }
    /// <summary>
    /// Color
    /// </summary>
    public string Color { get; set; }
    /// <summary>
    /// Account
    /// </summary>
    public List<WalletAccountWithBalanceResponseModel> Account { get; set; }
}
