namespace O24OpenAPI.CMS.Domain.AggregateModels;

public partial class AppTypeConfig : BaseEntity
{
    public string? AppCode { get; set; }
    public string? AppName { get; set; }
    public string? AppTypeDescription { get; set; }
    public string? AppTypeIcon { get; set; }
    public int OrderIndex { get; set; }
    public string? RedirectPage { get; set; }
    public bool IsActive { get; set; } = true;
}
