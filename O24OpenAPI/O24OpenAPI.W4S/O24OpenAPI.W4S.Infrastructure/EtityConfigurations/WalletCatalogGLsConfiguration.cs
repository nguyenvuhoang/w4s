using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.W4S.Domain.AggregatesModel.WalletMasterAggregate;

namespace O24OpenAPI.W4S.Infrastructure.EtityConfigurations;

/// <summary>
/// WalletCatalogGLs builder
/// </summary>
public partial class WalletCatalogGLsConfiguration : O24OpenAPIEntityBuilder<WalletCatalogGLs>
{
    /// <summary>
    /// Map entity
    /// </summary>
    /// <param name="table"></param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(WalletCatalogGLs.CatalogCode)).AsString(100).Nullable()
            .WithColumn(nameof(WalletCatalogGLs.SysAccountName)).AsString(100).Nullable()
            .WithColumn(nameof(WalletCatalogGLs.COAAccount)).AsString(100).Nullable()
            .WithColumn(nameof(WalletCatalogGLs.AccountAlias)).AsString(100).Nullable();
    }
}
