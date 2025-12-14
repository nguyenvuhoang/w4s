using LinqToDB;
using O24OpenAPI.ControlHub.Domain;
using O24OpenAPI.ControlHub.Services.Interfaces;

namespace O24OpenAPI.ControlHub.Services;

public class UserAuthenService(
    IRepository<UserAuthen> userAuthenRepository
) : IUserAuthenService
{
    /// <summary>
    /// The repository
    /// </summary>
    private readonly IRepository<UserAuthen> _userAuthenRepository = userAuthenRepository;
    public async Task<UserAuthen> GetByUserCodeAsync(string userCode)
    {
        return await _userAuthenRepository.Table.Where(s => s.UserCode == userCode).FirstOrDefaultAsync();
    }

    public async Task<UserAuthen> AddAsync(UserAuthen user)
    {
        return await _userAuthenRepository.InsertAsync(user);
    }
    public async Task UpdateAsync(UserAuthen user)
    {
        await _userAuthenRepository.Update(user);
    }

    public async Task<UserAuthen> GetByUserAuthenInfoAsync(string userCode, string authenType, string phoneNumber)
    {
        return await _userAuthenRepository.Table.Where(
            s => s.UserCode == userCode
            && s.AuthenType == authenType
            && s.Phone == phoneNumber).
            FirstOrDefaultAsync();
    }
}
