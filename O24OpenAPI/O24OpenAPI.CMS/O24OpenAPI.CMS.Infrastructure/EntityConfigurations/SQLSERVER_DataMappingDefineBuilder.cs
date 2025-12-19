using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.CMS.Domain;
using O24OpenAPI.Data;
using O24OpenAPI.Data.Attributes;
using O24OpenAPI.Data.Mapping.Builders;

namespace O24OpenAPI.CMS.Infrastructure.EntityConfigurations;

[DatabaseType(DataProviderType.SqlServer)]
public class SQLSERVER_DataMappingDefineBuilder : O24OpenAPIEntityBuilder<DataMappingDefine>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(DataMappingDefine.ServiceId))
            .AsString(20)
            .NotNullable()
            .WithColumn(nameof(DataMappingDefine.WorkflowId))
            .AsString(200)
            .NotNullable()
            .WithColumn(nameof(DataMappingDefine.From))
            .AsString(int.MaxValue)
            .NotNullable()
            .WithColumn(nameof(DataMappingDefine.To))
            .AsString(int.MaxValue)
            .NotNullable()
            .WithColumn(nameof(DataMappingDefine.Enable))
            .AsBoolean()
            .NotNullable()
            .WithColumn(nameof(DataMappingDefine.TwoSide))
            .AsBoolean()
            .NotNullable();
    }
}
