using O24OpenAPI.ControlHub.Domain;

namespace O24OpenAPI.ControlHub.Services.Interfaces;

public interface IUserPasswordService
{
    Task<UserPassword> GetByUserCodeAsync(string userCode);
    Task UpdateAsync(UserPassword entity);
    Task<UserPassword> AddAsync(UserPassword user);
    Task DeletePasswordByUserIdAsync(string userId);
}
