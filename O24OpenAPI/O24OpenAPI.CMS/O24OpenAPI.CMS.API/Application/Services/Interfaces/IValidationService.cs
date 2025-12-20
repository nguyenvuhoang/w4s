using System.Text.Json;
using Newtonsoft.Json.Linq;

namespace O24OpenAPI.CMS.API.Application.Services.Interfaces;

public interface IValidationService
{
    bool EvaluateCondition(JsonElement config, JsonElement data);
    Task<bool> EvaluateCondition(string config, JObject data);
}
