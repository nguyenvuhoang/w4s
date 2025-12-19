using O24OpenAPI.Client.Scheme.Workflow;
using O24OpenAPI.Core.Abstractions;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Helpers;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.Framework.Services.Queue;
using O24OpenAPI.O24NCH.Domain;
using O24OpenAPI.O24NCH.Models.Request;
using O24OpenAPI.O24NCH.Models.Response;
using O24OpenAPI.O24NCH.Services.Interfaces;

namespace O24OpenAPI.O24NCH.Queues;

public class SMSProviderQueue : BaseQueue
{
    private readonly ISMSProviderService _smsProviderService =
        EngineContext.Current.Resolve<ISMSProviderService>();

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
                var value = await _smsProviderService.Search(model);
                return value.ToPagedListModel<SMSProvider, SMSProviderResponse>();
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
        var model = await workflow.ToModel<SMSProviderUpdateModel>();
        return await Invoke<SMSProviderUpdateModel>(
            workflow,
            async () =>
            {
                var rs = await _smsProviderService.Update(model);
                return rs;
            }
        );
    }

    /// <summary>
    /// Update
    /// </summary>
    /// <param name="workflow"></param>
    /// <returns></returns>
    public async Task<WFScheme> Create(WFScheme workflow)
    {
        var model = await workflow.ToModel<SMSProviderCreateModel>();
        return await Invoke<SMSProviderCreateModel>(
            workflow,
            async () =>
            {
                var rs = await _smsProviderService.CreateAsync(model);
                return rs;
            }
        );
    }
}
