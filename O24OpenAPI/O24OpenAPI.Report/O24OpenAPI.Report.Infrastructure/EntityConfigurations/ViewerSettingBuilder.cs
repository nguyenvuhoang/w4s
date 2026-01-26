using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.Report.Domain.AggregateModels.ViewerSettingAggregate;

namespace O24OpenAPI.Report.Infrastructure.EntityConfigurations;

public partial class ViewerSettingBuilder : O24OpenAPIEntityBuilder<ViewerSetting>
{
    /// <summary>
    /// map entity ViewerSetting table
    /// </summary>
    /// <param name="table"></param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(ViewerSetting.CodeTemplate))
            .AsString(25)
            .NotNullable()
            .WithColumn(nameof(ViewerSetting.CacheMode))
            .AsString(50)
            .Nullable()
            .WithColumn(nameof(ViewerSetting.CacheTimeout))
            .AsInt32()
            .Nullable()
            .WithColumn(nameof(ViewerSetting.RequestTimeout))
            .AsInt32()
            .Nullable()
            .WithColumn(nameof(ViewerSetting.ShowDesignButton))
            .AsBoolean()
            .Nullable()
            .WithColumn(nameof(ViewerSetting.ShowOpenButton))
            .AsBoolean()
            .Nullable()
            .WithColumn(nameof(ViewerSetting.ShowPrintButton))
            .AsBoolean()
            .Nullable()
            .WithColumn(nameof(ViewerSetting.ShowResourcesButton))
            .AsBoolean()
            .Nullable()
            .WithColumn(nameof(ViewerSetting.ShowSaveButton))
            .AsBoolean()
            .Nullable()
            .WithColumn(nameof(ViewerSetting.ShowSendEmailButton))
            .AsBoolean()
            .Nullable()
            .WithColumn(nameof(ViewerSetting.UseCompression))
            .AsBoolean()
            .Nullable();
    }
}
