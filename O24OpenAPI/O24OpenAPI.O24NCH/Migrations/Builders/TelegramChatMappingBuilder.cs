using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.O24NCH.Domain;

namespace O24OpenAPI.O24NCH.Migrations.Builders;

public class TelegramChatMappingBuilder : O24OpenAPIEntityBuilder<TelegramChatMapping>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(TelegramChatMapping.UserCode)).AsString(100).Nullable()
            .WithColumn(nameof(TelegramChatMapping.PhoneNumber)).AsString(20).Nullable()
            .WithColumn(nameof(TelegramChatMapping.ChatId)).AsString(100).NotNullable()
            .WithColumn(nameof(TelegramChatMapping.TelegramUsername)).AsString(100).Nullable()
            .WithColumn(nameof(TelegramChatMapping.Fullname)).AsString(255).Nullable()
            .WithColumn(nameof(TelegramChatMapping.LanguageCode)).AsString(10).Nullable()
            .WithColumn(nameof(TelegramChatMapping.IsBot)).AsBoolean().Nullable()
            .WithColumn(nameof(TelegramChatMapping.CreatedOnUtc)).AsDateTime().NotNullable()
            .WithColumn(nameof(TelegramChatMapping.UpdatedOnUtc)).AsDateTime().Nullable();
    }
}
