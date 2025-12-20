using O24OpenAPI.ControlHub.Domain;
using O24OpenAPI.ControlHub.Models.User;
using O24OpenAPI.ControlHub.Services.Interfaces;
using O24OpenAPI.Framework.Extensions;

namespace O24OpenAPI.ControlHub.Services;

public class UserBannerService(IRepository<UserBanner> userBanner) : IUserBannerService
{
    private readonly IRepository<UserBanner> _userBanner = userBanner;

    /// <summary>
    /// Get user banner
    /// </summary>
    /// <param name="usercode"></param>
    /// <returns></returns>
    public async Task<string> GetUserBannerAsync(string usercode)
    {
        try
        {
            return _userBanner
                    .Table.Where(x => x.UserCode == usercode)
                    .Select(x => x.BannerSource)
                    .FirstOrDefault() ?? "default";
        }
        catch (Exception ex)
        {
            await ex.LogErrorAsync();
            return string.Empty;
        }
    }

    /// <summary>
    /// Update user banner
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<bool> UpdateUserBannerAsync(UserBannerModel model)
    {
        var userBanner = _userBanner
            .Table.Where(x => x.UserCode == model.UserCode)
            .FirstOrDefault();
        if (userBanner != null)
        {
            userBanner.BannerSource = model.Banner;
            userBanner.UpdatedOnUTC = DateTime.UtcNow;
            await _userBanner.Update(userBanner);
        }
        else
        {
            userBanner = new UserBanner
            {
                UserCode = model.UserCode,
                BannerSource = model.Banner,
                CreatedOnUTC = DateTime.UtcNow,
            };
            await _userBanner.Insert(userBanner);
        }
        return true;
    }
}
