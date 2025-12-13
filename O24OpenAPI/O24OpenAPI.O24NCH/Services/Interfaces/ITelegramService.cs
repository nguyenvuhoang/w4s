using O24OpenAPI.O24NCH.Models.Request.Telegram;

namespace O24OpenAPI.O24NCH.Services.Interfaces;

public interface ITelegramService
{
    /// <summary>
    /// Send Message
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<bool> SendMessage(TelegramSendModel model);
    /// <summary>
    /// Store Chat Telegram
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<bool> StoreChatTelegram(TelegramChatModel model);
}
