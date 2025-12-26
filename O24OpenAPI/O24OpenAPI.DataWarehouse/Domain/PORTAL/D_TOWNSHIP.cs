using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.DataWarehouse.Domain.PORTAL;

public partial class D_TOWNSHIP : BaseEntity
{
    public string TownshipCode { get; set; }
    public string TownshipName { get; set; }
    public string DistCode { get; set; }
    public string TownshipNameMM { get; set; }
}
