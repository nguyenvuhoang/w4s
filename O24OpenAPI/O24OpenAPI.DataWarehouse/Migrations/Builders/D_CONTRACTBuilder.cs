using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.DataWarehouse.Domain;

namespace O24OpenAPI.DataWarehouse.Migrations.Builders;

public class D_CONTRACTBuilder : O24OpenAPIEntityBuilder<D_CONTRACT>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(D_CONTRACT.ContractNumber)).AsString(50).NotNullable()
            .WithColumn(nameof(D_CONTRACT.ContractCode)).AsString(50).Nullable()
            .WithColumn(nameof(D_CONTRACT.CustomerCode)).AsString(50).NotNullable()
            .WithColumn(nameof(D_CONTRACT.ContractType)).AsString(20).Nullable()
            .WithColumn(nameof(D_CONTRACT.UserType)).AsString(20).NotNullable()
            .WithColumn(nameof(D_CONTRACT.ProductID)).AsString(50).Nullable()
            .WithColumn(nameof(D_CONTRACT.BranchID)).AsString(50).NotNullable()
            .WithColumn(nameof(D_CONTRACT.CreateDate)).AsDateTime().NotNullable()
            .WithColumn(nameof(D_CONTRACT.EndDate)).AsDateTime().NotNullable()
            .WithColumn(nameof(D_CONTRACT.LastModify)).AsDateTime().Nullable()
            .WithColumn(nameof(D_CONTRACT.UserCreate)).AsString(50).NotNullable()
            .WithColumn(nameof(D_CONTRACT.UserLastModify)).AsString(50).Nullable()
            .WithColumn(nameof(D_CONTRACT.UserApprove)).AsString(50).NotNullable()
            .WithColumn(nameof(D_CONTRACT.ApproveDate)).AsDateTime().Nullable()
            .WithColumn(nameof(D_CONTRACT.Status)).AsString(20).NotNullable()
            .WithColumn(nameof(D_CONTRACT.IsSpecialMan)).AsString(5).Nullable()
            .WithColumn(nameof(D_CONTRACT.IsReceiverList)).AsString(5).Nullable()
            .WithColumn(nameof(D_CONTRACT.IsAutoRenew)).AsString(5).Nullable()
            .WithColumn(nameof(D_CONTRACT.Description)).AsString(1000).Nullable()
            .WithColumn(nameof(D_CONTRACT.ContractLevelId)).AsInt32().NotNullable()
            .WithColumn(nameof(D_CONTRACT.Mer_Code)).AsString(50).Nullable()
            .WithColumn(nameof(D_CONTRACT.ShopName)).AsString(255).Nullable()
            .WithColumn(nameof(D_CONTRACT.LocalShopName)).AsString(255).Nullable()
            .WithColumn(nameof(D_CONTRACT.ParentContract)).AsString(50).Nullable()
            .WithColumn(nameof(D_CONTRACT.ControlType)).AsString(20).Nullable()
            .WithColumn(nameof(D_CONTRACT.TransactionID)).AsString(50).Nullable()
            .WithColumn(nameof(D_CONTRACT.PolicyId)).AsInt32().Nullable()
            .WithColumn(nameof(D_CONTRACT.CreatedOnUtc)).AsDateTime().Nullable()
            .WithColumn(nameof(D_CONTRACT.UpdatedOnUtc)).AsDateTime().Nullable();
    }
}
