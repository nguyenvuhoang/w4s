using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;

/// <summary>
/// The user session class
/// </summary>
/// <seealso cref="BaseEntity"/>
public partial class Authenticate : BaseEntity
{
    public string Token { get; set; }

    public string RefreshToken { get; set; }

    public DateTime ExpiredIn { get; set; }

    public long ExpiredDuration { get; set; }

    public bool IsFirstLogin { get; set; } = true;
}
