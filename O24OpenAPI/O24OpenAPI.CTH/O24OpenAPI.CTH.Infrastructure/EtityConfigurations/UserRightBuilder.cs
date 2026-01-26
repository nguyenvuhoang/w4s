using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Data.Mapping.Builders;

namespace O24OpenAPI.CTH.Infrastructure.EtityConfigurations;

public class UserRightBuilder : O24OpenAPIEntityBuilder<UserRight>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(UserRight.RoleId))
            .AsInt32()
            .NotNullable()
            .WithColumn(nameof(UserRight.CommandId))
            .AsString(50)
            .NotNullable()
            .WithColumn(nameof(UserRight.CommandIdDetail))
            .AsString(50)
            .NotNullable()
            .WithColumn(nameof(UserRight.Invoke))
            .AsInt32()
            .NotNullable()
            .WithColumn(nameof(UserRight.Approve))
            .AsInt32()
            .NotNullable()
            .WithColumn(nameof(UserRight.UpdatedOnUtc))
            .AsDateTime2()
            .Nullable()
            .WithColumn(nameof(UserRight.CreatedOnUtc))
            .AsDateTime2()
            .Nullable();
    }
}
