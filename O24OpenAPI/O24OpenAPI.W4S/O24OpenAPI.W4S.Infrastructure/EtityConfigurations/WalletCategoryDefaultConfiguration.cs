using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.W4S.Domain.AggregatesModel.WalletMasterAggregate;

namespace O24OpenAPI.W4S.Infrastructure.EtityConfigurations
{
    public class WalletCategoryDefaultConfiguration
        : O24OpenAPIEntityBuilder<WalletCategoryDefault>
    {
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(WalletCategoryDefault.CategoryCode))
                    .AsString(50)
                    .NotNullable()
                .WithColumn(nameof(WalletCategoryDefault.ParentCategoryCode))
                    .AsString(50)
                    .NotNullable()
                    .WithDefaultValue(string.Empty)
                .WithColumn(nameof(WalletCategoryDefault.CategoryGroup))
                    .AsString(50)
                    .Nullable()

                .WithColumn(nameof(WalletCategoryDefault.CategoryType))
                    .AsString(20)
                    .NotNullable()
                .WithColumn(nameof(WalletCategoryDefault.CategoryName))
                    .AsString(200)
                    .NotNullable()

                .WithColumn(nameof(WalletCategoryDefault.Icon))
                    .AsString(200)
                    .Nullable()

                .WithColumn(nameof(WalletCategoryDefault.Color))
                    .AsString(50)
                    .Nullable()
                .WithColumn(nameof(WalletCategoryDefault.SortOrder))
                    .AsInt32()
                    .NotNullable()
                    .WithDefaultValue(0)

                .WithColumn(nameof(WalletCategoryDefault.IsActive))
                    .AsBoolean()
                    .NotNullable()
                    .WithDefaultValue(true)
                .WithColumn(nameof(WalletCategoryDefault.Language))
                    .AsString(10)
                    .Nullable();
        }
    }
}
