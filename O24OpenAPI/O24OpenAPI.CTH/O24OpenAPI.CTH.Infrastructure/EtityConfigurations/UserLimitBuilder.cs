using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Data.Mapping.Builders;

namespace O24OpenAPI.CTH.Infrastructure.EtityConfigurations;

/// <summary>
/// The user limit builder class
/// </summary>
public class UserLimitBuilder : O24OpenAPIEntityBuilder<UserLimit>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(UserLimit.RoleId)).AsInt32().NotNullable()
            .WithColumn(nameof(UserLimit.CommandId)).AsString(50).NotNullable()
            .WithColumn(nameof(UserLimit.CurrencyCode)).AsString(3).NotNullable()
            .WithColumn(nameof(UserLimit.ULimit)).AsDecimal(20, 3).Nullable()
            .WithColumn(nameof(UserLimit.CvTable)).AsString(1).Nullable()
            .WithColumn(nameof(UserLimit.LimitType)).AsString(1).NotNullable()
            .WithColumn(nameof(UserLimit.Margin)).AsInt32().Nullable()
            .WithColumn(nameof(UserLimit.UpdatedOnUtc)).AsDateTime2().Nullable()
            .WithColumn(nameof(UserLimit.CreatedOnUtc)).AsDateTime2().Nullable();
    }
}
