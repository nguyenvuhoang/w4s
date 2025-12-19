using System.ComponentModel.DataAnnotations.Schema;

namespace O24OpenAPI.CMS.Domain;

[Table("D_FAVORITEFEATURESUBITEM")]
public class FavoriteFeatureSubItem : BaseEntity
{
    public string SubItemCode { get; set; }
    public string Icon { get; set; }
    public string Label { get; set; }
    public string Description { get; set; }

    public string Url { get; set; }

    /// <summary>
    /// Last update date
    /// </summary>
    public DateTime? CreatedOnUtc { get; set; }

    /// <summary>
    /// Last update date
    /// </summary>
    public DateTime? UpdatedOnUtc { get; set; }
}
