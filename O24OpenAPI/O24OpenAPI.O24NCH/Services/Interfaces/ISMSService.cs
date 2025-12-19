using O24OpenAPI.O24NCH.Models.Request;
using O24OpenAPI.O24NCH.Models.Request.SMSGateway;
using O24OpenAPI.O24NCH.Models.Response;

namespace O24OpenAPI.O24NCH.Services.Interfaces;

public interface ISMSService
{
    /// <summary>
    /// Tạo và lưu mã OTP vào cơ sở dữ liệu, sau đó tạo nội dung tin nhắn để gửi đến khách hàng.
    /// </summary>
    /// <param name="PhoneNumber">Số điện thoại của khách hàng.</param>
    /// <param name="UserCode">ID của khách hàng (tuỳ chọn nếu cần).</param>
    /// <param name="Purpose">Purpose</param>
    /// <returns>TransactionID</returns>
    Task<GenerateOTPResponseModel> GenerateAndSendOTPAsync(GenerateOTPRequestModel model);

    /// <summary>
    /// Verify OTP
    /// </summary>
    /// <param name="PhoneNumber">Số điện thoại của khách hàng.</param>
    /// <param name="UserCode">ID của khách hàng (tuỳ chọn nếu cần).</param>
    /// <param name="Purpose">Purpose</param>
    /// <param name="OTP">Purpose</param>
    /// <returns>Verify OTP</returns>
    Task<bool> VerifyOTPAsync(VeriveryOTPRequestModel model);
    /// <summary>
    /// SendSMS
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<bool> SendSMS(SMSRequestModel model);
    /// <summary>
    /// Retrieve Info Async
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<RetrieveSMSInfoResponseModel> RetrieveInfoAsync(RetrieveSMSInfoRequestModel model);

    /// <summary>
    /// SMS Gateway Send
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<SMSGatewayResponseModel> SMSGatewaySend(SMSGatewayRequestModel model);

    /// <summary>
    /// Generate and Send Content Async
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<GenerateSMSContentResponseModel> GenerateAndSendContentAsync(GenerateSMSContentRequestModel model);
    /// <summary>
    /// Bulk send SMS
    /// </summary>
    /// <param name="requests"></param>
    /// <param name="maxDegreeOfParallelism"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    Task<bool> BulkSendSMS(List<SMSRequestModel> requests, int maxDegreeOfParallelism = 20, CancellationToken ct = default);
}
