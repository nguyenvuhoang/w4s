using LinqToDB;
using O24OpenAPI.ControlHub.Domain;
using O24OpenAPI.ControlHub.Services.Interfaces;

namespace O24OpenAPI.ControlHub.Services;

public class UserAvatarService(
    IRepository<UserAvatar> userAvatarRepository
) : IUserAvatarService
{
    private readonly IRepository<UserAvatar> _userAvatarRepository = userAvatarRepository;
    public async Task<UserAvatar> AddAsync(UserAvatar user)
    {
        return await _userAvatarRepository.InsertAsync(user);
    }

    public async Task<UserAvatar> GetByUserCodeAsync(string userCode)
    {
        return await _userAvatarRepository.Table.Where(s => s.UserCode == userCode).FirstOrDefaultAsync();
    }

    public async Task UpdateAsync(UserAvatar entity)
    {
        await _userAvatarRepository.Update(entity);
    }
}
