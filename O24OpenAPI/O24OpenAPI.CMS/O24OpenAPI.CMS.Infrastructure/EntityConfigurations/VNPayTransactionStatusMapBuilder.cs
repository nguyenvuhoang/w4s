using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.CMS.Domain.AggregateModels.VNPayAggregate;
using O24OpenAPI.Data.Mapping.Builders;

namespace O24OpenAPI.CMS.Infrastructure.EntityConfigurations;

public class VNPayTransactionStatusMapBuilder : O24OpenAPIEntityBuilder<VNPayTransactionStatusMap>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(VNPayTransactionStatusMap.StatusCode))
            .AsString(50)
            .NotNullable()
            .WithColumn(nameof(VNPayTransactionStatusMap.StatusMessage))
            .AsString(250)
            .NotNullable();
    }
}
