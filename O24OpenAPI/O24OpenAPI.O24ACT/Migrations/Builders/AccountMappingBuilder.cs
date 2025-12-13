using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.O24ACT.Domain;

namespace O24OpenAPI.O24ACT.Migrations.Builders;

/// <summary>
/// AccountMappingBuilder
/// </summary>
public partial class AccountMappingBuilder : O24OpenAPIEntityBuilder<AccountMapping>
{
    /// <summary>
    /// MapEntity
    /// </summary>
    /// <param name="table"></param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(AccountMapping.MappingId)).AsString(50).NotNullable()
            .WithColumn(nameof(AccountMapping.MappingTableName)).AsString(500).NotNullable()
            .WithColumn(nameof(AccountMapping.AccountName)).AsString(500).Nullable()
            .WithColumn(nameof(AccountMapping.MappingType)).AsString(50).NotNullable()
            .WithColumn(nameof(AccountMapping.CreatedOnUtc)).AsDateTime2().Nullable()
            .WithColumn(nameof(AccountMapping.UpdatedOnUtc)).AsDateTime2().Nullable();

    }
}
