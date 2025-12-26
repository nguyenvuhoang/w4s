using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.Web.Report.Domain.MIS;

public partial class Branch : BaseEntity
{
    public string BranchCode { get; set; }
    public string BranchName { get; set; }
    public string BranchAddress { get; set; }
    public string Phone { get; set; }
}
