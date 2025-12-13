using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace O24OpenAPI.Web.CMS.Models.O9;

/// <summary>
///
/// </summary>
public class JsonConverterCore : JsonConverter
{
    private readonly Type[] _types;

    /// <summary>
    ///
    /// </summary>
    public JsonConverterCore(params Type[] types)
    {
        _types = types;
    }

    /// <summary>
    ///
    /// </summary>
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        JToken t = JToken.FromObject(value);
    }

    /// <summary>
    ///
    /// /// </summary>
    public override object ReadJson(
        JsonReader reader,
        Type objectType,
        object existingValue,
        JsonSerializer serializer
    )
    {
        if (reader.TokenType == JsonToken.Null)
        {
            return null;
        }

        var token = (JObject)JToken.Load(reader);
        var o = token.ToObject(objectType);

        return token.ToObject(objectType);
    }

    /// <summary>
    ///
    /// </summary>
    public override bool CanConvert(Type objectType)
    {
        return true;
        //return _types.Any(t => t == objectType);
    }
}
