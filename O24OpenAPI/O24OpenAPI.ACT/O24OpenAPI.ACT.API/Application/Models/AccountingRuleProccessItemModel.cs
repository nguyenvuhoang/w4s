using O24OpenAPI.Core.Abstractions;
using O24OpenAPI.ACT.Domain;

namespace O24OpenAPI.ACT.API.Application.Models;

public class AccountingRuleProccessItemModel : BaseO24OpenAPIModel
{
    public TemporaryPosting EntryJournal { get; set; } = new TemporaryPosting();
    public AccountChart AccountChart { get; set; } = new AccountChart();
}
