using O24OpenAPI.CMS.API.Application.Services.Interfaces;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Services.Queue;

namespace O24OpenAPI.CMS.API.Queues;

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
