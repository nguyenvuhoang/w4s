namespace O24OpenAPI.Web.CMS.Domain;

public partial class S_USERACCOUNT : BaseEntity
{
    public string UserCode { get; set; }
    public string UserName { get; set; }
    public string Remark { get; set; }
    public string UserAccountStatus { get; set; }
    public string UserCanChangePassword { get; set; }
    public string PasswordNeverExpire { get; set; }
    public string ChangePasswordWhenLogin { get; set; }
    public long EnforcePasswordHistory { get; set; }
    public long MaximumPasswordAge { get; set; }
    public long MinimumPasswordAge { get; set; }
    public long MinimumPasswordLength { get; set; }
    public string PasswordComplexityRequirements { get; set; }
    public decimal TimeZone { get; set; }
    public string ThousandSeparateCharacter { get; set; }
    public string DecimalSeparateCharacter { get; set; }
    public string DateFormat { get; set; }
    public string LongDateFormat { get; set; }
    public string TimeFormat { get; set; }
    public long LockoutDur { get; set; }
    public long LockoutTthrs { get; set; }
    public long ResetLockout { get; set; }
    public long PolicyId { get; set; }
    public DateTime? ExpireDate { get; set; }
    public DateTime? UpdatedOnUtc { get; set; }
    public DateTime? CreatedOnUtc { get; set; }
}
