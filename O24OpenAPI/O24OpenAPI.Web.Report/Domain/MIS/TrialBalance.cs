using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.Web.Report.Domain.MIS;

public partial class TrialBalance : BaseEntity
{
    public string SubCOA { get; set; }
    public string BranchCode { get; set; }
    public DateTime StatementDate { get; set; }
    public decimal OpenBalance { get; set; }
    public decimal MovingDebit { get; set; }
    public decimal MovingCredit { get; set; }
    public decimal ClosingBalance { get; set; }
    public string Currency { get; set; }
    public DateTime ImportDate { get; set; }
    public string BalanceSide { get; set; }
}
