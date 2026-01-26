using LinKit.Core.Cqrs;
using LinqToDB;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.CTH.API.Application.Features.User;

public class UpdateUserRoleCommand : BaseTransactionModel, ICommand<bool>
{
    public int RoleId { get; set; }
    public List<string> ListOfUser { get; set; } = [];
    public bool IsAssign { get; set; }
}

[CqrsHandler]
public class UpdateUserRoleHandler(IUserInRoleRepository userInRoleRepository)
    : ICommandHandler<UpdateUserRoleCommand, bool>
{
    [WorkflowStep(WorkflowStepCode.CTH.WF_STEP_CTH_UPDATE_USER_ROLE_ASSIGNMENT)]
    public async Task<bool> HandleAsync(
        UpdateUserRoleCommand request,
        CancellationToken cancellationToken = default
    )
    {
        if (request.IsAssign)
        {
            List<UserInRole> toInsert = [];

            foreach (string userCode in request.ListOfUser)
            {
                UserInRole exists = await userInRoleRepository
                    .Table.Where(u => u.UserCode == userCode && u.RoleId == request.RoleId)
                    .FirstOrDefaultAsync();

                if (exists == null)
                {
                    toInsert.Add(
                        new UserInRole
                        {
                            UserCode = userCode,
                            RoleId = request.RoleId,
                            IsMain = "Y",
                            CreatedOnUtc = DateTime.UtcNow,
                            UpdatedOnUtc = DateTime.UtcNow,
                        }
                    );
                }
            }

            if (toInsert.Count != 0)
            {
                await userInRoleRepository.BulkInsert(toInsert);
            }
        }
        else
        {
            // Bulk delete users from role
            List<UserInRole> toDelete = await userInRoleRepository
                .Table.Where(u =>
                    request.ListOfUser.Contains(u.UserCode) && u.RoleId == request.RoleId
                )
                .ToListAsync();

            if (toDelete.Count != 0)
            {
                await userInRoleRepository.BulkDelete(toDelete);
            }
        }

        return true;
    }
}
