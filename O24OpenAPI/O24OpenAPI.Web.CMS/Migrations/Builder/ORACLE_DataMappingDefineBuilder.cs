using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Attributes;
using O24OpenAPI.Data.Extensions;
using O24OpenAPI.Data.Mapping.Builders;

namespace O24OpenAPI.Web.CMS.Migrations.Builder;

[DatabaseType(DataProviderType.Oracle)]

public class ORACLE_DataMappingDefineBuilder : O24OpenAPIEntityBuilder<DataMappingDefine>
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
            .AsNCLOB()
            .NotNullable()
            .WithColumn(nameof(DataMappingDefine.To))
            .AsNCLOB()
            .NotNullable()
            .WithColumn(nameof(DataMappingDefine.Enable))
            .AsBoolean()
            .NotNullable()
            .WithColumn(nameof(DataMappingDefine.TwoSide))
            .AsBoolean()
            .NotNullable();
    }
}
