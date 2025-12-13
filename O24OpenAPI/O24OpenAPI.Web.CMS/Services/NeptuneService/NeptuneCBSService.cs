using System.Text;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using O24OpenAPI.Core.Exceptions;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.Web.CMS.Commons;
using O24OpenAPI.Web.CMS.Configuration;
using O24OpenAPI.Web.CMS.Models;
using O24OpenAPI.Web.CMS.Models.Neptune;
using O24OpenAPI.Web.CMS.Services.Services.Logging;

namespace O24OpenAPI.Web.CMS.Services.NeptuneService;

public class NeptuneCBSService(
    CoreBankingSetting coreBankingSetting,
    IWorkflowStepLogService workflowStepLogService
) : INeptuneCBSService
{
    private readonly CoreBankingSetting _coreBankingSetting = coreBankingSetting;
    private readonly IWorkflowStepLogService _workflowStepLogService = workflowStepLogService;

    public async Task<ResponseModel> ExecuteWorkflow(ExecuteWorkflowNeptuneModel model)
    {
        var token = NeptuneSession.SessionToken;
        var refid = Guid.NewGuid().ToString();
        var content = new JObject
        {
            { "lang", "en" },
            { "token", $"{token}" },
            { "reference_id", refid },
            { "fields", model.Fields.ToJToken() },
        };

        var headers = new Dictionary<string, string>
        {
            { "Authorization", $"Bearer {_coreBankingSetting.StaticTokenPortal}" },
            { "Cache-Control", "no-cache" },
            { "Content-Type", "application/json" },
        };
        var callApiModel = new CallApiModel
        {
            WorkflowId = model.WorkflowId,
            Content = content.ToSerialize(),
            Header = headers,
        };
        var result = await CallApiAsync(callApiModel, refid);
        return result;
    }

    public async Task<ResponseModel> CallApiAsync(CallApiModel callApiModel, string refid)
    {
        await _workflowStepLogService.LogCallApiNeptune(model: callApiModel, refid: refid);
        HttpClientHandler clientHandler = new()
        {
            ServerCertificateCustomValidationCallback = (
                sender,
                cert,
                chain,
                sslPolicyErrors
            ) =>
            {
                return true;
            },
        };

        HttpClient httpClient = new(clientHandler);
        httpClient.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");

        var requestContent = new StringContent(
            callApiModel.Content,
            Encoding.UTF8,
            "application/json"
        );
        var requestMessage = new HttpRequestMessage(
            new HttpMethod("Post"),
            string.Format(_coreBankingSetting.NeptuneURL, callApiModel.WorkflowId)
        )
        {
            Content = requestContent,
        };
        foreach (var head in callApiModel.Header)
        {
            requestMessage.Headers.TryAddWithoutValidation(head.Key, head.Value);
        }
        httpClient.Timeout = TimeSpan.FromMinutes(10);
        HttpResponseMessage result = await httpClient.SendAsync(requestMessage);
        var responseData = new ResponseModel();
        if (!result.IsSuccessStatusCode)
        {
            _ = _workflowStepLogService.UpdateLogNeptune(
                refid: refid,
                stepCode: callApiModel.WorkflowId,
                status: "ERROR",
                responseData: responseData.ToSerialize()
            );
            throw new ExecuteWorkflowException(await result.GetResponseAsync<string>());
        }
        else
        {
            responseData = await result.GetResponseAsync<ResponseModel>();
            _ = _workflowStepLogService
                .UpdateLogNeptune(
                    refid: refid,
                    stepCode: callApiModel.WorkflowId,
                    status: responseData.Status,
                    responseData: responseData.ToSerialize()
                )
                .ConfigureAwait(false);
        }

        return responseData;
    }
}
