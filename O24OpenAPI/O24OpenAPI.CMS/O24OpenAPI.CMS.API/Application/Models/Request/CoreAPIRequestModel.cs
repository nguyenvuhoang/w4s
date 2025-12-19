using System.ComponentModel.DataAnnotations;

namespace O24OpenAPI.CMS.API.Application.Models.Request;

public class CoreAPIRequestModel : BaseO24OpenAPIModel
{
    [Required(ErrorMessage = "Missing key [workflowid] on the Body request")]
    public string WorkflowId { get; set; }

    [Required(ErrorMessage = "Missing key [data] on the Body request")]
    public Dictionary<string, object> Data { get; set; } = [];
}
