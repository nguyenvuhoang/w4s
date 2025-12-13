using Newtonsoft.Json;

namespace O24OpenAPI.Web.CMS.Models.O9;

public class JsonLogin
{
    /// <summary>
    ///
    /// </summary>
    public string L { get; set; }
    /// <summary>
    ///
    /// </summary>
    public string P { get; set; }
    /// <summary>
    ///
    /// </summary>
    public bool A { get; set; }
}

/// <summary>
/// Postion
/// </summary>
public class Position
{
    /// <summary>
    /// cashier
    /// </summary>
    [JsonProperty("C")]
    public int cashier { get; set; }
    /// <summary>
    /// officer
    /// </summary>
    [JsonProperty("O")]
    public int officer { get; set; }
    /// <summary>
    /// chief_cashier
    /// </summary>
    [JsonProperty("I")]
    public int chief_cashier { get; set; }
    /// <summary>
    /// operation_staff
    /// </summary>
    [JsonProperty("S")]
    public int operation_staff { get; set; }
    /// <summary>
    /// dealer
    /// </summary>
    [JsonProperty("D")]
    public int dealer { get; set; }
    /// <summary>
    /// inter_branch_user
    /// </summary>
    [JsonProperty("inter_branch_user")]
    public int inter_branch_user { get; set; }
    /// <summary>
    /// branch_manager
    /// </summary>
    [JsonProperty("branch_manager")]
    public int branch_manager { get; set; }
}
