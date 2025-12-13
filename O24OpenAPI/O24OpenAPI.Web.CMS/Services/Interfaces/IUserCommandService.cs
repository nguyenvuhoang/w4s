using O24OpenAPI.APIContracts.Models.CTH;
using O24OpenAPI.Web.CMS.Models;

namespace O24OpenAPI.Web.CMS.Services.Interfaces;

public interface IUserCommandService
{
    Task<List<UserCommand>> GetByAppAndRoleCommand(string appCode, HashSet<string> roleCommand);
    Task<List<UserCommand>> GetMenuByAppAndRole(string appCode, HashSet<string> roleCommand);
    Task<List<UserCommand>> GetInfoFromFormCode(string applicationCode, string formCode);
    Task<List<Models.UserCommandModel>> GetInfoFromCommandId(string applicationCode, string commandId);
    Task<List<Models.UserCommandModel>> GetInfoFromParentId(string applicationCode, string parentId);
    Task<List<UserCommand>> GetByAppAndRole(string applicationCode, HashSet<string> roleCommand);
    Task<List<CTHCommandIdInfoModel>> GetUserCommandInfoFromParentId(
        string applicationCode,
        string parentId
    );
    Task<List<CTHCommandIdInfoModel>> GetUserCommandInfoFromCommandId(
        string applicationCode,
        string commandId
    );
    Task<List<UserCommandResponse>> LoadUserCommand();
    Task<List<UserCommandResponse>> GetCommandMenuByChannel(string channelId);
    Task<List<UserCommand>> ListVisibleByChannel(string channelId);
    Task<HashSet<string>> GetVisibleTransactionCommandsByChannelAsync(string channelId);
}
