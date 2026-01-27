using LinKit.Core.Cqrs;
using LinqToDB;
using Microsoft.Extensions.AI;
using O24OpenAPI.AI.Domain.AggregatesModel.ChatHistoryAggregate;

namespace O24OpenAPI.AI.API.Application.Features.ChatClients;

public class GetOptimizedHistoryCommand : ICommand<List<ChatMessage>>
{
    public GetOptimizedHistoryCommand() { }

    public GetOptimizedHistoryCommand(string conversationId, string userCode)
    {
        ConversationId = conversationId;
        UserCode = userCode;
    }

    public string ConversationId { get; set; }
    public string UserCode { get; set; }
}

[CqrsHandler]
public class GetOptimizedHistoryHandler(
    IChatHistoryRepository chatHistoryRepository,
    IChatClient chatClient
) : ICommandHandler<GetOptimizedHistoryCommand, List<ChatMessage>>
{
    private const int BufferSize = 6;

    public async Task<List<ChatMessage>> HandleAsync(
        GetOptimizedHistoryCommand request,
        CancellationToken cancellationToken
    )
    {
        string conversationId = request.ConversationId;
        ChatHistory lastSummary = await chatHistoryRepository
            .Table.Where(m => m.ConversationId == conversationId && m.Role == "summary")
            .OrderByDescending(m => m.CreatedOnUtc)
            .FirstOrDefaultAsync();

        List<ChatHistory> recentMessages = await chatHistoryRepository
            .Table.Where(m =>
                m.ConversationId == conversationId && m.Role != "summary" && !m.IsSummarized
            )
            .OrderBy(m => m.CreatedOnUtc)
            .ToListAsync(token: cancellationToken);

        if (recentMessages.Count > BufferSize)
        {
            return await SummarizeOldMessagesAsync(conversationId, lastSummary, recentMessages);
        }

        List<ChatMessage> historyForAI = [];
        if (lastSummary != null)
        {
            historyForAI.Add(
                new ChatMessage(
                    ChatRole.System,
                    $"Tóm tắt nội dung hội thoại trước đó: {lastSummary.Content}"
                )
            );
        }

        historyForAI.AddRange(
            recentMessages.Select(m => new ChatMessage(new ChatRole(m.Role), m.Content))
        );
        return historyForAI;
    }

    private async Task<List<ChatMessage>> SummarizeOldMessagesAsync(
        string conversationId,
        ChatHistory oldSummary,
        List<ChatHistory> recentMessages
    )
    {
        int messagesToSummarizeCount = recentMessages.Count - BufferSize;
        List<ChatHistory> toSummarize = recentMessages.Take(messagesToSummarizeCount).ToList();
        List<ChatHistory> remaining = recentMessages.Skip(messagesToSummarizeCount).ToList();

        string historyText = string.Join("\n", toSummarize.Select(m => $"{m.Role}: {m.Content}"));
        string summaryPrompt = $"""
            Hãy tóm tắt nội dung hội thoại sau đây một cách cực kỳ ngắn gọn (dưới 100 từ),
            giữ lại các ý chính về tài chính và yêu cầu của người dùng.
            Nội dung tóm tắt cũ (nếu có): {oldSummary?.Content ?? "Không có"}
            Hội thoại mới cần bổ sung vào bản tóm tắt:
            {historyText}
            """;

        ChatResponse summaryResponse = await chatClient.GetResponseAsync(summaryPrompt);
        string newSummaryContent = summaryResponse.Text ?? "";

        foreach (ChatHistory msg in toSummarize)
        {
            msg.IsSummarized = true;
            await chatHistoryRepository.Update(msg);
        }

        await chatHistoryRepository.InsertAsync(
            new ChatHistory
            {
                ConversationId = conversationId,
                UserCode = oldSummary?.UserCode ?? toSummarize.First().UserCode,
                Role = "summary",
                Content = newSummaryContent,
            }
        );

        List<ChatMessage> result =
        [
            new(ChatRole.System, $"Tóm tắt hội thoại trước đó: {newSummaryContent}"),
            .. remaining.Select(m => new ChatMessage(new ChatRole(m.Role), m.Content)),
        ];
        return result;
    }
}
