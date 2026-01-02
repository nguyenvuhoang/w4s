using Newtonsoft.Json;
using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;

[Auditable]
public partial class UserInRole : BaseEntity
{
    /// <summary>
    /// roleid
    /// </summary>
    [JsonProperty("role_id")]
    public int RoleId { get; set; }

    /// <summary>
    /// user_code
    /// </summary>
    [JsonProperty("user_code")]
    public string? UserCode { get; set; }

    /// <summary>
    /// Last update date
    /// </summary>
    [JsonProperty("is_main")]
    public string? IsMain { get; set; }

    /// <summary>
    /// Last update date
    /// </summary>
    public DateTime? UpdatedOnUtc { get; set; }

    /// <summary>
    /// Last update date
    /// </summary>
    public DateTime? CreatedOnUtc { get; set; }
}
