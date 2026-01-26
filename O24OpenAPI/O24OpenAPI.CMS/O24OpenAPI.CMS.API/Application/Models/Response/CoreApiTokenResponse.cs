namespace O24OpenAPI.CMS.API.Application.Models.Response;

public class CoreApiTokenResponse
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime ExpiredAt { get; set; }
    public long ExpiredDuration { get; set; }
}
