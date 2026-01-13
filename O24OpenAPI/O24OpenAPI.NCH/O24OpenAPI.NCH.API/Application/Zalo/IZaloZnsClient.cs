using System.Threading;
using System.Threading.Tasks;

namespace O24OpenAPI.NCH.API.Application.Features.Zalo;

public interface IZaloZnsClient
{
    Task<ZaloSendOtpResult> SendOtpAsync(
        string phoneNumber,
        string otp,
        string trackingId,
        CancellationToken cancellationToken = default);
}

public class ZaloSendOtpResult
{
    public bool Success { get; set; }
    public int StatusCode { get; set; }
    public int? ErrorCode { get; set; }
    public string? ErrorMessage { get; set; }
    public string? ZnsMessageId { get; set; }
    public string? RawResponse { get; set; }
    public string? TrackingId { get; set; }
}
