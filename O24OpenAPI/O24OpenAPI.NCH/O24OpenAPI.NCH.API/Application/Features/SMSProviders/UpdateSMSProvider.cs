using LinKit.Core.Cqrs;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.NCH.API.Application.Features.SMS.Services;
using O24OpenAPI.NCH.API.Application.Models.Request;
using O24OpenAPI.NCH.API.Application.Models.Response;

namespace O24OpenAPI.NCH.API.Application.Features.SMSProviders;

public class UpdateSMSProviderCommand
    : SMSProviderUpdateModel,
        ICommand<UpdateSMSProviderResponseModel>
{ }

[CqrsHandler]
public class UpdateSMSProviderHandler(ISMSProviderService sMSProviderService)
    : ICommandHandler<UpdateSMSProviderCommand, UpdateSMSProviderResponseModel>
{
    [WorkflowStep(WorkflowStepCode.NCH.WF_STEP_NCH_UPDATE_SMSPROVIDER)]
    public async Task<UpdateSMSProviderResponseModel> HandleAsync(
        UpdateSMSProviderCommand request,
        CancellationToken cancellationToken = default
    )
    {
        return await sMSProviderService.Update(request);
    }
}
