using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace O24OpenAPI.Web.CMS.Models;

public class UserInRoleModel : BaseTransactionModel
{
    /// <summary>
    /// roleid
    /// </summary>
    [JsonPropertyName("role_id")]
    [JsonProperty("role_id")]
    public int RoleId { get; set; }

    /// <summary>
    /// usrid
    /// </summary>
    [JsonPropertyName("user_code")]
    [JsonProperty("user_code")]
    public string UserCode { get; set; }
}

/// <summary>
/// UserInRoleSearchResponseModel
/// </summary>
public partial class UserInRoleSearchResponseModel : BaseO24OpenAPIModel
{
    /// <summary>
    /// country id
    /// </summary>
    public int? Id { get; set; }

    /// <summary>
    /// roleid
    /// </summary>
    public int? RoleId { get; set; }

    /// <summary>
    /// usrid
    /// </summary>
    public string UserCode { get; set; }

    /// <summary>
    /// RoleName
    /// </summary>
    public string RoleName { get; set; }

    /// <summary>
    /// UserCode
    /// </summary>
    public string UsrCode { get; set; }

    /// <summary>
    /// UserName
    /// </summary>
    public string UsrName { get; set; }

    /// <summary>
    /// Login name
    /// </summary>
    public string LoginName { get; set; }
}

public partial class UserInRoleSearchModel : BaseTransactionModel
{
    /// <summary>
    /// roleid
    /// </summary>
    public int? RoleId { get; set; }

    /// <summary>
    /// usrid
    /// </summary>
    public string UserCode { get; set; }

    /// <summary>
    /// PageIndex
    /// </summary>
    ///
    public int PageIndex { get; set; } = 0;

    /// <summary>
    /// PageSize
    /// </summary>
    public int PageSize { get; set; } = int.MaxValue;
}
