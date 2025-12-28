using FluentMigrator;
using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.NCH.Domain;
using O24OpenAPI.NCH.Domain.AggregatesModel.OtpAggregate;

namespace O24OpenAPI.NCH.Migrations.Builders;

/// <summary>
/// The user password builder class
/// </summary>
/// <seealso cref="O24OpenAPIEntityBuilder{OTP_REQUESTS}"/>
public class OtpRequestBuilder : O24OpenAPIEntityBuilder<OTP_REQUESTS>
{
    /// <summary>
    /// Maps the entity using the specified table
    /// </summary>
    /// <param name="table">The table</param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(OTP_REQUESTS.Id))
            .AsInt32()
            .PrimaryKey()
            .Identity()
            .WithColumn(nameof(OTP_REQUESTS.UserCode))
            .AsString(100)
            .NotNullable()
            .WithColumn(nameof(OTP_REQUESTS.PhoneNumber))
            .AsString(20)
            .NotNullable()
            .WithColumn(nameof(OTP_REQUESTS.OtpCode))
            .AsString(500)
            .NotNullable()
            .WithColumn(nameof(OTP_REQUESTS.Purpose))
            .AsString(100)
            .NotNullable()
            .WithColumn(nameof(OTP_REQUESTS.IsUsed))
            .AsBoolean()
            .NotNullable()
            .WithDefaultValue(false)
            .WithColumn(nameof(OTP_REQUESTS.CreatedAt))
            .AsDateTime2()
            .NotNullable()
            .WithColumn(nameof(OTP_REQUESTS.ExpiresAt))
            .AsDateTime2()
            .NotNullable()
            .WithColumn(nameof(OTP_REQUESTS.TransactionId))
            .AsGuid()
            .NotNullable()
            .WithDefault(SystemMethods.NewGuid)
            .WithColumn(nameof(OTP_REQUESTS.Status))
            .AsString(20)
            .NotNullable()
            .WithDefaultValue("Pending");
    }
}
