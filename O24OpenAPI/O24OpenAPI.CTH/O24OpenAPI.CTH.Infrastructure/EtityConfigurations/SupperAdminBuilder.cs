using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Data.Mapping.Builders;

namespace O24OpenAPI.CTH.Infrastructure.EtityConfigurations;

/// <summary>
/// The supper admin builder class
/// </summary>
/// <seealso cref="O24OpenAPIEntityBuilder{SupperAdmin}"/>
public class SupperAdminBuilder : O24OpenAPIEntityBuilder<SupperAdmin>
{
    /// <summary>
    /// Maps the entity using the specified table
    /// </summary>
    /// <param name="table">The table</param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(SupperAdmin.UserId))
            .AsString(100)
            .NotNullable()
            .WithColumn(nameof(SupperAdmin.LoginName))
            .AsString(100)
            .NotNullable()
            .WithColumn(nameof(SupperAdmin.PasswordHash))
            .AsString(1000)
            .NotNullable()
            .WithColumn(nameof(SupperAdmin.CreatedOnUtc))
            .AsDateTime2()
            .Nullable()
            .WithColumn(nameof(SupperAdmin.UpdatedOnUtc))
            .AsDateTime2()
            .Nullable();
    }
}
