using LinKit.Core.Cqrs;
using LinqToDB;
using Newtonsoft.Json;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.CTH.API.Application.Models.Roles;
using O24OpenAPI.CTH.API.Application.Models.UserCommandModels;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.CTH.API.Application.Features.UserCommands;

public class LoadMenuByChannelCommand
    : BaseTransactionModel,
        ICommand<List<UserCommandResponseModel>>
{
    public string Channel { get; set; } = string.Empty;

}

[CqrsHandler]
public class LoadMenuByChannelHandle(
        IUserInRoleRepository userInRoleRepository,
        IUserCommandRepository userCommandRepository,
        IUserRightRepository userRightRepository,
        IUserRoleRepository userRoleRepository
    )
    : ICommandHandler<LoadMenuByChannelCommand, List<UserCommandResponseModel>>
{
    [WorkflowStep(WorkflowStepCode.CTH.WF_STEP_CTH_LOAD_MENU)]
    public async Task<List<UserCommandResponseModel>> HandleAsync(
        LoadMenuByChannelCommand request,
        CancellationToken cancellationToken = default
    )
    {
        List<UserInRole> listRoleofUser = await userInRoleRepository.ListOfRole(
            request.CurrentUserCode
        );

        List<CommandHierarchyModel> allMenus = [];

        foreach (UserInRole role in listRoleofUser)
        {
            List<CommandHierarchyModel> menus = await GetMenuInfoFromRoleId(
                role.RoleId,
                request.Channel,
                request.Language ?? "en"
            );
            allMenus.AddRange(menus);
        }

        List<CommandHierarchyModel> uniqueMenus = [.. allMenus
            .GroupBy(m => m.CommandId)
            .Select(g => g.First())];

        var menuHierarchy = BuildMenuHierarchy(uniqueMenus, "0");

        return [.. menuHierarchy.OrderBy(x => x.DisplayOrder)];
    }

    private async Task<List<CommandHierarchyModel>> GetMenuInfoFromRoleId(
        int roleId,
        string applicationCode,
        string lang
    )
    {
        List<UserRight> userRights = await userRightRepository
            .Table.Where(ur => ur.RoleId == roleId && ur.Invoke == 1)
            .ToListAsync();

        List<string> commandIds = userRights.Select(ur => ur.CommandId).Distinct().ToList();

        List<UserCommand> userCommands = await userCommandRepository
            .Table.Where(uc =>
                commandIds.Contains(uc.CommandId)
                && uc.ApplicationCode == applicationCode
                && uc.Enabled
            )
            .OrderBy(uc => uc.DisplayOrder)
            .ToListAsync();

        UserRole userRole = await userRoleRepository
            .Table.Where(r => r.RoleId == roleId)
            .FirstOrDefaultAsync();

        List<CommandHierarchyModel> result = [.. (
            from uc in userCommands
            join ur in userRights on uc.CommandId equals ur.CommandId
            select new CommandHierarchyModel
            {
                ParentId = uc.ParentId,
                CommandId = uc.CommandId,
                Label = TryGetLabelFromJson(uc.CommandNameLanguage, lang),
                CommandType = uc.CommandType,
                CommandUri = uc.CommandURI,
                RoleId = roleId,
                RoleName = userRole?.RoleName,
                Invoke = ur.Invoke == 1,
                Approve = ur.Approve == 1,
                Icon = uc.GroupMenuIcon,
                GroupMenuVisible = uc.GroupMenuVisible,
                GroupMenuId = uc.GroupMenuId,
                GroupMenuListAuthorizeForm = uc.GroupMenuListAuthorizeForm,
            }
        )];

        return result;
    }


    private static string TryGetLabelFromJson(string json, string lang)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            return "";
        }

        try
        {
            Dictionary<string, string> dict = JsonConvert.DeserializeObject<
                Dictionary<string, string>
            >(json);
            return dict.TryGetValue(lang, out string value) ? value : "";
        }
        catch
        {
            return "";
        }
    }

    /// <summary>
    /// Build menu hierarchy from flat list
    /// </summary>
    /// <param name="flatList"></param>
    /// <param name="parentId"></param>
    /// <returns></returns>
    private static List<UserCommandResponseModel> BuildMenuHierarchy(List<CommandHierarchyModel> flatList, string parentId)
    {
        return [.. flatList
                .Where(x => x.ParentId == parentId)
                .Select(item => new UserCommandResponseModel
                {
                    ParentId = item.ParentId,
                    CommandId = item.CommandId,
                    Label = item.Label,
                    CommandType = item.CommandType,
                    Href = item.CommandUri,
                    RoleId = item.RoleId,
                    RoleName = item.RoleName,
                    Invoke = item.Invoke ? "true" : "false",
                    Approve = item.Approve ? "true" : "false",
                    Icon = item.Icon,
                    GroupMenuVisible = item.GroupMenuVisible,
                    Prefix = item.GroupMenuId,
                    GroupMenuListAuthorizeForm = item.GroupMenuListAuthorizeForm,
                    Children = BuildMenuHierarchy(flatList, item.CommandId)
                })];
    }
}
