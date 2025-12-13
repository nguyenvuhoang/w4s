using Newtonsoft.Json.Linq;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.Web.CMS.Models;
using O24OpenAPI.Web.CMS.Services.Interfaces;

namespace O24OpenAPI.Web.CMS.Services;

public class O9JournalService(
    IO9ClientService o9ClientService,
    IWorkflowStepService workflowStepService,
    IMappingService mappingService
) : ICoreJournalService
{
    private readonly IO9ClientService _o9ClientService = o9ClientService;
    private readonly IWorkflowStepService _workflowStepService = workflowStepService;
    private readonly IMappingService _mappingService = mappingService;

    public async Task<string> GetTransactionBodyAsync(
        string transactionDate,
        string transactionNumber
    )
    {
        var jsRequest = new Dictionary<string, object>
        {
            ["I"] = transactionNumber,
            ["B"] = transactionDate,
            ["P"] = false,
        };
        return await _o9ClientService.GenJsonDataRequestAsync(jsRequest, "FOF_GET_TXBODY");
    }

    public async Task<JToken> ViewF8(WorkflowRequestModel workflow)
    {
        string strJsonResult = await GetTransactionBodyAsync(
            workflow.GetStringTxdt(),
            workflow.fields["I"].ToString()
        );

        if (string.IsNullOrEmpty(strJsonResult))
        {
            throw new Exception("Transaction not found!");
        }

        var workflowid = workflow.fields["tran_name"].ToString();

        var workflowDefinition =
            await _workflowStepService.GetByWorkflowIdAndStep(workflowid, "workflowid")
            ?? throw new Exception("Workflow definition not found!");

        JObject jsResult = JObject.Parse(strJsonResult)["txbody"].ToObject<JObject>();

        var transactionNumber = jsResult["TXREFID"].ToString();
        var valuedt = jsResult["VALUEDT"].ToString();
        var txBodyData = jsResult["TXBODY"] ?? new JObject();
        var txcode = jsResult["TXCODE"].ToString();

        txBodyData["transaction_number"] = transactionNumber;
        txBodyData["tran_reference"] = workflowid;
        txBodyData["valuedt"] = valuedt;
        JToken mappingDataSet = null;
        if (!jsResult["DATASET"].IsEmptyOrNull())
        {
            var dt = jsResult["DATASET"];
            var tokenMapping =
                await _mappingService.MappingResponse(workflowDefinition.MappingResponse, dt)
                ?? throw new Exception("Invalid workflow mapping response!");
            ((JObject)tokenMapping).Remove("account_number");
            mappingDataSet = tokenMapping.ToObject<JObject>().ConvertToJObjectArrayDetails();
        }

        var mappingResponse =
            await _mappingService.MappingResponse(
                workflowDefinition.MappingResponse,
                txBodyData
            ) ?? throw new Exception("Invalid workflow mapping response!");

        //var response = JObject.Parse(journal.ToSerialize());
        //var resultView = response
        //    .ToDictionary()
        //    .MergeDictionary(mappingResponse.ToDictionary());
        //resultView["dataset"] = mappingDataSet;
        // var responseDone = await _transactionDoneService.GetByTransactionNumber(
        //     transactionNumber
        // );
        // if (responseDone != null)
        // {
        //     var doneBody = JObject.Parse(responseDone.ResponseBody);
        //     if (doneBody.TryGetValue("postings_debit", out var postings_debit))
        //     {
        //         resultView["postings_debit"] = postings_debit;
        //     }
        //     if (doneBody.TryGetValue("postings_credit", out var postings_credit))
        //     {
        //         resultView["postings_credit"] = postings_credit;
        //     }

        //     if (doneBody.TryGetValue("data_fees", out var dataFees))
        //     {
        //         resultView["fee_data"] = dataFees.MapToModel<List<DataFeeModel>>();
        //     }
        // }
        //return resultView.BuildWorkflowResponseSuccess(false);
        return null;
    }
}
