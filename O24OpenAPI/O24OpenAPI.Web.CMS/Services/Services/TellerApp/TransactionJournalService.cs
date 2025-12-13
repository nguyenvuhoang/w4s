using Newtonsoft.Json.Linq;
using O24OpenAPI.Web.CMS.Configuration;
using O24OpenAPI.Web.CMS.Models;
using O24OpenAPI.Web.CMS.Services.Interfaces;
using static Jits.Neptune.Web.CMS.LogicOptimal9.Common.O9Extensions;

namespace O24OpenAPI.Web.CMS.Services.Services.TellerApp;

public class TransactionJournalService(IBaseO9WorkflowService baseO9WorkflowService)
    : ITransactionJournalService
{
    private readonly IBaseO9WorkflowService _baseO9WorkflowService = baseO9WorkflowService;

    public async Task<JToken> SimpleSearch(WorkflowRequestModel model)
    {
        var data = await _baseO9WorkflowService.SimpleSearch(model);
        var workflowResponse = data.ToObject<WorkflowResponseModel>();
        var pageList = workflowResponse.result.ToObject<PageListModel>();
        for (var i = 0; i < pageList.Items.Count; i++)
        {
            var code = pageList.Items[i].Value<string>("transname");
            if (TellerAppConfiguration.VisibleTransaction.Contains(code))
            {
                pageList.Items[i]["is_view"] = 1;
            }
        }
        return pageList.ToJToken();
    }
}
