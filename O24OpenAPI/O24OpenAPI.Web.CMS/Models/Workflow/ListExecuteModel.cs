using System.Collections.Concurrent;
using O24OpenAPI.Web.Framework.Services.Events;

namespace O24OpenAPI.Web.CMS.Models;

/// <summary>
///
/// </summary>
public class ListExecuteModel
{
    /// <summary>
    ///
    /// </summary>
    public ListExecuteModel() { }

    /// <summary>
    /// </summary>
    public ConcurrentDictionary<string, WorkflowEvent> listExecuteModel { get; set; } =
        new ConcurrentDictionary<string, WorkflowEvent>();
}
