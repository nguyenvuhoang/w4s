using LinKit.Core.Cqrs;
using LinqToDB;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.CTH.API.Application.Constants;
using O24OpenAPI.CTH.API.Application.Models;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Infrastructure.Mapper.Extensions;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.CTH.API.Application.Features.User;

public class UnblockUserCommand : BaseTransactionModel, ICommand<bool>
{
    public string UserName { get; set; }
}

[CqrsHandler]
public class UnblockUserHandle(IUserAccountRepository userAccountRepository)
    : ICommandHandler<UnblockUserCommand, bool>
{
    [WorkflowStep(WorkflowStepCode.CTH.WF_STEP_CTH_UNBLOCK_USER)]
    public async Task<bool> HandleAsync(
        UnblockUserCommand request,
        CancellationToken cancellationToken = default
    )
    {
        var model = request.ToModel<UnblockUserModel>();
        return await UnBlockUserAsync(model);
    }

    public async Task<bool> UnBlockUserAsync(UnblockUserModel model)
    {
        var entity = await userAccountRepository.Table.FirstOrDefaultAsync(x =>
            x.UserName == model.UserName
        );

        if (entity != null)
        {
            entity.Status = Common.ACTIVE;
            entity.Failnumber = 0;
            entity.LockedUntil = null;
            entity.UpdatedOnUtc = DateTime.UtcNow;
            await userAccountRepository.Update(entity);
            return true;
        }
        return false;
    }
}
