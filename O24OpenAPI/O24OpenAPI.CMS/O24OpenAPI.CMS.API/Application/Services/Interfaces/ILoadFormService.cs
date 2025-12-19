using Newtonsoft.Json.Linq;

namespace O24OpenAPI.CMS.API.Application.Services.Interfaces;

public interface ILoadFormService
{
    Task<JToken> LoadFormAndRoleTask(FormModelRequest model);
}
