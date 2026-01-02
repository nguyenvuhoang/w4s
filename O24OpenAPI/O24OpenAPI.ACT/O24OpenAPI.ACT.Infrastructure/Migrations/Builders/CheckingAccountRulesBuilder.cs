using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.ACT.Domain.AggregatesModel.RulesAggregate;

namespace O24OpenAPI.ACT.Migrations.Builders;


/// <summary>
/// Stock Transaction builder
/// </summary>
public partial class CheckingAccountRulesBuilder : O24OpenAPIEntityBuilder<CheckingAccountRules>
{
    /// <summary>
    /// Map entity
    /// </summary>
    /// <param name="table"></param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(CheckingAccountRules.AccountClassification)).AsString(1).Nullable()
            .WithColumn(nameof(CheckingAccountRules.ReverseBalance)).AsString(1).Nullable()
            .WithColumn(nameof(CheckingAccountRules.BalanceSide)).AsString(1).Nullable()
            .WithColumn(nameof(CheckingAccountRules.PostingSide)).AsString(1).Nullable()
            .WithColumn(nameof(CheckingAccountRules.AccountGroup)).AsString(1).Nullable()
            .WithColumn(nameof(CheckingAccountRules.AccountCategories)).AsString(1).Nullable()
            .WithColumn(nameof(CheckingAccountRules.CreatedOnUtc)).AsDateTime2().Nullable()
            .WithColumn(nameof(CheckingAccountRules.UpdatedOnUtc)).AsDateTime2().Nullable();
    }
}
