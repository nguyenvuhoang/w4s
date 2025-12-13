using O24OpenAPI.ControlHub.Models;

namespace O24OpenAPI.ControlHub.Services.Interfaces;

public interface IAvatarMigrationService
{
    Task<AvatarMigrationResultModel> MigrateBase64AvatarsAsync();
}
