using System.ComponentModel.DataAnnotations.Schema;

namespace O24OpenAPI.CMS.Domain.AggregateModels;

[Table("D_FAVORITEFEATURESUBITEM")]
public partial class FavoriteFeatureSubItem : BaseEntity
{
    public string? SubItemCode { get; set; }
    public string? Icon { get; set; }
    public string? Label { get; set; }
    public string? Description { get; set; }

    public string? Url { get; set; }
}
