using LinKit.Core.Cqrs;
using O24OpenAPI.AI.Domain.AggregatesModel.ChatHistoryAggregate;

namespace O24OpenAPI.AI.API.Application.Features.ChatClients;

public class SaveChatCommand : ICommand<bool>
{
    public SaveChatCommand() { }

    public SaveChatCommand(int walletId, string role, string content)
    {
        WalletId = walletId;
        Role = role;
        Content = content;
    }

    public int WalletId { get; set; }
    public string Role { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}

[CqrsHandler]
public class SaveChatCommandHandler(IChatHistoryRepository chatHistoryRepository)
    : ICommandHandler<SaveChatCommand, bool>
{
    public async Task<bool> HandleAsync(
        SaveChatCommand request,
        CancellationToken cancellationToken
    )
    {
        await chatHistoryRepository.InsertAsync(request.ToChatHistory());
        return true;
    }
}
