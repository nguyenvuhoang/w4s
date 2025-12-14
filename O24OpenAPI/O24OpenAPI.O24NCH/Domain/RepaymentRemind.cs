namespace O24OpenAPI.O24NCH.Domain;

using O24OpenAPI.Core.Domain;

/// <summary>
/// Defines the <see cref="RepaymentRemind" />
/// </summary>
public class RepaymentRemind : BaseEntity
{
    /// <summary>
    /// Gets or sets the AccountNumber
    /// </summary>
    public string AccountNumber { get; set; }
    /// <summary>
    /// Get or sets the CustomerName
    /// </summary>
    public string CustomerName { get; set; }

    /// <summary>
    /// Gets or sets the DueDate
    /// </summary>
    public DateTime DueDate { get; set; }
    /// <summary>
    /// Due amount
    /// </summary>
    public decimal DueAmount { get; set; }

    /// <summary>
    /// Gets or sets the MessageType
    /// </summary>
    public string MessageType { get; set; }

    /// <summary>
    /// Gets or sets the LastSentOn
    /// </summary>
    public DateTime LastSentOn { get; set; }

    /// <summary>
    /// Gets or sets the Status
    /// </summary>
    public string Status { get; set; }

    public string ErrorMessage { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the CreatedOn
    /// </summary>
    public DateTime CreatedOn { get; set; }

    /// <summary>
    /// Gets or sets the UpdatedOn
    /// </summary>
    public DateTime UpdatedOn { get; set; }
}
