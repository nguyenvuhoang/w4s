using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.CTH.Domain.AggregatesModel.SampleAggregate;
using O24OpenAPI.Data.Mapping.Builders;

namespace O24OpenAPI.CTH.Infrastructure.EtityConfigurations;

internal class SampleConfiguration : O24OpenAPIEntityBuilder<Sample>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        throw new NotImplementedException();
    }
}
