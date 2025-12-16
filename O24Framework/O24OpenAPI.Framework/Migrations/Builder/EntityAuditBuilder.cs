using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.Framework.Domain;

namespace O24OpenAPI.Framework.Migrations.Builder;

/// <summary>
/// The entity audit builder class
/// </summary>
/// <seealso cref="O24OpenAPIEntityBuilder{EntityAudit}"/>
public class EntityAuditBuilder : O24OpenAPIEntityBuilder<EntityAudit>
{
    /// <summary>
    /// Maps the entity using the specified table
    /// </summary>
    /// <param name="table">The table</param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(EntityAudit.EntityName))
            .AsString(255)
            .NotNullable()
            .WithColumn(nameof(EntityAudit.ActionType))
            .AsString(255)
            .NotNullable()
            .WithColumn(nameof(EntityAudit.Status))
            .AsInt16()
            .NotNullable()
            .WithColumn(nameof(EntityAudit.Data))
            .AsString(int.MaxValue)
            .NotNullable()
            .WithColumn(nameof(EntityAudit.CreatedOnUtc))
            .AsDateTime2()
            .NotNullable();
    }
}
