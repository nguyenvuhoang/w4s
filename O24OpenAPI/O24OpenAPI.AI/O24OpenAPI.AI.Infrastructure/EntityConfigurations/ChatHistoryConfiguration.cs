using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.AI.Domain.AggregatesModel.ChatHistoryAggregate;
using O24OpenAPI.Data.Mapping.Builders;

namespace O24OpenAPI.AI.Infrastructure.EntityConfigurations;

public class ChatHistoryConfiguration : O24OpenAPIEntityBuilder<ChatHistory>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(ChatHistory.ConversationId))
            .AsString(100)
            .NotNullable()
            .WithColumn(nameof(ChatHistory.UserCode))
            .AsString(255)
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
