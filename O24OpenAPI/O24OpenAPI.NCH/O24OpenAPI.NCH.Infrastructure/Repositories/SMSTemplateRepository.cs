using O24OpenAPI.Core.Caching;
using O24OpenAPI.Data;

using O24OpenAPI.NCH.Domain.AggregatesModel.SmsAggregate;

namespace O24OpenAPI.NCH.Infrastructure.Repositories;

public class SMSTemplateRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
) : EntityRepository<SMSTemplate>(dataProvider, staticCacheManager), ISMSTemplateRepository
{
    public Task<SMSTemplate?> GetByCodeAsync(string code) => throw new NotImplementedException();

    public Task<IReadOnlyList<SMSTemplate>> GetActiveAsync() => throw new NotImplementedException();

    public Task<SMSTemplate> GetById(
        int id,
        Func<IStaticCacheManager, CacheKey>? getCacheKey = null
    )
    {
        throw new NotImplementedException();
    }

    public Task<IList<SMSTemplate>> GetByIds(
        IList<int> ids,
        Func<IStaticCacheManager, CacheKey>? getCacheKey = null
    )
    {
        throw new NotImplementedException();
    }

    public Task<IList<SMSTemplate>> GetAll(
        Func<IQueryable<SMSTemplate>, IQueryable<SMSTemplate>>? func = null,
        Func<IStaticCacheManager, CacheKey>? getCacheKey = null
    )
    {
        throw new NotImplementedException();
    }

    public Task<IList<SMSTemplate>> GetAll(
        Func<IQueryable<SMSTemplate>, Task<IQueryable<SMSTemplate>>>? func = null,
        Func<IStaticCacheManager, CacheKey>? getCacheKey = null
    )
    {
        throw new NotImplementedException();
    }

    public Task<Core.IPagedList<SMSTemplate>> GetAllPaged(
        Func<IQueryable<SMSTemplate>, IQueryable<SMSTemplate>>? func = null,
        int pageIndex = 0,
        int pageSize = int.MaxValue,
        bool getOnlyTotalCount = false
    )
    {
        throw new NotImplementedException();
    }

    public Task<Core.IPagedList<SMSTemplate>> GetAllPaged(
        Func<IQueryable<SMSTemplate>, Task<IQueryable<SMSTemplate>>>? func = null,
        int pageIndex = 0,
        int pagSize = int.MaxValue,
        bool getOnlyTotalCount = false
    )
    {
        throw new NotImplementedException();
    }

    public Task<List<SMSTemplate>> SearchByFields(Dictionary<string, string> searchInput)
    {
        throw new NotImplementedException();
    }

    public Task<SMSTemplate> GetByFields(Dictionary<string, string> searchInput)
    {
        throw new NotImplementedException();
    }

    public Task<SMSTemplate> InsertAsync(SMSTemplate entity)
    {
        throw new NotImplementedException();
    }

    public Task<SMSTemplate> Insert(SMSTemplate entity)
    {
        throw new NotImplementedException();
    }

    public Task BulkInsert(IList<SMSTemplate> entities)
    {
        throw new NotImplementedException();
    }

    public Task Update(SMSTemplate entity)
    {
        throw new NotImplementedException();
    }

    public Task Delete(SMSTemplate entity)
    {
        throw new NotImplementedException();
    }

    public Task BulkDelete(IList<SMSTemplate> entities)
    {
        throw new NotImplementedException();
    }

    public Task UpdateNoAudit(IQueryable<SMSTemplate> query, string propertyName, string value)
    {
        throw new NotImplementedException();
    }

    public Task FilterAndUpdate(
        Dictionary<string, string> searchInput,
        string propertyName,
        string value
    )
    {
        throw new NotImplementedException();
    }

    public Task<int> DeleteWhere(
        System.Linq.Expressions.Expression<Func<SMSTemplate, bool>> predicate
    )
    {
        throw new NotImplementedException();
    }

    public Task<SMSTemplate> LoadOriginalCopy(SMSTemplate entity)
    {
        throw new NotImplementedException();
    }

    public Task Truncate(bool resetIdentity = false)
    {
        throw new NotImplementedException();
    }

    public IQueryable<SMSTemplate> GetTable()
    {
        throw new NotImplementedException();
    }

    public IQueryable<SMSTemplate> TableFilterExpression(
        System.Linq.Expressions.Expression<Func<SMSTemplate, bool>> filter
    )
    {
        throw new NotImplementedException();
    }

    public IQueryable<SMSTemplate> TableFilter(Dictionary<string, string> searchInput)
    {
        throw new NotImplementedException();
    }

    public Task FilterAndDelete(Dictionary<string, string> searchInput)
    {
        throw new NotImplementedException();
    }

    public Task UpdateRangeNoAuditAsync(IEnumerable<SMSTemplate> entities)
    {
        throw new NotImplementedException();
    }

    Task<SMSTemplate?> ISMSTemplateRepository.GetByCodeAsync(string code)
    {
        throw new NotImplementedException();
    }

    Task<IReadOnlyList<SMSTemplate>> ISMSTemplateRepository.GetActiveAsync()
    {
        throw new NotImplementedException();
    }

    Task<SMSTemplate> Core.SeedWork.IRepository<SMSTemplate>.GetById(
        int id,
        Func<IStaticCacheManager, CacheKey>? getCacheKey
    )
    {
        throw new NotImplementedException();
    }

    Task<IList<SMSTemplate>> Core.SeedWork.IRepository<SMSTemplate>.GetByIds(
        IList<int> ids,
        Func<IStaticCacheManager, CacheKey>? getCacheKey
    )
    {
        throw new NotImplementedException();
    }
}
