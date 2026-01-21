using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.PMT.Domain.AggregatesModel.PMTAggregate;

namespace O24OpenAPI.PMT.Infrastructure.EntityConfigurations;

internal class PMTConfiguration : O24OpenAPIEntityBuilder<PMTDomain>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        // TODO: Define table structure
        throw new NotImplementedException();
    }
}
