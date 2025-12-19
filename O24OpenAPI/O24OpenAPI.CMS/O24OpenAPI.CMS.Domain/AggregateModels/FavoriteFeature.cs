using System.ComponentModel.DataAnnotations.Schema;

namespace O24OpenAPI.CMS.Domain;

[Table("D_FAVORITEFEATURE")]
public class FavoriteFeature : BaseEntity
{
    public string FavoriteFeatureCode { get; set; }

    public string FavoriteFeatureName { get; set; }

    public string SubItemCode { get; set; }

    public string CommandId { get; set; }

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
