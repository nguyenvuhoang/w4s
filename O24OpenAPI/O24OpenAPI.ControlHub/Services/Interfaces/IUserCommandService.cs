using O24OpenAPI.APIContracts.Models.CTH;
using O24OpenAPI.ControlHub.Domain;
using O24OpenAPI.ControlHub.Models;
using O24OpenAPI.ControlHub.Models.Roles;

namespace O24OpenAPI.ControlHub.Services.Interfaces;

public interface IUserCommandService
{
    /// <summary>
    /// Get Menu Info From Role Id
    /// </summary>
    /// <param name="roleId"></param>
    /// <param name="applicationCode"></param>
    /// <param name="lang"></param>
    /// <returns></returns>
    Task<List<CommandHierarchyModel>> GetMenuInfoFromRoleId(
        int roleId,
        string applicationCode,
        string lang
    );

    /// <summary>
    /// Get Visible Transactions
    /// </summary>
    /// <param name="channelId"></param>
    /// <returns></returns>
    Task<List<VisibleTransactionResponse>> GetVisibleTransactions(string channelId);

    /// <summary>
    /// Load User Command
    /// </summary>
    /// <param name="applicationCode"></param>
    /// <param name="roleCommand"></param>
    /// <returns></returns>
    Task<List<UserCommand>> LoadUserCommand(string applicationCode, string roleCommand);

    /// <summary>
    /// Get User Command Info From Parent Id
    /// </summary>
    /// <param name="applicationCode"></param>
    /// <param name="parentId"></param>
    /// <returns></returns>
    Task<List<CommandIdInfoModel>> GetUserCommandInfoFromParentId(
        string applicationCode,
        string parentId
    );

    /// <summary>
    /// Get User Command Info From Command Id
    /// </summary>
    /// <param name="applicationCode"></param>
    /// <param name="commandId"></param>
    /// <returns></returns>
    Task<List<CommandIdInfoModel>> GetUserCommandInfoFromCommandId(
        string applicationCode,
        string commandId
    );

    /// <summary>
    /// Get Command Menu By Channel
    /// </summary>
    /// <param name="channelId"></param>
    /// <returns></returns>
    Task<List<UserCommandResponseModel>> GetCommandMenuByChannel(string channelId);

    /// <summary>
    /// Get Info From Form Code
    /// </summary>
    /// <param name="applicationCode"></param>
    /// <param name="formCode"></param>
    /// <returns></returns>
    Task<List<UserCommand>> GetInfoFromFormCode(string applicationCode, string formCode);

    /// <summary>
    /// Get Info From Command Id
    /// </summary>
    /// <param name="applicationCode"></param>
    /// <param name="commandId"></param>
    /// <returns></returns>
    Task<List<UserCommandResponseModel>> GetInfoFromCommandId(
        string applicationCode,
        string commandId
    );

    /// <summary>
    /// Get Info From Parent Id
    /// </summary>
    /// <param name="applicationCode"></param>
    /// <param name="parentId"></param>
    /// <returns></returns>
    Task<List<UserCommandResponseModel>> GetInfoFromParentId(
        string applicationCode,
        string parentId
    );

    /// <summary>
    /// Get List Command Parent
    /// </summary>
    /// <param name="applicationCode"></param>
    /// <returns></returns>
    Task<List<string>> GetListCommandParentAsync(string applicationCode);
    /// <summary>
    /// Get Full User Commands
    /// </summary>
    /// <returns></returns>
    Task<List<CTHUserCommandModel>> LoadFullUserCommandsAsync();
}
