using LinKit.Core.Cqrs;
using LinqToDB;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.CTH.API.Application.Models;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Infrastructure.Mapper.Extensions;
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
    [WorkflowStep(WorkflowStep.CTH.WF_STEP_CTH_TRANSITION_USER_STATUS)]
    public async Task<bool> HandleAsync(
        TransitionUserStatusCommand request,
        CancellationToken cancellationToken = default
    )
    {
        var model = request.ToModel<TransitionUserStatusModel>();
        return await TransitionUserStatusAsync(model);
    }

    public async Task<bool> TransitionUserStatusAsync(TransitionUserStatusModel model)
    {
        if (model == null || string.IsNullOrWhiteSpace(model.ContractNumber))
        {
            return false;
        }

        var entity = await userAccountRepository.Table.FirstOrDefaultAsync(x =>
            x.ContractNumber == model.ContractNumber
        );

        if (entity == null)
        {
            return false;
        }

        var previousStatus = entity.Status;

        try
        {
            entity.Status = model.Status;
            await userAccountRepository.Update(entity);
            return true;
        }
        catch (Exception ex)
        {
            if (model.IsReverse)
            {
                entity.Status = previousStatus;
                await userAccountRepository.Update(entity);
            }

            await ex.LogErrorAsync();

            return false;
        }
    }
}
