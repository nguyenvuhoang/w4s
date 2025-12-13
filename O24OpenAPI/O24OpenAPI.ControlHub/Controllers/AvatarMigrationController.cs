using Microsoft.AspNetCore.Mvc;
using O24OpenAPI.ControlHub.Services.Interfaces;

namespace O24OpenAPI.ControlHub.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AvatarMigrationController : ControllerBase
{
    private readonly IAvatarMigrationService _avatarMigrationService;

    public AvatarMigrationController(IAvatarMigrationService avatarMigrationService)
    {
        _avatarMigrationService = avatarMigrationService;
    }

    [HttpPost("migrate")]
    public async Task<IActionResult> MigrateAvatars()
    {
        var result = await _avatarMigrationService.MigrateBase64AvatarsAsync();
        return Ok(new
        {
            message = "Migration completed",
            totalMigrated = result.TotalMigrated,
            totalFailed = result.TotalFailed,
            errors = result.Errors
        });
    }
}
