namespace O24OpenAPI.Web.CMS.Models.OpenAPI;

public class CreateOpenAPIResponseModel : BaseO24OpenAPIModel
{
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public string DisplayName { get; set; }
    public string Environment { get; set; }
    public string Scopes { get; set; }
    public string BICCode { get; set; }
}
