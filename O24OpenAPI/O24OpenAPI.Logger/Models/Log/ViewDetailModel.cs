using O24OpenAPI.Web.Framework.Models;

namespace O24OpenAPI.Logger.Models.Log;

/// <summary>
/// ViewDetailModel
/// </summary>
/// <seealso cref="BaseTransactionModel"/>
public class ViewDetailModel : BaseTransactionModel
{
    /// <summary>
    /// Gets or sets the value of the log type
    /// </summary>
    public string LogType { get; set; }

    /// <summary>
    /// Gets or sets the value of the log id
    /// </summary>
    public string ExecutionId { get; set; }
}
