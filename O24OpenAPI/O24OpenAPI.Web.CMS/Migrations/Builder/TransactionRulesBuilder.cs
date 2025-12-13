using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;

namespace O24OpenAPI.Web.CMS.Migrations.Builder;

public class TransactionRulesBuilder : O24OpenAPIEntityBuilder<TransactionRules>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(TransactionRules.WorkflowId))
            .AsString(50)
            .NotNullable()
            .WithColumn(nameof(TransactionRules.RuleName))
            .AsString(50)
            .NotNullable()
            .WithColumn(nameof(TransactionRules.Parameter))
            .AsString(3000)
            .Nullable()
            .WithColumn(nameof(TransactionRules.RuleOrder))
            .AsInt32()
            .NotNullable()
            .WithColumn(nameof(TransactionRules.IsEnable))
            .AsBoolean()
            .NotNullable()
            .WithColumn(nameof(TransactionRules.Spec))
            .AsString(1000)
            .Nullable()
            .WithColumn(nameof(TransactionRules.Example))
            .AsString(1000)
            .Nullable()
            .WithColumn(nameof(TransactionRules.Caption))
            .AsString(500)
            .Nullable()
            .WithColumn(nameof(TransactionRules.UpdatedOnUtc))
            .AsDateTime2()
            .Nullable()
            .WithColumn(nameof(TransactionRules.Condition))
            .AsString(3000)
            .Nullable();
    }
}
