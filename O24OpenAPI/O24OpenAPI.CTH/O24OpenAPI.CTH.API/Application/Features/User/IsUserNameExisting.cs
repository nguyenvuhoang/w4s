using LinKit.Core.Cqrs;
using LinqToDB;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.CTH.API.Application.Constants;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Exceptions;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.CTH.API.Application.Features.User;

public class IsUserNameExistingCommand : BaseTransactionModel, ICommand<bool>
{
    public string UserCode { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string DeviceId { get; set; }
}

[CqrsHandler]
public class IsUserNameExistingHandle(IUserAccountRepository userAccountRepository)
    : ICommandHandler<IsUserNameExistingCommand, bool>
{
    [WorkflowStep(WorkflowStepCode.CTH.WF_STEP_CTH_VALIDATE_USER)]
    public async Task<bool> HandleAsync(
        IsUserNameExistingCommand request,
        CancellationToken cancellationToken = default
    )
    {
        UserAccount existingUserName = await userAccountRepository.Table.FirstOrDefaultAsync(
            c => c.UserName == request.UserName.Trim() && c.UserCode == request.UserCode.Trim(),
            cancellationToken
        );

        if (existingUserName != null)
        {
            throw await O24Exception.CreateAsync(
                O24CTHResourceCode.Validation.UsernameIsNotExist,
                request.Language,
                [request.UserName]
            );
        }
        return true;
    }
}
