using FluentMigrator;
using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.NCH.Domain.AggregatesModel.NotificationAggregate;

namespace O24OpenAPI.NCH.Infrastructure.EntityCongurations;

public class RepaymentRemindBuilder : O24OpenAPIEntityBuilder<RepaymentRemind>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(RepaymentRemind.AccountNumber)).AsString(50).NotNullable()
            .WithColumn(nameof(RepaymentRemind.CustomerName)).AsString(1000).NotNullable()
            .WithColumn(nameof(RepaymentRemind.DueDate)).AsDate().NotNullable()
            .WithColumn(nameof(RepaymentRemind.DueAmount)).AsDecimal().NotNullable()
            .WithColumn(nameof(RepaymentRemind.MessageType)).AsString(50).NotNullable()
            .WithColumn(nameof(RepaymentRemind.LastSentOn)).AsDateTime2().NotNullable()
            .WithColumn(nameof(RepaymentRemind.Status)).AsString(20).NotNullable().WithDefaultValue("SENT")
            .WithColumn(nameof(RepaymentRemind.ErrorMessage)).AsString(int.MaxValue).Nullable()
            .WithColumn(nameof(RepaymentRemind.CreatedOn)).AsDateTime2().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)
            .WithColumn(nameof(RepaymentRemind.UpdatedOn)).AsDateTime2().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime);
    }
}
