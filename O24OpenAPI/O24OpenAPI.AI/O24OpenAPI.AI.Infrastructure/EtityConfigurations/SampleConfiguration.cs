using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.AI.Domain.AggregatesModel.ChatMessageAggregate;
using O24OpenAPI.Data.Mapping.Builders;

namespace O24OpenAPI.AI.Infrastructure.EtityConfigurations;

public class ChatHistoryConfiguration : O24OpenAPIEntityBuilder<ChatHistory>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(ChatHistory.WalletId))
            .AsInt32()
            .NotNullable()
            .WithColumn(nameof(ChatHistory.Role))
            .AsString(255)
            .NotNullable()
            .WithColumn(nameof(ChatHistory.Content))
            .AsString(int.MaxValue)
            .NotNullable()
            .WithColumn(nameof(ChatHistory.IsSummarized))
            .AsBoolean()
            .NotNullable();
    }
}
