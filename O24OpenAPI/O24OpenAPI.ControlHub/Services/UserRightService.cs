using LinqToDB;
using O24OpenAPI.ControlHub.Domain;
using O24OpenAPI.ControlHub.Services.Interfaces;
using O24OpenAPI.Data;

namespace O24OpenAPI.ControlHub.Services;

/// <summary>
/// The user right service class
/// </summary>
/// <seealso cref="IUserRightService"/>
public class UserRightService(
    IRepository<UserRightChannel> repository,
     IRepository<UserRight> repositoryUserRight
) : IUserRightService
{
    /// <summary>
    /// The repository
    /// </summary>
    private readonly IRepository<UserRightChannel> _repository = repository;
    private readonly IRepository<UserRight> _repositoryUserRight = repositoryUserRight;


    /// <summary>
    /// Adds the entity
    /// </summary>
    /// <param name="entity">The entity</param>
    /// <returns>A task containing the user right</returns>
    public async Task<UserRightChannel> AddAsync(UserRightChannel entity)
    {
        return await _repository.InsertAsync(entity);
    }
    /// <summary>
    /// AddUserRightAsync
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public async Task<UserRight> AddUserRightAsync(UserRight entity)
    {
        return await _repositoryUserRight.InsertAsync(entity);
    }

    /// <summary>
    /// GetByRoleIdAndCommandIdAsync
    /// </summary>
    /// <param name="roleId"></param>
    /// <param name="commandId"></param>
    /// <returns></returns>
    public async Task<UserRight> GetByRoleIdAndCommandIdAsync(int roleId, string commandId)
    {
        return await _repositoryUserRight
            .Table.Where(s => s.RoleId == roleId && s.CommandId == commandId)
            .FirstOrDefaultAsync();
    }

    public async Task<List<int>> GetListRoleIdByChannelAsync(string channelId)
    {
        return await _repository
            .Table.Where(s => s.ChannelId == channelId)
            .Select(s => s.RoleId)
            .ToListAsync();
    }

    /// <summary>
    /// Gets the set channel in role using the specified role id
    /// </summary>
    /// <param name="roleId">The role id</param>
    /// <returns>A task containing a hash set of string</returns>
    public async Task<HashSet<string>> GetSetChannelInRoleAsync(int roleId)
    {
        return await _repository
            .Table.Where(s => s.RoleId == roleId)
            .Select(s => s.ChannelId)
            .AsAsyncEnumerable()
            .ToHashSetAsync();
    }

    public async Task<HashSet<string>> GetSetChannelInRoleAsync(int[] roleId)
    {
        var result = new HashSet<string>();
        foreach (var role in roleId)
        {
            var set = await _repository
                .Table.Where(s => s.RoleId == role)
                .Select(s => s.ChannelId)
                .AsAsyncEnumerable()
                .ToHashSetAsync();

            result.UnionWith(set);
        }
        return result;
    }

    /// <summary>
    /// Updates the entity
    /// </summary>
    /// <param name="entity">The entity</param>
    public async Task UpdateAsync(UserRightChannel entity)
    {
        await _repository.Update(entity);
    }

    public async Task UpdateAsync(UserRight userRight)
    {
        await _repositoryUserRight.Update(userRight);
    }
}
