using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.CMS.Domain.AggregateModels;

public class AppTypeConfig : BaseEntity
{
    public string AppCode { get; set; }
    public string AppName { get; set; }
    public string AppTypeDescription { get; set; }
    public string AppTypeIcon { get; set; }
    public int OrderIndex { get; set; }
    public string RedirectPage { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedOnUtc { get; set; }
    public DateTime? UpdatedOnUtc { get; set; }
}
