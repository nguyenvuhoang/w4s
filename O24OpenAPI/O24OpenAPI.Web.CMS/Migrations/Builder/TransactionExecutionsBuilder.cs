using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;

namespace O24OpenAPI.Web.CMS.Migrations.Builder;

public class TransactionExecutionsBuilder : O24OpenAPIEntityBuilder<TransactionExecutions>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(TransactionExecutions.WorkflowId))
            .AsString(50)
            .NotNullable()
            .WithColumn(nameof(TransactionExecutions.StoreProcedure))
            .AsString(100)
            .NotNullable()
            .WithColumn(nameof(TransactionExecutions.Parameter))
            .AsString(3000)
            .Nullable()
            .WithColumn(nameof(TransactionExecutions.ExecOrder))
            .AsInt32()
            .NotNullable()
            .WithColumn(nameof(TransactionExecutions.IsEnable))
            .AsBoolean()
            .NotNullable()
            .WithColumn(nameof(TransactionExecutions.Condition))
            .AsString(3000)
            .Nullable()
            .WithColumn(nameof(TransactionExecutions.UpdatedOnUtc))
            .AsDateTime2()
            .Nullable();
    }
}
