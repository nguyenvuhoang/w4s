using Newtonsoft.Json.Linq;
using O24OpenAPI.Web.CMS.Models;

namespace O24OpenAPI.Web.CMS.Services.Interfaces;

public interface ICoreJournalService
{
    Task<string> GetTransactionBodyAsync(string transactionDate, string transactionNumber);

    Task<JToken> ViewF8(WorkflowRequestModel workflow);
}
