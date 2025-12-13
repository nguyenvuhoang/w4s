using Newtonsoft.Json;

namespace O24OpenAPI.Web.CMS.Models.Digital;

/// <summary>

/// The bankmodel class
/// </summary>
/// <seealso cref="BaseEntity"/>

public class GetUserRewardByIDModel : BaseTransactionModel
{
    /// <summary>
    /// Gets or sets the value of the name
    /// </summary>
    public int Id { get; set; }
}

public class UserRewardModel : BaseTransactionModel
{
    [JsonProperty("id")]
    public int Id { get; set; }
    [JsonProperty("usercode")]
    public string UserCode { get; set; }
    [JsonProperty("totalpoint")]
    public decimal TotalPoint { get; set; }
    [JsonProperty("usedpoint")]
    public decimal UsedPoint { get; set; }
    [JsonProperty("giftid")]
    public int GiftId { get; set; }
    [JsonProperty("eventid")]
    public int EventId { get; set; }
    [JsonProperty("issuedate")]
    public DateTime? IssueDate { get; set; }
    [JsonProperty("expirydate")]
    public DateTime? ExpiryDate { get; set; }
    [JsonProperty("descriptions")]
    public string Descriptions { get; set; }

}
public class SearchUserRewardModel : BaseTransactionModel
{
    /// <summary>
    /// Gets or sets the value of the name
    /// </summary>
     [JsonProperty("usercode")]
    public string UserCode { get; set; }
    [JsonProperty("giftid")]
    public int GiftId { get; set; }

}
