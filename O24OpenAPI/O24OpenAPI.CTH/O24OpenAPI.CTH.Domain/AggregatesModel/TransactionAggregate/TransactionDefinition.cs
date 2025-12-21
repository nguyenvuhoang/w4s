using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.CTH.Domain.AggregatesModel.TransactionAggregate;

public class TransactionDefinition : BaseEntity
{
    public string TransactionCode { get; set; } = null!;
    public string WorkflowId { get; set; }
    public string TransactionName { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string TransactionType { get; set; } = null!;
    public string ServiceId { get; set; } = null!;
    public bool InterBranch { get; set; }
    public bool Reverseable { get; set; }
    public bool Enabled { get; set; }
    public string MessageAccount { get; set; } = null!;
    public string MessageAmount { get; set; }
    public string Voucher { get; set; }
    public bool ShowF8 { get; set; }
    public string ApplicationCode { get; set; } = null!;
    public string MessageCurrency { get; set; }
    public string TransactionModel { get; set; }
    public string Channel { get; set; }
    public DateTime? UpdatedOnUtc { get; set; }
    public DateTime? CreatedOnUtc { get; set; }
}
