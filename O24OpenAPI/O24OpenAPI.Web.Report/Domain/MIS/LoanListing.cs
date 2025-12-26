using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.Web.Report.Domain.MIS;

public partial class LoanListing : BaseEntity
{
    public string AccountNumber { get; set; }
    public DateTime OpenDate { get; set; }
    public DateTime? CloseDate { get; set; }
    public decimal CurrentBalance { get; set; }
    public DateTime StatementDate { get; set; }
    public string CustomerCode { get; set; }
    public string LoanStatus { get; set; }
    public string ClassificationStatus { get; set; }
    public string LatePayment { get; set; }
}
