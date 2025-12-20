using O24OpenAPI.Core.Abstractions;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.O24ACT.Domain;

namespace O24OpenAPI.O24ACT.Models;

public class AccountingRuleProccessItemModel : BaseO24OpenAPIModel
{
    public TemporaryPosting EntryJournal { get; set; } = new TemporaryPosting();
    public AccountChart AccountChart { get; set; } = new AccountChart();
}
