using MediatR;
using O24OpenAPI.Logger.Models.Log;

namespace O24OpenAPI.Logger.Services.QueryHandler;

/// <summary>
/// The view detail query class
/// </summary>
/// <typeparam name="T">The entity type</typeparam>
public class ViewDetailQuery<T> : IRequest<T>
    where T : class
{
    /// <summary>
    /// Gets or sets the view detail model
    /// </summary>
    public string ExecutionId { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ViewDetailQuery{T}"/> class
    /// </summary>
    /// <param name="model">The view detail model</param>
    public ViewDetailQuery(ViewDetailModel model)
    {
        ExecutionId = model.ExecutionId;
    }
}
