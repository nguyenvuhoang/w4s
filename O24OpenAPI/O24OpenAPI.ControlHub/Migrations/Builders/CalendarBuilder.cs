using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.ControlHub.Domain;
using O24OpenAPI.Data.Mapping.Builders;

namespace O24OpenAPI.ControlHub.Migrations.Builders;


/// <summary>
/// Calendar builder
/// </summary>
public partial class CalendarBuilder : O24OpenAPIEntityBuilder<Calendar>
{

    /// <summary>
    /// mapping entity calendar table
    /// </summary>
    /// <param name="table"></param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(Calendar.SqnDate)).AsDateTime2().NotNullable()
            .WithColumn(nameof(Calendar.IsCurrentDate)).AsInt64().NotNullable().WithDefaultValue(0)
            .WithColumn(nameof(Calendar.IsHoliday)).AsInt64().NotNullable().WithDefaultValue(0)
            .WithColumn(nameof(Calendar.IsEndOfWeek)).AsInt64().NotNullable().WithDefaultValue(0)
            .WithColumn(nameof(Calendar.IsEndOfMonth)).AsInt64().NotNullable().WithDefaultValue(0)
            .WithColumn(nameof(Calendar.IsEndOfQuater)).AsInt64().NotNullable().WithDefaultValue(0)
            .WithColumn(nameof(Calendar.IsEndOfHalfYear)).AsInt64().NotNullable().WithDefaultValue(0)
            .WithColumn(nameof(Calendar.IsEndOfYear)).AsInt64().NotNullable().WithDefaultValue(0)
            .WithColumn(nameof(Calendar.IsBeginOfWeek)).AsInt64().NotNullable().WithDefaultValue(0)
            .WithColumn(nameof(Calendar.IsBeginOfMonth)).AsInt64().NotNullable().WithDefaultValue(0)
            .WithColumn(nameof(Calendar.IsBeginOfQuater)).AsInt64().NotNullable().WithDefaultValue(0)
            .WithColumn(nameof(Calendar.IsBeginOfHalfYear)).AsInt64().NotNullable().WithDefaultValue(0)
            .WithColumn(nameof(Calendar.IsBeginOfYear)).AsInt64().NotNullable().WithDefaultValue(0)
            .WithColumn(nameof(Calendar.IsFiscalEndOfWeek)).AsInt64().NotNullable().WithDefaultValue(0)
            .WithColumn(nameof(Calendar.IsFiscalEndOfMonth)).AsInt64().NotNullable().WithDefaultValue(0)
            .WithColumn(nameof(Calendar.IsFiscalEndOfQuater)).AsInt64().NotNullable().WithDefaultValue(0)
            .WithColumn(nameof(Calendar.IsFiscalEndOfHalfYear)).AsInt64().NotNullable().WithDefaultValue(0)
            .WithColumn(nameof(Calendar.IsFiscalEndOfYear)).AsInt64().NotNullable().WithDefaultValue(0)
            .WithColumn(nameof(Calendar.IsFiscalBeginOfWeek)).AsInt64().NotNullable().WithDefaultValue(0)
            .WithColumn(nameof(Calendar.IsFiscalBeginOfMonth)).AsInt64().NotNullable().WithDefaultValue(0)
            .WithColumn(nameof(Calendar.IsFiscalBeginOfQuater)).AsInt64().NotNullable().WithDefaultValue(0)
            .WithColumn(nameof(Calendar.IsFiscalBeginOfHalfYear)).AsInt64().NotNullable().WithDefaultValue(0)
            .WithColumn(nameof(Calendar.IsFiscalBeginOfYear)).AsInt64().NotNullable().WithDefaultValue(0)
            .WithColumn(nameof(Calendar.Descs)).AsString(100).Nullable()
            .WithColumn(nameof(Calendar.CurrencyCode)).AsString(3).NotNullable()
            .WithColumn(nameof(Calendar.UpdatedOnUtc)).AsDateTime2().Nullable()
            .WithColumn(nameof(Calendar.CreatedOnUtc)).AsDateTime2().Nullable();
    }
}
