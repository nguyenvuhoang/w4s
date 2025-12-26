namespace O24OpenAPI.Web.CMS.Domain;

public partial class D_REWARDS : BaseEntity
{
    public string GiftName { get; set; }

    public string LocalGiftName { get; set; }

    public string Type { get; set; }

    public string BranchID { get; set; }

    public decimal Price { get; set; }

    public decimal RequiredPoints { get; set; }

    public int LimitGiftPerRedeem { get; set; }

    public int QuantityGift { get; set; }

    public string Status { get; set; }

    public string ImagePath { get; set; }

    public string CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public DateTime? ExpireAt { get; set; }

    public string Reason { get; set; }
}
