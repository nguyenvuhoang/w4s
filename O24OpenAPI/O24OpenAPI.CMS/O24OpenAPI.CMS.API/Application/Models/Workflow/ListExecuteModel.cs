using System.Collections.Concurrent;
using O24OpenAPI.Framework.Services.Events;

namespace O24OpenAPI.CMS.API.Application.Models.Workflow;

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
