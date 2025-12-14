using O24OpenAPI.Core.Domain;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.O24OpenAPIClient.Scheme.Workflow;
using O24OpenAPI.Web.Framework.Extensions;
using O24OpenAPI.Web.Framework.Helpers;
using O24OpenAPI.Web.Framework.Models;

namespace O24OpenAPI.Web.Framework.Services.Queue;

public class StoreCommandQueue : BaseQueue
{
    private readonly IStoredCommandService _service =
        EngineContext.Current.Resolve<IStoredCommandService>();

    public async Task<WFScheme> SimpleSearch(WFScheme workflow)
    {
        var model = await workflow.ToModel<SimpleSearchModel>();

        return await Invoke<SimpleSearchModel>(
            workflow,
            async () =>
            {
                var value = await _service.SimpleSearch(model);
                return value.ToPagedListModel<StoredCommand, StoredCommandResponse>();
            }
        );
    }
}
