using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data;
using O24OpenAPI.Data.Attributes;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.WFO.Domain;

namespace O24OpenAPI.WFO.Migrations.Builder;

[DatabaseType(DataProviderType.SqlServer)]
public class WorkflowStepBuilder : O24OpenAPIEntityBuilder<WorkflowStep>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(WorkflowStep.WorkflowId))
            .AsString(100)
            .NotNullable()
            .WithColumn(nameof(WorkflowStep.StepCode))
            .AsString(100)
            .Nullable()
            .WithColumn(nameof(WorkflowStep.StepOrder))
            .AsInt16()
            .Nullable()
            .WithColumn(nameof(WorkflowStep.ServiceId))
            .AsString(100)
            .Nullable()
            .WithColumn(nameof(WorkflowStep.Status))
            .AsBoolean()
            .NotNullable()
            .WithColumn(nameof(WorkflowStep.Description))
            .AsString(4000)
            .Nullable()
            .WithColumn(nameof(WorkflowStep.SendingTemplate))
            .AsString(int.MaxValue)
            .Nullable()
            .WithColumn(nameof(WorkflowStep.SubSendingTemplate))
            .AsString(int.MaxValue)
            .Nullable()
            .WithColumn(nameof(WorkflowStep.MappingResponse))
            .AsString(int.MaxValue)
            .Nullable()
            .WithColumn(nameof(WorkflowStep.StepTimeout))
            .AsInt32()
            .Nullable()
            .WithColumn(nameof(WorkflowStep.SendingCondition))
            .AsString(int.MaxValue)
            .Nullable()
            .WithColumn(nameof(WorkflowStep.ProcessingNumber))
            .AsInt16()
            .Nullable()
            .WithColumn(nameof(WorkflowStep.IsReverse))
            .AsBoolean()
            .Nullable()
            .WithColumn(nameof(WorkflowStep.ShouldAwaitStep))
            .AsBoolean()
            .Nullable();
    }
}
