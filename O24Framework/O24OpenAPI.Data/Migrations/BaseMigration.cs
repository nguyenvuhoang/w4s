using FluentMigrator;
using FluentMigrator.Infrastructure;
using LinqToDB;
using O24OpenAPI.Core.Domain;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Data.Extensions;
using O24OpenAPI.Data.Utils;
using System.Linq.Expressions;
using System.Reflection;

namespace O24OpenAPI.Data.Migrations;

/// <summary>
/// The base migration class
/// </summary>
/// <seealso cref="Migration"/>
public abstract class BaseMigration : Migration
{
    /// <summary>
    ///
    /// /// </summary>
    public abstract override void Up();

    /// <inheritdoc />
    public sealed override void Down() { }

    /// <summary>
    /// Gets the down expressions using the specified context
    /// </summary>
    /// <param name="context">The context</param>
    public override void GetDownExpressions(IMigrationContext context)
    {
        GetUpExpressions(context);
        context.Expressions = context.Expressions.Select(e => e.Reverse()).Reverse().ToList();
    }

    /// <summary>
    /// The data provider
    /// </summary>
    private readonly IO24OpenAPIDataProvider _dataProvider =
        EngineContext.Current.Resolve<IO24OpenAPIDataProvider>();

    /// <summary>
    /// Gets the value of the data provider
    /// </summary>
    public IO24OpenAPIDataProvider DataProvider
    {
        get { return _dataProvider; }
    }

    /// <summary>
    /// ///
    /// </summary>
    public IQueryable<TEntity> GetTable<TEntity>()
        where TEntity : BaseEntity
    {
        return _dataProvider.GetTable<TEntity>();
    }

