namespace O24OpenAPI.Web.CMS.Domain;

public partial class D_DIGITALBANKINGUSER : BaseEntity
{
    public string UserName { get; set; }

    public string UserCode { get; set; }

    public string ApplicationCode { get; set; }

    public string ContractNumber { get; set; }

    public string FirstName { get; set; }

    public string MiddleName { get; set; }

    public string LastName { get; set; }

    public string LocalFullName { get; set; }

    public string FullName { get; set; }

    public string Gender { get; set; }

    public DateTime? Birthday { get; set; }

    public string Address { get; set; }

    public string Email { get; set; }

    public string Phone { get; set; }

    public string Status { get; set; }

    public string UserCreate { get; set; }

    public DateTime? DateCreate { get; set; }

    public string UserModify { get; set; }

    public DateTime? LastModify { get; set; }

    public string UserApprove { get; set; }

    public DateTime? DateApprove { get; set; }

    public bool? IsLoginIB { get; set; }

    public bool? IsLoginMB { get; set; }

    public string BranchCode { get; set; }
}
