using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Data.Mapping.Builders;

namespace O24OpenAPI.CTH.Infrastructure.EtityConfigurations;

public class UserAgreementBuilder : O24OpenAPIEntityBuilder<UserAgreement>
{
    /// <summary>
    /// Map entity UserAgreement to database table.
    /// </summary>
    /// <param name="table">The table builder.</param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(UserAgreement.AgreementNumber))
                .AsString(100)
                .NotNullable()
            .WithColumn(nameof(UserAgreement.AgreementType))
                .AsString(50)
                .NotNullable()
            .WithColumn(nameof(UserAgreement.TransactionCode))
                .AsString(100)
                .NotNullable()
            .WithColumn(nameof(UserAgreement.Content))
                .AsString(int.MaxValue)
                .Nullable()
            .WithColumn(nameof(UserAgreement.StartDate))
                .AsDateTime()
                .NotNullable()
            .WithColumn(nameof(UserAgreement.EndDate))
                .AsDateTime()
                .NotNullable()
            .WithColumn(nameof(UserAgreement.IsActive))
                .AsBoolean()
                .NotNullable()
            .WithColumn(nameof(UserAgreement.CreatedUTC))
                .AsDateTime()
                .Nullable()
            .WithColumn(nameof(UserAgreement.ModifiedUTC))
                .AsDateTime()
                .Nullable();
    }
}
