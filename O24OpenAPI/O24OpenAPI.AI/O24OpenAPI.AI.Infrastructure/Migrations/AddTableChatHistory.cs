using FluentMigrator;
using O24OpenAPI.AI.Domain.AggregatesModel.ChatHistoryAggregate;
using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Data.Extensions;
using O24OpenAPI.Data.Migrations;

namespace O24OpenAPI.AI.Infrastructure.Migrations;

[O24OpenAPIMigration(
    "2025/01/01 10:01:03:0000000",
    "AddTableChatHistory",
    MigrationProcessType.Installation
)]
[Environment(EnvironmentType.All)]
public class AddTableChatHistory : AutoReversingMigration
{
    public override void Up()
    {
        if (!Schema.Table(nameof(ChatHistory)).Exists())
        {
            Create.TableFor<ChatHistory>();

            Create
                .Index()
                .OnTable(nameof(ChatHistory))
                .OnColumn(nameof(ChatHistory.ConversationId))
                .Ascending()
                .OnColumn(nameof(ChatHistory.Role))
                .Ascending()
                .OnColumn(nameof(ChatHistory.CreatedOnUtc))
                .Descending()
                .WithOptions()
                .NonClustered();

            Create
                .Index()
                .OnTable(nameof(ChatHistory))
                .OnColumn(nameof(ChatHistory.ConversationId))
                .Ascending()
                .OnColumn(nameof(ChatHistory.IsSummarized))
                .Ascending()
                .OnColumn(nameof(ChatHistory.CreatedOnUtc))
                .Descending()
                .WithOptions()
                .NonClustered();
        }
    }
}
