using System.Text.Json;
using Newtonsoft.Json.Linq;

namespace O24OpenAPI.Web.CMS.Services.Interfaces;

public interface IValidationService
{
    bool EvaluateCondition(JsonElement config, JsonElement data);
    Task<bool> EvaluateCondition(string config, JObject data);
}
