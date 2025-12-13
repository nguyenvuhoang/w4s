using Microsoft.AspNetCore.Mvc;
using O24OpenAPI.O24NCH.Models.Request.Telegram;
using O24OpenAPI.O24NCH.Services.Interfaces;
using O24OpenAPI.O24NCH.Services.Services;
using O24OpenAPI.O24NCH.Services.Services.Telegram;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace O24OpenAPI.O24NCH.Controllers;

[ProducesResponseType(typeof(string), 401)]
[ApiController]
[Produces("application/json", [])]
[Route("api/[controller]/[action]", Order = 2147483647)]
public partial class TelegramChannelController : ControllerBase
{
    private readonly ITelegramService _telegramService;
    private readonly ITelegramBotClient _telegramBotClient;
    public TelegramChannelController(ITelegramService telegramService, ITelegramBotClient telegramBotClient)
    {
        _telegramService = telegramService;
        _telegramBotClient = telegramBotClient;
    }

    [HttpPost]
    public async Task<IActionResult> SendMessage([FromBody] TelegramSendModel model)
    {
        if (string.IsNullOrWhiteSpace(model.ChatId) || string.IsNullOrWhiteSpace(model.Message))
        {
            return BadRequest("ChatId and Message are required.");
        }

        var result = await _telegramService.SendMessage(model);
        if (!result)
        {
            return StatusCode(500, "Failed to send message to Telegram.");
        }

        return Ok("Message sent successfully.");
    }

    [HttpGet("qr")]
    public IActionResult GetQrCode([FromQuery] string userCode)
    {
        if (string.IsNullOrWhiteSpace(userCode))
        {
            return BadRequest("userCode is required.");
        }

        var botUsername = "kcmteam_bot";
        var qrBytes = TelegramService.GenerateTelegramQrCode(botUsername, userCode);

        return File(qrBytes, "image/png");
    }

    [HttpPost]
    public async Task<string> SetWebHook([FromBody] string webhookUrl)
    {
        await _telegramBotClient.SetWebhook(webhookUrl, allowedUpdates: [], secretToken: "O24TELEGRAMBOT");
        return $"Webhook set to {webhookUrl}";
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Update update, [FromServices] ITelegramBotClient bot, [FromServices] UpdateHandler handleUpdateService, CancellationToken ct)
    {
        //if (Request.Headers["X-Telegram-Bot-Api-Secret-Token"] != Config.Value.SecretToken)
        //    return Forbid();
        try
        {
            await handleUpdateService.HandleUpdateAsync(bot, update, ct);
        }
        catch (Exception exception)
        {
            await handleUpdateService.HandleErrorAsync(bot, exception, Telegram.Bot.Polling.HandleErrorSource.HandleUpdateError, ct);
        }
        return Ok();
    }
}
