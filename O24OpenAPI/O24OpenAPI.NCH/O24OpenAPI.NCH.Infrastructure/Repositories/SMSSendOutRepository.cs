using O24OpenAPI.Core.Caching;
using O24OpenAPI.Data;
using O24OpenAPI.NCH.Domain.AggregatesModel.SmsAggregate;
using O24OpenAPI.NCH.Domain.Constants;

namespace O24OpenAPI.NCH.Infrastructure.Repositories;

public class SMSSendOutRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
) : EntityRepository<SMSSendOut>(dataProvider, staticCacheManager), ISMSSendOutRepository
{
    public Task<IReadOnlyList<SMSSendOut>> GetPendingAsync(int take) =>
        throw new NotImplementedException();

    public static SMSSendOut CreateSendOut(
        string providerName,
        string phoneNumber,
        string messageContent,
        string soapLogXml,
        string transactionId,
        string endtoend,
        int retryCount,
        bool isResend
    )
    {
        return new SMSSendOut
        {
            SMSProviderId = providerName,
            PhoneNumber = phoneNumber,
            MessageContent = messageContent,
            SentAt = DateTime.UtcNow,
            Status = SMSSendOutStatus.PENDING,
            OtpRequestId = transactionId,
            RetryCount = retryCount,
            IsResend = isResend,
            RequestMessage = soapLogXml,
            EndToEnd = endtoend,
        };
    }
}
