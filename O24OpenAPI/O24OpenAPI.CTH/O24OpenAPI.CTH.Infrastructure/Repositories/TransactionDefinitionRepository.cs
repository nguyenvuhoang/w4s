using LinKit.Core.Abstractions;
using O24OpenAPI.Core;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.CTH.Domain.AggregatesModel.TransactionAggregate;
using O24OpenAPI.Data;

namespace O24OpenAPI.CTH.Infrastructure.Repositories;

using O24OpenAPI.Core.Extensions;
using O24OpenAPI.Data.System.Linq;

[RegisterService(Lifetime.Scoped)]
public class TransactionDefinitionRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
)
    : EntityRepository<TransactionDefinition>(dataProvider, staticCacheManager),
        ITransactionDefinitionRepository
{
    public async Task<List<TransactionDefinition>> GetAllAsync()
    {
        return await Table.ToListAsync();
    }

    public async Task<TransactionDefinition?> GetByTransactionCodeAsync(string transactionCode)
    {
        return await Table.Where(t => t.TransactionCode == transactionCode).FirstOrDefaultAsync();
    }

    public async Task<IPagedList<TransactionDefinition>> SearchAsync(
        O24OpenAPI.Framework.Models.SimpleSearchModel model
    )
    {
        model.PageSize = model.PageSize == 0 ? int.MaxValue : model.PageSize;
        model.SearchText ??= string.Empty;

        var query = Table.Where(c =>
            c.TransactionCode.ContainsInvariantCultureIgnoreCase(model.SearchText)
            || c.WorkflowId.ContainsInvariantCultureIgnoreCase(model.SearchText)
            || c.TransactionName.ContainsInvariantCultureIgnoreCase(model.SearchText)
            || c.Description.ContainsInvariantCultureIgnoreCase(model.SearchText)
            || c.TransactionType.ContainsInvariantCultureIgnoreCase(model.SearchText)
            || c.ServiceId.ContainsInvariantCultureIgnoreCase(model.SearchText)
            || c.MessageAccount.ContainsInvariantCultureIgnoreCase(model.SearchText)
            || c.MessageAmount.ContainsInvariantCultureIgnoreCase(model.SearchText)
            || c.MessageCurrency.ContainsInvariantCultureIgnoreCase(model.SearchText)
            || c.Voucher.ContainsInvariantCultureIgnoreCase(model.SearchText)
            || c.ApplicationCode.ContainsInvariantCultureIgnoreCase(model.SearchText)
            || c.TransactionModel.ContainsInvariantCultureIgnoreCase(model.SearchText)
            || c.Channel.ContainsInvariantCultureIgnoreCase(model.SearchText)
        );

        return await query.ToPagedList(model.PageIndex, model.PageSize);
    }
}
