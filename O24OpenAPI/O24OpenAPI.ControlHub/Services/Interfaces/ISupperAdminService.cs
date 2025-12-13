using O24OpenAPI.ControlHub.Domain;

namespace O24OpenAPI.ControlHub.Services.Interfaces;

/// <summary>
/// The supper admin service interface
/// </summary>
public interface ISupperAdminService
{
    /// <summary>
    /// Adds the supper admin
    /// </summary>
    /// <param name="supperAdmin">The supper admin</param>
    Task AddAsync(SupperAdmin supperAdmin);

    /// <summary>
    /// Ises the exit
    /// </summary>
    /// <returns>A task containing the bool</returns>
    Task<SupperAdmin> IsExit();
}
