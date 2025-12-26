namespace O24OpenAPI.Web.CMS.Domain;

public partial class D_USER_REWARD : BaseEntity
{
    public string UserCode { get; set; }
    public decimal TotalPoint { get; set; }
    public decimal UsedPoint { get; set; }
    public int GiftId { get; set; }
    public int EventId { get; set; }
    public DateTime? IssueDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public string Descriptions { get; set; }
}
