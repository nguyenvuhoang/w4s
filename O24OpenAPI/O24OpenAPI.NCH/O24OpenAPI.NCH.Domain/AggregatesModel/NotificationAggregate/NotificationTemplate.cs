using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.NCH.Domain.AggregatesModel.NotificationAggregate;

public partial class NotificationTemplate : BaseEntity
{
    public string TemplateID { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Body { get; set; } = null!;
    public string? Data { get; set; }
    public bool? IsShowButton { get; set; }
    public string? LearnApiSending { get; set; }
}
