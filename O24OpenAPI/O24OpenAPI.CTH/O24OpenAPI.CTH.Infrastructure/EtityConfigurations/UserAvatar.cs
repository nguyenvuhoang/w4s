using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Data.Mapping.Builders;

namespace O24OpenAPI.CTH.Infrastructure.EtityConfigurations;

public class UserAvatarBuilder : O24OpenAPIEntityBuilder<UserAvatar>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(UserAvatar.UserCode))
            .AsString(100)
            .NotNullable()
            .WithColumn(nameof(UserAvatar.ImageUrl))
            .AsString(2000)
            .NotNullable()
            .WithColumn(nameof(UserAvatar.DateInsert))
            .AsDateTime()
            .Nullable();
    }
}
