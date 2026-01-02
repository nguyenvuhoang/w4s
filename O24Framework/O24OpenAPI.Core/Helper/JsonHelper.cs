using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

namespace O24OpenAPI.Core.Helper;

/// <summary>
/// The json helper class
/// </summary>
public static partial class JsonHelper
{
    /// <summary>
    /// The write indented
    /// </summary>
    private static readonly JsonSerializerOptions _options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower, // Chuyển đổi tên property sang snake_case
        WriteIndented = true, // Format JSON cho dễ đọc
    };

    /// <summary>
    /// Serializes the object with snake case naming using the specified obj
    /// </summary>
    /// <param name="obj">The obj</param>
    /// <returns>The string</returns>
    public static string SerializeObjectWithSnakeCaseNaming(object obj)
    {
        return obj == null ? null : JsonSerializer.Serialize(obj, _options);
    }

    /// <summary>
    /// Converts the json element to object using the specified element
    /// </summary>
    /// <param name="element">The element</param>
    /// <returns>The root</returns>
    public static object ConvertJsonElementToObject(JsonElement element)
    {
        Stack<(JsonElement, object)> stack = new();
        object root = element.ValueKind switch
        {
            JsonValueKind.Object => new Dictionary<string, object>(),
            JsonValueKind.Array => new List<object>(),
            _ => ConvertSimpleJsonValue(element),
        };

        stack.Push((element, root));

        while (stack.Count > 0)
        {
            var (currentElement, currentObj) = stack.Pop();

            if (
                currentElement.ValueKind == JsonValueKind.Object
                && currentObj is Dictionary<string, object> dict
            )
            {
                foreach (var prop in currentElement.EnumerateObject())
                {
                    var childObj = prop.Value.ValueKind switch
                    {
                        JsonValueKind.Object => new Dictionary<string, object>(),
                        JsonValueKind.Array => new List<object>(),
                        _ => ConvertSimpleJsonValue(prop.Value),
                    };

                    dict[prop.Name] = childObj;
                    if (
                        prop.Value.ValueKind == JsonValueKind.Object
                        || prop.Value.ValueKind == JsonValueKind.Array
                    )
                    {
                        stack.Push((prop.Value, childObj));
                    }
                }
            }
            else if (
                currentElement.ValueKind == JsonValueKind.Array
                && currentObj is List<object> list
            )
            {
                foreach (var item in currentElement.EnumerateArray())
                {
                    var childObj = item.ValueKind switch
                    {
                        JsonValueKind.Object => new Dictionary<string, object>(),
                        JsonValueKind.Array => new List<object>(),
                        _ => ConvertSimpleJsonValue(item),
                    };

                    list.Add(childObj);
                    if (
                        item.ValueKind == JsonValueKind.Object
                        || item.ValueKind == JsonValueKind.Array
                    )
                    {
                        stack.Push((item, childObj));
                    }
                }
            }
        }

        return root;
    }

    /// <summary>
    /// Converts the simple json value using the specified element
    /// </summary>
    /// <param name="element">The element</param>
    /// <returns>The object</returns>
    private static object? ConvertSimpleJsonValue(JsonElement element)
    {
        return element.ValueKind switch
        {
            JsonValueKind.String => element.GetString(),
            JsonValueKind.Number => element.TryGetInt64(out long l) ? l : element.GetDouble(),
            JsonValueKind.True => true,
            JsonValueKind.False => false,
            JsonValueKind.Null => null,
            _ => element.ToString(),
        };
    }

    public static JsonNode? GetValueByPath(this JsonNode node, string path)
    {
        if (node == null || string.IsNullOrEmpty(path))
        {
            return null;
        }

        var parts = MyRegex1().Split(path).Select(p => p.Trim()).ToArray();
        JsonNode current = node;

        foreach (string part in parts)
        {
            if (current == null)
            {
                return null;
            }

            if (Regex.IsMatch(part, @"^\w+$"))
            {
                if (current is JsonObject obj)
                {
                    current = obj[part];
                }
                else
                {
                    return null;
                }
            }
            else if (MyRegex().IsMatch(part))
            {
                var match = MyRegex2().Match(part);
                string key = match.Groups[1].Value;
                int index = int.Parse(match.Groups[2].Value);

                if (current is JsonObject obj && obj[key] is JsonArray arr && index < arr.Count)
                {
                    current = arr[index];
                }
                else
                {
                    return null;
                }
            }
        }

        return current;
    }

    public static JsonNode DictionaryToJsonNode(this Dictionary<string, object> dictionary)
    {
        if (dictionary == null)
        {
            return null;
        }

        JsonObject jsonObject = new();
        foreach (var kvp in dictionary)
        {
            jsonObject[kvp.Key] = ConvertToJsonNode(kvp.Value);
        }
        return jsonObject;
    }

    private static JsonNode ConvertToJsonNode(object value)
    {
        switch (value)
        {
            case null:
                return null;
            case string str:
                return JsonValue.Create(str);
            case bool b:
                return JsonValue.Create(b);
            case int i:
                return JsonValue.Create(i);
            case long l:
                return JsonValue.Create(l);
            case double d:
                return JsonValue.Create(d);
            case decimal dec:
                return JsonValue.Create(dec);
            case DateTime dt:
                return JsonValue.Create(dt);
            case Dictionary<string, object> dict:
                return DictionaryToJsonNode(dict);
            case IEnumerable<object> list:
                JsonArray jsonArray = new();
                foreach (var item in list)
                {
                    jsonArray.Add(ConvertToJsonNode(item));
                }
                return jsonArray;
            default:
                return JsonNode.Parse(JsonSerializer.Serialize(value));
        }
    }

    [GeneratedRegex(@"^\w+\[\d+\]$")]
    private static partial Regex MyRegex();

    [GeneratedRegex(@"\.(?![^\[]*\])")]
    private static partial Regex MyRegex1();

    [GeneratedRegex(@"(\w+)\[(\d+)\]")]
    private static partial Regex MyRegex2();
}
