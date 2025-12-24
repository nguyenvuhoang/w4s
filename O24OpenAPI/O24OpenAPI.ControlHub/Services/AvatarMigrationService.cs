using LinqToDB;
using O24OpenAPI.ControlHub.Domain;
using O24OpenAPI.ControlHub.Models;
using O24OpenAPI.ControlHub.Services.Interfaces;

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
        AvatarMigrationResultModel result = new AvatarMigrationResultModel();

        List<UserAvatar> users = await _userAvatarRepository.Table.ToListAsync();

        foreach (UserAvatar user in users)
        {
            try
            {
                string fileName = $"{user.UserCode}.png";
                string relativePath = Path.Combine("uploads", "avatars", fileName);
                string absolutePath = Path.Combine(_env.ContentRootPath, relativePath);

                Directory.CreateDirectory(Path.GetDirectoryName(absolutePath)!);

                byte[] bytes = Convert.FromBase64String(user.ImageUrl);
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
