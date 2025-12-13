using System.Text.Json.Nodes;

namespace O24OpenAPI.Web.Framework.Services.Mapping;

public interface IDataMapper
{
    Task<Dictionary<string, object>> MapDataToDictionaryAsync(
        JsonNode source,
        JsonNode target,
        Func<string, Task<object>> func = null
    );
}
