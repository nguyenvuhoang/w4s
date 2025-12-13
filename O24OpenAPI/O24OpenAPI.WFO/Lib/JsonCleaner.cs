using System.Text;
using System.Text.Json;

namespace O24OpenAPI.WFO.Lib;

public class JsonCleaner
{
    public static string RemoveValueKind(string json)
    {
        using JsonDocument doc = JsonDocument.Parse(json);
        using var outputStream = new MemoryStream();
        using var writer = new Utf8JsonWriter(outputStream);

        RemoveValueKindRecursive(doc.RootElement, writer);

        writer.Flush();
        return Encoding.UTF8.GetString(outputStream.ToArray());
    }

    private static void RemoveValueKindRecursive(JsonElement element, Utf8JsonWriter writer)
    {
        switch (element.ValueKind)
        {
            case JsonValueKind.Object:
                writer.WriteStartObject();
                foreach (var prop in element.EnumerateObject())
                {
                    if (prop.NameEquals("value_kind"))
                    {
                        continue;
                    }

                    writer.WritePropertyName(prop.Name);
                    RemoveValueKindRecursive(prop.Value, writer);
                }
                writer.WriteEndObject();
                break;

            case JsonValueKind.Array:
                writer.WriteStartArray();
                foreach (var item in element.EnumerateArray())
                {
                    RemoveValueKindRecursive(item, writer);
                }
                writer.WriteEndArray();
                break;

            default:
                WriteValue(element, writer);
                break;
        }
    }

    private static void WriteValue(JsonElement element, Utf8JsonWriter writer)
    {
        switch (element.ValueKind)
        {
            case JsonValueKind.String:
                writer.WriteStringValue(element.GetString());
                break;
            case JsonValueKind.Number:
                if (element.TryGetInt64(out long longValue))
                {
                    writer.WriteNumberValue(longValue);
                }
                else
                {
                    writer.WriteNumberValue(element.GetDouble());
                }

                break;
            case JsonValueKind.True:
                writer.WriteBooleanValue(true);
                break;
            case JsonValueKind.False:
                writer.WriteBooleanValue(false);
                break;
            case JsonValueKind.Null:
                writer.WriteNullValue();
                break;
        }
    }
}
