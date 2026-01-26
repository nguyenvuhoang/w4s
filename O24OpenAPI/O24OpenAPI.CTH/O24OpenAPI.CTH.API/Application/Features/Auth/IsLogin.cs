using LinKit.Core.Cqrs;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.CTH.API.Application.Constants;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Exceptions;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.CTH.API.Application.Features.Auth;

public class IsLoginCommand : BaseTransactionModel, ICommand<bool>
{
    public string UserCode { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string DeviceId { get; set; }
}

[CqrsHandler]
public class IsLoginHandle(IUserAccountRepository userAccountRepository)
    : ICommandHandler<IsLoginCommand, bool>
{
    [WorkflowStep(WorkflowStepCode.CTH.WF_STEP_CTH_GET_USER_STATUS_LOGIN)]
    public async Task<bool> HandleAsync(
        IsLoginCommand request,
        CancellationToken cancellationToken = default
    )
    {
        UserAccount userAccount =
            await userAccountRepository.GetByUserCodeAsync(request.UserCode)
            ?? throw await O24Exception.CreateAsync(
                O24CTHResourceCode.Validation.UsernameIsNotExist,
                request.Language,
                [request.UserCode]
            );

        return userAccount.IsLogin ?? false;
    }
}
