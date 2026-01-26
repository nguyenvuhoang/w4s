namespace O24OpenAPI.CMS.API.Application.Models.Response;

public class ErrorInfoModel : BaseO24OpenAPIModel
{
    public ErrorInfoModel() { }

    public ErrorInfoModel(
        string type,
        string typeError,
        string info,
        string code,
        string key,
        string nextAction = "",
        string executeId = ""
    )
    {
        Type = type;
        TypeError = typeError;
        Info = info;
        Code = code;
        Key = key;
        NextAction = nextAction;
        ExecuteId = executeId;
    }

    public string Key { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Info { get; set; } = string.Empty;
    public string TypeError { get; set; } = string.Empty;
    public string ExecuteId { get; set; } = string.Empty;
    public string NextAction { get; set; } = string.Empty;
}
