using O24OpenAPI.Web.CMS.Models;
using O24OpenAPI.Web.CMS.Models.OpenAPI;
using O24OpenAPI.Web.CMS.Services.Interfaces;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Helpers;
using O24OpenAPI.Framework.Services.Queue;

namespace O24OpenAPI.Web.CMS.Queues;

public class OpenAPIQueue : BaseQueue
{
    private readonly ICoreAPIService _service = EngineContext.Current.Resolve<ICoreAPIService>();

    public async Task<WFScheme> Search(WFScheme workflow)
    {
        var model = await workflow.ToModel<SimpleSearchModel>();

        return await Invoke<SimpleSearchModel>(
            workflow,
            async () =>
            {
                var value = await _service.SimpleSearch(model);
                return value.ToPagedListModel<CoreApiKeys, CoreAPIKeyModel>();
            }
        );
    }

    public async Task<WFScheme> Create(WFScheme workflow)
    {
        var model = await workflow.ToModel<CreateOpenAPIRequestModel>();
        return await Invoke<CreateOpenAPIRequestModel>(
            workflow,
            async () =>
            {
                var rs = await _service.CreateAsync(model);
                return rs;
            }
        );
    }

    public async Task<WFScheme> Update(WFScheme workflow)
    {
        var model = await workflow.ToModel<OpenAPIQueueRequestModel>();
        return await Invoke<OpenAPIQueueRequestModel>(
            workflow,
            async () =>
            {
                var rs = await _service.UpdateAsync(model);
                return rs;
            }
        );
    }

    public async Task<WFScheme> RotateSecret(WFScheme workflow)
    {
        var model = await workflow.ToModel<RotateSecretRequestModel>();
        return await Invoke<RotateSecretRequestModel>(
            workflow,
            async () =>
            {
                var rs = await _service.RotateSecretAsync(model);
                return rs;
            }
        );
    }
}
