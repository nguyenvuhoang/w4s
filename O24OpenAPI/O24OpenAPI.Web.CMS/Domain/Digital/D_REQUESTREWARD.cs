namespace O24OpenAPI.Web.CMS.Domain;

public class D_REQUESTREWARD : BaseEntity
{
    public decimal IPCTRANSID { get; set; }

    public string UserCode { get; set; }

    public decimal Amount { get; set; }

    public string QRCode { get; set; }

    public int GiftID { get; set; }
    public string GiftName { get; set; }

    public int Quantity { get; set; }

    public string BranchID { get; set; }

    public string CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public string  Status { get; set; }

    public string Reason { get; set; }



}
