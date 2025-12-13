using O24OpenAPI.ControlHub.Domain;
using O24OpenAPI.ControlHub.Models;
using O24OpenAPI.ControlHub.Services.Interfaces;
using O24OpenAPI.Data;
using O24OpenAPI.Web.Framework.Extensions;

namespace O24OpenAPI.ControlHub.Services;

public class AvatarMigrationService(
    IRepository<UserAvatar> userAvatarRepository,
    IWebHostEnvironment environment
) : IAvatarMigrationService
{
    private readonly IWebHostEnvironment _env = environment;
    private readonly IRepository<UserAvatar> _userAvatarRepository = userAvatarRepository;


    public async Task<AvatarMigrationResultModel> MigrateBase64AvatarsAsync()
    {
        var result = new AvatarMigrationResultModel();

        var users = await _userAvatarRepository.Table.ToListAsync();

        foreach (var user in users)
        {
            try
            {
                var fileName = $"{user.UserCode}.png";
                var relativePath = Path.Combine("uploads", "avatars", fileName);
                var absolutePath = Path.Combine(_env.ContentRootPath, relativePath);

                Directory.CreateDirectory(Path.GetDirectoryName(absolutePath)!);

                var bytes = Convert.FromBase64String(user.ImageUrl);
                await File.WriteAllBytesAsync(absolutePath, bytes);

                user.ImageUrl = $"/{relativePath.Replace("\\", "/")}";

                result.TotalMigrated++;

                await _userAvatarRepository.Update(user);
            }
            catch (Exception ex)
            {
                result.TotalFailed++;
                result.Errors.Add($"User: {user.UserCode}, Error: {ex.Message}");
            }
        }
        return result;
    }
}
