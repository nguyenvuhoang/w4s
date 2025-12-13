using Newtonsoft.Json;

namespace O24OpenAPI.Web.CMS.Models.Digital;

/// <summary>

/// The bankmodel class
/// </summary>
/// <seealso cref="BaseEntity"/>

public class GetRewardByIDModel : BaseTransactionModel
{
    /// <summary>
    /// Gets or sets the value of the name
    /// </summary>
    public int Id { get; set; }
}

public class RewardModel : BaseTransactionModel
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("giftname")]
    public string GiftName { get; set; }

    [JsonProperty("localgiftname")]
    public string LocalGiftName { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("branchid")]
    public string BranchID { get; set; }

    [JsonProperty("price")]
    public decimal Price { get; set; }

    [JsonProperty("requiredpoints")]
    public string RequiredPoints { get; set; }

    [JsonProperty("limitgiftperredeem")]
    public int LimitGiftPerRedeem { get; set; }

    [JsonProperty("quantitygift")]
    public int QuantityGift { get; set; }

    [JsonProperty("status")]
    public string Status { get; set; }

    [JsonProperty("imagepath")]
    public string ImagePath { get; set; }

    [JsonProperty("createdby")]
    public string CreatedBy { get; set; }

    [JsonProperty("createdat")]
    public DateTime? CreatedAt { get; set; }

    [JsonProperty("modifiedby")]
    public string ModifiedBy { get; set; }

    [JsonProperty("modifiedat")]
    public DateTime? ModifiedAt { get; set; }

    [JsonProperty("expireat")]
    public DateTime? ExpireAt { get; set; }

    [JsonProperty("reason")]
    public string Reason { get; set; }

}
public class SearchRewardModel : BaseTransactionModel
{
    /// <summary>
    /// Gets or sets the value of the name
    /// </summary>
    [JsonProperty("giftname")]
    public string GiftName { get; set; }
    [JsonProperty("status")]
    public string Status { get; set; }
}
