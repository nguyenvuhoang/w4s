using Newtonsoft.Json;

namespace O24OpenAPI.Web.CMS.Models.Digital;

/// <summary>

/// The bankmodel class
/// </summary>
/// <seealso cref="BaseEntity"/>

public class GetRewardDefineByIDModel : BaseTransactionModel
{
    /// <summary>
    /// Gets or sets the value of the name
    /// </summary>
    public string CurrencyCode { get; set; }
}

public class RewardDefineModel : BaseTransactionModel
{
    [JsonProperty("rewardtypeid")]
    public int RewardTypeID { get; set; }

    [JsonProperty("rewardname")]
    public string RewardName { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("isapprove")]
    public bool isApprove { get; set; }

    [JsonProperty("status")]
    public string Status { get; set; }
}
