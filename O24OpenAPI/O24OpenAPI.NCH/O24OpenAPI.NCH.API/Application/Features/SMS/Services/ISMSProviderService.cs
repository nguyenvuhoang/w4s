using O24OpenAPI.Core;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.NCH.API.Application.Models.Request;
using O24OpenAPI.NCH.API.Application.Models.Response;
using O24OpenAPI.NCH.Domain.AggregatesModel.SmsAggregate;

namespace O24OpenAPI.NCH.API.Application.Features.SMS.Services;

public interface ISMSProviderService
{
    /// <summary>
    /// Get SMS Provider by phone number
    /// </summary>
    /// <param name="phoneNumber"></param>
    /// <returns></returns>
    Task<SMSProvider> GetProviderByPhoneNumber(string phoneNumber);

    /// <summary>
    /// Gửi SMS đến khách hàng qua provider đã cấu hình
    /// </summary>
    /// <param name="phoneNumber">Số điện thoại người nhận</param>
    /// <param name="message">Nội dung tin nhắn</param>
    /// <returns>True nếu gửi thành công</returns>
    Task<SendSOAPResponseModel> SendSMSAsync(
        string phoneNumber,
        string message,
        string transactionId,
        string providerName = "UNITEL",
        string endtoend = "",
        string messagetype = "SMS"
    );

    /// <summary>
    /// Xây dựng SOAP request từ template lưu trong DB, thay thế các biến bằng giá trị thực tế.
    /// </summary>
    /// <param name="provider">Đối tượng SMSProvider</param>
    /// <param name="values">Dictionary chứa các giá trị cần chèn vào template</param>
    /// <returns>SOAP request string</returns>
    Task<string> BuildSOAPRequestAsync(SMSProvider provider, Dictionary<string, string> values);

    /// <summary>
    /// Gửi SOAP request tới endpoint và nhận phản hồi
    /// </summary>
    /// <param name="providerId">ID nhà cung cấp</param>
    /// <param name="soapXml">SOAP body đã build</param>
    /// <returns>Chuỗi phản hồi từ provider</returns>
    Task<string> SendSOAPRequestAsync(string providerId, string soapXml);

    /// <summary>
    /// Thực hiện gọi kiểm tra trạng thái từng SMS Provider đã cấu hình (health check),
    /// sau đó cập nhật bảng SMSProviderStatus.
    /// </summary>
    /// <returns>Task hoàn tất đồng bộ</returns>
    Task SyncSMSProviderStatusAsync();

    /// <summary>
    /// Send bulk SMS messages asynchronously with controlled parallelism.
    /// </summary>
    /// <param name="messages"></param>
    /// <param name="maxDegreeOfParallelism"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<SendSOAPResponseModel>> SendBulkSMSAsync(
        List<(
            string ProviderName,
            string PhoneNumber,
            string Message,
            string TransactionId,
            string EndToEnd
        )> messages,
        int maxDegreeOfParallelism = 20,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Search SMS Providers based on the given search model.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<IPagedList<SMSProvider>> Search(SimpleSearchModel model);

    /// <summary>
    /// Update an existing SMS provider with the given model.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<UpdateSMSProviderResponseModel> Update(SMSProviderUpdateModel model);

    /// <summary>
    /// Create Model
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<bool> CreateAsync(SMSProviderCreateModel model);
}
