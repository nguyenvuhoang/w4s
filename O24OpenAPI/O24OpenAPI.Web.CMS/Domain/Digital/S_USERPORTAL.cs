namespace O24OpenAPI.Web.CMS.Domain;

public partial class S_USERPORTAL : BaseEntity
{
    public string UserName { get; set; }

    public string UserCode { get; set; }

    public string FirstName { get; set; }

    public string MiddleName { get; set; }

    public string LastName { get; set; }

    public int Gender { get; set; } = 0;

    public string Address { get; set; }

    public string Email { get; set; }

    public DateTime? Birthday { get; set; }

    public string Phone { get; set; }

    public string Status { get; set; }

    public string UserCreated { get; set; }

    public DateTime? DateCreated { get; set; }

    public DateTime? LastLoginTime { get; set; }

    public string UserModified { get; set; }

    public DateTime? DateModified { get; set; }

    public bool? IsLogin { get; set; }

    public DateTime? ExpireTime { get; set; }

    public string BranchID { get; set; }

    public string DepartmentCode { get; set; }

    public int UserLevel { get; set; } = 0;

    public string UserType { get; set; }

    public string IsShow { get; set; }

    public int PolicyID { get; set; } = 0;

    public string UUID { get; set; }

    public int Failnumber { get; set; } = 0;

    public bool IsSuperAdmin { get; set; } = false;
}
