using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.ACT.Domain;

namespace O24OpenAPI.ACT.Migrations.Builders;

/// <summary>
/// AccountCommonBuilder
/// </summary>
public partial class AccountCommonBuilder : O24OpenAPIEntityBuilder<AccountCommon>
{
    /// <summary>
    /// MapEntity
    /// </summary>
    /// <param name="table"></param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(AccountCommon.AccountNumber)).AsString(100).Nullable()
            .WithColumn(nameof(AccountCommon.AccountName)).AsString(50).NotNullable()
            .WithColumn(nameof(AccountCommon.RefAccountNumber)).AsString(25).Nullable()
            .WithColumn(nameof(AccountCommon.RefAccountNumber2)).AsString(25).Nullable()
            .WithColumn(nameof(AccountCommon.CreatedOnUtc)).AsDateTime2().Nullable()
            .WithColumn(nameof(AccountCommon.UpdatedOnUtc)).AsDateTime2().Nullable();
    }
}
