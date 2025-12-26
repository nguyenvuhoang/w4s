using LinKit.Core.Cqrs;
using LinqToDB;
using Newtonsoft.Json;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.Core.Configuration;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.CTH.API.Application.Models;
using O24OpenAPI.CTH.API.Application.Models.Roles;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.CTH.API.Application.Features.Application;

public class ApplicationInfoCommand
    : BaseTransactionModel,
        ICommand<ApplicationInfoResponseModel> { }

public class ApplicationInfoResponseModel(
    string userCode,
    string avatar,
    object userCommand,
    string fullName,
    string loginName,
    DateTime? lastLoginTime,
    string contractNumber,
    bool isBiometricSupported,
    bool isSmartOTPActive,
    bool? isLogin,
    string userBanner,
    List<UserInRole> role,
    bool? isFirstLogin
)
{
    public string UserCode { get; set; } = userCode;
    public string Avatar { get; set; } = avatar;
    public object UserCommand { get; set; } = userCommand;
    public string Name { get; set; } = fullName;
    public string LoginName { get; set; } = loginName;
    public bool? IsFirstLogin { get; set; } = isFirstLogin;
    public string ContractNumber { get; set; } = contractNumber;
    public bool IsBiometricSupported { get; set; } = isBiometricSupported;
    public bool IsSmartOTPActive { get; set; } = isSmartOTPActive;
    public bool? IsLogin { get; set; } = isLogin;
    public List<UserInRole> Role { get; set; } = role;
    public string UserBanner { get; set; } = userBanner;
    public DateTime LastLoginTime { get; set; } = lastLoginTime ?? DateTime.MinValue;
}

[CqrsHandler]
public class ApplicationInfoHandler(
    IUserInRoleRepository userInRoleRepository,
    IUserRightRepository userRightRepository,
    IUserCommandRepository userCommandRepository,
    IUserRoleRepository userRoleRepository,
    IUserAgreementRepository userAgreementRepository,
    IUserAccountRepository userAccountService,
    IUserAvatarRepository userAvatarRepository,
    IUserAuthenRepository userAuthenRepository,
    IUserBannerRepository userBannerRepository
) : ICommandHandler<ApplicationInfoCommand, ApplicationInfoResponseModel>
{
    [WorkflowStep(WorkflowStep.CTH.WF_STEP_CTH_APP_INFO)]
    public async Task<ApplicationInfoResponseModel> HandleAsync(
        ApplicationInfoCommand command,
        CancellationToken cancellationToken
    )
    {
        List<UserInRole> listRoleofUser = await userInRoleRepository.ListOfRole(
            command.CurrentUserCode
        );

        List<CommandHierarchyModel> allMenus = [];

        foreach (UserInRole role in listRoleofUser)
        {
            List<CommandHierarchyModel> menus = await GetMenuInfoFromRoleId(
                role.RoleId,
                command.ChannelId,
                command.Language ?? "en"
            );
            allMenus.AddRange(menus);
        }

        List<CommandHierarchyModel> uniqueMenus = allMenus
            .GroupBy(m => m.CommandId)
            .Select(g => g.First())
            .ToList();

        List<UserCommandResponseModel> menuHierarchy = uniqueMenus
            .Where(m => m.ParentId == "0")
            .Select(parent => new UserCommandResponseModel
            {
                ParentId = parent.ParentId,
                CommandId = parent.CommandId,
                Label = parent.Label,
                CommandType = parent.CommandType,
                Href = parent.CommandUri,
                RoleId = parent.RoleId,
                RoleName = parent.RoleName,
                Invoke = parent.Invoke ? "true" : "false",
                Approve = parent.Approve ? "true" : "false",
                Icon = parent.Icon,
                GroupMenuVisible = parent.GroupMenuVisible,
                Prefix = parent.GroupMenuId,
                GroupMenuListAuthorizeForm = parent.GroupMenuListAuthorizeForm,
                GroupMenuId = parent.GroupMenuId,
                IsAgreement = parent.IsAgreement,
                Children =
                [
                    .. uniqueMenus
                        .Where(child => child.ParentId == parent.CommandId)
                        .Select(child => new UserCommandResponseModel
                        {
                            ParentId = child.ParentId,
                            CommandId = child.CommandId,
                            Label = child.Label,
                            CommandType = child.CommandType,
                            Href = child.CommandUri,
                            RoleId = child.RoleId,
                            RoleName = child.RoleName,
                            Invoke = child.Invoke ? "true" : "false",
                            Approve = child.Approve ? "true" : "false",
                            Icon = child.Icon,
                            GroupMenuVisible = child.GroupMenuVisible,
                            Prefix = child.GroupMenuId,
                            GroupMenuListAuthorizeForm = child.GroupMenuListAuthorizeForm,
                            GroupMenuId = child.GroupMenuId,
                            IsAgreement = child.IsAgreement,
                            Children = null,
                        }),
                ],
            })
            .ToList();

        UserAccount userAccount = await userAccountService.GetByUserCodeAsync(
            command.CurrentUserCode
        );

        UserAvatar avatarRecord = await userAvatarRepository.GetByUserCodeAsync(
            userAccount.UserCode
        );

        O24OpenAPIConfiguration openApiConfig =
            Singleton<AppSettings>.Instance.Get<O24OpenAPIConfiguration>();

        string avatarUrl = avatarRecord?.ImageUrl;
        if (string.IsNullOrWhiteSpace(avatarUrl))
        {
            string imagePath = "/uploads/avatars/default.png";
            string baseUrl = openApiConfig.OpenAPICMSURI;
            avatarUrl = $"{baseUrl}/{imagePath}";
        }

        UserAuthen userAuthen = await userAuthenRepository.GetByUserCodeAsync(userAccount.UserCode);
        string userBanner = await userBannerRepository.GetUserBannerAsync(userAccount.UserCode);
        ApplicationInfoResponseModel responsedata = new(
            userCode: command.CurrentUserCode,
            fullName: $"{userAccount.FirstName ?? ""} {userAccount.MiddleName ?? ""} {userAccount.LastName ?? ""}".Trim(),
            avatar: avatarUrl,
            userCommand: menuHierarchy,
            loginName: userAccount.LoginName,
            lastLoginTime: userAccount.LastLoginTime,
            contractNumber: userAccount.ContractNumber,
            isBiometricSupported: userAccount.IsBiometricSupported,
            isSmartOTPActive: userAuthen?.IsActive ?? false,
            isLogin: userAccount.IsLogin,
            role: listRoleofUser,
            isFirstLogin: userAccount.IsFirstLogin,
            userBanner: userBanner ?? ""
        );
        return responsedata;
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

        List<CommandHierarchyModel> result = (
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
        ).ToList();

        foreach (CommandHierarchyModel item in result)
        {
            item.IsAgreement = await IsUserAgreement(item.CommandId);
        }

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

    private async Task<bool> IsUserAgreement(string commandid)
    {
        UserAgreement isAgreement = await userAgreementRepository
            .Table.Where(ua => ua.TransactionCode.Contains(commandid) && ua.IsActive)
            .FirstOrDefaultAsync();
        if (isAgreement != null)
        {
            return true;
        }
        return false;
    }
}
