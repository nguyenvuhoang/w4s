using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.PMT.Domain.AggregatesModel.VNPayAggregate;

namespace O24OpenAPI.PMT.Infrastructure.EntityConfigurations;

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
