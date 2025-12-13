using O24OpenAPI.ControlHub.Domain;

namespace O24OpenAPI.ControlHub.Services.Interfaces;

public interface IUserAuthenService
{
    /// <summary>
    /// Get User by code
    /// </summary>
    /// <param name="userCode"></param>
    /// <returns></returns>
    Task<UserAuthen> GetByUserCodeAsync(string userCode);
    /// <summary>
    /// Add User Code
    /// </summary>
    /// <param name="userCode"></param>
    /// <returns></returns>
    Task<UserAuthen> AddAsync(UserAuthen user);
    /// <summary>
    /// Verify UserAuthen Innformation
    /// </summary>
    /// <param name="userCode"></param>
    /// <param name="authenType"></param>
    /// <param name="phoneNumber"></param>
    /// <returns></returns>
    Task<UserAuthen> GetByUserAuthenInfoAsync(string userCode, string authenType, string phoneNumber);

    Task UpdateAsync(UserAuthen user);
}
