using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.W4S.Domain.AggregatesModel.WalletMasterAggregate;

namespace O24OpenAPI.W4S.Infrastructure.EtityConfigurations;

/// <summary>
/// DepositAccountGLs builder
/// </summary>
public partial class WalletAccountGLsConfiguration : O24OpenAPIEntityBuilder<WalletAccountGLs>
{
    /// <summary>
    /// Map entity
    /// </summary>
    /// <param name="table"></param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(WalletAccountGLs.WalletAccount)).AsString(100).NotNullable()
            .WithColumn(nameof(WalletAccountGLs.SysAccountName)).AsString(100).NotNullable()
            .WithColumn(nameof(WalletAccountGLs.CatalogCode)).AsString(100).NotNullable()
            .WithColumn(nameof(WalletAccountGLs.GLAccount)).AsString(100).NotNullable();
    }
}
