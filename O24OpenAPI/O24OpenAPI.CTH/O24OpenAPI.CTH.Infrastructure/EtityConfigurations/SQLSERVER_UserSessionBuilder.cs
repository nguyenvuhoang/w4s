using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Data;
using O24OpenAPI.Data.Attributes;
using O24OpenAPI.Data.Mapping.Builders;

namespace O24OpenAPI.CTH.Infrastructure.EtityConfigurations;

[DatabaseType(DataProviderType.SqlServer)]

/// <summary>
/// The user session builder class
/// </summary>
/// <seealso cref="O24OpenAPIEntityBuilder{UserSession}"/>
public partial class SQLSERVER_UserSessionBuilder : O24OpenAPIEntityBuilder<UserSession>
{
    /// <summary>
    /// Maps the entity using the specified table
    /// </summary>
    /// <param name="table">The table</param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(UserSession.UserId))
            .AsString(1000)
            .Nullable()
            .WithColumn(nameof(UserSession.LoginName))
            .AsString(100)
            .NotNullable()
            .WithColumn(nameof(UserSession.Token))
            .AsString(2000)
            .NotNullable()
            .WithColumn(nameof(UserSession.RefreshToken))
            .AsString(2000)
            .Nullable()
            .WithColumn(nameof(UserSession.RefreshTokenExpiresAt))
            .AsDateTime2()
            .NotNullable()
            .WithColumn(nameof(UserSession.Reference))
            .AsString(2000)
            .Nullable()
            .WithColumn(nameof(UserSession.UserCode))
            .AsString(100)
            .Nullable()
            .WithColumn(nameof(UserSession.BranchCode))
            .AsString(100)
            .Nullable()
            .WithColumn(nameof(UserSession.UserName))
            .AsString(100)
            .Nullable()
            .WithColumn(nameof(UserSession.ChannelRoles))
            .AsString(1000)
            .Nullable()
            .WithColumn(nameof(UserSession.IpAddress))
            .AsString(100)
            .Nullable()
            .WithColumn(nameof(UserSession.Device))
            .AsString(2000)
            .Nullable()
            .WithColumn(nameof(UserSession.ExpiresAt))
            .AsDateTime2()
            .NotNullable()
            .WithColumn(nameof(UserSession.IsRevoked))
            .AsBoolean()
            .NotNullable()
            .WithColumn(nameof(UserSession.ChannelId))
            .AsString(50)
            .NotNullable()
            .WithColumn(nameof(UserSession.SignatureKey))
            .AsString(2048)
            .Nullable()
            .WithColumn(nameof(UserSession.CreatedOnUtc))
            .AsDateTime2()
            .Nullable()
            .WithColumn(nameof(UserSession.UpdatedOnUtc))
            .AsDateTime2()
            .Nullable();
    }
}
