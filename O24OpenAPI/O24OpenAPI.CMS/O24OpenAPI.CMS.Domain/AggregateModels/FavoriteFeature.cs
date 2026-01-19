using System.ComponentModel.DataAnnotations.Schema;

namespace O24OpenAPI.CMS.Domain.AggregateModels;

[Table("D_FAVORITEFEATURE")]
public partial class FavoriteFeature : BaseEntity
{
    public string? FavoriteFeatureCode { get; set; }

    public string? FavoriteFeatureName { get; set; }

    public string? SubItemCode { get; set; }

    public string? CommandId { get; set; }

    public string? Description { get; set; }
}
