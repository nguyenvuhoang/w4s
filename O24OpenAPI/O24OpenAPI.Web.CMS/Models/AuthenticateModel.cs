using System.Text.Json.Serialization;
using Newtonsoft.Json;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.Web.CMS.Models.O9;

namespace O24OpenAPI.Web.CMS.Models;

public class LoginCoreResponse : BaseO24OpenAPIModel
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="token"></param>
    public LoginCoreResponse(string token)
    {
        Token = token;
    }

    public LoginCoreResponse(LoginO9ResponseModel responseModel)
    {
        Id = responseModel.usrid;
        UserCode = responseModel.usrcd;
        UserName = responseModel.usrname;
        LoginName = responseModel.lgname;
        Token = responseModel.token;
        WorkingDate = DateTime.ParseExact(responseModel.busdate, "dd/MM/yyyy", null);
        StringWorkingDate = responseModel.busdate;
        BranchId = responseModel.branchid;
        BranchCode = responseModel.branchcd;
        BranchName = responseModel.brname;
        DepartmentId = responseModel.deprtid;
        DepartmentCode = responseModel.deprtcd;
        BankStatus = responseModel.bankactive;
        ResetPassword = responseModel.pwdreset.HasValue();
        ExpireTime = GlobalVariable.ExpireTime();
        BranchStatus = responseModel.status;
        RefreshToken = responseModel.RefreshToken;
    }

    public LoginCoreResponse(UserSessions userSessions)
    {
        Id = userSessions.Usrid;
        UserCode = userSessions.UserCode;
        UserName = userSessions.Usrname;
        LoginName = userSessions.LoginName;
        WorkingDate = userSessions.Txdt;
        StringWorkingDate = userSessions.Txdt.ToString("dd/MM/yyyy");
        BranchId = userSessions.UsrBranchid;
        BranchCode = userSessions.UsrBranch;
        BankStatus = "Y";
        ExpireTime = userSessions.Exptime;
    }

    /// <summary>
    /// Id
    /// </summary>
    /// /// <value></value>
    [JsonProperty("id")]
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// UserCode
    /// </summary>
    /// <value></value>
    [JsonProperty("usercode")]
    [JsonPropertyName("user_code")]
    public string UserCode { get; set; }

    /// <summary>
    /// UserName
    /// </summary>
    /// <value></value>
    [JsonProperty("username")]
    [JsonPropertyName("user_name")]
    public string UserName { get; set; }

    /// <summary>
    /// LoginName
    /// </summary>
    /// <value></value>
    [JsonProperty("loginname")]
    [JsonPropertyName("login_name")]
    public string LoginName { get; set; }

    /// <summary>
    /// Token
    /// </summary>
    /// <value></value>
    [JsonProperty("token", Required = Required.Always)]
    [JsonPropertyName("token")]
    public string Token { get; set; }

    [JsonProperty("refresh_token")]
    [JsonPropertyName("refresh_token")]
    public string RefreshToken { get; set; }

    /// <summary>
    /// WorkingDate
    /// </summary>
    /// <value></value>
    [JsonProperty("workingdate")]
    [JsonPropertyName("working_date")]
    public DateTime WorkingDate { get; set; }

    [JsonProperty("stringworkingdate")]
    public string StringWorkingDate { get; set; }

    /// <summary>
    /// BranchId
    /// </summary>
    /// <value></value>
    [JsonProperty("branchid")]
    [JsonPropertyName("branch_id")]
    public int BranchId { get; set; }

    /// <summary>
    /// BranchCode
    /// </summary>
    /// <value></value>
    [JsonProperty("branchcode")]
    [JsonPropertyName("branch_code")]
    public string BranchCode { get; set; }

    /// <summary>
    /// BranchName
    /// </summary>
    /// <value></value>
    [JsonProperty("branchname")]
    [JsonPropertyName("branch_name")]
    public string BranchName { get; set; }

    /// <summary>
    /// DepartmentId
    /// </summary>
    /// <value></value>
    [JsonProperty("departmentid")]
    [JsonPropertyName("department_id")]
    public int DepartmentId { get; set; }

    /// <summary>
    /// DepartmentCode
    /// </summary>
    /// <value></value>
    [JsonProperty("departmentcode")]
    [JsonPropertyName("department_code")]
    public string DepartmentCode { get; set; }

    /// <summary>
    /// Pst
    /// </summary>
    /// <value></value>
    [Newtonsoft.Json.JsonIgnore]
    public string Pst { get; set; }

    /// <summary>
    /// Region
    /// </summary>
    /// <value></value>
    [JsonProperty("region")]
    [JsonPropertyName("region")]
    public string Region { get; set; }

    /// <summary>
    /// BranchStatus
    /// </summary>
    /// <value></value>
    [JsonProperty("branchstatus")]
    [JsonPropertyName("branch_status")]
    public string BranchStatus { get; set; }

    /// <summary>
    /// BankStatus
    /// </summary>
    /// <value></value>
    [JsonProperty("bankstatus")]
    [JsonPropertyName("bank_status")]
    public string BankStatus { get; set; }

    /// <summary>
    /// ResetPassword
    /// </summary>
    /// <value></value>
    [JsonProperty("resetpassword")]
    [JsonPropertyName("reset_password")]
    public bool ResetPassword { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    [JsonProperty("expiretime")]
    [JsonPropertyName("expire_time")]
    public DateTimeOffset ExpireTime { get; set; }
}

