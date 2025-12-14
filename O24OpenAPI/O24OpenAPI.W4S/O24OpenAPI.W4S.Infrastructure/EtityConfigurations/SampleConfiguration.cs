using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.W4S.Domain.AggregatesModel.SampleAggregate;

namespace O24OpenAPI.W4S.Infrastructure.EtityConfigurations;

internal class SampleConfiguration : O24OpenAPIEntityBuilder<Sample>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        throw new NotImplementedException();
    }
}
