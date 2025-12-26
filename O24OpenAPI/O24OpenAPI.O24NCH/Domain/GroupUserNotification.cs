using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.O24NCH.Domain;

public partial class GroupUserNotification : BaseEntity
{
    public int GroupID { get; set; }
    public string UserCode { get; set; }
}
