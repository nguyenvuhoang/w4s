using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.O24ACT.Domain;

public partial class RuleDefinition : BaseEntity
{
    public string RuleName { get; set; }
    public string FullClassName { get; set; }
    public string MethodName { get; set; }
    public DateTime? UpdatedOnUtc { get; set; }
}
