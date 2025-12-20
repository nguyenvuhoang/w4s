using MediatR;
using O24OpenAPI.Client.Scheme.Workflow;
using O24OpenAPI.Core.Abstractions;
using O24OpenAPI.Core.Helper;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.Framework.Services.Queue;
using O24OpenAPI.Logger.Models.Log;
using O24OpenAPI.Logger.Services.QueryHandler;
using O24OpenAPI.Logger.Utils;

namespace O24OpenAPI.Logger.Queues;

/// <summary>
/// The log queue class
/// </summary>
/// <seealso cref="BaseQueue"/>
public class LogQueue : BaseQueue
{
    /// <summary>
    /// The mediator
    /// </summary>
    private readonly IMediator _mediator = EngineContext.Current.Resolve<IMediator>();

    /// <summary>
    /// Processes the workflow scheme
    /// </summary>
    /// <param name="workflowScheme">The workflow scheme</param>
    /// <returns>A task containing the wf scheme</returns>
    public async Task<WFScheme> SimpleSearch(WFScheme workflowScheme)
    {
        var model = await workflowScheme.ToModel<SearchModel>(
            SerializerOptions.JsonSerializerOptions
        );
        return await Invoke<SearchModel>(
            workflowScheme,
            async () =>
            {
                var type = TypeLogUtils.GetTypeLog(model.LogType);
                var queryType = typeof(SimpleSearchQuery<>).MakeGenericType(type);
                var queryModel = Activator.CreateInstance(queryType, model);
                var response = await _mediator.Send((IRequest<PagedModel>)queryModel);
                return response;
            }
        );
    }

    /// <summary>
    /// ViewDetail
    /// </summary>
    /// <param name="workflowScheme"></param>
    /// <returns></returns>
    public async Task<WFScheme> ViewDetail(WFScheme workflowScheme)
    {
        var model = await workflowScheme.ToModel<ViewDetailModel>(
            SerializerOptions.JsonSerializerOptions
        );

        return await Invoke<ViewDetailModel>(
            workflowScheme,
            async () =>
            {
                var type = TypeLogUtils.GetTypeLog(model.LogType);
                var queryType = typeof(ViewDetailQuery<>).MakeGenericType(type);
                var queryModel =
                    Activator.CreateInstance(queryType, model)
                    ?? throw new InvalidOperationException(
                        $"Could not create instance of {queryType.Name}"
                    );
                dynamic dynamicQuery = queryModel;
                var response = await _mediator.Send(dynamicQuery);
                return response;
            }
        );
    }
}
