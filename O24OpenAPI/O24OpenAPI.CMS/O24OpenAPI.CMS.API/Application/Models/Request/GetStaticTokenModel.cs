using System.ComponentModel.DataAnnotations;

namespace O24OpenAPI.CMS.API.Application.Models.Request;

public class GetStaticTokenModel
{
    /// <summary>
    /// ClientId
    /// </summary>
    [Required(ErrorMessage = "ClientId is required")]
    public string ClientId { get; set; }

    /// <summary>
    /// ClientSecret
    /// </summary>
    [Required(ErrorMessage = "ClientSecret is required")]
    public string ClientSecret { get; set; }

    /// <summary>
    /// GET_INFO, FO, REVERSAL
    /// Optional – The system will check in the DB to see if the client is granted this permission.
    /// </summary>
    public List<StaticTokenScope> Scopes { get; set; }

    /// <summary>
    /// (Optional) Require token expiration (if not transmitted will use default from DB or system configuration)
    /// </summary>
    public DateTime? ExpiredOnUtc { get; set; }
}

public enum StaticTokenScope
{
    GET_INFO,
    FO,
    REVERSAL,
    FUNC,
}
