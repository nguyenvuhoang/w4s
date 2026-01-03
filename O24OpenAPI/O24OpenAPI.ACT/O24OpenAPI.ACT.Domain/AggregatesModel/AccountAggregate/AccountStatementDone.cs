using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.ACT.Domain.AggregatesModel.AccountAggregate;

[Auditable]
public partial class AccountStatementDone
{
    public string TransId { get; set; } = Guid.NewGuid().ToString();
    public string? AccountNumber { get; set; }

    public DateTime StatementDate { get; set; }

    public string? ReferenceId { get; set; }

    public string? TransactionNumber { get; set; }

    public DateTime ValueDate { get; set; }

    public decimal Amount { get; set; }

    public string? CurrencyCode { get; set; }

    public decimal ConvertAmount { get; set; }

    public string? StatementCode { get; set; }

    public string? StatementStatus { get; set; }

    public string? RefNumber { get; set; }

    public string? TransCode { get; set; }

    public string? Description { get; set; }
}
