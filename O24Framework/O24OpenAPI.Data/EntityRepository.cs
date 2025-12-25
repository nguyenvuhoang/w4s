using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Transactions;
using LinqToDB;
using O24OpenAPI.Core;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Core.Domain;
using O24OpenAPI.Core.SeedWork;
using O24OpenAPI.Data.System.Linq;

namespace O24OpenAPI.Data;

/// <summary>
/// The entity repository class
/// </summary>
/// <seealso cref="IRepository{TEntity}"/>
public class EntityRepository<TEntity>(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
) : IRepository<TEntity>
    where TEntity : BaseEntity
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
            ?? _staticCacheManager.PrepareKeyForDefaultCache(
                O24OpenAPIEntityCacheDefaults<TEntity>.AllCacheKey
            );
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
            return await Table.FirstOrDefaultAsync(entity => entity.Id == Convert.ToInt32(id));
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
        ArgumentNullException.ThrowIfNull(entity);
        entity.CreatedOnUtc = DateTime.UtcNow;
        entity.UpdatedOnUtc = DateTime.UtcNow;
        TEntity entity1 = await _dataProvider.InsertEntity(entity);
    }

    public virtual async Task<TEntity> InsertAsync(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        entity.CreatedOnUtc = DateTime.UtcNow;
        entity.UpdatedOnUtc = DateTime.UtcNow;

        TEntity entity1 = await _dataProvider.InsertEntity(entity);

        return entity1;
    }

    public virtual async Task BulkInsert(IList<TEntity> entities)
    {
        ArgumentNullException.ThrowIfNull(entities);
        if (entities.Count == 0)
        {
            return;
        }

        using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        await _dataProvider.BulkInsertEntities(entities);
        transaction.Complete();
    }

    public virtual async Task Update(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        await _dataProvider.UpdateEntity(entity);
    }

    private static List<ActionChain> SortAction(List<ActionChain> actionChains)
    {
        List<ActionChain> source = new List<ActionChain>();
        string str1 = "xxx";
        Decimal num = 0M;
        string str2 = "";
        foreach (
            ActionChain actionChain in (IEnumerable<ActionChain>)
                actionChains
                    .OrderBy(a => a.UpdateField)
                    .ThenBy(a => a.UpdateFields)
                    .ThenBy(a => a.Action)
        )
        {
            ActionChain action = actionChain;
            if (
                (action.UpdateFields == null || !action.UpdateFields.Any())
                && str1 == action.UpdateField
                && "D,C".Contains(action.Action)
            )
            {
                if (str2 == action.Action)
                {
                    num += (Decimal)action.UpdateValue;
                }
                else if (num >= (Decimal)action.UpdateValue)
                {
                    num -= (Decimal)action.UpdateValue;
                }
                else
                {
                    num = (Decimal)action.UpdateValue - num;
                    string action1 = action.Action;
                }

                source.Where(s => s.UpdateField == action.UpdateField).First().UpdateValue = num;
            }
            else
            {
                source.Add(action);
                num = !(action.UpdateValue is Decimal) ? 0M : (Decimal)action.UpdateValue;
            }

            str1 = action.UpdateField;
            str2 = action.Action;
        }

        return source;
    }

    private static TransactionDetails GenUpdateAudit(
        string refId,
        string entityName,
        int entityId,
        PropertyInfo prop,
        object currentValue = null,
        object inputValue = null,
        string updateType = "A"
    )
    {
        object obj = inputValue;
        if (prop.PropertyType == typeof(Decimal) || prop.PropertyType == typeof(Decimal?))
        {
            switch (updateType)
            {
                case "C":
                    obj = (Decimal)(currentValue ?? 0M) + (Decimal)inputValue;
                    break;
                case "D":
                    obj = (Decimal)(currentValue ?? 0M) - (Decimal)inputValue;
                    break;
            }
        }
        else if (prop.PropertyType == typeof(int) || prop.PropertyType == typeof(int?))
        {
            switch (updateType)
            {
                case "C":
                    obj = (int)currentValue + (int)inputValue;
                    break;
                case "D":
                    obj = (int)currentValue - (int)inputValue;
                    break;
            }
        }

        string invariantString1 =
            currentValue == null
                ? ""
                : TypeDescriptor
                    .GetConverter(prop.PropertyType)
                    .ConvertToInvariantString(currentValue);
        string invariantString2 =
            obj == null
                ? ""
                : TypeDescriptor.GetConverter(prop.PropertyType).ConvertToInvariantString(obj);
        if (updateType == "U")
        {
            updateType = "A";
        }

        if (invariantString1.Equals(invariantString2))
        {
            return null;
        }

        if (prop.PropertyType == typeof(Decimal) || prop.PropertyType == typeof(Decimal?))
        {
            Decimal num1 = Convert.ToDecimal(currentValue);
            Decimal num2 = Convert.ToDecimal(obj);
            if (num1 == num2)
            {
                return null;
            }

            updateType = !(num1 > num2) ? "C" : "D";
        }
        else if (prop.PropertyType == typeof(int) || prop.PropertyType == typeof(int?))
        {
            int int32_1 = Convert.ToInt32(currentValue);
            int int32_2 = Convert.ToInt32(obj);
            if (int32_1 == int32_2)
            {
                return null;
            }

            updateType = int32_1 <= int32_2 ? "C" : "D";
        }

        return new TransactionDetails()
        {
            RefId = refId,
            Entity = entityName,
            EntityId = entityId,
            FieldName = prop.Name,
            OldValue = invariantString1,
            NewValue = invariantString2,
            Status = "N",
            UpdateType = updateType,
            Description = "",
        };
    }

    private static TransactionDetails GenDeleteAudit(
        string refId,
        string entityName,
        int entityId,
        PropertyInfo prop,
        object entity
    )
    {
        object obj = prop.GetValue(entity, (object[])null);
        string str = "R";
        string invariantString =
            obj == null
                ? ""
                : TypeDescriptor.GetConverter(prop.PropertyType).ConvertToInvariantString(obj);
        return new TransactionDetails()
        {
            RefId = refId,
            Entity = entityName,
            EntityId = entityId,
            FieldName = prop.Name,
            OldValue = invariantString,
            NewValue = string.Empty,
            Status = "N",
            UpdateType = str,
            Description = "",
        };
    }

    public virtual async Task Delete(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        await _dataProvider.DeleteEntity(entity);
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

    public virtual async Task<int> DeleteWhere(Expression<Func<TEntity, bool>> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        int num = await _dataProvider.BulkDeleteEntities(predicate);
        return num;
    }

    public virtual async Task<TEntity> LoadOriginalCopy(TEntity entity)
    {
        TEntity entity1 = await _dataProvider
            .GetTable<TEntity>()
            .FirstOrDefaultAsync(e => e.Id == Convert.ToInt32(entity.Id));
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
        foreach (var entity in entities)
        {
            await _dataProvider.UpdateEntity(entity);
        }
    }

    Task<TEntity> IRepository<TEntity>.Insert(TEntity entity)
    {
        return InsertAsync(entity);
    }
}
