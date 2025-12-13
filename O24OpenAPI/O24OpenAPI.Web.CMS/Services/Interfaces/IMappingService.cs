using Newtonsoft.Json.Linq;
using O24OpenAPI.Web.CMS.Models;

namespace O24OpenAPI.Web.CMS.Services.Interfaces;

public partial interface IMappingService
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="mappingResponse"></param>
    /// <param name="data"></param>
    /// <param name="learnApiData"></param>
    /// <returns></returns>
    Task<JToken> MappingResponse(string mappingResponse, JToken data, string learnApiData = "");

    /// <summary>
    ///
    /// </summary>
    /// <param name="workflowInfo"></param>
    /// <param name="dataRequest"></param>
    /// <returns></returns>
    Task<LearnApiModel> MappingRequest(WorkflowInfo workflowInfo, JObject dataRequest);

    /// <summary>
    ///
    /// </summary>
    /// <param name="learnApiContent"></param>
    /// <param name="dataMap"></param>
    /// <param name="appRequest"></param>
    /// <param name="isMappingLowerKey"></param>
    /// <returns></returns>
    Task<LearnApiModel> O9MapData(
        LearnApiModel learnApiContent,
        JObject dataMap,
        string appRequest,
        bool isMappingLowerKey = true
    );

    Task<LearnApiModel> O9MapDatas(
        LearnApiModel learnApiContent,
        JObject dataMap,
        string appRequest,
        bool isMappingLowerKey = true
    );

    Task<object> WorkflowStepMapData(
        string paramMapper,
        LearnApiRequestModel request,
        List<WorkflowScheme> response,
        bool isMappingLowerKey = false
    );
    Task<object> WorkflowResponseMapData(
        string paramMapper,
        List<WorkflowScheme> response,
        bool isMappingLowerKey = true
    );
}
