using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.CMS.Domain.AggregateModels.AppAggregate;
using O24OpenAPI.Data;
using O24OpenAPI.Data.Attributes;
using O24OpenAPI.Data.Extensions;
using O24OpenAPI.Data.Mapping.Builders;

namespace O24OpenAPI.CMS.Infrastructure.EntityConfigurations;

[DatabaseType(DataProviderType.Oracle)]
public class ORACLE_LearnApiBuilder : O24OpenAPIEntityBuilder<LearnApi>
{
    /// <summary>
    /// MapEntity
    /// </summary>
    /// <param name="table">The table.</param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(LearnApi.LearnApiId))
            .AsString(700)
            .WithColumn(nameof(LearnApi.LearnApiName))
            .AsString(700)
            .Nullable()
            .WithColumn(nameof(LearnApi.LearnApiData))
            .AsString(700)
            .Nullable()
            .WithColumn(nameof(LearnApi.LearnApiNodeData))
            .AsString(700)
            .Nullable()
            .WithColumn(nameof(LearnApi.LearnApiMethod))
            .AsString(700)
            .Nullable()
            .WithColumn(nameof(LearnApi.LearnApiHeader))
            .AsString(7000)
            .Nullable()
            .WithColumn(nameof(LearnApi.LearnApiMapping))
            .AsString(70000)
            .Nullable()
            .WithColumn(nameof(LearnApi.IsCache))
            .AsBoolean()
            .Nullable()
            .WithColumn(nameof(LearnApi.KeyReadData))
            .AsString(700)
            .Nullable()
            .WithColumn(nameof(LearnApi.LearnApiIdClear))
            .AsNCLOB()
            .Nullable()
            .WithColumn(nameof(LearnApi.Channel))
            .AsString(70)
            .WithColumn(nameof(LearnApi.LearnApiMappingResponse))
            .AsString(70000)
            .Nullable()
            .WithColumn(nameof(LearnApi.FullInterfaceName))
            .AsString(700)
            .Nullable()
            .WithColumn(nameof(LearnApi.MethodName))
            .AsString(700)
            .Nullable()
            .WithColumn(nameof(LearnApi.URI))
            .AsString(1000)
            .Nullable();
    }
}

[DatabaseType(DataProviderType.SqlServer)]
public class SQLSERVER_LearnApiBuilder : O24OpenAPIEntityBuilder<LearnApi>
{
    /// <summary>
    /// MapEntity
    /// </summary>
    /// <param name="table">The table.</param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(LearnApi.LearnApiId))
            .AsString(700)
            .WithColumn(nameof(LearnApi.LearnApiName))
            .AsString(700)
            .Nullable()
            .WithColumn(nameof(LearnApi.LearnApiData))
            .AsString(700)
            .Nullable()
            .WithColumn(nameof(LearnApi.LearnApiNodeData))
            .AsString(700)
            .Nullable()
            .WithColumn(nameof(LearnApi.LearnApiMethod))
            .AsString(700)
            .Nullable()
            .WithColumn(nameof(LearnApi.LearnApiHeader))
            .AsString(7000)
            .Nullable()
            .WithColumn(nameof(LearnApi.LearnApiMapping))
            .AsString(70000)
            .Nullable()
            .WithColumn(nameof(LearnApi.IsCache))
            .AsBoolean()
            .Nullable()
            .WithColumn(nameof(LearnApi.KeyReadData))
            .AsString(700)
            .Nullable()
            .WithColumn(nameof(LearnApi.LearnApiIdClear))
            .AsString(int.MaxValue)
            .Nullable()
            .WithColumn(nameof(LearnApi.Channel))
            .AsString(70)
            .WithColumn(nameof(LearnApi.LearnApiMappingResponse))
            .AsString(70000)
            .Nullable()
            .WithColumn(nameof(LearnApi.FullInterfaceName))
            .AsString(700)
            .Nullable()
            .WithColumn(nameof(LearnApi.MethodName))
            .AsString(700)
            .Nullable()
            .WithColumn(nameof(LearnApi.URI))
            .AsString(1000)
            .Nullable();
    }
}
