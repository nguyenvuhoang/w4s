using LinKit.Core.Cqrs;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.CTH.API.Application.Models.User;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.CTH.API.Application.Features.User;

public class RetrieveUserCommand : BaseTransactionModel, ICommand<RetrieveUserUserAccount>
{
    public string UserCode { get; set; } = default!;
}

[CqrsHandler]
public class RetrieveUserHandle(IUserAccountRepository userAccountRepository)
    : ICommandHandler<RetrieveUserCommand, RetrieveUserUserAccount>
{
    [WorkflowStep(WorkflowStepCode.CTH.WF_STEP_CTH_RETRIEVE_USER)]
    public async Task<RetrieveUserUserAccount> HandleAsync(
        RetrieveUserCommand request,
        CancellationToken cancellationToken = default
    )
    {
        if (request.UserCode == null)
        {
            throw new ArgumentNullException(nameof(request.UserCode));
        }

        if (request.CurrentUserCode != request.UserCode)
        {
            throw new UnauthorizedAccessException("Cannot retrieve other user's information.");
        }

        var userAccount = await userAccountRepository.GetByUserCodeAsync(request.UserCode);

        var userAccountModel = new RetrieveUserUserAccount
        {
            ContractNumber = userAccount?.ContractNumber,
            PhoneNumber = userAccount?.Phone
        };

        return userAccountModel;
    }
}
