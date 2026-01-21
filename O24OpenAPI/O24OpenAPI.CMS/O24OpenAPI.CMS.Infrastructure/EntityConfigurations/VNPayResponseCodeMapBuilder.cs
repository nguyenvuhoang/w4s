using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.CMS.Domain.AggregateModels.VNPayAggregate;
using O24OpenAPI.Data.Mapping.Builders;

namespace O24OpenAPI.CMS.Infrastructure.EntityConfigurations;

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
