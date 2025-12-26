using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.Web.Report.Domain.PORTAL;

public partial class D_CONTRACTACCOUNT : BaseEntity
{
    public string ContractNumber { get; set; }
    public string AccountNumber { get; set; }
    public string AccountType { get; set; }
    public string CurrencyCode { get; set; }
    public string Status { get; set; }
    public string BranchID { get; set; }
    public string BankAccountType { get; set; }
    public string BankId { get; set; }
    public bool? IsPrimary { get; set; }
}
