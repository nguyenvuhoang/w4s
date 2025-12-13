using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.Web.Framework.Domain;

namespace O24OpenAPI.O24ACT.Migrations.Builders;

public class C_CODELISTBuilder : O24OpenAPIEntityBuilder<C_CODELIST>
{
    /// <summary>
    /// MapEntity
    /// </summary>
    /// <param name="table">The table.</param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(C_CODELIST.CodeId))
            .AsString(20)
            .NotNullable()
            .WithColumn(nameof(C_CODELIST.CodeName))
            .AsString(50)
            .NotNullable()
            .WithColumn(nameof(C_CODELIST.CodeGroup))
            .AsString(250)
            .NotNullable()
            .WithColumn(nameof(C_CODELIST.Caption))
            .AsString(250)
            .NotNullable()
            .WithColumn(nameof(C_CODELIST.MCaption))
            .AsString(2000)
            .Nullable()
            .WithColumn(nameof(C_CODELIST.CodeIndex))
            .AsInt32()
            .Nullable()
            .WithColumn(nameof(C_CODELIST.CodeValue))
            .AsString(250)
            .Nullable()
            .WithColumn(nameof(C_CODELIST.Ftag))
            .AsString()
            .Nullable()
            .WithColumn(nameof(C_CODELIST.Visible))
            .AsBoolean()
            .NotNullable()
            .WithColumn(nameof(C_CODELIST.App))
            .AsString(50)
            .Nullable();
    }
}
