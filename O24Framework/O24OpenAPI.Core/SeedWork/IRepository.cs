using O24OpenAPI.Core.Caching;
using System.Linq.Expressions;

namespace O24OpenAPI.Core.SeedWork;

public interface IRepository<TEntity>
    where TEntity : BaseEntity
{
    Task<TEntity> GetById(int id, Func<IStaticCacheManager, CacheKey>? getCacheKey = null);
    Task<IList<TEntity>> GetByIds(
        IList<int> ids,
        Func<IStaticCacheManager, CacheKey>? getCacheKey = null
    );
    Task<IList<TEntity>> GetAll(
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? func = null,
        Func<IStaticCacheManager, CacheKey>? getCacheKey = null
    );
    Task<IList<TEntity>> GetAll(
        Func<IQueryable<TEntity>, Task<IQueryable<TEntity>>>? func = null,
        Func<IStaticCacheManager, CacheKey>? getCacheKey = null
    );
    Task<IPagedList<TEntity>> GetAllPaged(
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? func = null,
        int pageIndex = 0,
        int pageSize = 2147483647,
        bool getOnlyTotalCount = false
    );
    Task<IPagedList<TEntity>> GetAllPaged(
        Func<IQueryable<TEntity>, Task<IQueryable<TEntity>>>? func = null,
        int pageIndex = 0,
        int pagSize = 2147483647,
        bool getOnlyTotalCount = false
    );
    Task<List<TEntity>> SearchByFields(Dictionary<string, string> searchInput);
    Task<TEntity> GetByFields(Dictionary<string, string> searchInput);
    Task<TEntity> InsertAsync(TEntity entity);
    Task<TEntity> Insert(TEntity entity);
    Task BulkInsert(IList<TEntity> entities);
    Task Update(TEntity entity);
    Task Delete(TEntity entity);
    Task BulkDelete(IList<TEntity> entities);
    Task UpdateNoAudit(IQueryable<TEntity> query, string propertyName, string value);
    Task FilterAndUpdate(Dictionary<string, string> searchInput, string propertyName, string value);
    Task<int> DeleteWhere(Expression<Func<TEntity, bool>> predicate);
    Task<TEntity> LoadOriginalCopy(TEntity entity);
    Task Truncate(bool resetIdentity = false);
    IQueryable<TEntity> Table { get; }
    IQueryable<TEntity> GetTable();
    IQueryable<TEntity> TableFilterExpression(Expression<Func<TEntity, bool>> filter);
    IQueryable<TEntity> TableFilter(Dictionary<string, string> searchInput);
    Task FilterAndDelete(Dictionary<string, string> searchInput);
    Task UpdateRangeNoAuditAsync(IEnumerable<TEntity> entities);
}
