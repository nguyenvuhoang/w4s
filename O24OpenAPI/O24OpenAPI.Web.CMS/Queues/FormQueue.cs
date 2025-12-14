using O24OpenAPI.Web.CMS.Services.Interfaces;
using O24OpenAPI.Web.Framework.Extensions;
using O24OpenAPI.Web.Framework.Services.Queue;

namespace O24OpenAPI.Web.CMS.Queues;

public class FormQueue : BaseQueue
{
    private readonly ILoadFormService _formService =
        EngineContext.Current.Resolve<ILoadFormService>();

    public async Task<WFScheme> LoadForm(WFScheme wFScheme)
    {
        var model = await wFScheme.ToModel<FormModelRequest>();
        return await Invoke<FormModelRequest>(
            wFScheme,
            async () =>
            {
                var response = await _formService.LoadFormAndRoleTask(model);
                return response;
            }
        );
    }
}
