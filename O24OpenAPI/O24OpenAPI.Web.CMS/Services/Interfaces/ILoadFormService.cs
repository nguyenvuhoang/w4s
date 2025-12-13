using Newtonsoft.Json.Linq;

namespace O24OpenAPI.Web.CMS.Services.Interfaces;

public interface ILoadFormService
{
    Task<JToken> LoadFormAndRoleTask(FormModelRequest model);
}
