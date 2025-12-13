using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.O24ACT.Domain;

namespace O24OpenAPI.O24ACT.Migrations.Builders;

/// <summary>
/// Stock Transaction builder
/// </summary>
public partial class RuleDefinitionBuilder : O24OpenAPIEntityBuilder<RuleDefinition>
{
    /// <summary>
    /// Map entity
    /// </summary>
    /// <param name="table"></param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(RuleDefinition.RuleName)).AsString(50).NotNullable()
            .WithColumn(nameof(RuleDefinition.FullClassName)).AsString(100).NotNullable()
            .WithColumn(nameof(RuleDefinition.MethodName)).AsString(100).Nullable()
            .WithColumn(nameof(RuleDefinition.UpdatedOnUtc)).AsDateTime2().Nullable();
    }
}
