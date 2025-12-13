using Newtonsoft.Json.Linq;

namespace O24OpenAPI.Web.CMS.Services.Interfaces;

public partial interface IPostService
{
    Task<JObject> GetDataPostAPI(string appCodeRequest);

    Task<JToken> O9CallAPIAsync(string appCodeRequest, string learn_api);

    Task<JObject> ExecuteAsync();
}
