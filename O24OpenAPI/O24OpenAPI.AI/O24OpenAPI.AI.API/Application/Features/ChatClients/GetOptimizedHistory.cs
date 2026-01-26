using LinKit.Core.Cqrs;
using LinqToDB;
using Microsoft.Extensions.AI;
using O24OpenAPI.AI.Domain.AggregatesModel.ChatHistoryAggregate;
using O24OpenAPI.AI.Domain.AggregatesModel.ChatMessageAggregate;

namespace O24OpenAPI.AI.API.Application.Features.ChatClients;

public class GetOptimizedHistoryCommand : ICommand<List<ChatMessage>>
{
    public int WalletId { get; set; }
}

[CqrsHandler]
public class GetOptimizedHistoryHandler(
    IChatHistoryRepository chatHistoryRepository,
    IChatClient chatClient
) : ICommandHandler<GetOptimizedHistoryCommand, List<ChatMessage>>
{
    private const int BufferSize = 6; // Giữ 6 tin nhắn gần nhất

    public async Task<List<ChatMessage>> HandleAsync(
        GetOptimizedHistoryCommand request,
        CancellationToken cancellationToken
    )
    {
        int walletId = request.WalletId;
        ChatHistory lastSummary = await chatHistoryRepository
            .Table.Where(m => m.WalletId == walletId && m.Role == "summary")
            .OrderByDescending(m => m.CreatedOnUtc)
            .FirstOrDefaultAsync();

        // 2. Lấy các tin nhắn chi tiết chưa bị tóm tắt
        List<ChatHistory> recentMessages = await chatHistoryRepository
            .Table.Where(m => m.WalletId == walletId && m.Role != "summary" && !m.IsSummarized)
            .OrderBy(m => m.CreatedOnUtc)
            .ToListAsync();

        // 3. Nếu số lượng tin nhắn vượt quá Buffer, tiến hành tóm tắt bớt
        if (recentMessages.Count > BufferSize)
        {
            return await SummarizeOldMessagesAsync(walletId, lastSummary, recentMessages);
        }

        // 4. Build danh sách gửi cho AI
        var historyForAI = new List<ChatMessage>();
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
        int walletId,
        ChatHistory? oldSummary,
        List<ChatHistory> recentMessages
    )
    {
        // Tách những tin nhắn cũ cần tóm tắt (tất cả trừ N câu cuối)
        int messagesToSummarizeCount = recentMessages.Count - BufferSize;
        var toSummarize = recentMessages.Take(messagesToSummarizeCount).ToList();
        var remaining = recentMessages.Skip(messagesToSummarizeCount).ToList();

        // Tạo prompt tóm tắt
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

        // Lưu bản tóm tắt mới và đánh dấu tin nhắn cũ
        foreach (ChatHistory msg in toSummarize)
            msg.IsSummarized = true;

        await chatHistoryRepository.InsertAsync(
            new ChatHistory
            {
                WalletId = walletId,
                Role = "summary",
                Content = newSummaryContent,
            }
        );

        // Trả về danh sách đã tối ưu: [System Summary] + [Remaining Messages]
        var result = new List<ChatMessage>
        {
            new(ChatRole.System, $"Tóm tắt hội thoại trước đó: {newSummaryContent}"),
        };
        result.AddRange(remaining.Select(m => new ChatMessage(new ChatRole(m.Role), m.Content)));
        return result;
    }
}
