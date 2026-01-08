using O24OpenAPI.Core;

namespace O24OpenAPI.Contracts.Models;

public class BaseResponse<T>
{
    public bool success { get; set; }
    public long time_in_milliseconds { get; set; }
    public string? status { get; set; }
    public string? description { get; set; }
    public string? error_code { get; set; }
    public string? error_message { get; set; }
    public string? execution_id { get; set; }
    public string? error_next_action { get; set; }
    public object? transaction_number { get; set; }
    public object? transaction_date { get; set; }
    public object? value_date { get; set; }
    public T? data { get; set; }

    public BaseResponse() { }

    public BaseResponse(T? _data)
    {
        data = _data;
        success = true;
    }

    public BaseResponse(Exception ex)
    {
        success = false;
        error_message = ex.Message;
    }

    public BaseResponse(O24OpenAPIException ex)
    {
        success = false;
        error_code = ex.ErrorCode;
        error_message = ex.Message;
    }
}
