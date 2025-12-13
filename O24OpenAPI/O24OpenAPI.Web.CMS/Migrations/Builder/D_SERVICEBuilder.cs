using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;

namespace O24OpenAPI.Web.CMS.Migrations.Builder;

public class D_SERVICEBuilder : O24OpenAPIEntityBuilder<D_SERVICE>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(D_SERVICE.ServiceID))
            .AsString(50)
            .NotNullable()
            .WithColumn(nameof(D_SERVICE.ServiceName))
            .AsString(255)
            .NotNullable()
            .WithColumn(nameof(D_SERVICE.Status))
            .AsString(1)
            .NotNullable()
            .WithColumn(nameof(D_SERVICE.BankService))
            .AsBoolean()
            .Nullable()
            .WithColumn(nameof(D_SERVICE.CorpService))
            .AsBoolean()
            .Nullable()
            .WithColumn(nameof(D_SERVICE.checkuseronline))
            .AsBoolean()
            .Nullable();
    }
}
