using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;

namespace O24OpenAPI.W4S.Infrastructure.EtityConfigurations;

public class WalletCategoryConfiguration : O24OpenAPIEntityBuilder<WalletCategory>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(WalletCategory.CategoryId))
                .AsString(50)
                .NotNullable()

            .WithColumn(nameof(WalletCategory.WalletId))
                .AsString(50)
                .NotNullable()

            .WithColumn(nameof(WalletCategory.ParentCategoryId))
                .AsString(50)
                .NotNullable()
                .WithDefaultValue(string.Empty)

            .WithColumn(nameof(WalletCategory.CategoryGroup))
                .AsString(50)
                .Nullable()

            .WithColumn(nameof(WalletCategory.CategoryType))
                .AsString(20)
                .NotNullable()

            .WithColumn(nameof(WalletCategory.CategoryName))
                .AsString(200)
                .NotNullable()

            .WithColumn(nameof(WalletCategory.Icon))
                .AsString(200)
                .Nullable()

            .WithColumn(nameof(WalletCategory.Color))
                .AsString(50)
                .Nullable();
    }
}
