using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data;
using O24OpenAPI.Data.Attributes;
using O24OpenAPI.Data.Extensions;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.WFO.Domain.AggregateModels.WorkflowAggregate;

namespace O24OpenAPI.WFO.API.Application.Migrations.Builder;

[DatabaseType(DataProviderType.Oracle)]
public class ORACLE_WorkflowStepBuilder : O24OpenAPIEntityBuilder<WorkflowStep>
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
            .AsString(2000)
            .Nullable()
            .WithColumn(nameof(WorkflowStep.SendingTemplate))
            .AsNCLOB()
            .Nullable()
            .WithColumn(nameof(WorkflowStep.SubSendingTemplate))
            .AsNCLOB()
            .Nullable()
            .WithColumn(nameof(WorkflowStep.MappingResponse))
            .AsNCLOB()
            .Nullable()
            .WithColumn(nameof(WorkflowStep.StepTimeout))
            .AsInt32()
            .Nullable()
            .WithColumn(nameof(WorkflowStep.SendingCondition))
            .AsNCLOB()
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
