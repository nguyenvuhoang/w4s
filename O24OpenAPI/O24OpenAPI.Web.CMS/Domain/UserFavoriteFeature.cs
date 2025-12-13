using System.ComponentModel.DataAnnotations.Schema;

namespace O24OpenAPI.Web.CMS.Domain;

[Table("D_USER_FAVORITEFEATURE")]
public class UserFavoriteFeature : BaseEntity
{
    public string UserCode { get; set; }
    public int FavoriteFeatureID { get; set; }
    public bool Favorite { get; set; } = false;
    public string Description { get; set; }

    /// <summary>
    /// Last update date
    /// </summary>
    public DateTime? CreatedOnUtc { get; set; }

    /// <summary>
    /// Last update date
    /// </summary>
    public DateTime? UpdatedOnUtc { get; set; }
}
