using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.O24NCH.Models.SMSTemplate;
using O24OpenAPI.O24NCH.Services.Interfaces;
using O24OpenAPI.O24OpenAPIClient.Scheme.Workflow;
using O24OpenAPI.Web.Framework.Extensions;
using O24OpenAPI.Web.Framework.Models;
using O24OpenAPI.Web.Framework.Services.Queue;

namespace O24OpenAPI.O24NCH.Queues;

public class SMSTemplateQueue : BaseQueue
{
    private readonly ISMSTemplateService _SMSTemplateService =
        EngineContext.Current.Resolve<ISMSTemplateService>();

    public async Task<WFScheme> Insert(WFScheme wfScheme)
    {
        var model = await wfScheme.ToModel<SMSTemplateModel>();
        return await Invoke<SMSTemplateModel>(
            wfScheme,
            async () =>
            {
                var response = await _SMSTemplateService.Insert(model);
                return response;
            }
        );
    }

    public async Task<WFScheme> Update(WFScheme wfScheme)
    {
        var model = await wfScheme.ToModel<UpdateSMSTemplateRequestModel>();

        return await Invoke<UpdateSMSTemplateRequestModel>(
            wfScheme,
            async () =>
            {
                var response = await _SMSTemplateService.Update(model);
                return response;
            }
        );
    }
}
