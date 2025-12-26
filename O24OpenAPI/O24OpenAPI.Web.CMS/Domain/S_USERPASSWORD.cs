namespace O24OpenAPI.Web.CMS.Domain;

public partial class S_USERPASSWORD : BaseEntity
{
    public string UserCode { get; set; }
    public string Password { get; set; }
    public DateTime LastLogin { get; set; }
    public int FailureCount { get; set; }
    public string PasswordSalt { get; set; }
    public DateTime? UpdatedOnUtc { get; set; }
    public DateTime? CreatedOnUtc { get; set; }
}
