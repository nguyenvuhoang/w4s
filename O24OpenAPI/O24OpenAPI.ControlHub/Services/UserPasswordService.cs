using LinqToDB;
using O24OpenAPI.ControlHub.Domain;
using O24OpenAPI.ControlHub.Services.Interfaces;
using O24OpenAPI.Data;

namespace O24OpenAPI.ControlHub.Services;

/// <summary>
/// The user account service class
/// </summary>
/// <seealso cref="IUserPasswordService"/>
public class UserPasswordService(
    IRepository<UserPassword> passwordRepository
) : IUserPasswordService
{

    /// <summary>
    /// The password repository
    /// </summary>
    private readonly IRepository<UserPassword> _passwordRepository = passwordRepository;

    /// <summary>
    /// Get User by code
    /// </summary>
    /// <param name="userCode"></param>
    /// <returns></returns>
    public async Task<UserPassword> GetByUserCodeAsync(string userCode)
    {
        return await _passwordRepository.Table.Where(s => s.UserId == userCode).FirstOrDefaultAsync();
    }

    /// <summary>
    /// Update User Password
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public async Task UpdateAsync(UserPassword entity)
    {
        await _passwordRepository.Update(entity);
    }

    public async Task<UserPassword> AddAsync(UserPassword user)
    {
        return await _passwordRepository.InsertAsync(user);
    }

    public async Task DeletePasswordByUserIdAsync(string userId)
    {
        var entity = await _passwordRepository.Table
        .FirstOrDefaultAsync(x => x.UserId == userId);

        if (entity != null)
        {
            await _passwordRepository.Delete(entity);
        }
    }
}
