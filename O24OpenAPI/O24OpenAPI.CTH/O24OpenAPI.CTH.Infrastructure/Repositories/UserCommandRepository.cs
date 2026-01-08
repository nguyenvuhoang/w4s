using Linh.JsonKit.Json;
using LinKit.Core.Abstractions;
using LinqToDB;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Data;

namespace O24OpenAPI.CTH.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class UserCommandRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
) : EntityRepository<UserCommand>(dataProvider, staticCacheManager), IUserCommandRepository
{
    public async Task<List<string>> GetListCommandParentAsync(string applicationCode)
    {
        return await Table
            .Where(s =>
                s.ApplicationCode == applicationCode
                && s.CommandType == "M"
                && s.Enabled
                && s.ParentId == "0"
            )
            .Select(s => s.CommandId)
            .Distinct()
            .ToListAsync();
    }

    public async Task<List<UserCommand>> LoadUserCommand(string applicationCode, string roleCommand)
    {
        var commandListHashSet = roleCommand.FromJson<HashSet<string>>();

        return await Table
            .Where(s =>
                s.ApplicationCode == applicationCode
                && roleCommand.Contains(s.CommandId)
                && s.IsVisible
            )
            .Select(s => new UserCommand
            {
                ApplicationCode = s.ApplicationCode,
                ParentId = s.ParentId,
                CommandId = s.CommandId,
                CommandName = s.CommandName,
                CommandNameLanguage = s.CommandNameLanguage,
                CommandType = s.CommandType,
                CommandURI = s.CommandURI,
                Enabled = s.Enabled,
                DisplayOrder = s.DisplayOrder,
                GroupMenuIcon = s.GroupMenuIcon,
                GroupMenuVisible = s.GroupMenuVisible,
                GroupMenuId = s.GroupMenuId,
            })
            .ToListAsync();
    }

    public virtual async Task<List<UserCommand>> GetInfoFromFormCode(
        string applicationCode,
        string formCode
    )
    {
        var result = await Table
            .Where(s =>
                s.ApplicationCode == applicationCode
                && (s.GroupMenuId == formCode)
                && s.Enabled == true
            )
            .ToListAsync();
        return result;
    }

    public async Task<UserCommand> AddAsync(UserCommand command)
    {
        return await InsertAsync(command); ;
    }

    public async Task<UserCommand> GetByCommandIdAsync(string commandId, string applicationCode)
    {
        if (string.IsNullOrEmpty(commandId) || string.IsNullOrEmpty(applicationCode) || string.IsNullOrEmpty(commandId))
        {
            throw new ArgumentNullException("commandId or applicationCode is null or empty");
        }

        return await Table
            .Where(s =>
                s.CommandId == commandId && s.ApplicationCode == applicationCode
            )
            .FirstOrDefaultAsync();
    }

    public async Task<UserCommand> ModifyAsync(UserCommand command)
    {
        await Update(command);
        return command;
    }
}
