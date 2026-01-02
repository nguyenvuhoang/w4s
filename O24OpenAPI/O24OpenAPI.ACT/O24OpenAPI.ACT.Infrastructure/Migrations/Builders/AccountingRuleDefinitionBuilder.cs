using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.ACT.Domain.AggregatesModel.RulesAggregate;

namespace O24OpenAPI.ACT.Migrations.Builders;

/// <summary>
/// AccountingRuleDefinitionBuilder
/// </summary>
public partial class AccountingRuleDefinitionBuilder : O24OpenAPIEntityBuilder<AccountingRuleDefinition>
{
    /// <summary>
    /// MapEntity
    /// </summary>
    /// <param name="table"></param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(AccountingRuleDefinition.TransactionCode)).AsString(50).NotNullable()
            .WithColumn(nameof(AccountingRuleDefinition.StepCode)).AsString(100).Nullable()
            .WithColumn(nameof(AccountingRuleDefinition.NodeData)).AsString(50).Nullable()
            .WithColumn(nameof(AccountingRuleDefinition.AccountType)).AsString(1).NotNullable().WithDefaultValue("N")
            .WithColumn(nameof(AccountingRuleDefinition.AccountName)).AsString(500).NotNullable()
            .WithColumn(nameof(AccountingRuleDefinition.EntryCondition)).AsString(1000).Nullable()
            .WithColumn(nameof(AccountingRuleDefinition.EntryParameterCode)).AsString(1000).Nullable()
            .WithColumn(nameof(AccountingRuleDefinition.EntryParameterValue)).AsString(1000).Nullable()
            .WithColumn(nameof(AccountingRuleDefinition.DebitOrCredit)).AsString(1).NotNullable()
            .WithColumn(nameof(AccountingRuleDefinition.ValueTypeOfAmount)).AsString(1).NotNullable()
            .WithColumn(nameof(AccountingRuleDefinition.TagOfField)).AsString(2000).Nullable()
            .WithColumn(nameof(AccountingRuleDefinition.DirectValue)).AsString(2000).Nullable()
            .WithColumn(nameof(AccountingRuleDefinition.ValueName)).AsString(2000).Nullable()
            .WithColumn(nameof(AccountingRuleDefinition.AccountingEntryGroup)).AsInt64().NotNullable()
            .WithColumn(nameof(AccountingRuleDefinition.AccountingEntryIndex)).AsInt64().NotNullable().WithDefaultValue(1)
            .WithColumn(nameof(AccountingRuleDefinition.AccountingEntry)).AsString(1).NotNullable()
            .WithColumn(nameof(AccountingRuleDefinition.ModuleCode)).AsString(25).Nullable()
            .WithColumn(nameof(AccountingRuleDefinition.AccountMasterTag)).AsString(100).Nullable()
            .WithColumn(nameof(AccountingRuleDefinition.BaseAmount)).AsString(2000).Nullable()
            .WithColumn(nameof(AccountingRuleDefinition.WorkingBranch)).AsString(200).Nullable()
            .WithColumn(nameof(AccountingRuleDefinition.MasterBranch)).AsString(200).Nullable()
            .WithColumn(nameof(AccountingRuleDefinition.Currency)).AsString(200).Nullable()
            .WithColumn(nameof(AccountingRuleDefinition.Voucher)).AsString(200).Nullable()
            .WithColumn(nameof(AccountingRuleDefinition.IsCompress)).AsBoolean().Nullable().WithDefaultValue(0)
            .WithColumn(nameof(AccountingRuleDefinition.ClearingType)).AsString(2).Nullable()
            .WithColumn(nameof(AccountingRuleDefinition.FXClearingType)).AsString(2).Nullable()
            .WithColumn(nameof(AccountingRuleDefinition.DefAccountNumber)).AsString(36).Nullable()
            .WithColumn(nameof(AccountingRuleDefinition.CreatedOnUtc)).AsDateTime2().Nullable()
            .WithColumn(nameof(AccountingRuleDefinition.UpdatedOnUtc)).AsDateTime2().Nullable();

    }
}
