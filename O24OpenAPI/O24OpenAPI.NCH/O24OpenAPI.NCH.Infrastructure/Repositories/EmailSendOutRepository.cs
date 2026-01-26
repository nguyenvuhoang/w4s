using LinKit.Core.Abstractions;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Data;
using O24OpenAPI.NCH.Domain.AggregatesModel.MailAggregate;

namespace O24OpenAPI.NCH.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class EmailSendOutRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
) : EntityRepository<EmailSendOut>(dataProvider, staticCacheManager), IEmailSendOutRepository
{
    public Task<IReadOnlyList<EmailSendOut>> GetPendingAsync(int take) =>
        throw new NotImplementedException();

    public virtual async Task<EmailSendOut> Create(EmailSendOut mailSendOut)
    {
        await InsertAsync(mailSendOut);
        return mailSendOut;
    }
}
