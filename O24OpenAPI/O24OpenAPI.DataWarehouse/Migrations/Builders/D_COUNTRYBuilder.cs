using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;

namespace O24OpenAPI.DataWarehouse.Migrations.Builders;

public class D_COUNTRYBuilder : O24OpenAPIEntityBuilder<D_COUNTRY>
{
    /// <summary>
    /// MapEntity
    /// </summary>
    /// <param name="table">The table.</param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(D_COUNTRY.CountryID))
            .AsString(3)
            .Unique()
            .NotNullable()
            .WithColumn(nameof(D_COUNTRY.CountryCode))
            .AsString(3)
            .NotNullable()
            .WithColumn(nameof(D_COUNTRY.CountryName))
            .AsString(200)
            .NotNullable()
            .WithColumn(nameof(D_COUNTRY.MCountryName))
            .AsString()
            .Nullable()
            .WithColumn(nameof(D_COUNTRY.CapitalName))
            .AsString(200)
            .Nullable()
            .WithColumn(nameof(D_COUNTRY.CurrencyID))
            .AsString(3)
            .NotNullable()
            .WithColumn(nameof(D_COUNTRY.Language))
            .AsString(2)
            .Nullable()
            .WithColumn(nameof(D_COUNTRY.Status))
            .AsString(3)
            .NotNullable()
            .WithColumn(nameof(D_COUNTRY.Order))
            .AsDecimal(3, 0)
            .NotNullable()
            .WithColumn(nameof(D_COUNTRY.Description))
            .AsString(250)
            .Nullable()
            .WithColumn(nameof(D_COUNTRY.UserCreated))
            .AsString(50)
            .NotNullable()
            .WithColumn(nameof(D_COUNTRY.DateCreated))
            .AsDateTime2()
            .NotNullable()
            .WithColumn(nameof(D_COUNTRY.UserModified))
            .AsString(50)
            .Nullable()
            .WithColumn(nameof(D_COUNTRY.LastModified))
            .AsDateTime2()
            .Nullable()
            .WithColumn(nameof(D_COUNTRY.UserApproved))
            .AsString(50)
            .Nullable()
            .WithColumn(nameof(D_COUNTRY.DateApproved))
            .AsDateTime2()
            .Nullable()
            .WithColumn(nameof(D_COUNTRY.TimeZone))
            .AsString(3)
            .Nullable()
            .WithColumn(nameof(D_COUNTRY.PhoneCountryCode))
            .AsString(50)
            .NotNullable();
    }
}
