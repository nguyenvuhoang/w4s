using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.O24ACT.Models;

public class AccountingRuleDefinitionModel : BaseTransactionModel
{
    public List<TemporaryPosting> EntryJournals { get; set; } = new();
}
