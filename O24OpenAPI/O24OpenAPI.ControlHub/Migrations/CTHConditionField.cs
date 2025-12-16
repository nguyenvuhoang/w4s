using O24OpenAPI.ControlHub.Domain;
using O24OpenAPI.Core.Domain;
using O24OpenAPI.Framework.Domain;

namespace O24OpenAPI.ControlHub.Migrations;

/// <summary>
/// The cth condition field class
/// </summary>
public class CTHConditionField
{
    /// <summary>
    /// The role id
    /// </summary>
    public static readonly List<string> UserRoleCondition = [nameof(UserRole.RoleId)];

    /// <summary>
    /// User role channel condition
    /// </summary>
    public static readonly List<string> UserRoleChannelCondition = [nameof(UserRoleChannel.RoleId)];

    /// <summary>
    /// The channel id
    /// /// </summary>
    public static readonly List<string> UserRightCondition =
    [
        nameof(UserRight.RoleId),
        nameof(UserRight.CommandId),
    ];

    public static readonly List<string> UserCommandCondition =
    [
        nameof(UserCommand.ApplicationCode),
        nameof(UserCommand.CommandId),
        nameof(UserCommand.ParentId),
    ];

    public static readonly List<string> O24OpenAPIServiceCondition =
    [
        nameof(O24OpenAPIService.StepCode),
    ];
    public static readonly List<string> UserAccountCondition =
    [
        nameof(UserAccount.UserId),
        nameof(UserAccount.UserCode),
        nameof(UserAccount.UserName),
        nameof(UserAccount.LoginName),
        nameof(UserAccount.ContractNumber),
    ];
    public static readonly List<string> UserPasswordCondition = [nameof(UserPassword.UserId)];

    public static readonly List<string> UserRightChannelCondition =
    [
        nameof(UserRightChannel.RoleId),
    ];

    public static readonly List<string> SupperAdminCondition =
    [
        nameof(SupperAdmin.UserId),
        nameof(SupperAdmin.LoginName),
    ];

    public static readonly List<string> C_CODELISTCondition =
    [
        nameof(C_CODELIST.CodeId),
        nameof(C_CODELIST.CodeName),
        nameof(C_CODELIST.CodeGroup),
    ];

    public static readonly List<string> UserInRoleCondition =
    [
        nameof(UserInRole.RoleId),
        nameof(UserInRole.UserCode),
    ];

    public static readonly List<string> StoredCommandCondition = [nameof(StoredCommand.Name)];
    public static readonly List<string> UserLevelCondition = [nameof(UserLevel.LevelCode)];

    public static readonly List<string> ChannelCondition =
    [
        nameof(Channel.ChannelId),
        nameof(Channel.ChannelName),
    ];
    public static readonly List<string> UserPolicyCondition =
    [
        nameof(UserPolicy.PolicyCode),
        nameof(UserPolicy.ServiceID),
    ];

    public static readonly List<string> ChannelConditionScheduleInterval =
    [
        nameof(ChannelScheduleInterval.ChannelScheduleIdRef),
    ];

    public static readonly List<string> ChannelScheduleCondition =
    [
        nameof(ChannelSchedule.ChannelIdRef),
    ];

    public static readonly List<string> BankWorkingCalendarDataCondition =
    [
        nameof(BankWorkingCalendar.BankCode),
        nameof(BankWorkingCalendar.BranchCode),
    ];
}
