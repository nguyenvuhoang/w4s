using System.Text.Json.Serialization;
using O24OpenAPI.CMS.API.Application.Features.Requests;

namespace O24OpenAPI.CMS.API.Application.Extensions;

[JsonSerializable(typeof(RequestModel))]
[JsonSerializable(typeof(string))]
[JsonSerializable(typeof(int))]
[JsonSerializable(typeof(long))]
[JsonSerializable(typeof(bool))]
[JsonSerializable(typeof(decimal))]
[JsonSerializable(typeof(double))]
[JsonSerializable(typeof(DateTime))]
[JsonSerializable(typeof(Guid))]
[JsonSerializable(typeof(object))]
[JsonSerializable(typeof(Dictionary<string, object>))]
[JsonSerializable(typeof(List<object>))]
[JsonSourceGenerationOptions(
    ReferenceHandler = JsonKnownReferenceHandler.IgnoreCycles,
    GenerationMode = JsonSourceGenerationMode.Metadata
)]
public partial class CMSJsonContext : JsonSerializerContext { }
