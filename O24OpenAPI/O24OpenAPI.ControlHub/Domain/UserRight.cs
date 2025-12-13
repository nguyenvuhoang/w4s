using Newtonsoft.Json;
using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.ControlHub.Domain;

public partial class UserRight : BaseEntity
{
    /// <summary>
    /// User right domain constructor
    /// </summary>
    public UserRight() { }

    /// <summary>
    /// roleid
    /// </summary>
    [JsonProperty("roleid")]
    public int RoleId { get; set; }

    /// <summary>
    /// cmdid
    /// </summary>
    [JsonProperty("commandid")]
    public string CommandId { get; set; }

    /// <summary>
    /// cmdiddt
    /// </summary>
    [JsonProperty("commandiddetail")]
    public string CommandIdDetail { get; set; }

    /// <summary>
    /// invoke
    /// </summary>
    [JsonProperty("invoke")]
    public int Invoke { get; set; }

    /// <summary>
    /// approve
    /// </summary>
    [JsonProperty("approve")]
    public int Approve { get; set; }

    /// <summary>
    /// Last update date
    /// </summary>
    public DateTime? UpdatedOnUtc { get; set; }

    /// <summary>
    /// Last update date
    /// </summary>
    public DateTime? CreatedOnUtc { get; set; }
}
