using O24OpenAPI.ControlHub.Models.User;

namespace O24OpenAPI.ControlHub.Services.Interfaces;

public interface IUserBannerService
{
    /// <summary>
    /// Get user banner
    /// </summary>
    /// <param name="usercode"></param>
    /// <returns></returns>
    public Task<string> GetUserBannerAsync(string usercode);

    /// <summary>
    /// Update user banner
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public Task<bool> UpdateUserBannerAsync(UserBannerModel model);
}
