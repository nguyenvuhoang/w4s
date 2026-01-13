namespace O24OpenAPI.NCH.API.Application.Features.Zalo;

public class ZaloZnsOptions
{
    public string BaseUrl { get; set; } = string.Empty;
    public string SendOtpPath { get; set; } = "/Zalo/SendZNS";
    public string OaId { get; set; } = string.Empty;
    public string AccessToken { get; set; } = string.Empty;
    public int OtpTemplateId { get; set; }
    public bool DevelopmentMode { get; set; } = true;
}
