using Newtonsoft.Json.Linq;
using O24OpenAPI.Web.CMS.Models;

namespace O24OpenAPI.Web.CMS.Services.Interfaces;

public interface ITransactionJournalService
{
    Task<JToken> SimpleSearch(WorkflowRequestModel model);
}
