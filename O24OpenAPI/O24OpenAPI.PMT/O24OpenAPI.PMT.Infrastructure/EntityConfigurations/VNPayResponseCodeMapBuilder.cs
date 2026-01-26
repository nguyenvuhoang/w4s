using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.PMT.Domain.AggregatesModel.VNPayAggregate;

namespace O24OpenAPI.PMT.Infrastructure.EntityConfigurations;

public class VNPayResponseCodeMapBuilder : O24OpenAPIEntityBuilder<VNPayResponseCodeMap>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(VNPayResponseCodeMap.ResponseCode))
            .AsString(50)
            .NotNullable()
            .WithColumn(nameof(VNPayResponseCodeMap.Description))
            .AsString(250)
            .NotNullable();
    }
}
