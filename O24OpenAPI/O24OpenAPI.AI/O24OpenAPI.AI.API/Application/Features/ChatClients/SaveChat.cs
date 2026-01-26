using LinKit.Core.Cqrs;
using O24OpenAPI.AI.Domain.AggregatesModel.ChatHistoryAggregate;

namespace O24OpenAPI.AI.API.Application.Features.ChatClients;

public class SaveChatCommand : ICommand<bool>
{
    public SaveChatCommand() { }

    public SaveChatCommand(string userCode, string role, string content)
    {
        UserCode = userCode;
        Role = role;
        Content = content;
    }

    public string UserCode { get; set; }
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
