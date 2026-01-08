using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using O24OpenAPI.WFO.API.Application.Models;

namespace O24OpenAPI.WFO.API.Application.Extensions;

[JsonSerializable(typeof(WorkflowInput))]
// ===== Primitive =====
[JsonSerializable(typeof(string))]
[JsonSerializable(typeof(int))]
[JsonSerializable(typeof(long))]
[JsonSerializable(typeof(short))]
[JsonSerializable(typeof(byte))]
[JsonSerializable(typeof(bool))]
[JsonSerializable(typeof(decimal))]
[JsonSerializable(typeof(double))]
[JsonSerializable(typeof(float))]
[JsonSerializable(typeof(DateTime))]
[JsonSerializable(typeof(DateTimeOffset))]
[JsonSerializable(typeof(Guid))]
[JsonSerializable(typeof(TimeSpan))]
// ===== Nullable primitive =====
[JsonSerializable(typeof(int?))]
[JsonSerializable(typeof(long?))]
[JsonSerializable(typeof(bool?))]
[JsonSerializable(typeof(decimal?))]
[JsonSerializable(typeof(double?))]
[JsonSerializable(typeof(DateTime?))]
[JsonSerializable(typeof(Guid?))]
// ===== Object / Dynamic =====
[JsonSerializable(typeof(object))]
[JsonSerializable(typeof(JsonElement))]
[JsonSerializable(typeof(JsonNode))]
[JsonSerializable(typeof(JsonObject))]
[JsonSerializable(typeof(JsonArray))]
[JsonSerializable(typeof(JsonValue))]
// ===== Collections =====
[JsonSerializable(typeof(Dictionary<string, object>))]
[JsonSerializable(typeof(Dictionary<string, JsonElement>))]
[JsonSerializable(typeof(Dictionary<string, JsonNode>))]
[JsonSerializable(typeof(List<object>))]
[JsonSerializable(typeof(List<JsonElement>))]
[JsonSerializable(typeof(List<JsonNode>))]
[JsonSerializable(typeof(object[]))]
[JsonSerializable(typeof(JsonElement[]))]
[JsonSerializable(typeof(JsonNode[]))]
public partial class WFOJsonContext : JsonSerializerContext { }
