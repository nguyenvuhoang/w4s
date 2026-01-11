using O24OpenAPI.APIContracts.Models.CTH;

namespace O24OpenAPI.GrpcContracts.GrpcClientServices.CTH;

public interface ICTHGrpcClientService
{
    Task<CTHUserSessionModel> GetUserSessionAsync(string token);
    Task<HashSet<string>> GetChannelRolesAsync(int roleId);
    Task<string> GetUserPushIdAsync(string userCode);
    Task<List<CTHUserCommandModel>> LoadUserCommandsAsync(
        string applicationCode,
        string roleCommand
    );
    Task<List<CTHUserCommandModel>> GetInfoFromFormCodeAsync(
        string applicationCode,
        string formCode
    );
    Task<List<CTHUserInRoleModel>> GetListRoleByUserCodeAsync(string userCode);
    Task<List<CTHCommandIdInfoModel>> GetInfoFromCommandIdAsync(
        string applicationCode,
        string commandId
    );
    Task<List<CTHCommandIdInfoModel>> GetInfoFromParentIdAsync(
        string applicationCode,
        string parentId
    );
    Task<CTHUserAccountModel> GetUserInfoByUserCodeAsync(string userCode);
    Task<CTHUserPushModel> GetUserPushIdByContractNumberAsync(string contractNumber);

    /// <summary>
    /// Get User Notification
    /// </summary>
    /// <returns></returns>
    Task<List<CTHUserNotificationModel?>> GetUserNotificationAsync();

    /// <summary>
    /// Load Full User Command
    /// </summary>
    /// <returns></returns>
    Task<List<CTHUserCommandModel?>> LoadFullUserCommandAsync();
}
