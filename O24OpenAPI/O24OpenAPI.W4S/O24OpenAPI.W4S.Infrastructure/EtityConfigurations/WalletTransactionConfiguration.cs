using FluentMigrator;
using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;

namespace O24OpenAPI.W4S.Infrastructure.EtityConfigurations;

public class WalletTransactionConfiguration : O24OpenAPIEntityBuilder<WalletTransaction>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn("TRANSACTIONDATE").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentDateTime)
            .WithColumn("TRANSACTIONENDDATE").AsDateTime().Nullable()
            .WithColumn("TRANSACTIONWORKDATE").AsDateTime().Nullable()
            .WithColumn("TRANSACTIONID").AsString(20)
            .WithColumn("TRANSACTIONCODE").AsString(100).NotNullable()
            .WithColumn("CCYID").AsString(10).Nullable()
            .WithColumn("SOURCEID").AsString(20).NotNullable()
            .WithColumn("SOURCETRANREF").AsString(20).NotNullable()
            .WithColumn("USERID").AsString(50).NotNullable()
            .WithColumn("USERCURAPP").AsString(2000).Nullable()
            .WithColumn("NEXTUSERAPP").AsString(2000).Nullable()
            .WithColumn("LISTUSERAPP").AsString(int.MaxValue).Nullable()
            .WithColumn("TRANDESC").AsString(400).NotNullable()
            .WithColumn("STATUS").AsString(10).NotNullable()
            .WithColumn("APPRSTS").AsInt32().NotNullable()
            .WithColumn("OFFLSTS").AsString(1).NotNullable()
            .WithColumn("DELETED").AsBoolean().NotNullable()
            .WithColumn("DESTID").AsString(20).NotNullable()
            .WithColumn("DESTTRANREF").AsString(20).Nullable()
            .WithColumn("DESTERRORCODE").AsString(50).Nullable()
            .WithColumn("ERRORCODE").AsString(10).NotNullable().WithDefaultValue("")
            .WithColumn("ERRORDESC").AsString(1000).NotNullable().WithDefaultValue("")
            .WithColumn("ONLINE").AsBoolean().NotNullable().WithDefaultValue(true);

        for (int i = 1; i <= 30; i++)
        {
            table.WithColumn($"CHAR{i:D2}").AsString(1000).Nullable();
        }

        for (int i = 1; i <= 20; i++)
        {
            table.WithColumn($"NUM{i:D2}").AsDecimal(24, 5).Nullable();
        }

        table.WithColumn("ISBATCH").AsString(1).Nullable()
        .WithColumn("BATCHREF").AsString(50).Nullable()
        .WithColumn("AUTHENTYPE").AsString(50).Nullable()
        .WithColumn("AUTHENCODE").AsString(50).Nullable();
    }
}
