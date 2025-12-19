using System.ComponentModel.DataAnnotations;

namespace O24OpenAPI.CMS.API.Application.Models.Request;

public class RefreshStaticTokenModel
{
    [Required(ErrorMessage = "ClientId is required")]
    public string ClientId { get; set; }

    [Required(ErrorMessage = "RefreshToken is required")]
    public string RefreshToken { get; set; }
}
