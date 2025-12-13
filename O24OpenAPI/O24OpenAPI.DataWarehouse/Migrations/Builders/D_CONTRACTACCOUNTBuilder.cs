using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;

namespace O24OpenAPI.DataWarehouse.Migrations.Builders;

public class D_CONTRACTACCOUNTBuilder : O24OpenAPIEntityBuilder<D_CONTRACTACCOUNT>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(D_CONTRACTACCOUNT.ContractNumber))
            .AsString(50)
            .NotNullable()
            .WithColumn(nameof(D_CONTRACTACCOUNT.AccountNumber))
            .AsString(50)
            .NotNullable()
            .WithColumn(nameof(D_CONTRACTACCOUNT.AccountType))
            .AsString(50)
            .NotNullable()
            .WithColumn(nameof(D_CONTRACTACCOUNT.CurrencyCode))
            .AsString(50)
            .NotNullable()
            .WithColumn(nameof(D_CONTRACTACCOUNT.Status))
            .AsString(50)
            .NotNullable()
            .WithColumn(nameof(D_CONTRACTACCOUNT.BranchID))
            .AsString(50)
            .Nullable()
            .WithColumn(nameof(D_CONTRACTACCOUNT.BankAccountType))
            .AsString(50)
            .Nullable()
            .WithColumn(nameof(D_CONTRACTACCOUNT.BankId))
            .AsString(50)
            .Nullable()
            .WithColumn(nameof(D_CONTRACTACCOUNT.IsPrimary))
            .AsBoolean()
            .Nullable();
    }
}
