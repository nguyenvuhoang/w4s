namespace O24OpenAPI.APIContracts.Events;

using O24OpenAPI.Contracts.Events;

/// <summary>
/// Defines the <see cref="BalanceModifyEvent" />
/// </summary>
public class BalanceModifyEvent : IntegrationEvent
{
    /// <summary>
    /// Gets or sets the ContractNumber
    /// </summary>
    public string ContractNumber { get; set; }

    /// <summary>
    /// Gets or sets the AccountNumber
    /// </summary>
    public string AccountNumber { get; set; }

    /// <summary>
    /// Gets or sets the UserId
    /// </summary>
    public string UserId { get; set; }

    /// <summary>
    /// Gets or sets the DeviceId
    /// </summary>
    public string DeviceId { get; set; }

    /// <summary>
    /// Gets or sets the CurrencyCode
    /// </summary>
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the Balance
    /// </summary>
    public decimal Balance { get; set; }

    /// <summary>
    /// Gets or sets the AvailableBalance
    /// </summary>
    public decimal AvailableBalance { get; set; }
}
