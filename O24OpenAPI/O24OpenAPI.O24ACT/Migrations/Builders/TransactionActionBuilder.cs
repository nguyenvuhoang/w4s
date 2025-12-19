using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.Framework.Domain;

namespace O24OpenAPI.O24ACT.Migrations.Builders;

public class TransactionActionBuilder : O24OpenAPIEntityBuilder<TransactionAction>
{
    /// <summary>
    /// MapEntity
    /// </summary>
    /// <param name="table">The table.</param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(TransactionAction.TransCode))
            .AsString(100)
            .NotNullable()
            .WithColumn(nameof(TransactionAction.FieldNames))
            .AsString(int.MaxValue)
            .Nullable()
            .WithColumn(nameof(TransactionAction.Action))
            .AsString(255)
            .NotNullable()
            .WithColumn(nameof(TransactionAction.HasStatement))
            .AsBoolean()
            .NotNullable()
            .WithDefaultValue(false);
    }
}
