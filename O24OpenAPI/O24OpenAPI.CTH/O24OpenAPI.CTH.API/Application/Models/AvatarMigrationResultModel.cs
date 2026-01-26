namespace O24OpenAPI.CTH.API.Application.Models;

public class AvatarMigrationResultModel
{
    public int TotalMigrated { get; set; } = 0;
    public int TotalFailed { get; set; } = 0;
    public List<string> Errors { get; set; } = [];
}
