using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.O24NCH.Domain;

namespace O24OpenAPI.O24NCH.Migrations.Builders;

/// <summary>
/// The SMS template builder class
/// </summary>
/// <seealso cref="O24OpenAPIEntityBuilder{SMSTemplate}"/>
public class SMSTemplateBuilder : O24OpenAPIEntityBuilder<SMSTemplate>
{
    /// <summary>
    /// Maps the entity using the specified table
    /// </summary>
    /// <param name="table">The table</param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(SMSTemplate.Id))
            .AsInt32()
            .PrimaryKey()
            .Identity()
            .WithColumn(nameof(SMSTemplate.TemplateCode))
            .AsString(100)
            .NotNullable()
            .WithColumn(nameof(SMSTemplate.MessageContent))
            .AsString(int.MaxValue)
            .NotNullable()
            .WithColumn(nameof(SMSTemplate.Description))
            .AsString(255)
            .Nullable()
            .WithColumn(nameof(SMSTemplate.IsActive))
            .AsBoolean()
            .NotNullable()
            .WithDefaultValue(true)
            .WithColumn(nameof(SMSTemplate.CreatedOnUtc))
            .AsDateTime2()
            .Nullable()
            .WithColumn(nameof(SMSTemplate.UpdatedOnUtc))
            .AsDateTime2()
            .Nullable();
    }
}
