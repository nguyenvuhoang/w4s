using LinqToDB.Tools;
using O24OpenAPI.APIContracts.Models.CTH;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Core.Logging.Helpers;
using O24OpenAPI.GrpcContracts.GrpcClientServices.CTH;
using O24OpenAPI.Web.CMS.Models;
using O24OpenAPI.Web.CMS.Services.Interfaces;
using O24OpenAPI.Web.Framework.Extensions;

namespace O24OpenAPI.Web.CMS.Services.Services;

public class UserCommandService(
    IRepository<UserCommand> userCommandRepository,
    IRepository<UserRole> userRoleRepository,
    IRepository<UserRight> userRightRepository,
    ICTHGrpcClientService cthGrpcClientService,
    IStaticCacheManager staticCacheManager
) : IUserCommandService
{
    private readonly IRepository<UserCommand> _userCommandRepository = userCommandRepository;
    private readonly IRepository<UserRole> _userRoleRepository = userRoleRepository;
    private readonly IRepository<UserRight> _userRightRepository = userRightRepository;
    private readonly ICTHGrpcClientService _cthGrpcClientService = cthGrpcClientService;
    private readonly IStaticCacheManager _staticCacheManager = staticCacheManager;

    public async Task<UserCommand> GetById(int id)
    {
        return await _userCommandRepository.GetById(id);
    }

    public async Task<List<UserCommand>> GetByAppCode(string appCode)
    {
        return await _userCommandRepository
            .Table.Where(x => x.ApplicationCode == appCode)
            .ToListAsync();
    }

    public async Task<List<UserCommand>> GetByAppAndRoleCommand(
        string appCode,
        HashSet<string> roleCommand
    )
    {
        return await _userCommandRepository
            .Table.Where(x => x.ApplicationCode == appCode && roleCommand.Contains(x.CommandId))
            .ToListAsync();
    }

    public async Task<List<UserCommand>> ProcessRoleCommand(
        string appCode,
        HashSet<string> roleCommand
    )
    {
        var commands = await GetByAppAndRoleCommand(appCode, roleCommand);
        return commands;
    }

    public async Task<List<UserCommand>> GetMenuByAppAndRole(
        string appCode,
        HashSet<string> roleCommand
    )
    {
        return await _userCommandRepository
            .Table.Where(x =>
                x.ApplicationCode == appCode
                && x.CommandType.In("M", "T")
                && roleCommand.Contains(x.CommandId)
            )
            .ToListAsync();
    }

    public virtual async Task<List<UserCommand>> GetInfoFromFormCode(
        string applicationCode,
        string formCode
    )
    {
        var result = await _userCommandRepository
            .Table.Where(s =>
                s.ApplicationCode == applicationCode
                && (s.GroupMenuId == formCode)
                && s.Enabled == true
            )
            .ToListAsync();
        return result;
    }

    public virtual async Task<List<Models.UserCommandModel>> GetInfoFromCommandId(
        string applicationCode,
        string commandId
    )
    {
        var listLeftJoin = await (
            from userCommand in _userCommandRepository.Table
            join userRight in _userRightRepository.Table
                on userCommand.CommandId equals userRight.CommandId
            join userRole in _userRoleRepository.Table
                on userRight.RoleId equals userRole.RoleId
            where
                userCommand.ApplicationCode == applicationCode
                && userCommand.CommandId == commandId
                && userCommand.Enabled
            select new Models.UserCommandModel
            {
                ParentId = userCommand.ParentId,
                CommandId = userCommand.CommandId,
                CommandName = userCommand.CommandName,
                GroupMenuIcon = userCommand.GroupMenuIcon,
                GroupMenuVisible = userCommand.GroupMenuVisible,
                GroupMenuId = userCommand.GroupMenuId,
            }
        ).ToListAsync();

        return listLeftJoin;
    }

    /// <summary>
    /// Get info CommandId from ApplicationCode and ParentId
    /// </summary>
    /// <param name="applicationCode"></param>
    /// <param name="parentId"></param>
    /// <returns></returns>
    public virtual async Task<List<Models.UserCommandModel>> GetInfoFromParentId(
        string applicationCode,
        string parentId
    )
    {
        var listLeftJoin = await (
            from userCommand in _userCommandRepository.Table.Where(s =>
                s.ApplicationCode == applicationCode && s.ParentId == parentId && s.Enabled
            )
            from userRole in _userRoleRepository.Table.DefaultIfEmpty()
            from userRight in _userRightRepository
                .Table.Where(s =>
                    s.CommandId == userCommand.CommandId && s.RoleId == userRole.Id
                )
                .DefaultIfEmpty()

            select new Models.UserCommandModel()
            {
                ParentId = userCommand.ParentId,
                CommandId = userCommand.CommandId,
                CommandName = userCommand.CommandName,
                GroupMenuIcon = userCommand.GroupMenuIcon,
                GroupMenuVisible = userCommand.GroupMenuVisible,
                GroupMenuId = userCommand.GroupMenuId,
            }
        ).OrderBy(s => s.DisplayOrder).ThenBy(s => s.ParentId).ThenBy(s => s.CommandId).ToListAsync();

        return listLeftJoin;
    }

    public async Task<List<UserCommand>> GetByAppAndRole(
        string applicationCode,
        HashSet<string> roleCommand
    )
    {
        return await _userCommandRepository
            .Table.Where(s =>
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
                CommandNameLanguage = s.CommandNameLanguage.GetLangValue(null),
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

    /// <summary>
    /// Load user command by application code and role command
    /// </summary>
    /// <param name="applicationCode"></param>
    /// <param name="roleCommand"></param>
    /// <returns></returns>
    public async Task<List<UserCommandResponse>> LoadUserCommand()
    {
        try
        {
            var userCommand = await _cthGrpcClientService.LoadFullUserCommandAsync();

            var cacheKey = CachingKey.EntityKey<List<UserCommandResponse>>(
                $"LoadUserCommand"
            );

            var cachedUserCommand = await _staticCacheManager.Get<List<UserCommandResponse>>(cacheKey);
            if (cachedUserCommand != null)
                return cachedUserCommand;

            var userCommandList = userCommand.Select(x => new UserCommandResponse
            {
                ApplicationCode = x.ApplicationCode,
                CommandId = x.CommandId,
                ParentId = x.ParentId,
                CommandName = x.CommandName,
                CommandNameLanguage = x.CommandNameLanguage,
                CommandType = x.CommandType,
                CommandURI = x.CommandURI,

                // handle null boolean
                Enabled = x.Enabled ?? false,
                IsVisible = x.IsVisible ?? false,

                DisplayOrder = x.DisplayOrder,
                GroupMenuIcon = x.GroupMenuIcon,
                GroupMenuVisible = x.GroupMenuVisible,
                GroupMenuId = x.GroupMenuId,
                GroupMenuListAuthorizeForm = x.GroupMenuListAuthorizeForm,

            }).ToList();


            return userCommandList;
        }
        catch (Exception ex)
        {
            await ex.LogErrorAsync();
            BusinessLogHelper.Error(ex, "Error while loading UserCommand");
            return [];
        }
    }


    public virtual async Task<List<CTHCommandIdInfoModel>> GetUserCommandInfoFromParentId(
        string applicationCode,
        string parentId
    )
    {
        var listLeftJoin = await (
            from userCommand in _userCommandRepository.Table.Where(s =>
                s.ApplicationCode == applicationCode && s.ParentId == parentId && s.Enabled
            )
            from userRole in _userRoleRepository.Table.DefaultIfEmpty()
            from userRight in _userRightRepository
                .Table.Where(s =>
                    s.CommandId == userCommand.CommandId && s.RoleId == userRole.Id
                )
                .DefaultIfEmpty()

            select new CTHCommandIdInfoModel()
            {
                ParentId = userCommand.ParentId,
                CommandId = userCommand.CommandId,
                CommandName = userCommand.CommandName,
                ApplicationCode = userCommand.ApplicationCode,
                CommandType = userCommand.CommandType,
                RoleId = userRole.Id,
                RoleName = userRole.RoleName,
                CommandIdDetail =
                    (
                        userCommand.CommandId == userRight.CommandId
                        && userRole.Id == userRight.RoleId
                    )
                        ? userRight.CommandIdDetail
                        : null,
                Invoke =
                    (
                        userCommand.CommandId == userRight.CommandId
                        && userRole.Id == userRight.RoleId
                    )
                        ? userRight.Invoke
                        : 0,
                Approve =
                    (
                        userCommand.CommandId == userRight.CommandId
                        && userRole.Id == userRight.RoleId
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

    public virtual async Task<List<CTHCommandIdInfoModel>> GetUserCommandInfoFromCommandId(
        string applicationCode,
        string commandId
    )
    {
        var listLeftJoin = await (
            from userCommand in _userCommandRepository.Table.Where(s =>
                s.ApplicationCode == applicationCode && s.CommandId == commandId && s.Enabled
            )
            from userRole in _userRoleRepository.Table.DefaultIfEmpty()
            from userRight in _userRightRepository
                .Table.Where(s => s.CommandId == commandId && s.RoleId == userRole.Id)
                .DefaultIfEmpty()

            select new CTHCommandIdInfoModel()
            {
                ParentId = userCommand.ParentId,
                CommandId = userCommand.CommandId,
                CommandName = userCommand.CommandName,
                ApplicationCode = userCommand.ApplicationCode,
                CommandType = userCommand.CommandType,
                RoleId = userRole.Id,
                RoleName = userRole.RoleName,
                CommandIdDetail =
                    (
                        userCommand.CommandId == userRight.CommandId
                        && userRole.Id == userRight.RoleId
                    )
                        ? userRight.CommandIdDetail
                        : null,
                Invoke =
                    (
                        userCommand.CommandId == userRight.CommandId
                        && userRole.Id == userRight.RoleId
                    )
                        ? userRight.Invoke
                        : 0,
                Approve =
                    (
                        userCommand.CommandId == userRight.CommandId
                        && userRole.Id == userRight.RoleId
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

    public async Task<List<UserCommandResponse>> GetCommandMenuByChannel(string channelId)
    {
        return await _userCommandRepository
            .Table.Where(s =>
                s.ApplicationCode == channelId && s.CommandType == "M" && s.Enabled
            )
            .Select(s => new UserCommandResponse(s))
            .ToListAsync();
    }

    public async Task<List<UserCommand>> ListVisibleByChannel(string channelId)
    {
        return await _userCommandRepository
            .Table.Where(s => s.ApplicationCode == channelId && s.IsVisible)
            .ToListAsync();
    }

    public async Task<HashSet<string>> GetVisibleTransactionCommandsByChannelAsync(
        string channelId
    )
    {
        return
        [
            .. await _userCommandRepository
                .Table.Where(s =>
                    s.ApplicationCode == channelId && s.CommandType == "T" && s.IsVisible
                )
                .Select(s => s.CommandId)
                .ToListAsync(),
        ];
    }
}
