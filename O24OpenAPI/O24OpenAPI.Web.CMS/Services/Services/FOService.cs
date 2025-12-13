using System.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.Web.CMS.Models;
using O24OpenAPI.Web.CMS.Models.O9;
using O24OpenAPI.Web.CMS.Services.Interfaces;

namespace O24OpenAPI.Web.CMS.Services.Services;

/// <summary>
///
/// </summary>
public class FOService(IMappingService mappingService, IO9ClientService o9clientService)
    : IFOService
{
    private readonly IMappingService _mappingService = mappingService;
    private readonly IO9ClientService _o9clientService = o9clientService;

    /// <summary>
    ///
    /// </summary>
    /// <param name="workflow"></param>
    /// <returns></returns>
    public (JObject jsRequest, string status, string approver) PrepareRequest(
        WorkflowRequestModel workflow
    )
    {
        JObject jsRequest = workflow.ObjectField;
        string approver = GetApproverAndStatus(jsRequest, out string status);
        jsRequest = O9Utils.ProcessIfcFees(workflow.WorkflowFunc, jsRequest);
        return (jsRequest, status, approver);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="workflow"></param>
    /// <param name="jsRequest"></param>
    /// <param name="status"></param>
    /// <param name="jsFees"></param>
    /// <returns></returns>
    public async Task<string> GenerateFrontOfficeRequest(
        WorkflowRequestModel workflow,
        JObject jsRequest,
        string status,
        JObject jsFees
    )
    {
        var frontOfficeModel = new FrontOfficeModel(workflow, status, jsRequest, jsFees);
        return await _o9clientService.GenJsonFrontOfficeRequestAsync(
            frontOfficeModel,
            workflow.IsFrontOfficeMapping
        );
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="workflow"></param>
    /// <param name="objectFo"></param>
    /// <returns></returns>
    public async Task<(JToken txBody, JToken refId)> ProcessFrontOfficeResponse(
        WorkflowRequestModel workflow,
        JObject objectFo
    )
    {
        var hasTxRefId = objectFo.TryGetValue("txrefid", out var refId);
        var hasTxDt = objectFo.TryGetValue("txdt", out var tranDateToken);

        var txBodyObject = objectFo.Value<JObject>("txbody") ?? [];

        if (hasTxRefId)
        {
            txBodyObject["transaction_number"] = refId;
            txBodyObject["txrefid"] = refId;
        }
        var requestField = workflow.ObjectField.ConvertToJObject();
        requestField.Merge(txBodyObject);

        txBodyObject = requestField;

        if (hasTxDt)
        {
            txBodyObject["time"] = tranDateToken;
        }

        var isDataSet = objectFo["dataset"] is not null && !objectFo["dataset"].IsNullOrEmpty();
        if (!objectFo["dataset"].IsEmptyOrNull()) { }
        var dataMappingResponse = isDataSet
            ? JToken.Parse(objectFo["dataset"].ToString())
            : txBodyObject;

        workflow.MappingResponse ??= new JObject
        {
            { "transaction_number", "MapS.dataS(transaction_number)" },
        }.ToSerialize();
        var txBody = await _mappingService.MappingResponse(
            workflow.MappingResponse,
            dataMappingResponse
        );
        if (isDataSet)
        {
            txBody = new JObject { { "dataset", txBody }, { "transaction_number", refId } };
        }
        if (hasTxRefId)
        {
            txBody["ref_id"] = refId;
            objectFo["ref_id"] = refId;
            txBody["txdt"] = tranDateToken;
        }

        return (txBody, refId ?? JValue.CreateNull());
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="objectFo"></param>
    /// <param name="txBody"></param>
    /// <param name="vouchers"></param>
    /// <param name="transactionFees"></param>
    /// <returns></returns>
    public JObject PrepareTranResponse(
        JObject objectFo,
        JToken txBody,
        JToken vouchers,
        JArray transactionFees
    )
    {
        JObject tranResponse = objectFo;

        if (vouchers != null)
        {
            txBody["vouchers"] = vouchers;
            tranResponse["vouchers"] = vouchers;
        }

        tranResponse["data_fees"] = transactionFees;

        return tranResponse;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="workflow"></param>
    /// <param name="txBody"></param>
    /// <param name="tranResponse"></param>
    /// <param name="startTime"></param>
    /// <param name="stopwatch"></param>
    /// <returns></returns>
    public async Task<JToken> FinalizeWithoutPostings(
        WorkflowRequestModel workflow,
        JToken txBody,
        JObject tranResponse,
        long startTime,
        Stopwatch stopwatch
    )
    {
        stopwatch.Stop();
        tranResponse["mapping_response"] = txBody;

        return await Task.FromResult(txBody);
    }

    //
    // /// <summary>
    // /// Adds the entry journals to response using the specified tx body token
    // /// </summary>
    // /// <param name="txBodyToken">The tx body token</param>
    // /// <param name="objectFo">The postings</param>
    // /// <returns>The response</returns>
    // public JObject AddEntryJournalsToResponse(JObject txBodyToken, JObject objectFo)
    // {
    //     try
    //     {
    //         if (txBodyToken == null)
    //         {
    //             return null;
    //         }
    //         JObject response = txBodyToken ?? new JObject();
    //         if (objectFo.TryGetValue("postings", out var postingsToken))
    //         {
    //             List<TemporaryPosting> postings =
    //                         postingsToken?.ToObject<JArray>().MapToModel<List<TemporaryPosting>>();
    //
    //             var debits = postings.Where(s => s.action == "D").ToList();
    //             var credits = postings.Where(s => s.action == "C").ToList();
    //             response.Add("postings_debit", debits.ToJToken());
    //             response.Add("postings_credit", credits.ToJToken());
    //         }
    //
    //         return response;
    //     }
    //     catch (Exception)
    //     {
    //         throw;
    //     }
    //
    // }

    /// <summary>
    /// /
    /// </summary>
    /// <param name="jsRequest"></param>
    /// <param name="status"></param>
    /// <returns></returns>
    public string GetApproverAndStatus(JObject jsRequest, out string status)
    {
        string approverIdKey = "approverid";
        string approver = null;
        status = "C";

        if (jsRequest.ContainsKey(approverIdKey))
        {
            int? approverId = jsRequest.Value<int?>(approverIdKey);
            approver = approverId?.ToString();
            if (approverId == -1)
            {
                status = "P";
            }
        }

        return approver;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="jsRequest"></param>
    /// <param name="transactionFees"></param>
    /// <returns></returns>
    public JObject ExtractAndRemoveIfcFees(JObject jsRequest, ref JArray transactionFees)
    {
        JObject jsFees = null;
        if (jsRequest.ContainsKey("ifcfees"))
        {
            if (jsRequest.SelectToken("ifcfees").HasValues)
            {
                transactionFees = (JArray)jsRequest.SelectToken("ifcfees");
                jsFees = transactionFees.ConvertToJArrayDetails();
            }

            jsRequest.Remove("ifcfees");
        }

        return jsFees;
    }
}
