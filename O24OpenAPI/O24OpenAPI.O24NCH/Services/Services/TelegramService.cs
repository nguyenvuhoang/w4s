using LinqToDB;
using O24OpenAPI.O24NCH.Domain;
using O24OpenAPI.O24NCH.Models.Request.Telegram;
using O24OpenAPI.O24NCH.Services.Interfaces;
using QRCoder;
using System.Text;
using System.Text.Json;

namespace O24OpenAPI.O24NCH.Services.Services;

public class TelegramService(IRepository<TelegramChatMapping> telegramRepository) : ITelegramService
{
    private readonly HttpClient _httpClient = new HttpClient();
    private readonly IRepository<TelegramChatMapping> _telegramRepository = telegramRepository;

    public async Task<bool> SendMessage(TelegramSendModel model)
    {
        if (string.IsNullOrEmpty(model.ChatId) || string.IsNullOrEmpty(model.Message))
        {
            return false;
        }

        var _botToken = "8197855911:AAHId-nVPso_dkkBTsE5WHO-Ae0dp9HzMpE";
        var requestUrl = $"https://api.telegram.org/bot{_botToken}/sendMessage";

        var payload = new
        {
            chat_id = model.ChatId,
            text = model.Message,
            parse_mode = "Markdown"
        };
        var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(requestUrl, content);

        return response.IsSuccessStatusCode;
    }

    /// <summary>
    /// GenerateTelegramQrCode
    /// </summary>
    /// <param name="botUsername"></param>
    /// <param name="userCode"></param>
    /// <returns></returns>
    public static byte[] GenerateTelegramQrCode(string botUsername, string userCode)
    {
        var url = $"https://t.me/{botUsername}?start={userCode}";
        var qrGenerator = new QRCodeGenerator();
        var qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);

        var qrCode = new PngByteQRCode(qrCodeData);
        return qrCode.GetGraphic(20);
    }

    public async Task<bool> StoreChatTelegram(TelegramChatModel model)
    {
        if (string.IsNullOrWhiteSpace(model.UserCode) || string.IsNullOrWhiteSpace(model.ChatId))
        {
            return false;
        }

        var existing = await _telegramRepository.Table
            .Where(x => x.UserCode == model.UserCode || x.ChatId == model.ChatId)
            .FirstOrDefaultAsync();

        if (existing != null)
        {
            existing.ChatId = model.ChatId;
            existing.PhoneNumber = model.PhoneNumber;
            existing.TelegramUsername = model.TelegramUsername;
            existing.Fullname = model.Fullname;
            existing.LanguageCode = model.LanguageCode;
            existing.IsBot = model.IsBot;
            existing.UpdatedOnUtc = DateTime.UtcNow;
            await _telegramRepository.Update(existing);
        }
        else
        {
            var entity = new TelegramChatMapping
            {
                UserCode = model.UserCode,
                PhoneNumber = model.PhoneNumber,
                ChatId = model.ChatId,
                TelegramUsername = model.TelegramUsername,
                Fullname = model.Fullname,
                LanguageCode = model.LanguageCode,
                IsBot = model.IsBot,
                CreatedOnUtc = DateTime.UtcNow
            };
            await _telegramRepository.Insert(entity);
        }

        return true;
    }

}
