using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.Web.Report.Domain.PORTAL;

public partial class D_CUSTOMER : BaseEntity
{
    public string CUSTID { get; set; }
    public string FULLNAME { get; set; }
    public string SHORTNAME { get; set; }
    public DateTime? DOB { get; set; }
    public string ADDRRESIDENT { get; set; }
    public string ADDRTEMP { get; set; }
    public char SEX { get; set; }
    public string NATION { get; set; }
    public string TEL { get; set; }
    public string FAX { get; set; }
    public string Email { get; set; }
    public string LICENSETYPE { get; set; }
    public string LICENSEID { get; set; }
    public DateTime? ISSUEDATE { get; set; }
    public string ISSUEPLACE { get; set; }
    public string DESCRIPTION { get; set; }
    public string JOB { get; set; }
    public string OFFICEADDR { get; set; }
    public string OFFICEPHONE { get; set; }
    public char CFTYPE { get; set; }
    public string BRANCHID { get; set; }
    public string STATUS { get; set; }
    public string CUSTCODE { get; set; }
    public string CFCode { get; set; }
    public string CTYPE { get; set; }
    public string PhoneCountryCode { get; set; }
    public string LinkedUserID { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public string KycID { get; set; }
    public string UserCreated { get; set; }
    public DateTime? DateCreated { get; set; }
    public string UserModified { get; set; }
    public DateTime? LastModified { get; set; }
    public string UserApproved { get; set; }
    public DateTime? DateApproved { get; set; }
    public string Township { get; set; }
    public string Region { get; set; }
    public string FULLNAMEMM { get; set; }
    public string ADDRMM { get; set; }
    public string PHONEMM { get; set; }
    public string LATITUDE { get; set; }
    public string LONGITUDE { get; set; }
    public string AGENTLOCATION { get; set; }
}
