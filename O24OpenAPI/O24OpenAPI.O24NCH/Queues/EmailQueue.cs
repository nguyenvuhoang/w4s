using O24OpenAPI.Client.Scheme.Workflow;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Services.Queue;
using O24OpenAPI.O24NCH.Models.Request.Mail;
using O24OpenAPI.O24NCH.Services.Interfaces;

namespace O24OpenAPI.O24NCH.Queues;

public class EmailQueue : BaseQueue
{
    private readonly IEmailService _emailService = EngineContext.Current.Resolve<IEmailService>();

    public async Task<WFScheme> SendEmail(WFScheme wfScheme)
    {
        var model = await wfScheme.ToModel<SendMailRequestModel>();
        return await Invoke<SendMailRequestModel>(
            wfScheme,
            async () =>
            {
                var response = await _emailService.SendEmailAsync(model);
                return response;
            }
        );
    }

    public async Task<WFScheme> TestEmail(WFScheme wfScheme)
    {
        var model = await wfScheme.ToModel<TestMailRequestModel>();
        return await Invoke<TestMailRequestModel>(
            wfScheme,
            async () =>
            {
                var response = await _emailService.TestEmailAsync(model);
                return response;
            }
        );
    }
}
