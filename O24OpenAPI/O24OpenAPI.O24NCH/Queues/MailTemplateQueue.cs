using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.O24NCH.Domain;
using O24OpenAPI.O24NCH.Models.Response;
using O24OpenAPI.O24NCH.Services.Interfaces;
using O24OpenAPI.O24OpenAPIClient.Scheme.Workflow;
using O24OpenAPI.Web.Framework.Helpers;
using O24OpenAPI.Web.Framework.Models;
using O24OpenAPI.Web.Framework.Models.O24OpenAPI;
using O24OpenAPI.Web.Framework.Services.Queue;

namespace O24OpenAPI.O24NCH.Queues;

public class MailTemplateQueue : BaseQueue
{
    private readonly IMailTemplateService _service = EngineContext.Current.Resolve<IMailTemplateService>();

    /// <summary>
    /// SimpleSearch
    /// </summary>
    /// <param name="workflow"></param>
    /// <returns></returns>
    public async Task<WFScheme> SimpleSearch(WFScheme workflow)
    {
        var model = await workflow.ToModel<SimpleSearchModel>();

        return await Invoke<SimpleSearchModel>(
            workflow,
            async () =>
            {
                var value = await _service.Search(model);
                return value.ToPagedListModel<MailTemplate, MailTemplateResponse>();
            }
        );
    }
}
