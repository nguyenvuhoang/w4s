using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace O24OpenAPI.Framework.Models;

/// <summary>Represents base Neptune model</summary>
[JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
public abstract class BaseO24OpenAPIModel { }
