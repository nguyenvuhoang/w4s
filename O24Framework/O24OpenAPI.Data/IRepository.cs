using O24OpenAPI.Core;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Core.Domain;
using O24OpenAPI.Core.Domain.O24OpenAPI;
using System.Linq.Expressions;

namespace O24OpenAPI.Data;

/// <summary>
/// The repository interface
/// </summary>
public interface IRepository<TEntity>
    where TEntity : BaseEntity
{
    /// <summary>
    /// Gets the by id using the specified id
    /// </summary>
    /// <param name="id">The id</param>
    /// <param name="getCacheKey">The get cache key</param>
    /// <returns>A task containing the entity</returns>
    Task<TEntity> GetById(int? id, Func<IStaticCacheManager, CacheKey> getCacheKey = null);

    /// <summary>
    /// Gets the by ids using the specified ids
    /// </summary>
    /// <param name="ids">The ids</param>
    /// <param name="getCacheKey">The get cache key</param>
    /// <returns>A task containing a list of t entity</returns>
    Task<IList<TEntity>> GetByIds(
        IList<int> ids,
        Func<IStaticCacheManager, CacheKey> getCacheKey = null
    );

    /// <summary>
    /// Gets the all using the specified func
    /// </summary>
    /// <param name="func">The func</param>
    /// <param name="getCacheKey">The get cache key</param>
    /// <returns>A task containing a list of t entity</returns>
    Task<IList<TEntity>> GetAll(
        Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null,
        Func<IStaticCacheManager, CacheKey> getCacheKey = null
    );

    /// <summary>
    /// Gets the all using the specified func
    /// </summary>
    /// <param name="func">The func</param>
    /// <param name="getCacheKey">The get cache key</param>
    /// <returns>A task containing a list of t entity</returns>
    Task<IList<TEntity>> GetAll(
        Func<IQueryable<TEntity>, Task<IQueryable<TEntity>>> func = null,
        Func<IStaticCacheManager, CacheKey> getCacheKey = null
    );

    /// <summary>
    /// Gets the all paged using the specified func
    /// </summary>
    /// <param name="func">The func</param>
    /// <param name="pageIndex">The page index</param>
    /// <param name="pageSize">The page size</param>
    /// <param name="getOnlyTotalCount">The get only total count</param>
    /// <returns>A task containing a paged list of t entity</returns>
    Task<IPagedList<TEntity>> GetAllPaged(
        Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null,
        int pageIndex = 0,
        int pageSize = 2147483647,
        bool getOnlyTotalCount = false
    );

    /// <summary>
    /// Gets the all paged using the specified func
    /// </summary>
    /// <param name="func">The func</param>
    /// <param name="pageIndex">The page index</param>
    /// <param name="pagSize">The pag size</param>
    /// <param name="getOnlyTotalCount">The get only total count</param>
    /// <returns>A task containing a paged list of t entity</returns>
    Task<IPagedList<TEntity>> GetAllPaged(
        Func<IQueryable<TEntity>, Task<IQueryable<TEntity>>> func = null,
        int pageIndex = 0,
        int pagSize = 2147483647,
        bool getOnlyTotalCount = false
    );

    /// <summary>
    /// Gets the new update using the specified since date
    /// </summary>
    /// <param name="sinceDate">The since date</param>
    /// <param name="offset">The offset</param>
    /// <param name="limit">The limit</param>
    /// <returns>A task containing a list of t entity</returns>
    Task<List<TEntity>> GetNewUpdate(DateTime sinceDate, int offset = 0, int limit = 0);

    /// <summary>
    /// Searches the by fields using the specified search input
    /// </summary>
    /// <param name="searchInput">The search input</param>
    /// <returns>A task containing a list of t entity</returns>
    Task<List<TEntity>> SearchByFields(Dictionary<string, string> searchInput);

    /// <summary>
    /// Gets the by fields using the specified search input
    /// </summary>
    /// <param name="searchInput">The search input</param>
    /// <returns>A task containing the entity</returns>
    Task<TEntity> GetByFields(Dictionary<string, string> searchInput);

    /// <summary>
    /// Inserts the entity
    /// </summary>
    /// <param name="entity">The entity</param>
    /// <param name="referenceId">The reference id</param>
    /// <param name="publishEvent">The publish event</param>
    /// <param name="isReverse">The is reverse</param>
    /// <param name="isRemove">The is remove</param>
    Task Insert(
        TEntity entity,
        string referenceId = "",
        bool publishEvent = true,
        bool isReverse = false,
        bool isRemove = false
    );

    /// <summary>
    /// Inserts the entity
    /// </summary>
    /// <param name="entity">The entity</param>
    /// <param name="referenceId">The reference id</param>
    /// <param name="publishEvent">The publish event</param>
    /// <param name="isReverse">The is reverse</param>
    /// <param name="isRemove">The is remove</param>
    /// <returns>A task containing the entity</returns>
    Task<TEntity> InsertAsync(
        TEntity entity,
        string referenceId = "",
        bool publishEvent = true,
        bool isReverse = false,
        bool isRemove = false
    );

    /// <summary>
    /// Bulks the insert using the specified entities
    /// </summary>
    /// <param name="entities">The entities</param>
    /// <param name="referenceId">The reference id</param>
    /// <param name="publishEvent">The publish event</param>
    Task BulkInsert(IList<TEntity> entities, string referenceId = "", bool publishEvent = true);

    /// <summary>
    /// Updates the entity
    /// </summary>
    /// <param name="entity">The entity</param>
    /// <param name="referenceId">The reference id</param>
    /// <param name="publishEvent">The publish event</param>
    /// <param name="isReverse">The is reverse</param>
    /// <param name="isRemove">The is remove</param>
    Task Update(
        TEntity entity,
        string referenceId = "",
        bool publishEvent = true,
        bool isReverse = false,
        bool isRemove = false
    );

    /// <summary>
    /// Updates the chains using the specified entity
    /// </summary>
    /// <param name="entity">The entity</param>
    /// <param name="actions">The actions</param>
    /// <param name="referenceId">The reference id</param>
    /// <param name="publishEvent">The publish event</param>
    /// <param name="isReverse">The is reverse</param>
    Task UpdateChains(
        TEntity entity,
        List<ActionChain> actions,
        string referenceId = "",
        bool publishEvent = true,
        bool isReverse = false
    );

    /// <summary>
    /// Deletes the entity
    /// </summary>
    /// <param name="entity">The entity</param>
    /// <param name="referenceId">The reference id</param>
    /// <param name="publishEvent">The publish event</param>
    /// <param name="isReverse">The is reverse</param>
    /// <param name="isRemove">The is remove</param>
    Task Delete(
        TEntity entity,
        string referenceId = "",
        bool publishEvent = true,
        bool isReverse = false,
        bool isRemove = false
    );

    /// <summary>
    /// Bulks the delete using the specified entities
    /// </summary>
    /// <param name="entities">The entities</param>
    /// <param name="referenceId">The reference id</param>
    /// <param name="publishEvent">The publish event</param>
    Task BulkDelete(IList<TEntity> entities, string referenceId = "", bool publishEvent = true);

    /// <summary>
    /// Updates the no audit using the specified query
    /// </summary>
    /// <param name="query">The query</param>
    /// <param name="propertyName">The property name</param>
    /// <param name="value">The value</param>
    Task UpdateNoAudit(IQueryable<TEntity> query, string propertyName, string value);

    /// <summary>
    /// Filters the and update using the specified search input
    /// </summary>
    /// <param name="searchInput">The search input</param>
    /// <param name="propertyName">The property name</param>
    /// <param name="value">The value</param>
    Task FilterAndUpdate(Dictionary<string, string> searchInput, string propertyName, string value);

    /// <summary>
    /// Deletes the where using the specified predicate
    /// </summary>
    /// <param name="predicate">The predicate</param>
    /// <param name="referenceId">The reference id</param>
    /// <returns>A task containing the int</returns>
    Task<int> DeleteWhere(Expression<Func<TEntity, bool>> predicate, string referenceId = "");

    /// <summary>
    /// Loads the original copy using the specified entity
    /// </summary>
    /// <param name="entity">The entity</param>
    /// <returns>A task containing the entity</returns>
    Task<TEntity> LoadOriginalCopy(TEntity entity);

    /// <summary>
    /// Truncates the reset identity
    /// </summary>
    /// <param name="resetIdentity">The reset identity</param>
    Task Truncate(bool resetIdentity = false);

    /// <summary>
    /// Gets the value of the table
    /// </summary>
    IQueryable<TEntity> Table { get; }

    /// <summary>
    /// Gets the table
    /// </summary>
    /// <returns>A queryable of t entity</returns>
    IQueryable<TEntity> GetTable();

    /// <summary>
    /// Tables the filter expression using the specified filter
    /// </summary>
    /// <param name="filter">The filter</param>
    /// <returns>A queryable of t entity</returns>
    IQueryable<TEntity> TableFilterExpression(Expression<Func<TEntity, bool>> filter);

    /// <summary>
    /// Tables the filter using the specified search input
    /// </summary>
    /// <param name="searchInput">The search input</param>
    /// <returns>A queryable of t entity</returns>
    IQueryable<TEntity> TableFilter(Dictionary<string, string> searchInput);

    /// <summary>
    /// Filters the and delete using the specified search input
    /// </summary>
    /// <param name="searchInput">The search input</param>
    Task FilterAndDelete(Dictionary<string, string> searchInput);

    /// <summary>
    /// Update Range NoAudit
    /// </summary>
    /// <param name="entities"></param>
    /// <returns></returns>
    Task UpdateRangeNoAuditAsync(IEnumerable<TEntity> entities);
}
