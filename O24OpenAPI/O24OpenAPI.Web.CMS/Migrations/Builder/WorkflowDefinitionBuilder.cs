using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;

namespace O24OpenAPI.Web.CMS.Migrations.Builder;

public class WorkflowDefinitionBuilder : O24OpenAPIEntityBuilder<WorkflowDefinition>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(WorkflowDefinition.WFId))
            .AsString(50)
            .NotNullable()
            .WithColumn(nameof(WorkflowDefinition.WFName))
            .AsString(250)
            .NotNullable()
            .WithColumn(nameof(WorkflowDefinition.WFDescription))
            .AsString(1000)
            .Nullable()
            .WithColumn(nameof(WorkflowDefinition.AppCode))
            .AsString(50)
            .NotNullable()
            .WithColumn(nameof(WorkflowDefinition.Status))
            .AsBoolean()
            .NotNullable()
            .WithColumn(nameof(WorkflowDefinition.TimeOut))
            .AsInt64()
            .NotNullable()
            .WithColumn(nameof(WorkflowDefinition.IsReverse))
            .AsBoolean()
            .NotNullable()
            .WithColumn(nameof(WorkflowDefinition.LogUserAction))
            .AsBoolean()
            .NotNullable()
            .WithColumn(nameof(WorkflowDefinition.CreatedOnUtc))
            .AsDateTime2()
            .Nullable()
            .WithColumn(nameof(WorkflowDefinition.UpdatedOnUtc))
            .AsDateTime2()
            .Nullable();
    }
}
