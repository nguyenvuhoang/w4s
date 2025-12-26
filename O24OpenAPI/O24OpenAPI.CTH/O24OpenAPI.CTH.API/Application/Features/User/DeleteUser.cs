using LinKit.Core.Cqrs;
using LinqToDB;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.CTH.API.Application.Constants;
using O24OpenAPI.CTH.API.Application.Models;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Exceptions;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.CTH.API.Application.Features.User;

public class DeleteUserCommand : BaseTransactionModel, ICommand<bool>
{
    public DeleteUserModel Model { get; set; } = default!;
}

[CqrsHandler]
public class DeleteUserHandle(
    IUserAccountRepository userAccountRepository,
    IUserInRoleRepository userInRoleRepository,
    IUserPasswordRepository userPasswordRepository
) : ICommandHandler<DeleteUserCommand, bool>
{
    [WorkflowStep(WorkflowStep.CTH.WF_STEP_CTH_DELETE_USER)]
    public async Task<bool> HandleAsync(
        DeleteUserCommand request,
        CancellationToken cancellationToken = default
    )
    {
        return await DeleteUserById(request.Model);
    }

    public async Task<bool> DeleteUserById(DeleteUserModel model)
    {
        var entity = await userAccountRepository.Table.FirstOrDefaultAsync(x =>
            x.Id == model.Id
        );

        if (entity != null)
        {
            if (entity.LoginName == model.CurrentLoginname)
            {
                throw await O24Exception.CreateAsync(
                    O24CTHResourceCode.Operation.DeleteOwnUserError,
                    model.Language
                );
            }
            await userInRoleRepository.DeleteByUserCodeAsync(entity.UserId);
            await userPasswordRepository.DeletePasswordByUserIdAsync(entity.UserId);
            await userAccountRepository.DeleteUserByUserIdAsync(entity.UserId);
            return true;
        }
        return false;
    }
}
