using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.ACT.Domain.AggregatesModel.MappingAggregate;

namespace O24OpenAPI.ACT.Migrations.Builders;
/// <summary>
/// AccountMappingDetailBuilder
/// </summary>
public partial class AccountMappingDetailBuilder : O24OpenAPIEntityBuilder<AccountMappingDetail>
{
    /// <summary>
    /// MapEntity
    /// </summary>
    /// <param name="table"></param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(AccountMappingDetail.MappingId)).AsString(5).NotNullable()
            .WithColumn(nameof(AccountMappingDetail.SystemAccountNumber)).AsString(25).NotNullable()
            .WithColumn(nameof(AccountMappingDetail.BankAccountNumber)).AsString(25).NotNullable()
            .WithColumn(nameof(AccountMappingDetail.CreatedOnUtc)).AsDateTime2().Nullable()
            .WithColumn(nameof(AccountMappingDetail.UpdatedOnUtc)).AsDateTime2().Nullable();
    }
}
