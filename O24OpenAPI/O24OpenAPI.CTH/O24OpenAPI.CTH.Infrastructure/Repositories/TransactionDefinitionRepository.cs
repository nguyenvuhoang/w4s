using LinKit.Core.Abstractions;
using O24OpenAPI.Core;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Core.Events;
using O24OpenAPI.CTH.Domain.AggregatesModel.TransactionAggregate;
using O24OpenAPI.Data;

namespace O24OpenAPI.CTH.Infrastructure.Repositories;

using O24OpenAPI.Data.System.Linq;

[RegisterService(Lifetime.Scoped)]
public class TransactionDefinitionRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
)
    : EntityRepository<TransactionDefinition>(dataProvider, staticCacheManager),
        ITransactionDefinitionRepository
{
    private static readonly StringComparison ICIC = StringComparison.InvariantCultureIgnoreCase;

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
            c.TransactionCode.Contains(model.SearchText, ICIC)
            || c.WorkflowId.Contains(model.SearchText, ICIC)
            || c.TransactionName.Contains(model.SearchText, ICIC)
            || c.Description.Contains(model.SearchText, ICIC)
            || c.TransactionType.Contains(model.SearchText, ICIC)
            || c.ServiceId.Contains(model.SearchText, ICIC)
            || c.MessageAccount.Contains(model.SearchText, ICIC)
            || c.MessageAmount.Contains(model.SearchText, ICIC)
            || c.MessageCurrency.Contains(model.SearchText, ICIC)
            || c.Voucher.Contains(model.SearchText, ICIC)
            || c.ApplicationCode.Contains(model.SearchText, ICIC)
            || c.TransactionModel.Contains(model.SearchText, ICIC)
            || c.Channel.Contains(model.SearchText, ICIC)
        );

        return await query.ToPagedList(model.PageIndex, model.PageSize);
    }
}
