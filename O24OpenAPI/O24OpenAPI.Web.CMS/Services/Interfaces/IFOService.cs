using System.Diagnostics;
using Newtonsoft.Json.Linq;
using O24OpenAPI.Web.CMS.Models;

namespace O24OpenAPI.Web.CMS.Services.Interfaces;

public interface IFOService
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="workflow"></param>
    /// <returns></returns>
    (JObject jsRequest, string status, string approver) PrepareRequest(
        WorkflowRequestModel workflow
    );

    /// <summary>
    ///
    /// </summary>
    /// <param name="workflow"></param>
    /// <param name="jsRequest"></param>
    /// <param name="status"></param>
    /// <param name="jsFees"></param>
    /// <returns></returns>
    Task<string> GenerateFrontOfficeRequest(
        WorkflowRequestModel workflow,
        JObject jsRequest,
        string status,
        JObject jsFees
    );

    /// <summary>
    ///
    /// </summary>
    /// <param name="workflow"></param>
    /// <param name="objectFo"></param>
    /// <returns></returns>
    Task<(JToken txBody, JToken refId)> ProcessFrontOfficeResponse(
        WorkflowRequestModel workflow,
        JObject objectFo
    );

    /// <summary>
    ///
    /// </summary>
    /// <param name="objectFo"></param>
    /// <param name="txBody"></param>
    /// <param name="vouchers"></param>
    /// <param name="transactionFees"></param>
    /// <returns></returns>
    JObject PrepareTranResponse(
        JObject objectFo,
        JToken txBody,
        JToken vouchers,
        JArray transactionFees
    );

    /// <summary>
    ///
    /// </summary>
    /// <param name="workflow"></param>
    /// <param name="txBody"></param>
    /// <param name="tranResponse"></param>
    /// <param name="startTime"></param>
    /// <param name="stopwatch"></param>
    /// <returns></returns>
    Task<JToken> FinalizeWithoutPostings(
        WorkflowRequestModel workflow,
        JToken txBody,
        JObject tranResponse,
        long startTime,
        Stopwatch stopwatch
    );

    // /// <summary>
    // ///
    // /// </summary>
    // /// <param name="txBodyToken"></param>
    // /// <param name="objectFo"></param>
    // /// <returns></returns>
    // JObject AddEntryJournalsToResponse(JObject txBodyToken, JObject objectFo);
    /// <summary>
    ///
    /// </summary>
    /// <param name="jsRequest"></param>
    /// <param name="status"></param>
    /// <returns></returns>
    string GetApproverAndStatus(JObject jsRequest, out string status);

    /// <summary>
    ///
    /// </summary>
    /// <param name="jsRequest"></param>
    /// <param name="transactionFees"></param>
    /// <returns></returns>
    JObject ExtractAndRemoveIfcFees(JObject jsRequest, ref JArray transactionFees);
}
