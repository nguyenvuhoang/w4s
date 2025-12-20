using LinKit.Core.Cqrs;
using LinKit.Core.Endpoints;
using Microsoft.Extensions.AI;
using Test.Models;

namespace Test.Features;

[ApiEndpoint(ApiMethod.Post, "/chat")]
public sealed record ChatCommand(Guid UserId, string Message) : ICommand<ChatResult>;

public sealed record ChatResult(string Answer);

[CqrsHandler]
public class ChatHandler(IChatClient chatClient) : ICommandHandler<ChatCommand, ChatResult>
{
    public async Task<ChatResult> HandleAsync(
        ChatCommand request,
        CancellationToken cancellationToken = default
    )
    {
        // 1️⃣ Fake domain profile
        var profile = CreateFakeProfile(request.UserId);

        // 2️⃣ Build prompt từ Domain
        var prompt = $"""
            Bạn là chuyên gia tài chính cá nhân.

            Thông tin người dùng:
            - Thu nhập hàng tháng: {profile.MonthlyIncome.Value:N0} VND
            - Tổng chi tiêu hàng tháng: {profile.TotalMonthlyExpense:N0} VND
            - Khả năng tiết kiệm: {profile.SavingCapacity:N0} VND
            - Tỷ lệ tiết kiệm: {profile.CalculateSavingRate():P0}
            - Số khoản vay hiện tại: {profile.Loans.Count}

            Câu hỏi người dùng:
            {request.Message}

            Hãy phân tích rõ ràng, thực tế và có khuyến nghị cụ thể.
            """;

        // 3️⃣ Gọi AI (API mới)
        var response = await chatClient.GetResponseAsync(
            new ChatMessage(ChatRole.User, prompt),
            new ChatOptions { Temperature = 0.3f },
            cancellationToken
        );

        return new ChatResult(response.Text);
    }

    private static FinancialProfile CreateFakeProfile(Guid userId)
    {
        return new FinancialProfile(
            new UserId(userId),
            new Money(30_000_000),
            new[]
            {
                new SpendingItem(
                    SpendingCategory.Housing,
                    new Money(8_000_000),
                    SpendingFrequency.Monthly
                ),
                new SpendingItem(
                    SpendingCategory.Food,
                    new Money(5_000_000),
                    SpendingFrequency.Monthly
                ),
                new SpendingItem(
                    SpendingCategory.Transportation,
                    new Money(2_000_000),
                    SpendingFrequency.Monthly
                ),
                new SpendingItem(
                    SpendingCategory.Entertainment,
                    new Money(1_500_000),
                    SpendingFrequency.Monthly
                ),
            },
            new[] { new Loan("Vay tiêu dùng", new Money(50_000_000), 0.18m) }
        );
    }
}
