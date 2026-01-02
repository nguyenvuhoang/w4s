using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.ACT.API.Application.Models;

public class AccountingRuleDefinitionModel : BaseTransactionModel
{
    public List<TemporaryPosting> EntryJournals { get; set; } = new();
}
