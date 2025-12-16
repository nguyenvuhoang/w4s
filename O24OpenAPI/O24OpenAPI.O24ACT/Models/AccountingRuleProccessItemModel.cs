using O24OpenAPI.O24ACT.Domain;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.O24ACT.Models;

public class AccountingRuleProccessItemModel : BaseO24OpenAPIModel
{
    public TemporaryPosting EntryJournal { get; set; } = new TemporaryPosting();
    public AccountChart AccountChart { get; set; } = new AccountChart();

}
