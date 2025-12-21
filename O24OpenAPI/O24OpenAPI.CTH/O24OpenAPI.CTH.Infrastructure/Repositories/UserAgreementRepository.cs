using LinKit.Core.Abstractions;
using LinqToDB;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Core.Events;
using O24OpenAPI.Data;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;

namespace O24OpenAPI.CTH.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class UserAgreementRepository(
    IEventPublisher eventPublisher,
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
)
    : EntityRepository<UserAgreement>(eventPublisher, dataProvider, staticCacheManager),
        IUserAgreementRepository
{

public async Task<UserAgreement?> GetActiveByTransactionCodeAsync(string transactionCode)
{
    return await Table
        .Where(s => s.IsActive && s.TransactionCode == transactionCode)
        .FirstOrDefaultAsync();
}

public async Task<bool> IsAgreementForCommandAsync(string commandId)
{
    var isAgreement = await Table.Where(ua => ua.TransactionCode.Contains(commandId) && ua.IsActive).FirstOrDefaultAsync();
    return isAgreement != null;
}
}
