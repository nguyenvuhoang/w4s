using LinKit.Core.Cqrs;
using LinqToDB;
using O24OpenAPI.CTH.API.Application.Models.UserCommandModels;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;

namespace O24OpenAPI.CTH.API.Application.Features.UserCommands;

public class GetUserCommandInfoFromParentIdQuery : IQuery<List<CommandIdInfoModel>>
{
    public string ApplicationCode { get; set; }
    public string ParentId { get; set; }
}

[CqrsHandler]
public class GetUserCommandInfoFromParentIdHandler(
    IUserCommandRepository userCommandRepository,
    IUserRoleRepository userRoleRepository,
    IUserRightRepository userRightRepository
) : IQueryHandler<GetUserCommandInfoFromParentIdQuery, List<CommandIdInfoModel>>
{
    public async Task<List<CommandIdInfoModel>> HandleAsync(
        GetUserCommandInfoFromParentIdQuery request,
        CancellationToken cancellationToken = default
    )
    {
        List<CommandIdInfoModel> listLeftJoin = await (
            from userCommand in userCommandRepository.Table.Where(s =>
                s.ApplicationCode == request.ApplicationCode
                && s.ParentId == request.ParentId
                && s.Enabled
            )
            from userRole in userRoleRepository.Table.DefaultIfEmpty()
            from userRight in userRightRepository
                .Table.Where(s =>
                    s.CommandId == userCommand.CommandId && s.RoleId == userRole.RoleId
                )
                .DefaultIfEmpty()

            select new CommandIdInfoModel()
            {
                ParentId = userCommand.ParentId,
                CommandId = userCommand.CommandId,
                CommandName = userCommand.CommandName,
                ApplicationCode = userCommand.ApplicationCode,
                CommandType = userCommand.CommandType,
                RoleId = userRole.RoleId,
                RoleName = userRole.RoleName,
                CommandIdDetail =
                    (
                        userCommand.CommandId == userRight.CommandId
                        && userRole.RoleId == userRight.RoleId
                    )
                        ? userRight.CommandIdDetail
                        : null,
                Invoke =
                    (
                        userCommand.CommandId == userRight.CommandId
                        && userRole.RoleId == userRight.RoleId
                    )
                        ? userRight.Invoke
                        : 0,
                Approve =
                    (
                        userCommand.CommandId == userRight.CommandId
                        && userRole.RoleId == userRight.RoleId
                    )
                        ? userRight.Approve
                        : 0,
                GroupMenuIcon = userCommand.GroupMenuIcon,
                GroupMenuVisible = userCommand.GroupMenuVisible,
                GroupMenuId = userCommand.GroupMenuId,
                GroupMenuListAuthorizeForm = userCommand.GroupMenuListAuthorizeForm,
            }
        ).OrderBy(s => s.RoleId).ThenBy(s => s.ParentId).ThenBy(s => s.CommandId).ToListAsync();

        return listLeftJoin;
    }
}
