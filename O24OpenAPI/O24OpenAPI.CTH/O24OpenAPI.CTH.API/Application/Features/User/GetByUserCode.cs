using LinKit.Core.Cqrs;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.CTH.API.Application.Features.User;

public class GetByUserCodeCommand : BaseTransactionModel, ICommand<UserAccount>
{
    public string UserCode { get; set; } = default!;
}

[CqrsHandler]
public class GetByUserCodeHandle(IUserAccountRepository userAccountRepository)
    : ICommandHandler<GetByUserCodeCommand, UserAccount>
{
    [WorkflowStep(WorkflowStep.CTH.WF_STEP_CTH_GET_USER)]
    public async Task<UserAccount> HandleAsync(
        GetByUserCodeCommand request,
        CancellationToken cancellationToken = default
    )
    {
        return await userAccountRepository.GetByUserCodeAsync(request.UserCode);
    }
}
