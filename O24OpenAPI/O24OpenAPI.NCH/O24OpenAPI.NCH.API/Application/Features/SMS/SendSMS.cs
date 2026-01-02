using LinKit.Core.Cqrs;
using LinqToDB;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.Core.Abstractions;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.NCH.API.Application.Features.SMS.Services;
using O24OpenAPI.NCH.Domain.AggregatesModel.SmsAggregate;

namespace O24OpenAPI.NCH.API.Application.Features.SMS;

public class SendSMSComand : BaseTransactionModel, ICommand<SMSGatewayResponseModel>
{
    public string PhoneNumber { get; set; }
    public string Purpose { get; set; }
    public Dictionary<string, object> SenderData { get; set; }
    public string Message { get; set; }
    public string ProviderName { get; set; }
    public string TransactionId { get; set; }
    public string MessageType { get; set; } = "OTP";
}

public class SMSGatewayResponseModel : BaseO24OpenAPIModel
{
    public string PhoneNumber { get; set; }
    public string Message { get; set; }
    public string TransactionId { get; set; }
    public string Provider { get; set; }
    public bool IsSuccess { get; set; }
    public int ErrorCode { get; set; }
    public string ErrorMessage { get; set; }
    public string SMSGWTransactionId { get; set; }
}

[CqrsHandler]
public class SendSMSHandler(
    ISMSTemplateRepository sMSTemplateRepository,
    ISMSProviderService sMSProviderService
) : ICommandHandler<SendSMSComand, SMSGatewayResponseModel>
{
    [WorkflowStep(WorkflowStep.NCH.WF_STEP_NCH_SEND_SMS)]
    public async Task<SMSGatewayResponseModel> HandleAsync(
        SendSMSComand request,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            string message = await BuildSMSContentDynamicAsync(
                request.Purpose,
                request.SenderData,
                request.Message
            );

            // Generate transaction ID
            string transactionId = request.RefId;

            // Call provider service to send SMS
            Models.Response.SendSOAPResponseModel submitResult =
                await sMSProviderService.SendSMSAsync(
                    request.PhoneNumber,
                    message,
                    transactionId,
                    request.ProviderName,
                    request.TransactionId,
                    request.MessageType
                );

            // Map the result to response model
            SMSGatewayResponseModel sendResult = new()
            {
                PhoneNumber = request.PhoneNumber,
                Message = message,
                TransactionId = request.TransactionId,
                Provider = request.ProviderName,
                IsSuccess = submitResult.IsSuccess,
                SMSGWTransactionId = transactionId,
            };

            if (!submitResult.IsSuccess)
            {
                sendResult.ErrorMessage = submitResult.ErrorMessage;
                sendResult.ErrorCode = submitResult.ErrorCode;
            }

            return sendResult;
        }
        catch (Exception ex)
        {
            await ex.LogErrorAsync();

            return new SMSGatewayResponseModel
            {
                PhoneNumber = request.PhoneNumber,
                Message = request.Message,
                TransactionId = request.TransactionId,
                Provider = request.ProviderName,
                IsSuccess = false,
                ErrorMessage = ex.Message,
                ErrorCode = ex.HResult,
                SMSGWTransactionId = request.RefId,
            };
        }
    }

    public async Task<string> BuildSMSContentDynamicAsync(
        string templateCode,
        Dictionary<string, object> values,
        string message = ""
    )
    {
        SMSTemplate template = await sMSTemplateRepository.Table.FirstOrDefaultAsync(t =>
            t.TemplateCode == templateCode && t.IsActive
        );

        if (template == null)
        {
            return message;
        }

        string content = template.MessageContent;

        foreach (KeyValuePair<string, object> kvp in values)
        {
            content = content.Replace($"{{{kvp.Key}}}", kvp.Value?.ToString());
        }

        return content;
    }
}
