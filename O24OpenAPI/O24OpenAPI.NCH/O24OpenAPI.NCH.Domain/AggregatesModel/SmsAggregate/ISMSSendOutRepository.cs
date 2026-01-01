using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.NCH.Domain.AggregatesModel.SmsAggregate;

public interface ISMSSendOutRepository : IRepository<SMSSendOut>
{
    Task<IReadOnlyList<SMSSendOut>> GetPendingAsync(int take);
    SMSSendOut CreateSendOut(
        string providerName,
        string phoneNumber,
        string messageContent,
        string soapLogXml,
        string transactionId,
        string endtoend,
        int retryCount,
        bool isResend
    );
}
