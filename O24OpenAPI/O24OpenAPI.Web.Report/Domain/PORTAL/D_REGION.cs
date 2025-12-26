using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.Web.Report.Domain.PORTAL;

public partial class D_REGION : BaseEntity
{
    public string RegionCode { get; set; }
    public string RegionName { get; set; }
    public string RegionSpecial { get; set; }
    public string Description { get; set; }
    public string UserCreate { get; set; }
    public DateTime? DateCreate { get; set; } = DateTime.UtcNow;
    public string UserModify { get; set; }
    public DateTime? DateModify { get; set; }
    public string Status { get; set; }
}
