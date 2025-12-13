namespace O24OpenAPI.ControlHub.Models;

public class ScanResult
{
    public int TotalMigrated { get; set; }
    public int TotalFailed { get; set; }
    public List<string> Errors { get; set; } = new();
}
