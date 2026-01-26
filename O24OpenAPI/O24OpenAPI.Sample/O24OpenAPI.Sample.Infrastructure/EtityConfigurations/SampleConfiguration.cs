using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.Sample.Domain.AggregatesModel.SampleAggregate;

namespace O24OpenAPI.Sample.Infrastructure.EtityConfigurations;

internal class SampleConfiguration : O24OpenAPIEntityBuilder<SampleDomain>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        throw new NotImplementedException();
    }
}
