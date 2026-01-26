using LinKit.Json.Runtime;
using LinqToDB;
using O24OpenAPI.Core;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Core.Constants;
using O24OpenAPI.Core.Domain;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Core.SeedWork;
using O24OpenAPI.Data.System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Transactions;

namespace O24OpenAPI.Data;

/// <summary>
/// The entity repository class
/// </summary>
/// <seealso cref="IRepository{TEntity}"/>
public class EntityRepository<TEntity>(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
) : IRepository<TEntity>
    where TEntity : BaseEntity, new()
{
    private readonly IO24OpenAPIDataProvider _dataProvider = dataProvider;
    private readonly IStaticCacheManager _staticCacheManager = staticCacheManager;

    protected virtual async Task<IList<TEntity>> GetEntities(
        Func<Task<IList<TEntity>>> getAll,
        Func<IStaticCacheManager, CacheKey> getCacheKey
    )
    {
        if (getCacheKey == null)
        {
            return await getAll();
        }

        CacheKey cacheKey =
            getCacheKey(_staticCacheManager)
            ?? _staticCacheManager.PrepareKeyForDefaultCache(CachingKey.EntityKey<TEntity>("all"));
        IList<TEntity> entities1 = await _staticCacheManager.Get(cacheKey, getAll);
        return entities1;
    }

    public virtual async Task<TEntity> GetById(
        int id,
        Func<IStaticCacheManager, CacheKey> getCacheKey = null
    )
    {
        if (id == 0)
        {
            return null;
        }

        return await getEntity();

        async Task<TEntity> getEntity()
        {
            return await Table.FirstOrDefaultAsync(entity => entity.Id == id);
        }
    }

    public virtual async Task<IList<TEntity>> GetByIds(
        IList<int> ids,
        Func<IStaticCacheManager, CacheKey> getCacheKey = null
    )
    {
        IList<int> list = ids;
        if (list == null || !list.Any())
        {
            return [];
        }

        if (getCacheKey == null)
        {
            return await getByIds();
        }

        CacheKey cacheKey =
            getCacheKey(_staticCacheManager)
            ?? _staticCacheManager.PrepareKeyForDefaultCache(
                O24OpenAPIEntityCacheDefaults<TEntity>.ByIdsCacheKey,
                ids
            );
        return await _staticCacheManager.Get(cacheKey, getByIds);

        async Task<IList<TEntity>> getByIds()
        {
            IQueryable<TEntity> query = Table;
            List<TEntity> entries = await query
                .Where(entry => ids.Contains(entry.Id))
                .ToListAsync();
            List<TEntity> sortedEntries = [];
            foreach (int id in ids)
            {
                TEntity sortedEntry = entries.Find(entry => entry.Id == id);
                if (sortedEntry != null)
                {
                    sortedEntries.Add(sortedEntry);
                }
            }

            return sortedEntries;
        }
    }

    public virtual async Task<IList<TEntity>> GetAll(
        Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null,
        Func<IStaticCacheManager, CacheKey> getCacheKey = null
    )
    {
        return await getAll();

        async Task<IList<TEntity>> getAll()
        {
            IQueryable<TEntity> query = Table;
            query = (func != null) ? func(query) : query;
            return await query.ToListAsync();
        }
    }

    public virtual async Task<IList<TEntity>> GetAll(
        Func<IQueryable<TEntity>, Task<IQueryable<TEntity>>> func = null,
        Func<IStaticCacheManager, CacheKey> getCacheKey = null
    )
    {
        return await getAll();

        async Task<IList<TEntity>> getAll()
        {
            IQueryable<TEntity> query = Table;
            IQueryable<TEntity> queryable = (func == null) ? query : (await func(query));
            query = queryable;
            return await query.ToListAsync();
        }
    }

    public virtual async Task<IPagedList<TEntity>> GetAllPaged(
        Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null,
        int pageIndex = 0,
        int pageSize = 2147483647,
        bool getOnlyTotalCount = false
    )
    {
        IQueryable<TEntity> query = func != null ? func(Table) : Table;
        IPagedList<TEntity> pagedList = await query.ToPagedList(
            pageIndex,
            pageSize,
            getOnlyTotalCount
        );
        return pagedList;
    }

    public virtual async Task<IPagedList<TEntity>> GetAllPaged(
        Func<IQueryable<TEntity>, Task<IQueryable<TEntity>>> func = null,
        int pageIndex = 0,
        int pageSize = 2147483647,
        bool getOnlyTotalCount = false
    )
    {
        IQueryable<TEntity> queryable;
        if (func != null)
        {
            queryable = await func(Table);
        }
        else
        {
            queryable = Table;
        }

        IQueryable<TEntity> query = queryable;
        IPagedList<TEntity> pagedList = await query.ToPagedList(
            pageIndex,
            pageSize,
            getOnlyTotalCount
        );
        return pagedList;
    }

    public virtual async Task<List<TEntity>> SearchByFields(Dictionary<string, string> searchInput)
    {
        IQueryable<TEntity> query = TableFilter(searchInput);
        List<TEntity> listAsync = await query.ToListAsync();
        return listAsync;
    }

    public virtual async Task<TEntity> GetByFields(Dictionary<string, string> searchInput)
    {
        List<TEntity> entities = await SearchByFields(searchInput);
        TEntity byFields =
            entities != null && entities.Count != 0 ? entities.FirstOrDefault() : default(TEntity);
        return byFields;
    }

    public virtual async Task Insert(TEntity entity)
    {
        await InsertAsync(entity);
    }

    public virtual async Task<TEntity> InsertAsync(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        entity.CreatedOnUtc = DateTime.UtcNow;
        entity.UpdatedOnUtc = DateTime.UtcNow;

        TEntity entity1 = await _dataProvider.InsertEntity(entity);
        if (entity.IsAuditable())
        {
            WorkContext workContext = EngineContext.Current.Resolve<WorkContext>();
            EntityAudit entityAudit = new()
            {
                EntityName = typeof(TEntity).Name,
                EntityId = entity1.Id,
                UserId = workContext.UserContext.UserId,
                ExecutionId = workContext.ExecutionId,
                ActionType = EntityAuditActionType.Insert,
                Changes = string.Empty,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow,
            };
            await _dataProvider.InsertEntity(entityAudit);
        }

        return entity1;
    }

    public virtual async Task BulkInsert(IList<TEntity> entities)
    {
        ArgumentNullException.ThrowIfNull(entities);
        if (entities.Count == 0)
        {
            return;
        }

        using TransactionScope transaction = new(TransactionScopeAsyncFlowOption.Enabled);
        await _dataProvider.BulkInsertEntities(entities);
        transaction.Complete();
    }

    public virtual async Task Update(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        entity.UpdatedOnUtc = DateTime.UtcNow;
        await _dataProvider.UpdateEntity(entity);
        if (entity.IsAuditable())
        {
            WorkContext workContext = EngineContext.Current.Resolve<WorkContext>();
            List<AuditDiff> changes = entity.GetChanges(await LoadOriginalCopy(entity));
            EntityAudit entityAudit = new()
            {
                EntityName = typeof(TEntity).Name,
                EntityId = entity.Id,
                UserId = workContext.UserContext.UserId,
                ExecutionId = workContext.ExecutionId,
                ActionType = EntityAuditActionType.Update,
                Changes = changes.ToJson(),
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow,
            };
            await _dataProvider.InsertEntity(entityAudit);
        }
    }

    public virtual async Task Delete(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        await _dataProvider.DeleteEntity(entity);
        if (entity.IsAuditable())
        {
            WorkContext workContext = EngineContext.Current.Resolve<WorkContext>();
            List<AuditDiff> changes = new TEntity().GetChanges(entity);
            EntityAudit entityAudit = new()
            {
                EntityName = typeof(TEntity).Name,
                EntityId = entity.Id,
                UserId = workContext.UserContext.UserId,
                ExecutionId = workContext.ExecutionId,
                ActionType = EntityAuditActionType.Delete,
                Changes = changes.ToJson(),
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow,
            };
            await _dataProvider.InsertEntity(entityAudit);
        }
    }

    public virtual async Task BulkDelete(IList<TEntity> entities)
    {
        ArgumentNullException.ThrowIfNull(entities);
        await _dataProvider.BulkDeleteEntities(entities);
    }

    public virtual async Task UpdateNoAudit(
        IQueryable<TEntity> query,
        string propertyName,
        string value
    )
    {
        ArgumentNullException.ThrowIfNull(query);
        await _dataProvider.UpdateEntities(query, propertyName, value);
    }

    public virtual async Task FilterAndUpdate(
        Dictionary<string, string> searchInput,
        string propertyName,
        string value
    )
    {
        IQueryable<TEntity> query = TableFilter(searchInput);
        await UpdateNoAudit(query, propertyName, value);
    }

    public virtual async Task<int> DeleteWhere(
        Expression<Func<TEntity, bool>> predicate,
        int batchSize = 0
    )
    {
        ArgumentNullException.ThrowIfNull(predicate);
        int num = await _dataProvider.BulkDeleteEntities(predicate, batchSize);
        return num;
    }

    public virtual async Task<TEntity> LoadOriginalCopy(TEntity entity)
    {
        TEntity entity1 = await _dataProvider
            .GetTable<TEntity>()
            .FirstOrDefaultAsync(e => e.Id == entity.Id);
        return entity1;
    }

    public virtual async Task Truncate(bool resetIdentity = false)
    {
        await _dataProvider.Truncate<TEntity>(resetIdentity);
    }

    public virtual IQueryable<TEntity> Table => _dataProvider.GetTable<TEntity>();

    public virtual IQueryable<TEntity> TableFilter(Expression<Func<TEntity, bool>> filter)
    {
        return _dataProvider.GetTable<TEntity>().Where(filter);
    }

    public IQueryable<TEntity> TableFilterExpression(Expression<Func<TEntity, bool>> filter)
    {
        throw new NotImplementedException();
    }

    public virtual IQueryable<TEntity> TableFilter(Dictionary<string, string> searchInput)
    {
        IQueryable<TEntity> source = Table;
        foreach (KeyValuePair<string, string> keyValuePair in searchInput)
        {
            KeyValuePair<string, string> item = keyValuePair;
            PropertyInfo property = typeof(TEntity).GetProperty(item.Key);
            if (property != null)
            {
                if (property.PropertyType == typeof(string))
                {
                    if (!string.IsNullOrEmpty(item.Value))
                    {
                        source = source.Where(
                            (Expression<Func<TEntity, bool>>)(
                                e => Sql.Property<string>(e, item.Key) == item.Value
                            )
                        );
                    }
                }
                else if (property.PropertyType == typeof(int) && !string.IsNullOrEmpty(item.Value))
                {
                    source = source.Where(
                        (Expression<Func<TEntity, bool>>)(
                            e => Sql.Property<int>(e, item.Key) == int.Parse(item.Value)
                        )
                    );
                }
            }
        }

        return source;
    }

    public Task FilterAndDelete(Dictionary<string, string> searchInput)
    {
        throw new NotImplementedException();
    }

    public IQueryable<TEntity> GetTable()
    {
        return Table;
    }

    public virtual async Task UpdateRangeNoAuditAsync(IEnumerable<TEntity> entities)
    {
        foreach (TEntity entity in entities)
        {
            await _dataProvider.UpdateEntity(entity);
        }
    }

    Task<TEntity> IRepository<TEntity>.Insert(TEntity entity)
    {
        return InsertAsync(entity);
    }
}
