using LinKit.Core.Cqrs;
using LinqToDB;
using O24OpenAPI.APIContracts.Models.CTH;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Logging.Helpers;

namespace O24OpenAPI.CTH.API.Application.Features.UserCommands;

public class LoadFullUserCommandsQuery : IQuery<List<CTHUserCommandModel>> { }

[CqrsHandler]
public class LoadFullUserCommandsQueryHandler(IUserCommandRepository _userCommandRepository)
    : IQueryHandler<LoadFullUserCommandsQuery, List<CTHUserCommandModel>>
{
    public async Task<List<CTHUserCommandModel>> HandleAsync(
        LoadFullUserCommandsQuery query,
        CancellationToken cancellationToken
    )
    {
        try
        {
            List<UserCommand> listUserCommandDomain = await _userCommandRepository
                .Table.OrderBy(x => x.ApplicationCode)
                .ThenBy(x => x.CommandId)
                .ThenBy(x => x.DisplayOrder)
                .ToListAsync();

            List<CTHUserCommandModel> listUserCommand = listUserCommandDomain
                .Select(x => new CTHUserCommandModel
                {
                    ApplicationCode = x.ApplicationCode,
                    CommandId = x.CommandId,
                    ParentId = x.ParentId,
                    CommandName = x.CommandName,
                    CommandNameLanguage = x.CommandNameLanguage,
                    CommandType = x.CommandType,
                    CommandURI = x.CommandURI ?? "",
                    Enabled = x.Enabled,
                    IsVisible = x.IsVisible,
                    DisplayOrder = x.DisplayOrder,
                    GroupMenuIcon = x.GroupMenuIcon,
                    GroupMenuVisible = x.GroupMenuVisible,
                    GroupMenuId = x.GroupMenuId ?? "",
                    GroupMenuListAuthorizeForm = x.GroupMenuListAuthorizeForm ?? "",
                })
                .ToList();

            return listUserCommand;
        }
        catch (Exception ex)
        {
            await ex.LogErrorAsync();
            BusinessLogHelper.Error(ex, "Error while loading full user commands");
            return [];
        }
    }
}
