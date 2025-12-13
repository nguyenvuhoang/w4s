using Microsoft.Net.Http.Headers;
using O24OpenAPI.Core.Exceptions;
using O24OpenAPI.Web.CMS.Models;
using System.Text;

namespace O24OpenAPI.Web.CMS.Services.Services;

public class CrossWorkflowService : ICrossWorkflowService
{
    public async Task<WorkflowScheme> CallApiAsync(CallApiModel callApiModel)
    {
        HttpClientHandler clientHandler =
            new()
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
            ServiceUtils.GetServiceUrl(callApiModel.ServiceID)
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
        var responseData = new WorkflowScheme();
        if (!result.IsSuccessStatusCode)
        {
            throw new ExecuteWorkflowException(await result.GetResponseAsync<string>());
        }
        else
        {
            responseData = await result.GetResponseAsync<WorkflowScheme>();
        }

        return responseData;
    }
}
