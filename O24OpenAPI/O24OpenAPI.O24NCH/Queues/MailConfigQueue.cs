using O24OpenAPI.Client.Scheme.Workflow;
using O24OpenAPI.Core.Abstractions;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Helpers;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.Framework.Services.Queue;
using O24OpenAPI.O24NCH.Domain;
using O24OpenAPI.O24NCH.Models.Request.Mail;
using O24OpenAPI.O24NCH.Models.Response;
using O24OpenAPI.O24NCH.Services.Interfaces;

namespace O24OpenAPI.O24NCH.Queues;

public class MailConfigQueue : BaseQueue
{
    private readonly IMailConfigService _service =
        EngineContext.Current.Resolve<IMailConfigService>();

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
                return value.ToPagedListModel<MailConfig, MailConfigResponse>();
            }
        );
    }

    /// <summary>
    /// Update
    /// </summary>
    /// <param name="workflow"></param>
    /// <returns></returns>
    public async Task<WFScheme> Update(WFScheme workflow)
    {
        var model = await workflow.ToModel<MailConfigUpdateModel>();
        return await Invoke<MailConfigUpdateModel>(
            workflow,
            async () =>
            {
                var rs = await _service.Update(model, model.ReferenceId);
                return rs;
            }
        );
    }
}
