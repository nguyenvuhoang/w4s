namespace O24OpenAPI.GrpcContracts.Exceptions;

public class GrpcBusinessException(string message, string errorCode, string? detail = null)
    : Exception(
        $"{message} | Code: {errorCode}{(string.IsNullOrWhiteSpace(detail) ? "" : $" | Detail: {detail}")}"
    )
{
    public string ErrorCode { get; } = errorCode;
}
