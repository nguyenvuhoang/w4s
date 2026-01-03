using LinKit.Core.Cqrs;
using LinqToDB;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.CTH.API.Application.Constants;
using O24OpenAPI.CTH.API.Application.Models.Roles;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Infrastructure.Mapper.Extensions;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.CTH.API.Application.Features.User;

public class CreateUserRoleCommand : BaseTransactionModel, ICommand<bool>
{
    public string RoleName { get; set; }
    public string RoleType { get; set; }
    public string ServiceID { get; set; }
    public string RoleDescription { get; set; }
}

[CqrsHandler]
public class CreateUserRoleHandle(
    IUserRoleRepository userRoleRepository,
    IUserCommandRepository userCommandRepository,
    IUserRightRepository userRightRepository
) : ICommandHandler<CreateUserRoleCommand, bool>
{
    [WorkflowStep(WorkflowStepCode.CTH.WF_STEP_CTH_CREATE_ROLE)]
    public async Task<bool> HandleAsync(
        CreateUserRoleCommand request,
        CancellationToken cancellationToken = default
    )
    {
        var model = request.ToModel<CreateUserRoleModel>();
        return await CreateUserRoleAsync(model);
    }

    public async Task<bool> CreateUserRoleAsync(CreateUserRoleModel model)
    {
        var roleId = await userRoleRepository.GetNextRoleIdAsync();

        var newUserRole = new UserRole
        {
            RoleId = roleId,
            RoleName = model.RoleName,
            RoleDescription = model.RoleDescription,
            UserType = Code.UserType.BO,
            UserCreated = model.CurrentUserCode,
            DateCreated = DateTime.UtcNow,
            ServiceID = Code.UserType.BO,
            RoleType = model.RoleType,
            Status = Code.ShowStatus.ACTIVE,
            IsShow = Code.ShowStatus.YES,
            SortOrder = roleId,
        };

        var inserted = await userRoleRepository.AddAsync(newUserRole);
        if (inserted == null)
        {
            return false;
        }

        var parentCommandIds = await userCommandRepository.GetListCommandParentAsync(
            model.ChannelId
        );
        if (parentCommandIds == null || parentCommandIds.Count == 0)
        {
            return true;
        }

        var existedCmdIds = await userRightRepository
            .Table.Where(r => r.RoleId == roleId && parentCommandIds.Contains(r.CommandId))
            .Select(r => r.CommandId)
            .Distinct()
            .ToListAsync();

        var now = DateTime.UtcNow;
        var toInsert = parentCommandIds
            .Except(existedCmdIds)
            .Distinct()
            .Select(pid => new UserRight
            {
                RoleId = roleId,
                CommandId = pid,
                CommandIdDetail = Common.ACTIVE,
                Invoke = 1,
                Approve = 1,
                CreatedOnUtc = now,
                UpdatedOnUtc = now,
            })
            .ToList();

        if (toInsert.Count > 0)
        {
            await userRightRepository.BulkInsert(toInsert);
        }

        return true;
    }
}
