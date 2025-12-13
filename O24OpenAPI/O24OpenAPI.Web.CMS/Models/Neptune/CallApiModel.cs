namespace O24OpenAPI.Web.CMS.Models;

public class CallApiModel : BaseTransactionModel
{
    public string ServiceID { get; set; }
    public Dictionary<string, string> Header { get; set; }
    public string Content { get; set; }
    public string WorkflowId { get; set; }
}
