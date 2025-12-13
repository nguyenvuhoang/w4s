using Newtonsoft.Json;

namespace O24OpenAPI.Web.CMS.Models;

public class ExportAppTemplateModel : BaseO24OpenAPIModel
{
    [JsonProperty("templateId")]
    public string TemplateId { get; set; }
}

public class ExportWorkflowStepModel : BaseO24OpenAPIModel
{
    [JsonProperty("wfId")]
    public string WFId { get; set; } = "";

    [JsonProperty("stepCode")]
    public string StepCode { get; set; } = "";
}

public class ExportWorkflowDefModel : BaseO24OpenAPIModel
{
    [JsonProperty("wfId")]
    public string WFId { get; set; } = "";
}

public class ExportFieldDefModel : BaseO24OpenAPIModel
{
    [JsonProperty("templateId")]
    public string TemplateId { get; set; } = "";

    [JsonProperty("fieldName")]
    public string FieldName { get; set; } = "";
}

public class ExportUserCommandModel : BaseO24OpenAPIModel
{
    [JsonProperty("commandId")]
    public string CommandId { get; set; }

    [JsonProperty("parentId")]
    public string ParentId { get; set; }

}

public class ExportCodeListModel : BaseO24OpenAPIModel
{
    [JsonProperty("codeGroup")]
    public string CodeGroup { get; set; } = "";

    [JsonProperty("codeName")]
    public string CodeName { get; set; } = "";

    [JsonProperty("codeID")]
    public string CodeID { get; set; } = "";
}
