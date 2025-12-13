using O24OpenAPI.O24NCH.Models.Request.Telegram;
using O24OpenAPI.O24NCH.Services.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace O24OpenAPI.O24NCH.Services.Services.Telegram;

public class UpdateHandler(ITelegramBotClient bot, ILogger<UpdateHandler> logger, ITelegramService telegramService) : IUpdateHandler
{
    private readonly ITelegramService _telegramService = telegramService;
    public async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, HandleErrorSource source, CancellationToken cancellationToken)
    {
        logger.LogInformation("HandleError: {Exception}", exception);
        if (exception is RequestException)
        {
            await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
        }
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        await (update switch
        {
            { Message: { } message } => OnMessage(message),
            { EditedMessage: { } message } => OnMessage(message),
            _ => UnknownUpdateHandlerAsync(update)
        });
    }

    private Task UnknownUpdateHandlerAsync(Update update)
    {
        logger.LogInformation("Unknown update type: {UpdateType}", update.Type);
        return Task.CompletedTask;
    }

    private async Task OnMessage(Message msg)
    {
        logger.LogInformation("Receive message type: {MessageType}", msg.Type);
        if (msg.Text is not { } messageText)
        {
            return;
        }

        if (messageText.StartsWith("/start"))
        {
            var rawData = messageText.Length > 7 ? messageText[7..].Trim() : string.Empty;

            var parts = rawData.Split('_', StringSplitOptions.RemoveEmptyEntries);
            var userCode = parts.Length > 0 ? parts[0] : null;
            var phoneNumber = parts.Length > 1 ? parts[1] : null;

            logger.LogInformation("➡️ /start received: ChatId = {ChatId}, UserCode = {UserCode}, Phone = {PhoneNumber}",
                msg.Chat.Id,
                userCode ?? "(none)",
                phoneNumber ?? "(none)");

            if (!string.IsNullOrEmpty(userCode))
            {
                var model = new TelegramChatModel
                {
                    UserCode = userCode,
                    PhoneNumber = phoneNumber,
                    ChatId = msg.Chat.Id.ToString(),
                    TelegramUsername = msg.From?.Username,
                    Fullname = $"{msg.From?.FirstName} {msg.From?.LastName}".Trim(),
                    LanguageCode = msg.From?.LanguageCode,
                    IsBot = msg.From?.IsBot,
                    CreatedOnUtc = DateTime.UtcNow
                };

                await _telegramService.StoreChatTelegram(model);
                await bot.SendMessage(msg.Chat, $"🔐 Telegram has been linked to your account: {userCode} and phone number: {phoneNumber}");
            }
            else
            {
                await bot.SendMessage(msg.Chat, "⚠️ User code not found. Please scan the QR code from the app to link your Telegram.");
            }

            return;
        }

    }
}
