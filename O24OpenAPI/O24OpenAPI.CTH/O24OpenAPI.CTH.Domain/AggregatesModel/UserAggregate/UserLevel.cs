using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;

public partial class UserLevel : BaseEntity
{
    public string LevelCode { get; set; }
    public string LevelName { get; set; }
    public string Description { get; set; }
}