public class LoginInfoModel : BaseO24OpenAPIModel
{
    /// <summary>
    /// GetUserAccountByIdResponse constructor
    /// </summary>
    public LoginInfoModel() { }

    /// <summary>
    /// Id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Position
    /// </summary>
    [JsonProperty("0")]
    public object Position { get; set; }

    /// <summary>
    /// BranchCode
    /// </summary>
    [JsonProperty("1")]
    public string BranchCode { get; set; }

    /// <summary>
    /// BranchName
    /// </summary>
    [JsonProperty("5")]
    public string BranchName { get; set; }

    /// <summary>
    /// Position
    /// </summary>
    [JsonProperty("2")]
    public string LoginName { get; set; }

    /// <summary>
    /// UserCode
    /// </summary>
    [JsonProperty("3")]
    public string UserCode { get; set; }

    /// <summary>
    /// UserName
    /// </summary>
    [JsonProperty("4")]
    public string UserName { get; set; }

    /// <summary>
    /// ResetPassword
    /// </summary>
    /// <value></value>
    public bool ResetPassword { get; set; }

    /// <summary>
    /// BranchStatus
    /// </summary>
    /// <value></value>
    [JsonProperty("7")]
    public string BranchStatus { get; set; }

    /// <summary>
    /// BankStatus
    /// </summary>
    /// <value></value>
    public string BankStatus { get; set; } // add for cms

    /// <summary>
    /// Region
    /// </summary>
    /// <value></value>
    public string Region { get; set; } // add for cms

    /// <summary>
    /// WorkingDate
    /// </summary>
    /// <value></value>
    public DateTime WorkingDate { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public decimal TimeZone { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    [JsonProperty("6")]
    public string UserAccountStatus { get; set; }

    /// <summary>
    /// Is Cash Account
    /// </summary>
    public bool IsCashier
    {
        get
        {
            try
            {
                O9MultiPosition positionAccount =
                    System.Text.Json.JsonSerializer.Deserialize<O9MultiPosition>(
                        Position.ToSerialize()
                    );
                return positionAccount.Cashier == 1;
            }
            catch
            {
                return false;
            }
        }
    }

    /// <summary>
    /// Is Cash Account
    /// </summary>
    public bool IsChiefCashier
    {
        get
        {
            try
            {
                O9MultiPosition positionAccount =
                    System.Text.Json.JsonSerializer.Deserialize<O9MultiPosition>(
                        Position.ToSerialize()
                    );
                return positionAccount.ChiefCashier == 1;
            }
            catch
            {
                return false;
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public string Avatar { get; set; }
}

/// <summary>
///
/// </summary>
public partial class O9MultiPosition : BaseO24OpenAPIModel
{
    /// <summary>
    /// Cashier
    /// </summary>
    [JsonProperty("C")]
    public int? Cashier { get; set; } = 0;

    /// <summary>
    /// Officer
    /// </summary>
    [JsonProperty("O")]
    public int? Officer { get; set; } = 0;

    /// <summary>
    /// ChiefCashier
    /// </summary>
    [JsonProperty("I")]
    public int? ChiefCashier { get; set; } = 0;

    /// <summary>
    /// OperationStaff
    /// </summary>
    [JsonProperty("S")]
    public int? OperationStaff { get; set; } = 0;

    /// <summary>
    /// Dealer
    /// </summary>
    [JsonProperty("D")]
    public int? Dealer { get; set; } = 0;
}
