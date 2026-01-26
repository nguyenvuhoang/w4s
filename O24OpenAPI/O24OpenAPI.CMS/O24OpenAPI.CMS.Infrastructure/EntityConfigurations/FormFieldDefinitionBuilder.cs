using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.CMS.Domain.AggregateModels.FormAggregate;
using O24OpenAPI.Data;
using O24OpenAPI.Data.Attributes;
using O24OpenAPI.Data.Extensions;
using O24OpenAPI.Data.Mapping.Builders;

namespace O24OpenAPI.CMS.Infrastructure.EntityConfigurations;

[DatabaseType(DataProviderType.Oracle)]
public class ORACLE_FormFieldDefinitionBuilder : O24OpenAPIEntityBuilder<FormFieldDefinition>
{
    /// <summary>
    /// MapEntity
    /// </summary>
    /// <param name="table">The table.</param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(FormFieldDefinition.FormId))
            .AsString(20)
            .NotNullable()
            .WithColumn(nameof(FormFieldDefinition.FieldName))
            .AsString(20)
            .NotNullable()
            .WithColumn(nameof(FormFieldDefinition.FieldValue))
            .AsNCLOB()
            .NotNullable()
            .WithDefaultValue(string.Empty)
            .WithColumn(nameof(FormFieldDefinition.CreatedOnUtc))
            .AsDateTime()
            .NotNullable()
            .WithColumn(nameof(FormFieldDefinition.UpdatedOnUtc))
            .AsDateTime()
            .Nullable();
    }
}

[DatabaseType(DataProviderType.SqlServer)]
public class SQLSERVER_FormFieldDefinitionBuilder : O24OpenAPIEntityBuilder<FormFieldDefinition>
{
    /// <summary>
    /// MapEntity
    /// </summary>
    /// <param name="table">The table.</param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
           .WithColumn(nameof(FormFieldDefinition.FormId))
           .AsString(20)
           .NotNullable()
           .WithColumn(nameof(FormFieldDefinition.FieldName))
           .AsString(20)
           .NotNullable()
           .WithColumn(nameof(FormFieldDefinition.FieldValue))
           .AsString(int.MaxValue)
           .NotNullable()
           .WithDefaultValue(string.Empty)
           .WithColumn(nameof(FormFieldDefinition.CreatedOnUtc))
           .AsDateTime()
           .NotNullable()
           .WithColumn(nameof(FormFieldDefinition.UpdatedOnUtc))
           .AsDateTime()
           .Nullable();
    }
}
