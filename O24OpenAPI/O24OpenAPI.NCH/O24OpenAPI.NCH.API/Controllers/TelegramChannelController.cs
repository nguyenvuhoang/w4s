using Microsoft.AspNetCore.Mvc;
using O24OpenAPI.NCH.Models.Request.Telegram;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace O24OpenAPI.NCH.Controllers;

[ProducesResponseType(typeof(string), 401)]
[ApiController]
[Produces("application/json", [])]
[Route("api/[controller]/[action]", Order = int.MaxValue)]
public partial class TelegramChannelController : ControllerBase
{
    private readonly ITelegramBotClient _telegramBotClient;

    public TelegramChannelController(ITelegramBotClient telegramBotClient)
    {
        _telegramBotClient = telegramBotClient;
    }

    [HttpPost]
    public async Task<IActionResult> SendMessage([FromBody] TelegramSendModel model)
    {
        if (string.IsNullOrWhiteSpace(model.ChatId) || string.IsNullOrWhiteSpace(model.Message))
        {
            return BadRequest("ChatId and Message are required.");
        }

        try
        {
            // Telegram chat id có thể là numeric string; Telegram.Bot nhận ChatId kiểu long/string đều được qua implicit.
            await _telegramBotClient.SendMessage(model.ChatId, model.Message);
            return Ok("Message sent successfully.");
        }
        catch (Exception ex)
        {
            // Giữ đơn giản để project build/run trước. Sau này cần log đúng chuẩn framework.
            return StatusCode(500, $"Failed to send message to Telegram. {ex.Message}");
        }
    }

    /// <summary>
    /// Trả về link deep-link để app/khách hàng tự generate QR ở client (không phụ thuộc QRCoder).
    /// </summary>
    [HttpGet("qr")]
    public IActionResult GetQrCode([FromQuery] string userCode)
    {
        if (string.IsNullOrWhiteSpace(userCode))
        {
            return BadRequest("userCode is required.");
        }

        var botUsername = "kcmteam_bot";
        var url = $"https://t.me/{botUsername}?start={userCode}";
        return Ok(new { url });
    }

    [HttpPost]
    public async Task<string> SetWebHook([FromBody] string webhookUrl)
    {
        await _telegramBotClient.SetWebhook(webhookUrl, allowedUpdates: [], secretToken: "O24TELEGRAMBOT");
        return $"Webhook set to {webhookUrl}";
    }

    /// <summary>
    /// Webhook endpoint (tạm thời chỉ ACK để build/run). Sau này muốn xử lý /start thì port UpdateHandler từ O24NCH.
    /// </summary>
    [HttpPost]
    public IActionResult Post([FromBody] Update update, CancellationToken ct)
    {
        // TODO: handle telegram update if needed
        return Ok();
    }
}
