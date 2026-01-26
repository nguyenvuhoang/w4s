using Newtonsoft.Json;

namespace O24OpenAPI.CMS.API.Application.Models.Request;

public class AuthenJWTModel : BaseTransactionModel
{
    /// <summary>
    ///
    /// </summary>
    [JsonProperty("username")]
    public string UserName { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("password")]
    public string PassWord { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("usercode")]
    public string UserCode { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("loginname")]
    public string LoginName { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("branchcode")]
    public string BranchCode { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("deviceid")]
    public string DeviceId { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("istemporarytoken")]
    public bool IsTemporaryToken { get; set; } = false;

    public bool IsEncrypted { get; set; } = false;
    public bool IsDigital { get; set; } = false;
}

public class ChangePasswordModel : BaseTransactionModel
{
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
}
