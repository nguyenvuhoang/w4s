using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.O24ACT.Domain;

public partial class TransactionRules : BaseEntity
{
    public string WorkflowId { get; set; }
    public string RuleName { get; set; }
    public string Parameter { get; set; }
    public int RuleOrder { get; set; }
    public bool IsEnable { get; set; }
    public string Spec { get; set; }
    public string Example { get; set; }
    public string Caption { get; set; }
    public DateTime? UpdatedOnUtc { get; set; }
}
