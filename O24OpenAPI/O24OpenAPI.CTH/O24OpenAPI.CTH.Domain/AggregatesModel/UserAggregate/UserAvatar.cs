using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;

public class UserAvatar : BaseEntity
{
    public string UserCode { get; set; }
    public string ImageUrl { get; set; }
    public DateTime DateInsert { get; set; }
}