    /// <summary>
    /// Seeds the data using the specified entities
    /// </summary>
    /// <typeparam name="TEntity">The entity</typeparam>
    /// <param name="entities">The entities</param>
    /// <param name="conditionKeys">The condition keys</param>
    /// <param name="isTruncated">The is truncated</param>
    /// <exception cref="InvalidOperationException">Entity {typeof(TEntity).Name} does not have properties with the specified key names.</exception>
    private async Task SeedData<TEntity>(
        List<TEntity> entities,
        List<string> conditionKeys = null,
        bool isTruncated = false
    )
        where TEntity : BaseEntity
    {
        if (entities.Count == 0)
        {
            return;
        }

        if (isTruncated)
        {
            if (Schema.Table(typeof(TEntity).Name).Exists())
            {
                await _dataProvider.Truncate<TEntity>();
                await _dataProvider.BulkInsertEntities(entities);
            }
        }
        else
        {
            var keyProperties = typeof(TEntity)
                .GetProperties()
                .Where(prop => conditionKeys.Contains(prop.Name))
                .ToList();

            if (keyProperties.Count == 0)
            {
                throw new InvalidOperationException(
                    $"Entity {typeof(TEntity).Name} does not have properties with the specified key names."
                );
            }

            foreach (var item in entities)
            {
                var predicate = BuildPredicate(keyProperties, item);

                var old = await _dataProvider
                    .GetTable<TEntity>()
                    .Where(predicate)
                    .ToListAsync();
                if (old != null && old.Count > 0)
                {
                    try
                    {
                        var oldItem = old.FirstOrDefault();
                        if (oldItem != null)
                        {
                            var createdOnUtc = oldItem.GetProperty<DateTime>("CreatedOnUtc");
                            if (createdOnUtc != default)
                            {
                                item.SetPropertyDate("CreatedOnUtc", createdOnUtc);
                            }
                            else
                            {
                                item.SetPropertyDate("CreatedOnUtc", DateTime.UtcNow);
                            }
                            item.SetPropertyDate("UpdatedOnUtc", DateTime.UtcNow);
                            item.SetProperty("Id", oldItem.GetProperty<int>("Id"));
                            await _dataProvider.UpdateEntity(item);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(
                            $"Error updating entity {typeof(TEntity).Name}: {ex.Message}"
                        );
                        await _dataProvider.BulkDeleteEntities(old);
                        item.SetPropertyDate("CreatedOnUtc", DateTime.UtcNow);
                        item.SetPropertyDate("UpdatedOnUtc", DateTime.UtcNow);
                        await _dataProvider.InsertEntity(item);
                    }
                }
                else
                {
                    item.SetPropertyDate("CreatedOnUtc", DateTime.UtcNow);
                    item.SetPropertyDate("UpdatedOnUtc", DateTime.UtcNow);
                    await _dataProvider.InsertEntity(item);
                }
            }
        }
    }

    /// <summary>
    /// Bulks the delete using the specified predicate
    /// </summary>
    /// <typeparam name="TEntity">The entity</typeparam>
    /// <param name="predicate">The predicate</param>
    public async Task BulkDelete<TEntity>(Expression<Func<TEntity, bool>> predicate)
        where TEntity : BaseEntity
    {
        await _dataProvider.BulkDeleteEntities(predicate);
    }

    /// <summary>
    /// Builds the predicate using the specified key properties
    /// </summary>
    private static Expression<Func<TEntity, bool>> BuildPredicate<TEntity>(
        List<PropertyInfo> keyProperties,
        TEntity item
    )
    {
        var parameter = Expression.Parameter(typeof(TEntity), "x");
        Expression predicate = Expression.Constant(true);

        foreach (var keyProperty in keyProperties)
        {
            var left = Expression.Property(parameter, keyProperty);
            var right = Expression.Constant(keyProperty.GetValue(item));
            var equality = Expression.Equal(left, right);

            predicate = Expression.AndAlso(predicate, equality);
        }

        return Expression.Lambda<Func<TEntity, bool>>(predicate, parameter);
    }

    /// <summary>
    ///
    /// </summary>
    public async Task SeedDataCSV<TEntity>(
        string filePath,
        List<string> conditionKeys,
        bool isTruncate = false
    )
        where TEntity : BaseEntity
    {
        var data = await FileUtils.ReadCSV<TEntity>(filePath);

        await SeedData(data, conditionKeys, isTruncate);
    }

    /// <summary>
    ///
    /// </summary>
    public async Task SeedJsonFolder<TEntity>(
        string folderPath,
        List<string> conditionKeys,
        bool isTruncate = false
    )
        where TEntity : BaseEntity
    {
        string[] jsonFiles = Directory.GetFiles(folderPath, "*.json");

        List<TEntity> data = new();
        foreach (var filePath in jsonFiles)
        {
            var dataFile = await FileUtils.ReadJson<TEntity>(filePath);
            data.AddRange(dataFile);
        }
        await SeedData(data, conditionKeys, isTruncate);
    }

    /// <summary>
    ///
    /// </summary>
    public async Task SeedDataJson<TEntity>(
        string filePath,
        List<string> conditionKeys,
        bool isTruncate = false
    )
        where TEntity : BaseEntity
    {
        var data = await FileUtils.ReadJson<TEntity>(filePath);
        if (data.Count == 0)
        {
            throw new Exception("No data found in " + filePath);
        }
        await SeedData<TEntity>(data, conditionKeys, isTruncate);
    }

    /// <summary>
    ///
    /// </summary>
    public async Task SeedListData<TEntity>(
        List<TEntity> entities,
        List<string> conditionKeys,
        bool isTruncate = false
    )
        where TEntity : BaseEntity
    {
        await SeedData<TEntity>(entities, conditionKeys, isTruncate);
    }

    /// <summary>
    ///
    /// </summary>
    public bool CheckExistData<TEntity>()
        where TEntity : BaseEntity
    {
        return _dataProvider.GetTable<TEntity>().Any();
    }

    /// <summary>
    /// Executes the script using the specified path to script file
    /// </summary>
    /// <param name="pathToScriptFile">The path to script file</param>
    public void ExecuteScript(string pathToScriptFile)
    {
        base.Execute.Script(pathToScriptFile);
    }
}
