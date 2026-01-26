using LinKit.Core.Cqrs;
using LinqToDB;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.CTH.API.Application.Features.User;

public class TransitionUserStatusCommand : BaseTransactionModel, ICommand<bool>
{
    public string ContractNumber { get; set; }
    public new string Status { get; set; }
}

[CqrsHandler]
public class TransitionUserStatusHandle(IUserAccountRepository userAccountRepository)
    : ICommandHandler<TransitionUserStatusCommand, bool>
{
    [WorkflowStep(WorkflowStepCode.CTH.WF_STEP_CTH_TRANSITION_USER_STATUS)]
    public async Task<bool> HandleAsync(
        TransitionUserStatusCommand request,
        CancellationToken cancellationToken = default
    )
    {
        if (request == null || string.IsNullOrWhiteSpace(request.ContractNumber))
        {
            return false;
        }

        UserAccount entity = await userAccountRepository.Table.FirstOrDefaultAsync(
            x => x.ContractNumber == request.ContractNumber,
            token: cancellationToken
        );

        if (entity == null)
        {
            return false;
        }

        string previousStatus = entity.Status;

        try
        {
            entity.Status = request.Status;
            await userAccountRepository.Update(entity);
            return true;
        }
        catch (Exception ex)
        {
            if (request.IsReverse)
            {
                entity.Status = previousStatus;
                await userAccountRepository.Update(entity);
            }

            await ex.LogErrorAsync();

            return false;
        }
    }
}
