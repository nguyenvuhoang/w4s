namespace O24OpenAPI.Web.Framework.Exceptions;

public class ExceptionWithNextAction : Exception
{
    public ExceptionWithNextAction() { }

    public ExceptionWithNextAction(string errorCode, string message, string nextAction)
        : base(message)
    {
        ErrorCode = errorCode;
        NextAction = nextAction;
    }

    public string NextAction { get; set; }
    public string ErrorCode { get; set; } = "O24OpenAPIException";
}
