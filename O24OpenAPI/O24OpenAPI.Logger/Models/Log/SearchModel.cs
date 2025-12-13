using O24OpenAPI.Web.Framework.Models;

namespace O24OpenAPI.Logger.Models.Log;

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
