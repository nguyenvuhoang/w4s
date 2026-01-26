using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.Logger.API.Models.Log;

/// <summary>
/// The search model class
/// </summary>
/// <seealso cref="SimpleSearchModel"/>
public class SearchModel : SimpleSearchModel
{
    /// <summary>
    /// Gets or sets the value of the log type
    /// </summary>
    public string LogType { get; set; }
}
