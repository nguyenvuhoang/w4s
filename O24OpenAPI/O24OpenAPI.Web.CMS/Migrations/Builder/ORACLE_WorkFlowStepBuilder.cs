using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Attributes;
using O24OpenAPI.Data.Extensions;
using O24OpenAPI.Data.Mapping.Builders;

namespace O24OpenAPI.Web.CMS.Migrations.Builder;

[DatabaseType(DataProviderType.Oracle)]

internal partial class ORACLE_WorkFlowStepBuilder : O24OpenAPIEntityBuilder<WorkflowStep>
{
    /// <summary>
    /// MapEntity
    /// </summary>
    /// <param name="table">The table.</param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(WorkflowStep.WFId))
            .AsString(100)
            .NotNullable()
            .WithColumn(nameof(WorkflowStep.StepOrder))
            .AsInt16()
            .Nullable()
            .WithColumn(nameof(WorkflowStep.StepCode))
            .AsString(100)
            .Nullable()
            .WithColumn(nameof(WorkflowStep.ServiceID))
            .AsString(100)
            .Nullable()
            .WithColumn(nameof(WorkflowStep.Status))
            .AsBoolean()
            .NotNullable()
            .WithColumn(nameof(WorkflowStep.Description))
            .AsString(2000)
            .Nullable()
            .WithColumn(nameof(WorkflowStep.AppCode))
            .AsString(100)
            .Nullable()
            .WithColumn(nameof(WorkflowStep.SendingTemplate))
            .AsNCLOB()
            .Nullable()
            .WithColumn(nameof(WorkflowStep.MappingResponse))
            .AsNCLOB()
            .Nullable()
            .WithColumn(nameof(WorkflowStep.RequestProtocol))
            .AsString(1000)
            .Nullable()
            .WithColumn(nameof(WorkflowStep.RequestServerIp))
            .AsString(100)
            .Nullable()
            .WithColumn(nameof(WorkflowStep.RequestServerPort))
            .AsString(100)
            .Nullable()
            .WithColumn(nameof(WorkflowStep.RequestUri))
            .AsString(100)
            .Nullable()
            .WithColumn(nameof(WorkflowStep.IsReverse))
            .AsBoolean()
            .Nullable()
            .WithColumn(nameof(WorkflowStep.ShouldAwaitStep))
            .AsBoolean()
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
            .WithColumn(nameof(WorkflowStep.FullClassName))
            .AsString(1000)
            .Nullable()
            .WithColumn(nameof(WorkflowStep.MethodName))
            .AsString(1000)
            .Nullable();
    }
}
