using Newtonsoft.Json;

namespace O24OpenAPI.Web.CMS.Models;

public class WorkflowDefinitionSearchModel : SimpleSearchModel
{
    public string WFId { get; set; }
    public string WFName { get; set; }
    public string WFDescription { get; set; }
}

public class WorkflowDefinitionSearchResponseModel : BaseO24OpenAPIModel
{
    [JsonProperty("wfid")]
    public string WFId { get; set; }

    [JsonProperty("wfname")]
    public string WFName { get; set; }

    [JsonProperty("wfdescription")]
    public string WFDescription { get; set; }

    [JsonProperty("status")]
    public bool Status { get; set; }

    [JsonProperty("timeout")]
    public long TimeOut { get; set; }
}
