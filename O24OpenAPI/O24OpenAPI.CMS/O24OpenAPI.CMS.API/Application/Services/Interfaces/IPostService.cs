using Newtonsoft.Json.Linq;

namespace O24OpenAPI.CMS.API.Application.Services.Interfaces;

public partial interface IPostService
{

    Task<JObject> ExecuteAsync();
}
