using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.Web.Report.Domain.MIS;

public partial class CustomerListing : BaseEntity
{
    public string CustomerCode { get; set; }
    public DateTime CreateDate { get; set; }
    public string EconomicSector { get; set; }
    public string Districts { get; set; }
    public string Villages { get; set; }
    public string Province { get; set; }
    public string Gender { get; set; }
    public string CustomerType { get; set; }
}
