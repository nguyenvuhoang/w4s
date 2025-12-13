using Newtonsoft.Json.Linq;
using O24OpenAPI.ControlHub.Models.Roles;

namespace O24OpenAPI.ControlHub.Services.Interfaces;

public interface IRoleProfileService
{
    /// <summary>
    /// Load Role Operation Async
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<JObject> LoadRoleOperationAsync(UserCommandRequestModel model);

    /// <summary>
    /// Load Menu By Channel Async
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<List<UserCommandResponseModel>> LoadMenuByChannelAsync(UserCommandRequestModel model);
    /// <summary>
    /// UpdateUserRightAsync
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>

    Task<bool> UpdateUserRightAsync(UserRightUpdateModel model);
}
