using System.Text.Json.Serialization;

namespace O24OpenAPI.APIContracts.Models.CBG;

public class CBGatewayResponseModel<T>
{
    [JsonPropertyName("error_code")]
    public int ErrorCode { get; set; }

    [JsonPropertyName("error_description")]
    public string? ErrorDescription { get; set; }

    [JsonPropertyName("error_name")]
    public string? ErrorName { get; set; }

    [JsonPropertyName("error_source")]
    public string? ErrorSource { get; set; }

    [JsonPropertyName("execution_id")]
    public string? ExectionId { get; set; }

    [JsonPropertyName("result")]
    public T? Result { get; set; }

    [JsonPropertyName("txbody")]
    public T? TXBody { get; set; }

    [JsonPropertyName("dataset")]
    public T? DataSet { get; set; }
}
