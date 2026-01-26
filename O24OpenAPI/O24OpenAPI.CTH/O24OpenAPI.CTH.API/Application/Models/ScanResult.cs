namespace O24OpenAPI.CTH.API.Application.Models;

public class ScanResult
{
    public int TotalMigrated { get; set; }
    public int TotalFailed { get; set; }
    public List<string> Errors { get; set; } = new();
}
