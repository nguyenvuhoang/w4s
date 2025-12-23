using LinKit.Core.Cqrs;
using O24OpenAPI.CTH.API.Application.Constants;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Exceptions;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.CTH.API.Application.Features.User;

public class DeactivateSmartOTPAsyncCommand : BaseTransactionModel, ICommand<bool>
{
    public string AuthenType { get; set; }
    public string PhoneNumber { get; set; }
    public string UserCode { get; set; }
    public string SmartOTPCode { get; set; }
    public bool IsValidOTP { get; set; }
}

[CqrsHandler]
public class DeactivateSmartOTPAsyncHandle(IUserAuthenRepository userAuthenRepository)
    : ICommandHandler<DeactivateSmartOTPAsyncCommand, bool>
{
    [WorkflowStep("WF_STEP_CTH_DEACTIVATE_OTP")]
    public async Task<bool> HandleAsync(
        DeactivateSmartOTPAsyncCommand request,
        CancellationToken cancellationToken = default
    )
    {
        var existingAuthen = await userAuthenRepository.GetByUserCodeAsync(request.UserCode);

        if (existingAuthen == null || existingAuthen.IsActive != true)
        {
            throw await O24Exception.CreateAsync(
                O24CTHResourceCode.Operation.SmartOTPNotFound,
                request.Language,
                [request.UserCode]
            );
        }
        existingAuthen.UpdatedOnUtc = DateTime.UtcNow;
        existingAuthen.IsActive = false;

        await userAuthenRepository.UpdateAsync(existingAuthen);

        return true;
    }
}
