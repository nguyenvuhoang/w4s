using LinKit.Core.Cqrs;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.Core;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Localization;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.NCH.API.Application.Utils;
using O24OpenAPI.NCH.Domain.AggregatesModel.MailAggregate;

namespace O24OpenAPI.NCH.API.Application.Features.MailConfigs;

public class UpdateMailConfigCommand : BaseTransactionModel, ICommand<UpdateMailConfigResponse>
{
    public int Id { get; set; }

    /// <summary>
    /// ConfigId
    /// </summary>
    public string ConfigId { get; set; }

    /// <summary>
    /// Host
    /// </summary>
    public string Host { get; set; }

    /// <summary>
    /// Port
    /// </summary>
    public int Port { get; set; }

    /// <summary>
    /// Sender
    /// </summary>
    public string Sender { get; set; }

    /// <summary>
    /// Password
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// EnableTLS
    /// </summary>
    public bool EnableTLS { get; set; }

    /// <summary>
    /// EmailTest
    /// </summary>
    public string EmailTest { get; set; }
}

public partial class UpdateMailConfigResponse : UpdateMailConfigCommand { }

public class UpdateMailConfigHandler(
    IMailConfigRepository mailConfigRepository,
    ILocalizationService localizationService
) : ICommandHandler<UpdateMailConfigCommand, UpdateMailConfigResponse>
{
    [WorkflowStep(WorkflowStepCode.NCH.WF_STEP_NCH_UPDATE_MAIL_CONFIG)]
    public async Task<UpdateMailConfigResponse> HandleAsync(
        UpdateMailConfigCommand request,
        CancellationToken cancellationToken = default
    )
    {
        if (!request.Sender.IsValidEmail())
        {
            throw new O24OpenAPIException(
                await localizationService.GetResource("Email.InvalidEmail")
            );
        }

        if (
            !string.IsNullOrEmpty(request.EmailTest)
            && !request.EmailTest.IsValidEmail()
        )
        {
            throw new O24OpenAPIException(
                await localizationService.GetResource("Email.InvalidEmail")
            );
        }

        MailConfig getMailConfig =
            await mailConfigRepository.GetById(request.Id) ?? throw new O24OpenAPIException("MailConfig not found");
        getMailConfig.Sender = request.Sender;
        getMailConfig.Host = request.Host;
        getMailConfig.Port = request.Port;
        getMailConfig.EnableTLS = request.EnableTLS;
        getMailConfig.EmailTest = request.EmailTest;
        getMailConfig.Password = request.Password;

        await mailConfigRepository.Update(getMailConfig);
        return request.ToUpdateMailConfigResponse();
    }
}
