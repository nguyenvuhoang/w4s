using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Attributes;
using O24OpenAPI.Data.Extensions;
using O24OpenAPI.Data.Mapping.Builders;

namespace O24OpenAPI.Web.CMS.Migrations.Builder;

[DatabaseType(DataProviderType.Oracle)]
public class ORACLE_UserSessionsBuilder : O24OpenAPIEntityBuilder<UserSessions>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(UserSessions.Usrid))
            .AsInt32()
            .NotNullable()

            .WithColumn(nameof(UserSessions.Userid))
            .AsString(100)
            .NotNullable()

            .WithColumn(nameof(UserSessions.Token))
            .AsString(1000)
            .Nullable()

            .WithColumn(nameof(UserSessions.HashedRefreshToken))
            .AsString(500)
            .Nullable()

            .WithColumn(nameof(UserSessions.UsrBranch))
            .AsString(100)
            .Nullable()

            .WithColumn(nameof(UserSessions.UsrBranchid))
            .AsInt32()
            .Nullable()

            .WithColumn(nameof(UserSessions.Usrname))
            .AsString(200)
            .Nullable()

            .WithColumn(nameof(UserSessions.LoginName))
            .AsString(200)
            .Nullable()

            .WithColumn(nameof(UserSessions.Lang))
            .AsString(10)
            .Nullable()

            .WithColumn(nameof(UserSessions.Txdt))
            .AsDateTime2()
            .NotNullable()

            .WithColumn(nameof(UserSessions.Ssesionid))
            .AsString(200)
            .Nullable()

            .WithColumn(nameof(UserSessions.UserCode))
            .AsString(100)
            .Nullable()

            .WithColumn(nameof(UserSessions.Ssntime))
            .AsDateTime2()
            .NotNullable()

            .WithColumn(nameof(UserSessions.Exptime))
            .AsDateTime2()
            .NotNullable()

            .WithColumn(nameof(UserSessions.Wsip))
            .AsString(100)
            .Nullable()

            .WithColumn(nameof(UserSessions.Mac))
            .AsString(100)
            .Nullable()

            .WithColumn(nameof(UserSessions.Wsname))
            .AsString(200)
            .Nullable()

            .WithColumn(nameof(UserSessions.Acttype))
            .AsString(50)
            .Nullable()

            .WithColumn(nameof(UserSessions.ApplicationCode))
            .AsString(100)
            .Nullable()

            .WithColumn(nameof(UserSessions.Info))
            .AsString(2000)
            .Nullable()

            .WithColumn(nameof(UserSessions.CommandList))
            .AsNCLOB()
            .Nullable()

            .WithColumn(nameof(UserSessions.UpdatedOnUtc))
            .AsDateTime2()
            .Nullable()

            .WithColumn(nameof(UserSessions.ResetPassword))
            .AsBoolean()
            .NotNullable()

            .WithColumn(nameof(UserSessions.CreatedOnUtc))
            .AsDateTime2()
            .Nullable()

            .WithColumn(nameof(UserSessions.SessionDevice))
            .AsString(200)
            .Nullable()

            .WithColumn(nameof(UserSessions.ChannelRoles))
            .AsString(500)
            .Nullable()

            .WithColumn(nameof(UserSessions.Roles))
            .AsString(500)
            .Nullable()

            .WithColumn(nameof(UserSessions.SignatureKey))
            .AsString(500)
            .Nullable();
    }
}
