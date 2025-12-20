using Newtonsoft.Json.Linq;

namespace O24OpenAPI.Framework.Services.Mapping;

/// <summary>
/// The data mapping service interface
/// </summary>
public partial interface IDataMappingService
{
    /// <summary>
    /// Maps the data using the specified source
    /// </summary>
    /// <param name="source">The source</param>
    /// <param name="target">The target</param>
    /// <param name="func">The func</param>
    /// <returns>A task containing the object</returns>
    Task<JObject> MapDataAsync(
        JObject source,
        JObject target,
        Func<string, Task<object>> func = null
    );

    /// <summary>
    /// Maps the data to dictionary using the specified source
    /// </summary>
    /// <param name="source">The source</param>
    /// <param name="target">The target</param>
    /// <param name="func">The func</param>
    /// <returns>A task containing a dictionary of string and object</returns>
    Task<Dictionary<string, object>> MapDataToDictionaryAsync(
        JObject source,
        JObject target,
        Func<string, Task<object>> func = null
    );
}
