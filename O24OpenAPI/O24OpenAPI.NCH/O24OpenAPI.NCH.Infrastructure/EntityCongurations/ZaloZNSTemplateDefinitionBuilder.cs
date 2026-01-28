using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.NCH.Domain.AggregatesModel.ZaloAggregate;

namespace O24OpenAPI.NCH.Infrastructure.EntityCongurations;

public class ZaloZNSTemplateDefinitionBuilder : O24OpenAPIEntityBuilder<ZaloZNSTemplateDefinition>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(ZaloZNSTemplateDefinition.Id)).AsInt32().PrimaryKey().Identity()

            .WithColumn(nameof(ZaloZNSTemplateDefinition.OaId)).AsString(50).NotNullable()
            .WithColumn(nameof(ZaloZNSTemplateDefinition.TemplateCode)).AsString(50).NotNullable()

            .WithColumn(nameof(ZaloZNSTemplateDefinition.TemplateName)).AsString(60).NotNullable()
            .WithColumn(nameof(ZaloZNSTemplateDefinition.TemplateType)).AsInt32().NotNullable()
            .WithColumn(nameof(ZaloZNSTemplateDefinition.Tag)).AsString(5).NotNullable()

            .WithColumn(nameof(ZaloZNSTemplateDefinition.LayoutJson)).AsString(int.MaxValue).NotNullable()
            .WithColumn(nameof(ZaloZNSTemplateDefinition.ParamsJson)).AsString(int.MaxValue).Nullable()
            .WithColumn(nameof(ZaloZNSTemplateDefinition.Note)).AsString(400).Nullable()

            .WithColumn(nameof(ZaloZNSTemplateDefinition.IsActive)).AsBoolean().NotNullable().WithDefaultValue(true)

            .WithColumn(nameof(ZaloZNSTemplateDefinition.CreatedOnUtc)).AsDateTime2().NotNullable()
            .WithColumn(nameof(ZaloZNSTemplateDefinition.UpdatedOnUtc)).AsDateTime2().Nullable();
    }
}
