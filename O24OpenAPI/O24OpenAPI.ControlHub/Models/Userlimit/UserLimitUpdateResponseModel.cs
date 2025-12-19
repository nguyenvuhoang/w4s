using System.Text.Json.Serialization;
using O24OpenAPI.Core.Abstractions;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.ControlHub.Models.Userlimit;

public class UserLimitUpdateResponseModel : BaseO24OpenAPIModel
{
    /// <summary>
    /// user limit update response model constructor
    /// </summary>
    public UserLimitUpdateResponseModel() { }

    /// <summary>
    /// roleid
    /// </summary>
    [JsonPropertyName("role_id")]
    public int RoleId { get; set; }

    /// <summary>
    /// cmdid
    /// </summary>
    [JsonPropertyName("command_id")]
    public string CommandId { get; set; }

    /// <summary>
    /// ccrid
    /// </summary>
    [JsonPropertyName("currency_code")]
    public string CurrencyCode { get; set; }

    /// <summary>
    /// ulimit
    /// </summary>
    [JsonPropertyName("u_limit")]
    public decimal? ULimit { get; set; }

    /// <summary>
    /// limittype
    /// </summary>
    [JsonPropertyName("limit_type")]
    public string LimitType { get; set; }
}
