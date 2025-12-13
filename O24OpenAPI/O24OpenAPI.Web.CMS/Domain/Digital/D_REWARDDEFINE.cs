namespace O24OpenAPI.Web.CMS.Domain;

public class D_REWARDDEFINE : BaseEntity
{
    public int RewardTypeID { get; set; }

    public string RewardName { get; set; }

    public string Description { get; set; }

    public string Type { get; set; }

    public bool isApprove { get; set; }

    public string Status { get; set; }


}
