using Newtonsoft.Json.Linq;

namespace O24OpenAPI.WFO.Lib;

public class JTokenHelper
{
    public static string NormalizeToken(JToken token)
    {
        return (token is JObject obj && !obj.HasValues)
            ? null
            : token?.ToString(Newtonsoft.Json.Formatting.None);
    }
}
