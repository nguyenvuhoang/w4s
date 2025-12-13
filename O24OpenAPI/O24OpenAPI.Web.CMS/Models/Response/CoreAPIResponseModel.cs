namespace O24OpenAPI.Web.CMS.Models.Response;

public class CoreAPIResponseModel : BaseO24OpenAPIModel
{
    public string WorkflowId { get; set; }
    public Dictionary<string, object> Data { get; set; } = [];
    public ErrorInfoModel Error { get; set; } = new ErrorInfoModel();

}
