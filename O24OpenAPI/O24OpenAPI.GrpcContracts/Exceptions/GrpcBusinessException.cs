namespace O24OpenAPI.GrpcContracts.Exceptions;

public class GrpcBusinessException : Exception
{
    public string ErrorCode { get; }

    public GrpcBusinessException(string message, string errorCode, string? detail = null)
        : base($"{message} | Code: {errorCode}{(string.IsNullOrWhiteSpace(detail) ? "" : $" | Detail: {detail}")}")
    {
        ErrorCode = errorCode;
    }
}
