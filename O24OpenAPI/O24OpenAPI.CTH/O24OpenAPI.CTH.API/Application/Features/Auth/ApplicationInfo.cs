using LinKit.Core.Cqrs;
using LinqToDB;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.Core.Configuration;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.CTH.Domain.AggregatesModel.ChannelAggregate;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.CTH.API.Application.Features.Auth;

public class ApplicationInfoCommand
    : BaseTransactionModel,
        ICommand<ApplicationInfoResponseModel>
{ }

public class ApplicationInfoResponseModel(
    string userCode,
    string avatar,
    string fullName,
    string loginName,
    DateTime? lastLoginTime,
    string contractNumber,
    bool isBiometricSupported,
    bool isSmartOTPActive,
    bool? isLogin,
    string userBanner,
    List<UserInRole> role,
    bool? isFirstLogin,
    List<UserRoleChannelModel> roleChannel
)
{
    public string UserCode { get; set; } = userCode;
    public string Avatar { get; set; } = avatar;
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
    public List<UserRoleChannelModel> RoleChannel { get; set; } = roleChannel;
}
public class UserRoleChannelModel
{
    public int Id { get; set; }
    public string ChannelId { get; set; }
    public string ChannelName { get; set; }
    public int SortOrder { get; set; }
    public string AppIcon { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

[CqrsHandler]
public class ApplicationInfoHandler(
    IUserInRoleRepository userInRoleRepository,
    IUserAccountRepository userAccountService,
    IUserAvatarRepository userAvatarRepository,
    IUserAuthenRepository userAuthenRepository,
    IUserBannerRepository userBannerRepository,
    IUserRightChannelRepository userRightChannelRepository,
    IChannelRepository channelRepository
) : ICommandHandler<ApplicationInfoCommand, ApplicationInfoResponseModel>
{
    [WorkflowStep(WorkflowStepCode.CTH.WF_STEP_CTH_APP_INFO)]
    public async Task<ApplicationInfoResponseModel> HandleAsync(
        ApplicationInfoCommand command,
        CancellationToken cancellationToken
    )
    {
        List<UserInRole> listRoleofUser = await userInRoleRepository.ListOfRole(
            command.CurrentUserCode
        );

        var allRoleChannelLinks = new List<UserRightChannel>();
        foreach (UserInRole role in listRoleofUser)
        {
            var roleChannels = await userRightChannelRepository.GetListByRoleIdAsync(role.RoleId);
            allRoleChannelLinks.AddRange(roleChannels);
        }

        var channelIds = allRoleChannelLinks
        .Select(x => x.ChannelId)
        .Distinct()
        .ToList();

        var channels = await channelRepository.GetByChannelIdsAsync(channelIds, cancellationToken);
        var channelMap = channels.ToDictionary(
            x => x.ChannelId,
            x => x
        );

        var roleChannelModels = allRoleChannelLinks
        .Where(rc => channelMap.ContainsKey(rc.ChannelId))
        .Select(rc =>
        {
            var ch = channelMap[rc.ChannelId];

            return new UserRoleChannelModel
            {
                Id = rc.Id,
                ChannelName = ch.ChannelName ?? "",
                ChannelId = rc.ChannelId,
                SortOrder = ch.SortOrder,
                AppIcon = ch.AppICon ?? "",
                Description = ch.Description ?? ""
            };
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
            loginName: userAccount.LoginName,
            lastLoginTime: userAccount.LastLoginTime,
            contractNumber: userAccount.ContractNumber,
            isBiometricSupported: userAccount.IsBiometricSupported,
            isSmartOTPActive: userAuthen?.IsActive ?? false,
            isLogin: userAccount.IsLogin,
            role: listRoleofUser,
            isFirstLogin: userAccount.IsFirstLogin,
            userBanner: userBanner ?? "",
            roleChannel: roleChannelModels
        );
        return responsedata;
    }
}
