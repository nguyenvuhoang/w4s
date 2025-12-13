using Newtonsoft.Json.Linq;

namespace O24OpenAPI.Web.CMS.Services.Interfaces;

public interface ICallMapService
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="obMapp_"></param>
    /// <param name="appRequest"></param>
    /// <param name="uid"></param>
    /// <param name="dataMap"></param>
    /// <param name="learnApiData"></param>
    /// <param name="isO9Mapping"></param>
    /// <returns></returns>
    Task <JObject> LoopConfigMapping(
        JObject obMapp_,
        string appRequest,
        string uid,
        JObject dataMap,
        string learnApiData,
        bool isO9Mapping = false
    );

    /// <summary>
    /// 
    /// </summary>
    /// <param name="obMapp_"></param>
    /// <param name="appRequest"></param>
    /// <param name="uid"></param>
    /// <param name="dataMap"></param>
    /// <param name="learnApiData"></param>
    /// <param name="isO9Mapping"></param>
    /// <returns></returns>
    Task<JArray> LoopConfigMappingArray(
        JArray obMapp_,
        string appRequest,
        string uid,
        JObject dataMap,
        string learnApiData,
        bool isO9Mapping = false
    );
}