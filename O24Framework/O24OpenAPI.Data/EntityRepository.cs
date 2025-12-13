using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Transactions;
using LinqToDB;
using O24OpenAPI.Core;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Core.Domain;
using O24OpenAPI.Core.Domain.O24OpenAPI;
using O24OpenAPI.Core.Events;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.Data.Extensions;
using O24OpenAPI.Data.System.Linq;

namespace O24OpenAPI.Data;

/// <summary>
/// The entity repository class
/// </summary>
/// <seealso cref="IRepository{TEntity}"/>
public class EntityRepository<TEntity>(
    IEventPublisher eventPublisher,
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
) : IRepository<TEntity>
    where TEntity : BaseEntity
{
    /// <summary>
    /// The event publisher
    /// </summary>
    private readonly IEventPublisher _eventPublisher = eventPublisher;

    /// <summary>
    /// The data provider
    /// </summary>
    private readonly IO24OpenAPIDataProvider _dataProvider = dataProvider;

    /// <summary>
    /// The static cache manager
    /// </summary>
    private readonly IStaticCacheManager _staticCacheManager = staticCacheManager;

    /// <summary>
    /// Gets the entities using the specified get all
    /// </summary>
    /// <param name="getAll">The get all</param>
    /// <param name="getCacheKey">The get cache key</param>
    /// <returns>The entities</returns>
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
            getCacheKey(this._staticCacheManager)
            ?? this._staticCacheManager.PrepareKeyForDefaultCache(
                O24OpenAPIEntityCacheDefaults<TEntity>.AllCacheKey
            );
        IList<TEntity> entities1 = await this._staticCacheManager.Get(cacheKey, getAll);
        return entities1;
    }

    /// <summary>
    /// Gets the by id using the specified id
    /// </summary>
    /// <param name="id">The id</param>
    /// <param name="getCacheKey">The get cache key</param>
    /// <returns>A task containing the entity</returns>
    public virtual async Task<TEntity> GetById(
        int? id,
        Func<IStaticCacheManager, CacheKey> getCacheKey = null
    )
    {
        if (!id.HasValue || id == 0)
        {
            return null;
        }

        return await getEntity();

        async Task<TEntity> getEntity()
        {
            return await Table.FirstOrDefaultAsync(entity => entity.Id == Convert.ToInt32(id));
        }
    }

    /// <summary>
    /// Gets the by ids using the specified ids
    /// </summary>
    /// <param name="ids">The ids</param>
    /// <param name="getCacheKey">The get cache key</param>
    /// <returns>A task containing a list of t entity</returns>
    public virtual async Task<IList<TEntity>> GetByIds(
        IList<int> ids,
        Func<IStaticCacheManager, CacheKey> getCacheKey = null
    )
    {
        IList<int> list = ids;
        if (list == null || !list.Any())
        {
            return new List<TEntity>();
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
                .Where((TEntity entry) => ids.Contains(entry.Id))
                .ToListAsync();
            List<TEntity> sortedEntries = new List<TEntity>();
            foreach (int id in ids)
            {
                TEntity sortedEntry = entries.Find((TEntity entry) => entry.Id == id);
                if (sortedEntry != null)
                {
                    sortedEntries.Add(sortedEntry);
                }
            }

            return sortedEntries;
        }
    }

    /// <summary>
    /// Gets the all using the specified func
    /// </summary>
    /// <param name="func">The func</param>
    /// <param name="getCacheKey">The get cache key</param>
    /// <returns>A task containing a list of t entity</returns>
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

    /// <summary>
    /// Gets the all using the specified func
    /// </summary>
    /// <param name="func">The func</param>
    /// <param name="getCacheKey">The get cache key</param>
    /// <returns>A task containing a list of t entity</returns>
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

    /// <summary>
    /// Gets the all paged using the specified func
    /// </summary>
    /// <param name="func">The func</param>
    /// <param name="pageIndex">The page index</param>
    /// <param name="pageSize">The page size</param>
    /// <param name="getOnlyTotalCount">The get only total count</param>
    /// <returns>The paged list</returns>
    public virtual async Task<IPagedList<TEntity>> GetAllPaged(
        Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null,
        int pageIndex = 0,
        int pageSize = 2147483647,
        bool getOnlyTotalCount = false
    )
    {
        IQueryable<TEntity> query = func != null ? func(this.Table) : this.Table;
        IPagedList<TEntity> pagedList = await query.ToPagedList<TEntity>(
            pageIndex,
            pageSize,
            getOnlyTotalCount
        );
        query = (IQueryable<TEntity>)null;
        return pagedList;
    }

    /// <summary>
    /// Gets the all paged using the specified func
    /// </summary>
    /// <param name="func">The func</param>
    /// <param name="pageIndex">The page index</param>
    /// <param name="pageSize">The page size</param>
    /// <param name="getOnlyTotalCount">The get only total count</param>
    /// <returns>The paged list</returns>
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
            queryable = await func(this.Table);
        }
        else
        {
            queryable = this.Table;
        }

        IQueryable<TEntity> query = queryable;
        queryable = (IQueryable<TEntity>)null;
        IPagedList<TEntity> pagedList = await query.ToPagedList<TEntity>(
            pageIndex,
            pageSize,
            getOnlyTotalCount
        );
        query = (IQueryable<TEntity>)null;
        return pagedList;
    }

    /// <summary>
    /// Gets the new update using the specified since date
    /// </summary>
    /// <param name="sinceDate">The since date</param>
    /// <param name="offset">The offset</param>
    /// <param name="limit">The limit</param>
    /// <returns>The list</returns>
    public virtual async Task<List<TEntity>> GetNewUpdate(
        DateTime sinceDate,
        int offset = 0,
        int limit = 0
    )
    {
        IQueryable<TEntity> query = this.Table;
        if (typeof(TEntity).GetProperty("UpdatedOnUtc") != (PropertyInfo)null)
        {
            query = query.Where<TEntity>(
                (Expression<Func<TEntity, bool>>)(
                    e =>
                        Sql.Property<DateTime?>(e, "UpdatedOnUtc") > (DateTime?)sinceDate
                        || Sql.Property<DateTime?>(e, "UpdatedOnUtc") == new DateTime?()
                )
            );
        }

        if (typeof(TEntity).GetProperty("Id") != (PropertyInfo)null && offset > 0)
        {
            query = query.Where<TEntity>(
                (Expression<Func<TEntity, bool>>)(e => Sql.Property<int>(e, "Id") > offset)
            );
        }

        query =
            (IQueryable<TEntity>)
                query.OrderBy<TEntity, int>(
                    (Expression<Func<TEntity, int>>)(e => Sql.Property<int>(e, "Id"))
                );
        if (limit > 0)
        {
            query = query.Take<TEntity>(limit);
        }

        List<TEntity> listAsync = await query.ToListAsync<TEntity>();
        query = (IQueryable<TEntity>)null;
        return listAsync;
    }

    /// <summary>
    /// Searches the by fields using the specified search input
    /// </summary>
    /// <param name="searchInput">The search input</param>
    /// <returns>The list</returns>
    public virtual async Task<List<TEntity>> SearchByFields(
        Dictionary<string, string> searchInput
    )
    {
        IQueryable<TEntity> query = this.TableFilter(searchInput);
        List<TEntity> listAsync = await query.ToListAsync<TEntity>();
        query = (IQueryable<TEntity>)null;
        return listAsync;
    }

    /// <summary>
    /// Gets the by fields using the specified search input
    /// </summary>
    /// <param name="searchInput">The search input</param>
    /// <returns>The by fields</returns>
    public virtual async Task<TEntity> GetByFields(Dictionary<string, string> searchInput)
    {
        List<TEntity> entities = await this.SearchByFields(searchInput);
        TEntity byFields =
            entities != null && entities.Count != 0
                ? entities.FirstOrDefault<TEntity>()
                : default(TEntity);
        entities = (List<TEntity>)null;
        return byFields;
    }

    /// <summary>
    /// Sets the property date using the specified entity
    /// </summary>
    /// <param name="entity">The entity</param>
    /// <param name="propertyName">The property name</param>
    /// <param name="value">The value</param>
    private static void SetPropertyDate(TEntity entity, string propertyName, DateTime value)
    {
        if (!entity.HasMethod(propertyName))
        {
            return;
        }

        entity.GetType().GetProperty(propertyName).SetValue((object)entity, (object)value);
    }

    /// <summary>
    /// Inserts the entity
    /// </summary>
    /// <param name="entity">The entity</param>
    /// <param name="referenceId">The reference id</param>
    /// <param name="publishEvent">The publish event</param>
    /// <param name="isReverse">The is reverse</param>
    /// <param name="isRemove">The is remove</param>
    public virtual async Task Insert(
        TEntity entity,
        string referenceId = "",
        bool publishEvent = true,
        bool isReverse = false,
        bool isRemove = false
    )
    {
        ArgumentNullException.ThrowIfNull(entity);

        entity.SetPropertyDate("CreatedOnUtc", DateTime.UtcNow);
        entity.SetPropertyDate("UpdatedOnUtc", DateTime.UtcNow);

        TEntity entity1 = await this._dataProvider.InsertEntity(entity);

        if (!publishEvent)
        {
            return;
        }

        //await _eventPublisher.EntityInserted(entity);
        // _eventPublisher.EntityAction(entity, EntityActionEnum.INSERT);

        if (string.IsNullOrEmpty(referenceId))
        {
            return;
        }

        var type = entity.GetType();

        var details = type.GetProperties()
            .Where(prop =>
                prop.CanRead
                && prop.CanWrite
                && prop.Name != "CreatedOnUtc"
                && prop.Name != "UpdatedOnUtc"
            )
            .Select(prop => GenInsertAudit(referenceId, type.Name, entity.Id, prop, entity))
            .Where(detail => detail != null)
            .ToList();

        if (details.Count > 0)
        {
            foreach (var detail in details)
            {
                await detail.Insert(publishEvent: false);
            }

            //await _eventPublisher.EntityInserted<TEntity>(entity, details);
        }
    }

    /// <summary>
    /// Inserts the entity
    /// </summary>
    /// <param name="entity">The entity</param>
    /// <param name="referenceId">The reference id</param>
    /// <param name="publishEvent">The publish event</param>
    /// <param name="isReverse">The is reverse</param>
    /// <param name="isRemove">The is remove</param>
    /// <returns>The entity</returns>
    public virtual async Task<TEntity> InsertAsync(
        TEntity entity,
        string referenceId = "",
        bool publishEvent = true,
        bool isReverse = false,
        bool isRemove = false
    )
    {
        ArgumentNullException.ThrowIfNull(entity);

        entity.SetPropertyDate("CreatedOnUtc", DateTime.UtcNow);
        entity.SetPropertyDate("UpdatedOnUtc", DateTime.UtcNow);

        TEntity entity1 = await this._dataProvider.InsertEntity(entity);

        if (!publishEvent)
        {
            return entity1;
        }

        //await _eventPublisher.EntityInserted(entity);
        // _eventPublisher.EntityAction(entity, EntityActionEnum.INSERT);

        if (string.IsNullOrEmpty(referenceId))
        {
            return entity1;
        }

        var type = entity.GetType();

        var details = type.GetProperties()
            .Where(prop =>
                prop.CanRead
                && prop.CanWrite
                && prop.Name != "CreatedOnUtc"
                && prop.Name != "UpdatedOnUtc"
            )
            .Select(prop => GenInsertAudit(referenceId, type.Name, entity.Id, prop, entity))
            .Where(detail => detail != null)
            .ToList();

        if (details.Count > 0)
        {
            foreach (var detail in details)
            {
                await detail.Insert(publishEvent: false);
            }

            //await _eventPublisher.EntityInserted<TEntity>(entity, details);
        }
        return entity1;
    }

    /// <summary>
    /// Bulks the insert using the specified entities
    /// </summary>
    /// <param name="entities">The entities</param>
    /// <param name="referenceId">The reference id</param>
    /// <param name="publishEvent">The publish event</param>
    public virtual async Task BulkInsert(
        IList<TEntity> entities,
        string referenceId = "",
        bool publishEvent = true
    )
    {
        ArgumentNullException.ThrowIfNull(entities);
        if (entities.Count == 0)
        {
            return;
        }

        using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            await _dataProvider.BulkInsertEntities<TEntity>(entities);
            transaction.Complete();
        }
        if (!publishEvent)
        {
            return;
        }

        foreach (TEntity entity1 in (IEnumerable<TEntity>)entities)
        {
            TEntity entity = entity1;
            //await this._eventPublisher.EntityInserted<TEntity>(entity);
            // this._eventPublisher.EntityAction<TEntity>(entity, EntityActionEnum.INSERT);
        }
    }

    /// <summary>
    /// Updates the entity
    /// </summary>
    /// <param name="entity">The entity</param>
    /// <param name="referenceId">The reference id</param>
    /// <param name="publishEvent">The publish event</param>
    /// <param name="isReverse">The is reverse</param>
    /// <param name="isRemove">The is remove</param>
    public virtual async Task Update(
        TEntity entity,
        string referenceId = "",
        bool publishEvent = true,
        bool isReverse = false,
        bool isRemove = false
    )
    {
        ArgumentNullException.ThrowIfNull(entity);
        TEntity current;
        string updateProperty;
        Type type;
        List<TransactionDetails> details;
        if (isReverse & isRemove)
        {
            await this._dataProvider.DeleteEntity<TEntity>(entity);
            if (!publishEvent)
            {
                current = default(TEntity);
                updateProperty = (string)null;
                type = (Type)null;
                details = (List<TransactionDetails>)null;
            }
            else
            {
                //await this._eventPublisher.EntityDeleted<TEntity>(entity);
                current = default(TEntity);
                updateProperty = (string)null;
                type = (Type)null;
                details = (List<TransactionDetails>)null;
            }
        }
        else
        {
            current = default(TEntity);
            if (publishEvent)
            {
                TEntity entity1 = await this.LoadOriginalCopy(entity);
                current = entity1;
                entity1 = default(TEntity);
            }

            updateProperty = "UpdatedOnUtc";
            entity.SetPropertyDate(updateProperty, DateTime.UtcNow);
            await this._dataProvider.UpdateEntity<TEntity>(entity);

            if (!publishEvent)
            {
                current = default(TEntity);
                updateProperty = (string)null;
                type = (Type)null;
                details = (List<TransactionDetails>)null;
            }
            else
            {
                //await this._eventPublisher.EntityUpdated<TEntity>(entity);
                // this._eventPublisher.EntityAction<TEntity>(entity, EntityActionEnum.UPDATE);

                if (string.IsNullOrEmpty(referenceId))
                {
                    current = default(TEntity);
                    updateProperty = (string)null;
                    type = (Type)null;
                    details = (List<TransactionDetails>)null;
                }
                else if (isReverse)
                {
                    current = default(TEntity);
                    updateProperty = (string)null;
                    type = (Type)null;
                    details = (List<TransactionDetails>)null;
                }
                else if ((object)current == null)
                {
                    current = default(TEntity);
                    updateProperty = (string)null;
                    type = (Type)null;
                    details = (List<TransactionDetails>)null;
                }
                else
                {
                    type = entity.GetType();
                    details = new List<TransactionDetails>();
                    PropertyInfo[] propertyInfoArray = type.GetProperties();
                    for (int index = 0; index < propertyInfoArray.Length; ++index)
                    {
                        PropertyInfo prop = propertyInfoArray[index];
                        if (prop.CanRead && prop.CanWrite && !prop.Name.Equals(updateProperty))
                        {
                            TransactionDetails detail = GenUpdateAudit(
                                referenceId,
                                type.Name,
                                entity.Id,
                                prop,
                                prop.GetValue((object)current, (object[])null),
                                prop.GetValue((object)entity, (object[])null)
                            );
                            if (detail != null)
                            {
                                details.Add(detail);
                                detail = (TransactionDetails)null;
                                prop = (PropertyInfo)null;
                            }
                        }
                    }

                    propertyInfoArray = (PropertyInfo[])null;
                    if (details.Count <= 0)
                    {
                        current = default(TEntity);
                        updateProperty = (string)null;
                        type = (Type)null;
                        details = (List<TransactionDetails>)null;
                    }
                    else
                    {
                        foreach (TransactionDetails detail in details)
                        {
                            await detail.Insert(publishEvent: false);
                        }
                        //await this._eventPublisher.EntityUpdated<TEntity>(entity, details);
                        current = default(TEntity);
                        updateProperty = (string)null;
                        type = (Type)null;
                        details = (List<TransactionDetails>)null;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Sorts the action using the specified action chains
    /// </summary>
    /// <param name="actionChains">The action chains</param>
    /// <returns>The source</returns>
    private static List<ActionChain> SortAction(List<ActionChain> actionChains)
    {
        List<ActionChain> source = new List<ActionChain>();
        string str1 = "xxx";
        Decimal num = 0M;
        string str2 = "";
        foreach (
            ActionChain actionChain in (IEnumerable<ActionChain>)
                actionChains
                    .OrderBy<ActionChain, string>(
                        (Func<ActionChain, string>)(a => a.UpdateField)
                    )
                    .ThenBy<ActionChain, List<string>>(
                        (Func<ActionChain, List<string>>)(a => a.UpdateFields)
                    )
                    .ThenBy<ActionChain, string>((Func<ActionChain, string>)(a => a.Action))
        )
        {
            ActionChain action = actionChain;
            if (
                (action.UpdateFields == null || !action.UpdateFields.Any<string>())
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

                source
                    .Where<ActionChain>(
                        (Func<ActionChain, bool>)(s => s.UpdateField == action.UpdateField)
                    )
                    .First<ActionChain>()
                    .UpdateValue = (object)num;
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

    /// <summary>
    /// Updates the chains using the specified entity
    /// </summary>
    /// <param name="entity">The entity</param>
    /// <param name="actions">The actions</param>
    /// <param name="referenceId">The reference id</param>
    /// <param name="publishEvent">The publish event</param>
    /// <param name="isReverse">The is reverse</param>
    public virtual async Task UpdateChains(
        TEntity entity,
        List<ActionChain> actions,
        string referenceId = "",
        bool publishEvent = true,
        bool isReverse = false
    )
    {
        ArgumentNullException.ThrowIfNull(entity);
        actions = SortAction(actions);
        TEntity current = default(TEntity);
        if (!isReverse & publishEvent)
        {
            TEntity entity1 = await this.LoadOriginalCopy(entity);
            current = entity1;
            entity1 = default(TEntity);
        }

        if (!isReverse)
        {
            await this._dataProvider.UpdateEntityFields<TEntity>(entity, actions);
        }

        Type type;
        List<TransactionDetails> details;
        if (!publishEvent)
        {
            current = default(TEntity);
            type = (Type)null;
            details = (List<TransactionDetails>)null;
        }
        else
        {
            //await this._eventPublisher.EntityUpdated<TEntity>(entity);
            if (string.IsNullOrEmpty(referenceId))
            {
                current = default(TEntity);
                type = (Type)null;
                details = (List<TransactionDetails>)null;
            }
            else if (isReverse)
            {
                current = default(TEntity);
                type = (Type)null;
                details = (List<TransactionDetails>)null;
            }
            else if ((object)current == null)
            {
                current = default(TEntity);
                type = (Type)null;
                details = (List<TransactionDetails>)null;
            }
            else
            {
                type = entity.GetType();
                details = new List<TransactionDetails>();
                foreach (ActionChain action in actions)
                {
                    List<string> fields = action.UpdateFields;
                    if (fields == null || !fields.Any<string>())
                    {
                        fields = new List<string>() { action.UpdateField };
                    }

                    foreach (string str in fields)
                    {
                        string field = str;
                        PropertyInfo prop = type.GetProperty(field);
                        if ((object)prop != null && prop.CanRead && prop.CanWrite)
                        {
                            object currentValue = prop.GetValue(
                                (object)current,
                                (object[])null
                            );
                            TransactionDetails detail = GenUpdateAudit(
                                referenceId,
                                type.Name,
                                entity.Id,
                                prop,
                                currentValue,
                                action.UpdateValue,
                                action.Action
                            );
                            if (detail != null)
                            {
                                details.Add(detail);
                                prop = (PropertyInfo)null;
                                currentValue = (object)null;
                                detail = (TransactionDetails)null;
                                field = (string)null;
                            }
                        }
                    }

                    fields = (List<string>)null;
                }

                if (details.Count <= 0)
                {
                    current = default(TEntity);
                    type = (Type)null;
                    details = (List<TransactionDetails>)null;
                }
                else
                {
                    foreach (TransactionDetails detail in details)
                    {
                        await detail.Insert(publishEvent: false);
                    }
                    //await this._eventPublisher.EntityUpdated<TEntity>(entity, details);
                    current = default(TEntity);
                    type = (Type)null;
                    details = (List<TransactionDetails>)null;
                }
            }
        }
    }

    /// <summary>
    /// Gens the insert audit using the specified ref id
    /// </summary>
    /// <param name="refId">The ref id</param>
    /// <param name="entityName">The entity name</param>
    /// <param name="entityId">The entity id</param>
    /// <param name="prop">The prop</param>
    /// <param name="entity">The entity</param>
    /// <returns>The transaction details</returns>
    private static TransactionDetails GenInsertAudit(
        string refId,
        string entityName,
        int entityId,
        PropertyInfo prop,
        object entity
    )
    {
        object obj = prop.GetValue(entity, (object[])null);
        if (obj == null)
        {
            return (TransactionDetails)null;
        }

        string str = "I";
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
            OldValue = string.Empty,
            NewValue = invariantString,
            Status = "N",
            UpdateType = str,
            Description = "",
        };
    }

    /// <summary>
    /// Compares the values using the specified old value
    /// </summary>
    /// <param name="oldValue">The old value</param>
    /// <param name="newValue">The new value</param>
    /// <returns>The bool</returns>
    private static bool CompareValues(object oldValue, object newValue) => true;

    /// <summary>
    /// Gens the update audit using the specified ref id
    /// </summary>
    /// <param name="refId">The ref id</param>
    /// <param name="entityName">The entity name</param>
    /// <param name="entityId">The entity id</param>
    /// <param name="prop">The prop</param>
    /// <param name="currentValue">The current value</param>
    /// <param name="inputValue">The input value</param>
    /// <param name="updateType">The update type</param>
    /// <returns>The transaction details</returns>
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
                    obj = (object)((Decimal)(currentValue ?? (object)0M) + (Decimal)inputValue);
                    break;
                case "D":
                    obj = (object)((Decimal)(currentValue ?? (object)0M) - (Decimal)inputValue);
                    break;
            }
        }
        else if (prop.PropertyType == typeof(int) || prop.PropertyType == typeof(int?))
        {
            switch (updateType)
            {
                case "C":
                    obj = (object)((int)currentValue + (int)inputValue);
                    break;
                case "D":
                    obj = (object)((int)currentValue - (int)inputValue);
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
            return (TransactionDetails)null;
        }

        if (prop.PropertyType == typeof(Decimal) || prop.PropertyType == typeof(Decimal?))
        {
            Decimal num1 = Convert.ToDecimal(currentValue);
            Decimal num2 = Convert.ToDecimal(obj);
            if (num1 == num2)
            {
                return (TransactionDetails)null;
            }

            updateType = !(num1 > num2) ? "C" : "D";
        }
        else if (prop.PropertyType == typeof(int) || prop.PropertyType == typeof(int?))
        {
            int int32_1 = Convert.ToInt32(currentValue);
            int int32_2 = Convert.ToInt32(obj);
            if (int32_1 == int32_2)
            {
                return (TransactionDetails)null;
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

    /// <summary>
    /// Gens the delete audit using the specified ref id
    /// </summary>
    /// <param name="refId">The ref id</param>
    /// <param name="entityName">The entity name</param>
    /// <param name="entityId">The entity id</param>
    /// <param name="prop">The prop</param>
    /// <param name="entity">The entity</param>
    /// <returns>The transaction details</returns>
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

    /// <summary>
    /// Deletes the entity
    /// </summary>
    /// <param name="entity">The entity</param>
    /// <param name="referenceId">The reference id</param>
    /// <param name="publishEvent">The publish event</param>
    /// <param name="isReverse">The is reverse</param>
    /// <param name="isRemove">The is remove</param>
    public virtual async Task Delete(
        TEntity entity,
        string referenceId = "",
        bool publishEvent = true,
        bool isReverse = false,
        bool isRemove = false
    )
    {
        ArgumentNullException.ThrowIfNull(entity);
        await this._dataProvider.DeleteEntity<TEntity>(entity);
        // this._eventPublisher.EntityAction<TEntity>(entity, EntityActionEnum.DELETE);

        //if (publishEvent)
        //await this._eventPublisher.EntityDeleted<TEntity>(entity);
        string createdProperty;
        string updatedProperty;
        Type type;
        List<TransactionDetails> details;
        if (string.IsNullOrEmpty(referenceId))
        {
            createdProperty = (string)null;
            updatedProperty = (string)null;
            type = (Type)null;
            details = (List<TransactionDetails>)null;
        }
        else
        {
            createdProperty = "CreatedOnUtc";
            updatedProperty = "UpdatedOnUtc";
            type = entity.GetType();
            details = new List<TransactionDetails>();
            PropertyInfo[] propertyInfoArray = type.GetProperties();
            for (int index = 0; index < propertyInfoArray.Length; ++index)
            {
                PropertyInfo prop = propertyInfoArray[index];
                if (
                    prop.CanRead
                    && prop.CanWrite
                    && !prop.Name.Equals(createdProperty)
                    && !prop.Name.Equals(updatedProperty)
                )
                {
                    TransactionDetails detail = GenDeleteAudit(
                        referenceId,
                        type.Name,
                        entity.Id,
                        prop,
                        (object)entity
                    );
                    if (detail != null)
                    {
                        details.Add(detail);
                        detail = (TransactionDetails)null;
                        prop = (PropertyInfo)null;
                    }
                }
            }

            propertyInfoArray = (PropertyInfo[])null;
            if (details.Count <= 0)
            {
                createdProperty = (string)null;
                updatedProperty = (string)null;
                type = (Type)null;
                details = (List<TransactionDetails>)null;
            }
            else
            {
                foreach (TransactionDetails detail in details)
                {
                    await detail.Insert(publishEvent: false);
                }
                //await this._eventPublisher.EntityDeleted<TEntity>(entity, details);
                createdProperty = (string)null;
                updatedProperty = (string)null;
                type = (Type)null;
                details = (List<TransactionDetails>)null;
            }
        }
    }

    /// <summary>
    /// Bulks the delete using the specified entities
    /// </summary>
    /// <param name="entities">The entities</param>
    /// <param name="referenceId">The reference id</param>
    /// <param name="publishEvent">The publish event</param>
    public virtual async Task BulkDelete(
        IList<TEntity> entities,
        string referenceId = "",
        bool publishEvent = true
    )
    {
        ArgumentNullException.ThrowIfNull(entities);
        await this._dataProvider.BulkDeleteEntities<TEntity>(entities);
        if (!publishEvent)
        {
            return;
        }

        foreach (TEntity entity1 in (IEnumerable<TEntity>)entities)
        {
            TEntity entity = entity1;
            //await this._eventPublisher.EntityDeleted<TEntity>(entity);
            // this._eventPublisher.EntityAction<TEntity>(entity, EntityActionEnum.DELETE);

            entity = default(TEntity);
        }
    }

    /// <summary>
    /// Updates the no audit using the specified query
    /// </summary>
    /// <param name="query">The query</param>
    /// <param name="propertyName">The property name</param>
    /// <param name="value">The value</param>
    public virtual async Task UpdateNoAudit(
        IQueryable<TEntity> query,
        string propertyName,
        string value
    )
    {
        ArgumentNullException.ThrowIfNull(query);
        await this._dataProvider.UpdateEntities<TEntity>(query, propertyName, value);
    }

    /// <summary>
    /// Filters the and update using the specified search input
    /// </summary>
    /// <param name="searchInput">The search input</param>
    /// <param name="propertyName">The property name</param>
    /// <param name="value">The value</param>
    public virtual async Task FilterAndUpdate(
        Dictionary<string, string> searchInput,
        string propertyName,
        string value
    )
    {
        IQueryable<TEntity> query = this.TableFilter(searchInput);
        await this.UpdateNoAudit(query, propertyName, value);
        query = (IQueryable<TEntity>)null;
    }

    /// <summary>
    /// Deletes the where using the specified predicate
    /// </summary>
    /// <param name="predicate">The predicate</param>
    /// <param name="referenceId">The reference id</param>
    /// <returns>The num</returns>
    public virtual async Task<int> DeleteWhere(
        Expression<Func<TEntity, bool>> predicate,
        string referenceId = ""
    )
    {
        ArgumentNullException.ThrowIfNull(predicate);
        int num = await this._dataProvider.BulkDeleteEntities<TEntity>(predicate);
        return num;
    }

    /// <summary>
    /// Loads the original copy using the specified entity
    /// </summary>
    /// <param name="entity">The entity</param>
    /// <returns>The entity</returns>
    public virtual async Task<TEntity> LoadOriginalCopy(TEntity entity)
    {
        TEntity entity1 = await this
            ._dataProvider.GetTable<TEntity>()
            .FirstOrDefaultAsync<TEntity>(
                (Expression<Func<TEntity, bool>>)(e => e.Id == Convert.ToInt32(entity.Id))
            );
        return entity1;
    }

    /// <summary>
    /// Truncates the reset identity
    /// </summary>
    /// <param name="resetIdentity">The reset identity</param>
    public virtual async Task Truncate(bool resetIdentity = false)
    {
        await this._dataProvider.Truncate<TEntity>(resetIdentity);
    }

    /// <summary>
    /// Gets the value of the table
    /// </summary>
    public virtual IQueryable<TEntity> Table => this._dataProvider.GetTable<TEntity>();

    /// <summary>
    /// Tables the filter using the specified filter
    /// </summary>
    /// <param name="filter">The filter</param>
    /// <returns>A queryable of t entity</returns>
    public virtual IQueryable<TEntity> TableFilter(Expression<Func<TEntity, bool>> filter)
    {
        return this._dataProvider.GetTable<TEntity>().Where<TEntity>(filter);
    }

    /// <summary>
    /// Tables the filter expression using the specified filter
    /// </summary>
    /// <param name="filter">The filter</param>
    /// <exception cref="NotImplementedException"></exception>
    /// <returns>A queryable of t entity</returns>
    public IQueryable<TEntity> TableFilterExpression(Expression<Func<TEntity, bool>> filter)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Tables the filter using the specified search input
    /// </summary>
    /// <param name="searchInput">The search input</param>
    /// <returns>The source</returns>
    public virtual IQueryable<TEntity> TableFilter(Dictionary<string, string> searchInput)
    {
        IQueryable<TEntity> source = this.Table;
        foreach (KeyValuePair<string, string> keyValuePair in searchInput)
        {
            KeyValuePair<string, string> item = keyValuePair;
            PropertyInfo property = typeof(TEntity).GetProperty(item.Key);
            if (property != (PropertyInfo)null)
            {
                if (property.PropertyType == typeof(string))
                {
                    if (!string.IsNullOrEmpty(item.Value))
                    {
                        source = source.Where<TEntity>(
                            (Expression<Func<TEntity, bool>>)(
                                e => Sql.Property<string>(e, item.Key) == item.Value
                            )
                        );
                    }
                }
                else if (
                    property.PropertyType == typeof(int)
                    && !string.IsNullOrEmpty(item.Value)
                )
                {
                    source = source.Where<TEntity>(
                        (Expression<Func<TEntity, bool>>)(
                            e => Sql.Property<int>(e, item.Key) == int.Parse(item.Value)
                        )
                    );
                }
            }
        }

        return source;
    }

    /// <summary>
    /// Filters the and delete using the specified search input
    /// </summary>
    /// <param name="searchInput">The search input</param>
    /// <exception cref="NotImplementedException"></exception>
    public Task FilterAndDelete(Dictionary<string, string> searchInput)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Gets the table
    /// </summary>
    /// <returns>The table</returns>
    public IQueryable<TEntity> GetTable()
    {
        return Table;
    }

    public virtual async Task UpdateRangeNoAuditAsync(IEnumerable<TEntity> entities)
    {
        foreach (var entity in entities)
        {
            await _dataProvider.UpdateEntity<TEntity>(entity);
        }
    }
}
