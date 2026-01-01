using LinKit.Core.Cqrs;
using LinqToDB;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.CTH.API.Application.Models;
using O24OpenAPI.CTH.API.Application.Models.Roles;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Infrastructure.Mapper.Extensions;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.CTH.API.Application.Features.User;

public class UpdateUserRoleCommand : BaseTransactionModel, ICommand<bool>
{
    public int RoleId { get; set; }
    public List<string> ListOfUser { get; set; } = [];
    public bool IsAssign { get; set; }
}

[CqrsHandler]
public class UpdateUserRoleHandle(IUserInRoleRepository userInRoleRepository)
    : ICommandHandler<UpdateUserRoleCommand, bool>
{
    [WorkflowStep(WorkflowStep.CTH.WF_STEP_CTH_CERATE_USER)]
    public async Task<bool> HandleAsync(
        UpdateUserRoleCommand request,
        CancellationToken cancellationToken = default
    )
    {
        var model = request.ToModel<UpdateUserRoleModel>();
        return await UpdateUserRoleASync(model);
    }

    public async Task<bool> UpdateUserRoleASync(UpdateUserRoleModel model)
    {
        if (model.IsAssign)
        {
            List<UserInRole> toInsert = new();

            foreach (var userCode in model.ListOfUser)
            {
                var exists = await userInRoleRepository
                    .Table.Where(u => u.UserCode == userCode && u.RoleId == model.RoleId)
                    .FirstOrDefaultAsync();

                if (exists == null)
                {
                    toInsert.Add(
                        new UserInRole
                        {
                            UserCode = userCode,
                            RoleId = model.RoleId,
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
            var toDelete = await userInRoleRepository
                .Table.Where(u => model.ListOfUser.Contains(u.UserCode) && u.RoleId == model.RoleId)
                .ToListAsync();

            if (toDelete.Count != 0)
            {
                await userInRoleRepository.BulkDelete(toDelete);
            }
        }

        return true;
    }
}
