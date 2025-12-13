using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;

namespace O24OpenAPI.Web.CMS.Migrations.Builder;

public class LearnApiCacheBuilder : O24OpenAPIEntityBuilder<LearnApiCache>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(LearnApiCache.LearnApiId))
            .AsString(700)
            .NotNullable()
            .WithColumn(nameof(LearnApiCache.LearnApiIdClear))
            .AsString(700)
            .NotNullable()
            .WithColumn(nameof(LearnApiCache.CacheTime))
            .AsInt32()
            .Nullable();
    }
}
