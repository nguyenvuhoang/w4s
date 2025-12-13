using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.ControlHub.Domain;
using O24OpenAPI.Data.Mapping.Builders;

namespace O24OpenAPI.ControlHub.Migrations.Builders;

/// <summary>
/// The transaction definition builder class
/// </summary>
/// <seealso cref="O24OpenAPIEntityBuilder{TransactionDefinition}"/>
public class TransactionDefinitionBuilder : O24OpenAPIEntityBuilder<TransactionDefinition>
{
    /// <summary>
    /// Maps the entity using the specified table
    /// </summary>
    /// <param name="table">The table</param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(TransactionDefinition.TransactionCode)).AsString(50).NotNullable()
            .WithColumn(nameof(TransactionDefinition.WorkflowId)).AsString(50).Nullable()
            .WithColumn(nameof(TransactionDefinition.TransactionName)).AsString(50).NotNullable()
            .WithColumn(nameof(TransactionDefinition.Description)).AsString(500).NotNullable()
            .WithColumn(nameof(TransactionDefinition.TransactionType)).AsString(10).NotNullable()
            .WithColumn(nameof(TransactionDefinition.ServiceId)).AsString(10).NotNullable()
            .WithColumn(nameof(TransactionDefinition.InterBranch)).AsBoolean().NotNullable()
            .WithColumn(nameof(TransactionDefinition.Reverseable)).AsBoolean().NotNullable()
            .WithColumn(nameof(TransactionDefinition.Enabled)).AsBoolean().NotNullable()
            .WithColumn(nameof(TransactionDefinition.MessageAccount)).AsString(50).NotNullable()
            .WithColumn(nameof(TransactionDefinition.MessageAmount)).AsString(50).Nullable()
            .WithColumn(nameof(TransactionDefinition.Voucher)).AsString(20).Nullable()
            .WithColumn(nameof(TransactionDefinition.ShowF8)).AsBoolean().NotNullable()
            .WithColumn(nameof(TransactionDefinition.ApplicationCode)).AsString(50).NotNullable()
            .WithColumn(nameof(TransactionDefinition.MessageCurrency)).AsString(50).Nullable()
            .WithColumn(nameof(TransactionDefinition.TransactionModel)).AsString(100).Nullable()
            .WithColumn(nameof(TransactionDefinition.Channel)).AsString(20).Nullable()
            .WithColumn(nameof(TransactionDefinition.UpdatedOnUtc)).AsDateTime2().Nullable()
            .WithColumn(nameof(TransactionDefinition.CreatedOnUtc)).AsDateTime2().Nullable();
    }
}
