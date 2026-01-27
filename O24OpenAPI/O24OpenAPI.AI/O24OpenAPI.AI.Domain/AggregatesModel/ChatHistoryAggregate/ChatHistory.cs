using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.AI.Domain.AggregatesModel.ChatHistoryAggregate;

public class ChatHistory : BaseEntity
{
    public string ConversationId { get; set; } = string.Empty;
    public string UserCode { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public bool IsSummarized { get; set; } = false;
}
