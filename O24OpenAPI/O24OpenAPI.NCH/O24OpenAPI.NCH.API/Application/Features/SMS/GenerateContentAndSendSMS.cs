using LinKit.Core.Cqrs;
using LinqToDB;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.Core;
using O24OpenAPI.Core.Abstractions;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Exceptions;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.NCH.API.Application.Features.SMS.Services;
using O24OpenAPI.NCH.Domain.AggregatesModel.SmsAggregate;

namespace O24OpenAPI.NCH.API.Application.Features.SMS;

public class GenerateContentAndSendSMSCommand
    : BaseTransactionModel,
        ICommand<GenerateSMSContentResponseModel>
{
    public string PhoneNumber { get; set; }
    public string Purpose { get; set; }
    public string ContractNumber { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class GenerateSMSContentResponseModel : BaseO24OpenAPIModel
{
    public string TransactionId { get; set; }
}

[CqrsHandler]
public class GenerateContentAndSendSMSCommandHandler(
    ISMSTemplateRepository sMSTemplateRepository,
    ISMSProviderService sMSProviderService
) : ICommandHandler<GenerateContentAndSendSMSCommand, GenerateSMSContentResponseModel>
{
    [WorkflowStep(WorkflowStep.NCH.WF_STEP_NCH_SEND_SMS_ASYNC)]
    public async Task<GenerateSMSContentResponseModel> HandleAsync(
        GenerateContentAndSendSMSCommand request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            string transactionId = request.RefId;

            string message = await BuildSMSContentAsync(
                request.Purpose,
                new Dictionary<string, string>
                {
                    { "CONTRACTNUMBER", request.ContractNumber ?? string.Empty },
                    { "PHONENUMBER", request.PhoneNumber },
                    { "PASSWORD", request.Password },
                }
            );

            var sendResult = await sMSProviderService.SendSMSAsync(
                request.PhoneNumber,
                message,
                transactionId,
                null,
                transactionId
            );

            return sendResult.IsSuccess
                ? new GenerateSMSContentResponseModel { TransactionId = transactionId }
                : null;
        }
        catch (O24Exception)
        {
            throw;
        }
        catch (O24OpenAPIException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _ = ex.LogErrorAsync(ex, "Failed to generate or send OTP");
            return null;
        }
    }

    public async Task<string> BuildSMSContentAsync(
        string templateCode,
        Dictionary<string, string> values
    )
    {
        var template = await sMSTemplateRepository.Table.FirstOrDefaultAsync(t =>
            t.TemplateCode == templateCode && t.IsActive
        );

        if (template == null)
        {
            return string.Empty;
        }

        string content = template.MessageContent;

        foreach (var item in values)
        {
            content = content.Replace($"{{{item.Key}}}", item.Value);
        }

        return content;
    }
}
