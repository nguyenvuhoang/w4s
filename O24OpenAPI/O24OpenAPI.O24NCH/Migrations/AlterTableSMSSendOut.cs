using FluentMigrator;
using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Data;
using O24OpenAPI.Data.Attributes;
using O24OpenAPI.Data.Migrations;
using O24OpenAPI.O24NCH.Domain;

namespace O24OpenAPI.O24NCH.Migrations;

[O24OpenAPIMigration(
    "2025/11/20 14:31:01:0000000",
    "Add column EndToEnd/FullRequest/RequestHeader/ErrorMessage/RetryCount/ElapsedMilliseconds for SMSSendOut",
    MigrationProcessType.Installation
)]
[Environment(EnvironmentType.All)]
[DatabaseType(DataProviderType.SqlServer)]
public class AlterTableSMSSendOut : AutoReversingMigration
{
    public override void Up()
    {
        if (
        Schema.Table(nameof(SMSSendOut)).Exists()
        && !Schema
            .Table(nameof(SMSSendOut))
            .Column(nameof(SMSSendOut.EndToEnd))
            .Exists()
        )
        {
            Alter
                .Table(nameof(SMSSendOut))
                .AddColumn(nameof(SMSSendOut.EndToEnd))
                .AsString(100)
                .Nullable();
        }

        if (
        Schema.Table(nameof(SMSSendOut)).Exists()
        && !Schema
            .Table(nameof(SMSSendOut))
            .Column(nameof(SMSSendOut.FullRequest))
            .Exists()
        )
        {
            Alter
                .Table(nameof(SMSSendOut))
                .AddColumn(nameof(SMSSendOut.FullRequest))
                .AsString(int.MaxValue)
                .Nullable();
        }

        if (
        Schema.Table(nameof(SMSSendOut)).Exists()
        && !Schema
            .Table(nameof(SMSSendOut))
            .Column(nameof(SMSSendOut.RequestHeader))
            .Exists()
        )
        {
            Alter
                .Table(nameof(SMSSendOut))
                .AddColumn(nameof(SMSSendOut.RequestHeader))
                .AsString(int.MaxValue)
                .Nullable();
        }
        if (
           Schema.Table(nameof(SMSSendOut)).Exists()
           && !Schema
               .Table(nameof(SMSSendOut))
               .Column(nameof(SMSSendOut.ErrorMessage))
               .Exists()
           )
        {
            Alter
                .Table(nameof(SMSSendOut))
                .AddColumn(nameof(SMSSendOut.ErrorMessage))
                .AsString(int.MaxValue)
                .Nullable();
        }
        if (
          Schema.Table(nameof(SMSSendOut)).Exists()
          && !Schema
              .Table(nameof(SMSSendOut))
              .Column(nameof(SMSSendOut.FallbackFromProvider))
              .Exists()
          )
        {
            Alter
                .Table(nameof(SMSSendOut))
                .AddColumn(nameof(SMSSendOut.FallbackFromProvider))
                .AsString(100)
                .Nullable();
        }
        if (
          Schema.Table(nameof(SMSSendOut)).Exists()
          && !Schema
              .Table(nameof(SMSSendOut))
              .Column(nameof(SMSSendOut.IsFallback))
              .Exists()
          )
        {
            Alter
                .Table(nameof(SMSSendOut))
                .AddColumn(nameof(SMSSendOut.IsFallback))
                .AsBoolean()
                .Nullable();
        }
        if (
         Schema.Table(nameof(SMSSendOut)).Exists()
         && !Schema
             .Table(nameof(SMSSendOut))
             .Column(nameof(SMSSendOut.IsResend))
             .Exists()
         )
        {
            Alter
                .Table(nameof(SMSSendOut))
                .AddColumn(nameof(SMSSendOut.IsResend))
                .AsBoolean()
                .Nullable();
        }
        if (
        Schema.Table(nameof(SMSSendOut)).Exists()
        && !Schema
            .Table(nameof(SMSSendOut))
            .Column(nameof(SMSSendOut.RetryCount))
            .Exists()
        )
        {
            Alter
                .Table(nameof(SMSSendOut))
                .AddColumn(nameof(SMSSendOut.RetryCount))
                .AsInt32()
                .Nullable();
        }
        if (
        Schema.Table(nameof(SMSSendOut)).Exists()
        && !Schema
            .Table(nameof(SMSSendOut))
            .Column(nameof(SMSSendOut.ElapsedMilliseconds))
            .Exists()
        )
        {
            Alter
                .Table(nameof(SMSSendOut))
                .AddColumn(nameof(SMSSendOut.ElapsedMilliseconds))
                .AsInt32()
                .Nullable();
        }
    }
}
