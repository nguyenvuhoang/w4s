using System.Text.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.Web.CMS.Models;
using O24OpenAPI.Web.CMS.Models.ContextModels;
using O24OpenAPI.Web.CMS.Services.Interfaces;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace O24OpenAPI.Web.CMS.Services.Services;

public class MappingService : IMappingService
{
    private readonly ICallMapService _callMapService;

    /// <summary>
    ///
    /// </summary>
    public JWebUIObjectContextModel Context { get; set; } =
        EngineContext.Current.Resolve<JWebUIObjectContextModel>();

    /// <summary>
    ///
    /// </summary>
    /// <param name="callMapService"></param>
    public MappingService(ICallMapService callMapService)
    {
        _callMapService = callMapService;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="workflowInfo"></param>
    /// <param name="dataRequest"></param>
    /// <returns></returns>
    public async Task<LearnApiModel> MappingRequest(WorkflowInfo workflowInfo, JObject dataRequest)
    {
        if (string.IsNullOrEmpty(workflowInfo.MappingRequest))
        {
            throw new Exception("Missing mapping request value!");
        }

        var learnApiModel = new LearnApiModel()
        {
            LearnApiMapping = workflowInfo.MappingRequest,
            LearnApiData = "",
        };

        learnApiModel = await O9MapData(learnApiModel, dataRequest.ConvertToJObject(), "ncbsCbs");
        return learnApiModel;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="mappingResponse"></param>
    /// <param name="data"></param>
    /// <param name="learnApiData"></param>
    /// <returns></returns>
    public async Task<JToken> MappingResponse(
        string mappingResponse,
        JToken data,
        string learnApiData = ""
    )
    {
        if (string.IsNullOrEmpty(mappingResponse))
        {
            throw new Exception("Missing mapping response value!");
        }

        JToken result = null;
        var mappingResponseObject = JObject.Parse(mappingResponse);

        if (data is JArray jArray)
        {
            result = new JArray(
                jArray
                    .Select(async item =>
                    {
                        var learnApiModel = new LearnApiModel
                        {
                            LearnApiMapping = mappingResponse,
                            LearnApiData = learnApiData,
                        };

                        var mapObj =
                            await O9MapData(learnApiModel, item.ToObject<JObject>(), "ncbsCbs")
                            ?? throw new Exception(
                                "An error occurred while mapping data response!"
                            );

                        return JObject.Parse(mapObj.LearnApiMapping);
                    })
                    .Select(t => t.GetAsyncResult())
            );
        }
        else if (data is JObject)
        {
            var resultObject = new JObject();
            var dataObject = data.ToObject<JObject>();
            if (mappingResponseObject.TryGetValue("vouchers", out JToken mapVouchers))
            {
                var vouchers = mapVouchers.ToObject<JArray>();
                mappingResponseObject.Remove("vouchers");

                resultObject.Add(
                    "vouchers",
                    new JArray(
                        vouchers
                            .Select(async voucher =>
                            {
                                var mappingVoucher = await MappingResponse(
                                    voucher["tags"].ToSerialize(),
                                    data
                                );
                                voucher["tags"] = mappingVoucher;
                                return voucher;
                            })
                            .Select(t => t.GetAsyncResult())
                    )
                );
            }

            mappingResponse = mappingResponseObject.ToSerialize();

            var learnApiModel = new LearnApiModel
            {
                LearnApiMapping = mappingResponse,
                LearnApiData = "",
            };

            learnApiModel = await O9MapData(learnApiModel, dataObject, "ncbsCbs");
            resultObject.Merge(JObject.Parse(learnApiModel.LearnApiMapping));
            result = resultObject;
        }

        return result;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="learnApiContent"></param>
    /// <param name="dataMap"></param>
    /// <param name="appRequest"></param>
    /// <param name="isMappingLowerKey"></param>
    /// <returns></returns>
    public async Task<LearnApiModel> O9MapData(
        LearnApiModel learnApiContent,
        JObject dataMap,
        string appRequest,
        bool isMappingLowerKey = true
    )
    {
        if (learnApiContent != null)
        {
            var uid = Context.InfoUser.GetUserLogin().Token;

            try
            {
                if (learnApiContent.LearnApiMapping != null)
                {
                    string learnApiData = "";
                    if (!string.IsNullOrEmpty(learnApiContent.LearnApiData))
                    {
                        learnApiData = learnApiContent.LearnApiData;
                    }

                    LearnApiModel obPackApi = learnApiContent;

                    if (CMS.Utils.Utils.IsValidJsonObject(learnApiContent.LearnApiMapping.Trim()))
                    {
                        JObject obMapp_ = JsonConvert
                            .DeserializeObject(learnApiContent.LearnApiMapping.Trim())
                            .ToJObject();

                        obMapp_ ??= [];

                        await _callMapService.LoopConfigMapping(
                            obMapp_,
                            appRequest,
                            uid,
                            dataMap,
                            learnApiData,
                            isMappingLowerKey
                        );
                        //body
                        obPackApi.LearnApiMapping = JsonConvert.SerializeObject(obMapp_);
                    }
                    else if (
                        CMS.Utils.Utils.IsValidJsonArray(learnApiContent.LearnApiMapping.Trim())
                    )
                    {
                        JArray obMapp_ = JArray.FromObject(
                            JsonConvert.DeserializeObject(learnApiContent.LearnApiMapping.Trim())
                        );

                        obMapp_ ??= new JArray();

                        await _callMapService.LoopConfigMappingArray(
                            obMapp_,
                            appRequest,
                            uid,
                            dataMap,
                            learnApiData,
                            isMappingLowerKey
                        );
                        obPackApi.LearnApiMapping = JsonConvert.SerializeObject(obMapp_);
                    }

                    //header
                    if (!string.IsNullOrEmpty(learnApiContent.LearnApiHeader))
                    {
                        JObject headerApi = JsonConvert.DeserializeObject<JObject>(
                            learnApiContent.LearnApiHeader
                        );
                        await _callMapService.LoopConfigMapping(
                            headerApi,
                            appRequest,
                            uid,
                            dataMap,
                            learnApiData
                        );

                        obPackApi.LearnApiHeader = JsonConvert.SerializeObject(headerApi);
                    }
                    else
                    {
                        obPackApi.LearnApiHeader = null;
                    }

                    obPackApi.UserSessions =
                        SessionUtils.GetUserSession(Context)
                        ?? dataMap.SelectToken("user_session").JsonConvertToModel<UserSessions>()
                        ?? new UserSessions();

                    return obPackApi;
                }
            }
            catch (Exception ex)
            {
                if (Context.InfoUser.GetUserLogin().isDebug)
                {
                    JObject errorApi = new()
                    {
                        new JProperty("data", learnApiContent.LearnApiMapping),
                        new JProperty("mess", "Cant not parse learn_api_mapping"),
                        new JProperty("function", "BasicAction.txMapData"),
                    };
                    Context.Bo.AddPackFo("error_api", errorApi);
                }

                Console.WriteLine(ex.StackTrace);
            }
        }

        return null;
    }

    public async Task<LearnApiModel> O9MapDatas(
        LearnApiModel learnApiContent,
        JObject dataMap,
        string appRequest,
        bool isMappingLowerKey = true
    )
    {
        await Task.CompletedTask;
        if (learnApiContent != null)
        {
            var uid = Context.InfoUser.GetUserLogin().Token;

            try
            {
                if (learnApiContent.LearnApiMapping != null)
                {
                    string learnApiData = "";
                    if (!string.IsNullOrEmpty(learnApiContent.LearnApiData))
                    {
                        learnApiData = learnApiContent.LearnApiData;
                    }

                    LearnApiModel obPackApi = learnApiContent;
                    obPackApi.LearnApiMapping = JsonConvert.SerializeObject(dataMap);
                    obPackApi.UserSessions =
                        SessionUtils.GetUserSession(Context)
                        ?? dataMap.SelectToken("user_session").JsonConvertToModel<UserSessions>()
                        ?? new UserSessions();

                    return obPackApi;
                }
            }
            catch (Exception ex)
            {
                if (Context.InfoUser.GetUserLogin().isDebug)
                {
                    JObject errorApi = new()
                    {
                        new JProperty("data", learnApiContent.LearnApiMapping),
                        new JProperty("mess", "Cant not parse learn_api_mapping"),
                        new JProperty("function", "BasicAction.txMapData"),
                    };
                    Context.Bo.AddPackFo("error_api", errorApi);
                }

                Console.WriteLine(ex.StackTrace);
            }
        }

        return null;
    }

    public async Task<object> WorkflowStepMapData(
        string paramMapper,
        LearnApiRequestModel request,
        List<WorkflowScheme> response,
        bool isMappingLowerKey = false
    )
    {
        await Task.CompletedTask;
        if (paramMapper != null)
        {
            try
            {
                var dataMap = new JObject
                {
                    { "request", request.ObjectField },
                    {
                        "response",
                        response
                            .Select(e =>
                                e.Response.Data is JsonElement element
                                    ? JToken.Parse(element.GetRawText())
                                    : e.Response.Data
                            )
                            .ToJToken()
                    },
                };
                if (CMS.Utils.Utils.IsValidJsonObject(paramMapper.Trim()))
                {
                    var obMapp = JsonConvert.DeserializeObject(paramMapper.Trim()).ToJObject();

                    obMapp ??= [];

                    await _callMapService.LoopConfigMapping(
                        obMapp,
                        "",
                        "",
                        dataMap,
                        "",
                        isMappingLowerKey
                    );
                    return JsonSerializer.Deserialize<object>(JsonConvert.SerializeObject(obMapp));
                    ;
                }
                else if (Utils.Utils.IsValidJsonArray(paramMapper.Trim()))
                {
                    JArray obMapp = JArray.FromObject(
                        JsonConvert.DeserializeObject(paramMapper.Trim())
                    );

                    obMapp ??= [];

                    await _callMapService.LoopConfigMappingArray(
                        obMapp,
                        "",
                        "",
                        dataMap,
                        "",
                        isMappingLowerKey
                    );
                    return JsonSerializer.Deserialize<object>(JsonConvert.SerializeObject(obMapp));
                }

                return new { };
            }
            catch (Exception ex)
            {
                if (Context.InfoUser.GetUserLogin().isDebug)
                {
                    JObject errorApi = new()
                    {
                        new JProperty("data", paramMapper),
                        new JProperty("mess", "Cant not parse learn_api_mapping"),
                        new JProperty("function", "BasicAction.txMapData"),
                    };
                    Context.Bo.AddPackFo("error_api", errorApi);
                }

                Console.WriteLine(ex.StackTrace);
            }
        }

        return null;
    }

    public async Task<object> WorkflowResponseMapData(
        string paramMapper,
        List<WorkflowScheme> response,
        bool isMappingLowerKey = true
    )
    {
        if (paramMapper != null)
        {
            try
            {
                var dataMap = new JObject
                {
                    {
                        "response",
                        response
                            .Select(e => JToken.Parse(((JsonElement)e.Response.Data).GetRawText()))
                            .ToJToken()
                    },
                };
                if (Utils.Utils.IsValidJsonObject(paramMapper.Trim()))
                {
                    var obMapp = JsonConvert.DeserializeObject(paramMapper.Trim()).ToJObject();

                    obMapp ??= new JObject();

                    await _callMapService.LoopConfigMapping(
                        obMapp,
                        "",
                        "",
                        dataMap,
                        "",
                        isMappingLowerKey
                    );
                    return JsonSerializer.Deserialize<object>(JsonConvert.SerializeObject(obMapp));
                    ;
                }
                else if (Utils.Utils.IsValidJsonArray(paramMapper.Trim()))
                {
                    JArray obMapp = JArray.FromObject(
                        JsonConvert.DeserializeObject(paramMapper.Trim())
                    );

                    obMapp ??= new JArray();

                    await _callMapService.LoopConfigMappingArray(
                        obMapp,
                        "",
                        "",
                        dataMap,
                        "",
                        isMappingLowerKey
                    );
                    return JsonSerializer.Deserialize<object>(JsonConvert.SerializeObject(obMapp));
                }

                return new { };
            }
            catch (Exception ex)
            {
                if (Context.InfoUser.GetUserLogin().isDebug)
                {
                    JObject errorApi = new()
                    {
                        new JProperty("data", paramMapper),
                        new JProperty("mess", "Cant not parse learn_api_mapping"),
                        new JProperty("function", "BasicAction.txMapData"),
                    };
                    Context.Bo.AddPackFo("error_api", errorApi);
                }

                Console.WriteLine(ex.StackTrace);
            }
        }

        return null;
    }
}
