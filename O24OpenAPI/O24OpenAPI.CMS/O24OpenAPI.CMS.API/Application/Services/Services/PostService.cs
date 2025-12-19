using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using O24OpenAPI.CMS.API.Application.Models.ContextModels;
using O24OpenAPI.CMS.API.Application.Services.Interfaces;
using O24OpenAPI.CMS.API.Application.Utils;
using O24OpenAPI.Core.Configuration;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Services.Mapping;
using O24OpenAPI.GrpcContracts.GrpcClientServices.WFO;
using System.Text;

namespace O24OpenAPI.CMS.API.Application.Services.Services;

/// <summary>
/// The post service class
/// </summary>
public partial class PostService(
    ILearnApiService learnApiService,
    IApiService apiService,
    JWebUIObjectContextModel context1,
    IDataMappingService dataMappingService
) : IPostService
{
    private readonly ILearnApiService _learnApiService = learnApiService;
    private readonly IApiService _apiService = apiService;
    private readonly JWebUIObjectContextModel context = context1;
    private readonly IDataMappingService _dataMappingService = dataMappingService;
    private readonly WorkContext _workContext = EngineContext.Current.Resolve<WorkContext>();



    #region  ExecuteAsync
    public async Task<JObject> ExecuteAsync()
    {
        try
        {
            JObject boInput = context.Bo.GetBoInput();
            if (boInput == null)
            {
                throw new ArgumentNullException(
                    nameof(boInput),
                    "BoInput cannot be null or empty!"
                );
            }
            string response;
            if (
                !boInput.TryGetValue("workflowid", out JToken? workflowIdToken)
                || string.IsNullOrEmpty(workflowIdToken.ToString())
            )
            {
                string? learnApiId = boInput["learn_api"]?.ToString();
                if (string.IsNullOrEmpty(learnApiId))
                {
                    throw new ArgumentNullException(
                        nameof(learnApiId),
                        "LearnApi cannot be null or empty!"
                    );
                }
                LearnApi learnApi = await _learnApiService.GetByLearnApiIdAndChannel(
                    learnApiId,
                    context.InfoApp.GetApp()
                );
                JObject mappedRequest = BuildContentWorkflowRequest();
                if (learnApi.LearnApiMapping.HasValue())
                {
                    mappedRequest = await _dataMappingService.MapDataAsync(boInput, mappedRequest);
                }
                if (learnApi.URI.StartsWith("$"))
                {
                    string uri = learnApi.URI;
                    int firstKey = uri.IndexOf('$');
                    int secondKey = uri.IndexOf('$', firstKey + 1);

                    if (firstKey != -1 && secondKey != -1 && secondKey > firstKey)
                    {
                        string settingKey = uri.Substring(firstKey + 1, secondKey - firstKey - 1);
                        string remainingPath = uri.Substring(secondKey + 1);
                        if (settingKey.Contains("WFO"))
                        {
                            string wfoURI = Singleton<O24OpenAPIConfiguration>.Instance.WFOHttpURL;
                            learnApi.URI = wfoURI + remainingPath;
                        }
                        else
                        {
                            ICMSSettingService? settingService =
                                EngineContext.Current.Resolve<ICMSSettingService>();
                            string setting = await settingService.GetStringValue(settingKey);
                            learnApi.URI = setting + remainingPath;
                        }
                    }
                }
                response = await CallAPIAsync(
                    path: learnApi.URI,
                    method: learnApi.LearnApiMethod,
                    content: mappedRequest,
                    headerToken: context.InfoUser.GetUserLogin().Token
                );
            }
            else
            {
                JObject obContent = BuildContentWorkflowRequest();
                IWFOGrpcClientService? wfoGrpcClientService = EngineContext.Current.Resolve<IWFOGrpcClientService>();
                response = await wfoGrpcClientService.ExecuteWorkflowAsync(
                    JsonConvert.SerializeObject(obContent)
                );
            }

            JObject result = JObject.Parse(response);
            if (result.TryGetValue("error_message", out JToken? messageToken))
            {
                if (!string.IsNullOrEmpty(messageToken?.ToString()))
                {
                    result["data"] = new JObject
                    {
                        { "error_message", messageToken },
                        { "next_action", result["error_next_action"] },
                        { "error_code", result["error_code"] },
                    };
                }
            }
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.StackTrace);
            throw;
        }
    }

    private JObject BuildContentWorkflowRequest()
    {
        JObject sourceInput = context.Bo.GetBoInput();
        JObject wfInput = new(sourceInput)
        {
            ["lang"] = context.InfoRequest.Language,
            ["token"] = context.InfoUser.GetUserLogin().Token,
            ["user_id"] = context.InfoUser.UserSession?.UserId.ToString() ?? "0",
            ["user_code"] = context.InfoUser.UserSession?.UserCode.ToString() ?? "0",
            ["execution_id"] = _workContext?.ExecutionId ?? Guid.NewGuid().ToString(),
            ["channel_id"] = context.InfoApp.GetApp(),
            ["transaction_date"] = DateTime.UtcNow,
            //["value_date"] = context.InfoUser.UserSession? ?? DateTime.UtcNow,
            ["device"] = JToken.FromObject(
                new DeviceModel
                {
                    IpAddress = context.InfoRequest.GetIp() ?? "::1",
                    DeviceId = string.IsNullOrWhiteSpace(context.InfoRequest.DeviceID)
                        ? Guid.NewGuid().ToString()
                        : context.InfoRequest.DeviceID,
                    OsVersion =
                        context.InfoRequest.OsVersion
                        ?? context.InfoRequest.GetClientOs()
                        ?? "unknown",
                    UserAgent =
                        context.InfoRequest.UserAgent
                        ?? context.InfoRequest.GetClientBrowser()
                        ?? string.Empty,
                    DeviceType = context.InfoRequest.DeviceType ?? "unknown",
                    AppVersion = context.InfoRequest.AppVersion ?? "1.0.0",
                    DeviceName = context.InfoRequest.DeviceName ?? "Generic Device",
                    Brand = context.InfoRequest.Brand ?? "Unknown",
                    IsEmulator = context.InfoRequest.IsEmulator,
                    IsRootedOrJailbroken = context.InfoRequest.IsRootedOrJailbroken,
                    Memory = context.InfoRequest.Memory,
                    Network = context.InfoRequest.Network,
                }
            ),
        }; // Create a new copy of the input
        return wfInput;
    }

    private static async Task<string> CallAPIAsync(
        string path,
        string method,
        object content,
        string headerToken = ""
    )
    {
        HttpClientHandler clientHandler = new()
        {
            ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) =>
            {
                return true;
            },
        };

        HttpClient httpClient = new(clientHandler);
        httpClient.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
        if (!string.IsNullOrEmpty(headerToken))
        {
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + headerToken);
        }

        string requestString = JsonConvert.SerializeObject(content);
        StringContent requestContent = new(requestString, Encoding.Default, "application/json");
        HttpRequestMessage requestMessage = new(new HttpMethod(method), path)
        {
            Content = requestContent,
        };
        HttpResponseMessage httpResponse = await httpClient.SendAsync(requestMessage);
        httpResponse.EnsureSuccessStatusCode();
        string result = await httpResponse.Content.ReadAsStringAsync();

        return result;
    }
    #endregion
}
