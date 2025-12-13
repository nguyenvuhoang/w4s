using LinqToDB;
using O24OpenAPI.ControlHub.Domain;
using O24OpenAPI.ControlHub.Services.Interfaces;
using O24OpenAPI.Data;

namespace O24OpenAPI.ControlHub.Services;

/// <summary>
/// The supper admin service class
/// </summary>
/// <seealso cref="ISupperAdminService"/>
public class SupperAdminService(IRepository<SupperAdmin> repository) : ISupperAdminService
{
    /// <summary>
    /// The repository
    /// </summary>
    private readonly IRepository<SupperAdmin> _repository = repository;

    /// <summary>
    /// Adds the supper admin
    /// </summary>
    /// <param name="supperAdmin">The supper admin</param>
    public async Task AddAsync(SupperAdmin supperAdmin)
    {
        await _repository.Insert(supperAdmin);
    }

    /// <summary>
    /// Supper Admin is exited
    /// </summary>
    /// <returns>A task containing the bool</returns>
    public async Task<SupperAdmin> IsExit()
    {
        return await _repository.Table.FirstOrDefaultAsync();
    }
}
