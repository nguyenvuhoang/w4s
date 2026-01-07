using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.W4S.Domain.AggregatesModel.WalletMasterAggregate;

namespace O24OpenAPI.W4S.Infrastructure.EtityConfigurations;

public class WalletContractConfiguration : O24OpenAPIEntityBuilder<WalletContract>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            // ===== Identity =====
            .WithColumn(nameof(WalletContract.ContractNumber))
            .AsString(50)
            .NotNullable()
            // ===== Contract Info =====
            .WithColumn(nameof(WalletContract.ContractType))
            .AsInt32()
            .NotNullable()
            .WithColumn(nameof(WalletContract.WalletTier))
            .AsInt32()
            .NotNullable()
            .WithColumn(nameof(WalletContract.UserType))
            .AsInt32()
            .NotNullable()
            .WithColumn(nameof(WalletContract.UserLevel))
            .AsInt32()
            .Nullable()
            // ===== Policy =====
            .WithColumn(nameof(WalletContract.PolicyCode))
            .AsString(50)
            .NotNullable()
            // ===== Customer =====
            .WithColumn(nameof(WalletContract.CustomerCode))
            .AsString(50)
            .NotNullable()
            .WithColumn(nameof(WalletContract.FullName))
            .AsString(200)
            .NotNullable()
            .WithColumn(nameof(WalletContract.Phone))
            .AsString(20)
            .NotNullable()
            .WithColumn(nameof(WalletContract.Email))
            .AsString(200)
            .Nullable()
            // ===== Lifecycle =====
            .WithColumn(nameof(WalletContract.Status))
            .AsInt32()
            .NotNullable()
            .WithColumn(nameof(WalletContract.OpenDateUtc))
            .AsDateTime2()
            .NotNullable()
            .WithColumn(nameof(WalletContract.CloseDateUtc))
            .AsDateTime2()
            .Nullable()
            // ===== Channel =====
            .WithColumn(nameof(WalletContract.Channel))
            .AsInt32()
            .NotNullable();
    }
}
