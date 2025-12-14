using LinqToDB;
using O24OpenAPI.APIContracts.Models.CTH;
using O24OpenAPI.ControlHub.Domain;
using O24OpenAPI.ControlHub.Models;
using O24OpenAPI.ControlHub.Models.Roles;
using O24OpenAPI.ControlHub.Services.Interfaces;
using O24OpenAPI.ControlHub.Utils;
using O24OpenAPI.Core.Logging.Helpers;
using O24OpenAPI.Web.Framework.Extensions;

namespace O24OpenAPI.ControlHub.Services;

public class UserCommandService(
    IRepository<UserCommand> userCommandRepository,
    IRepository<UserRole> userRoleRepository,
    IRepository<UserRight> userRightRepository,
    IRepository<UserAgreement> userAgreementRepository
) : IUserCommandService
{
    private readonly IRepository<UserCommand> _userCommandRepository = userCommandRepository;
    private readonly IRepository<UserRole> _userRoleRepository = userRoleRepository;
    private readonly IRepository<UserRight> _userRightRepository = userRightRepository;
    private readonly IRepository<UserAgreement> _userAgreementRepository = userAgreementRepository;

    /// <summary>
    /// Get Menu Info From Role Id
    /// </summary>
    /// <param name="roleId"></param>
    /// <param name="applicationCode"></param>
    /// <param name="lang"></param>
    /// <returns></returns>
    public async Task<List<CommandHierarchyModel>> GetMenuInfoFromRoleId(
        int roleId,
        string applicationCode,
        string lang
    )
    {
        var userRights = await _userRightRepository.Table
            .Where(ur => ur.RoleId == roleId && ur.Invoke == 1)
            .ToListAsync();

        var commandIds = userRights.Select(ur => ur.CommandId).Distinct().ToList();

        var userCommands = await _userCommandRepository.Table
            .Where(uc => commandIds.Contains(uc.CommandId) && uc.ApplicationCode == applicationCode && uc.Enabled)
            .OrderBy(uc => uc.DisplayOrder)
            .ToListAsync();

        var userRole = await _userRoleRepository.Table
            .Where(r => r.RoleId == roleId)
            .FirstOrDefaultAsync();

        var result = (from uc in userCommands
                      join ur in userRights on uc.CommandId equals ur.CommandId
                      select new CommandHierarchyModel
                      {
                          ParentId = uc.ParentId,
                          CommandId = uc.CommandId,
                          Label = Utils.StringExtensions.TryGetLabelFromJson(uc.CommandNameLanguage, lang),
                          CommandType = uc.CommandType,
                          CommandUri = uc.CommandURI,
                          RoleId = roleId,
                          RoleName = userRole?.RoleName,
                          Invoke = ur.Invoke == 1,
                          Approve = ur.Approve == 1,
                          Icon = uc.GroupMenuIcon,
                          GroupMenuVisible = uc.GroupMenuVisible,
                          GroupMenuId = uc.GroupMenuId,
                          GroupMenuListAuthorizeForm = uc.GroupMenuListAuthorizeForm
                      }).ToList();

        foreach (var item in result)
        {
            item.IsAgreement = await IsUserAgreement(item.CommandId);
        }

        return result;
    }

    /// <summary>
    /// Get User Command Info From Parent Id
    /// </summary>
    /// <param name="applicationCode"></param>
    /// <param name="parentId"></param>
    /// <returns></returns>
    public virtual async Task<List<CommandIdInfoModel>> GetUserCommandInfoFromParentId(
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

    /// <summary>
    /// Get User Command Info From Command Id
    /// </summary>
    /// <param name="applicationCode"></param>
    /// <param name="commandId"></param>
    /// <returns></returns>
    public virtual async Task<List<CommandIdInfoModel>> GetUserCommandInfoFromCommandId(
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
                .Table.Where(s => s.CommandId == commandId && s.RoleId == userRole.RoleId)
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

    /// <summary>
    /// Get Visible Transactions
    /// </summary>
    /// <param name="channelId"></param>
    /// <returns></returns>
    public async Task<List<VisibleTransactionResponse>> GetVisibleTransactions(string channelId)
    {
        var q =
            from userCommand in _userCommandRepository.Table.Where(s =>
                s.ApplicationCode == channelId
                && s.Enabled
                && s.IsVisible
                && s.CommandType == "T"
                && s.ParentId != "0"
            )
            select new VisibleTransactionResponse
            {
                TransactionCode = userCommand.CommandId,
                TransactionName = userCommand.CommandName,
                TransactionNameLanguage = userCommand.CommandNameLanguage,
                ModuleCode = userCommand.ParentId,
            };
        return await q.ToListAsync();
    }

    /// <summary>
    /// Load User Command
    /// </summary>
    /// <param name="applicationCode"></param>
    /// <param name="roleCommand"></param>
    /// <returns></returns>
    public async Task<List<UserCommand>> LoadUserCommand(
        string applicationCode,
        string roleCommand
    )
    {
        var commandListHashSet = roleCommand.JsonConvertToModel<HashSet<string>>();

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

    public async Task<List<UserCommandResponseModel>> GetCommandMenuByChannel(string channelId)
    {
        return await _userCommandRepository
            .Table.Where(s =>
                s.ApplicationCode == channelId && s.CommandType == "M" && s.Enabled
            )
            .Select(s => new UserCommandResponseModel(s))
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

    public virtual async Task<List<UserCommandResponseModel>> GetInfoFromCommandId(
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
            select new UserCommandResponseModel
            {
                ParentId = userCommand.ParentId,
                CommandId = userCommand.CommandId,
                CommandNameLanguage = userCommand.CommandName,
                Icon = userCommand.GroupMenuIcon,
                GroupMenuVisible = userCommand.GroupMenuVisible,
                GroupMenuId = userCommand.GroupMenuId,
            }
        ).ToListAsync();

        return listLeftJoin;
    }

    public virtual async Task<List<UserCommandResponseModel>> GetInfoFromParentId(string applicationCode, string parentId)
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

            select new UserCommandResponseModel()
            {
                ParentId = userCommand.ParentId,
                CommandId = userCommand.CommandId,
                CommandNameLanguage = userCommand.CommandName,
                Icon = userCommand.GroupMenuIcon,
                GroupMenuVisible = userCommand.GroupMenuVisible,
                GroupMenuId = userCommand.GroupMenuId,
            }
        ).OrderBy(s => s.DisplayOrder).ThenBy(s => s.ParentId).ThenBy(s => s.CommandId).ToListAsync();

        return listLeftJoin;
    }
    /// <summary>
    /// Get List Command Parent Async
    /// </summary>
    /// <param name="applicationCode"></param>
    /// <returns></returns>
    public async Task<List<string>> GetListCommandParentAsync(string applicationCode)
    {
        return await _userCommandRepository.Table
            .Where(s => s.ApplicationCode == applicationCode
                        && s.CommandType == "M"
                        && s.Enabled
                        && s.ParentId == "0")
            .Select(s => s.CommandId)
            .Distinct()
            .ToListAsync();
    }
    /// <summary>
    /// Is User Agreement
    /// </summary>
    /// <param name="commandid"></param>
    /// <returns></returns>
    public async Task<bool> IsUserAgreement(string commandid)
    {

        var isAgreement = await _userAgreementRepository.Table
            .Where(ua => ua.TransactionCode.Contains(commandid) && ua.IsActive)
            .FirstOrDefaultAsync();
        if (isAgreement != null)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Load Full User Commands Async
    /// </summary>
    /// <returns></returns>
    public async Task<List<CTHUserCommandModel>> LoadFullUserCommandsAsync()
    {
        try
        {
            var listUserCommandDomain = await _userCommandRepository.Table
                .OrderBy(x => x.ApplicationCode)
                .ThenBy(x => x.CommandId)
                .ThenBy(x => x.DisplayOrder)
                .ToListAsync();

            var listUserCommand = listUserCommandDomain.Select(x => new CTHUserCommandModel
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
            }).ToList();

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
