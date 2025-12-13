namespace O24OpenAPI.Web.CMS.Models.Digital;

public class UserFavoriteFeatureModel : BaseTransactionModel
{
    public UserFavoriteFeatureModel() { }

    public string UserCode { get; set; }
    public int FavoriteFeatureID { get; set; }
    public bool Favorite { get; set; } = false;
    public new string Description { get; set; }
}
