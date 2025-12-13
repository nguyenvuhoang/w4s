using System.Text.Json;
using Newtonsoft.Json.Linq;

namespace O24OpenAPI.WFO.Lib.Mapping;

public class JsonMapper
{
    public class MappingField
    {
        public string func { get; set; }
        public string type { get; set; }
        public string[] paras { get; set; }
    }

    /// <summary>
    /// Maps data from input fields using a mapping template
    /// </summary>
    /// <param name="template">JSON mapping template</param>
    /// <param name="inputFields">Source input fields</param>
    /// <returns>Mapped object based on template</returns>
    public static object Map(string template, Dictionary<string, object> inputFields)
    {
        try
        {
            // Convert input fields to JObject for easier access
            var inputJson = new JObject
            {
                {
                    "input",
                    new JObject() { { "fields", JObject.FromObject(inputFields) } }
                },
            };

            // Parse template
            var templateElement = JsonDocument.Parse(template).RootElement;

            return MapElement(templateElement, inputJson);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error mapping fields: {ex.Message}");
        }
    }

    private static object MapElement(JsonElement element, JObject inputJson)
    {
        switch (element.ValueKind)
        {
            case JsonValueKind.Object:
                // Check if this is a mapping field
                if (IsMappingField(element))
                {
                    var mappingField = JsonSerializer.Deserialize<MappingField>(
                        element.ToString()
                    );
                    return MapFromInput(mappingField, inputJson);
                }

                // Regular object - map all properties
                var result = new Dictionary<string, object>();
                foreach (var property in element.EnumerateObject())
                {
                    result[property.Name] = MapElement(property.Value, inputJson);
                }
                return result;

            case JsonValueKind.Array:
                return element
                    .EnumerateArray()
                    .Select(item => MapElement(item, inputJson))
                    .ToList();

            default:
                return element.GetRawText();
        }
    }

    private static bool IsMappingField(JsonElement element)
    {
        return element.TryGetProperty("func", out var funcProp)
            && element.TryGetProperty("paras", out var parasProp)
            && funcProp.GetString()?.ToLower() == "mapfrominput";
    }

    private static object MapFromInput(MappingField field, JObject inputJson)
    {
        try
        {
            if (field.paras == null || field.paras.Length == 0)
            {
                throw new ArgumentException("No parameters specified for MapFromInput");
            }

            var path = field.paras[0];
            var token =
                inputJson.SelectToken(path) ?? throw new Exception($"Path not found: {path}");

            return ConvertValue(token, field.type);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error in MapFromInput: {ex.Message}");
        }
    }

    private static object ConvertValue(JToken token, string type)
    {
        if (token.Type == JTokenType.Null)
        {
            return null;
        }

        switch (type?.ToLower())
        {
            case "number":
                return token.Value<decimal>();

            case "boolean":
                return token.Value<bool>();

            case "date":
                return token.Value<DateTime>();

            case "array":
                return token.Type == JTokenType.Array ? token.Values<object>().ToList() : null;

            case "object":
                return token.Type == JTokenType.Object
                    ? token.ToObject<Dictionary<string, object>>()
                    : null;

            case "string":
            default:
                return token.ToString();
        }
    }
}
