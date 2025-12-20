using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.CMS.Domain.AggregateModels;
using O24OpenAPI.Data.Mapping.Builders;

namespace O24OpenAPI.CMS.Infrastructure.EntityConfigurations;

public partial class ParaServerBuilder : O24OpenAPIEntityBuilder<ParaServer>
{
    /// <summary>
    /// MapEntity
    /// </summary>
    /// <param name="table">The table.</param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(ParaServer.Code))
            .AsString(70)
            .WithColumn(nameof(ParaServer.Description))
            .AsString(70)
            .WithColumn(nameof(ParaServer.DataType))
            .AsString(7)
            .WithColumn(nameof(ParaServer.Value))
            .AsAnsiString(1000)
            .WithColumn(nameof(ParaServer.App))
            .AsString(70);
    }
}
