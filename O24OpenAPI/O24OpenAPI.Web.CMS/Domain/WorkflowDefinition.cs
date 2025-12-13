using O24OpenAPI.Web.CMS.Constant;

namespace O24OpenAPI.Web.CMS.Domain;

public class WorkflowDefinition : BaseEntity
{
    public string WFId { get; set; }
    public string WFName { get; set; }
    public string WFDescription { get; set; }
    public string AppCode { get; set; }
    public bool Status { get; set; } = WorkflowStatus.Active;
    public long TimeOut { get; set; } = 60000;
    public bool IsReverse { get; set; } = false;
    public bool LogUserAction { get; set; } = false;
    public DateTime? CreatedOnUtc { get; set; }
    public DateTime? UpdatedOnUtc { get; set; }
}
