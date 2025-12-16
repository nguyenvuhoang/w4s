using O24OpenAPI.Client.Scheme.Workflow;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.Framework.Services.Queue;
using O24OpenAPI.O24NCH.Models.Request;
using O24OpenAPI.O24NCH.Models.Request.SMS;
using O24OpenAPI.O24NCH.Services.Interfaces;

namespace O24OpenAPI.O24NCH.Queues;

/// <summary>
/// The auth queue class
/// </summary>
/// <seealso cref="BaseQueue"/>
public class SMSQueue : BaseQueue
{
    /// <summary>
    /// The SMS service
    /// </summary>
    private readonly ISMSService _smsService = EngineContext.Current.Resolve<ISMSService>();

    /// <summary>
    /// Logins the to o 24 open api using the specified wf scheme
    /// </summary>
    /// <param name="wfScheme">The wf scheme</param>
    /// <returns>A task containing the wf scheme</returns>
    public async Task<WFScheme> GenerateOTPandSendSMS(WFScheme wfScheme)
    {
        var model = await wfScheme.ToModel<GenerateOTPRequestModel>();
        return await Invoke<GenerateOTPRequestModel>(
            wfScheme,
            async () =>
            {
                var response = await _smsService.GenerateAndSendOTPAsync(model);
                return response;
            }
        );
    }

    public async Task<WFScheme> VeriveryOTP(WFScheme wfScheme)
    {
        var model = await wfScheme.ToModel<VeriveryOTPRequestModel>();
        return await Invoke<VeriveryOTPRequestModel>(
            wfScheme,
            async () =>
            {
                var response = await _smsService.VerifyOTPAsync(model);
                return response;
            }
        );
    }

    public async Task<WFScheme> RetrieveInfo(WFScheme wfScheme)
    {
        var model = await wfScheme.ToModel<RetrieveSMSInfoRequestModel>();
        return await Invoke<RetrieveSMSInfoRequestModel>(
            wfScheme,
            async () =>
            {
                var response = await _smsService.RetrieveInfoAsync(model);
                return response;
            }
        );
    }

    /// <summary>
    /// Logins the to o 24 open api using the specified wf scheme
    /// </summary>
    /// <param name="wfScheme">The wf scheme</param>
    /// <returns>A task containing the wf scheme</returns>
    public async Task<WFScheme> GenerateContentandSendSMS(WFScheme wfScheme)
    {
        var model = await wfScheme.ToModel<GenerateSMSContentRequestModel>();
        return await Invoke<GenerateSMSContentRequestModel>(
            wfScheme,
            async () =>
            {
                var response = await _smsService.GenerateAndSendContentAsync(model);
                return response;
            }
        );
    }

    /// <summary>
    /// Send SMS
    /// </summary>
    /// <param name="wfScheme"></param>
    /// <returns></returns>
    public async Task<WFScheme> SendSMS(WFScheme wfScheme)
    {
        var model = await wfScheme.ToModel<SMSGatewayRequestModel>();
        return await Invoke<SMSGatewayRequestModel>(
            wfScheme,
            async () =>
            {
                var response = await _smsService.SMSGatewaySend(model);
                return response;
            }
        );
    }
}
