using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.AI.Domain.AggregatesModel.ChatMessageAggregate;

public class ChatHistory : BaseEntity
{
    public int WalletId { get; set; }
    public string Role { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public bool IsSummarized { get; set; } = false;
}
