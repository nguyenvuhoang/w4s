using Newtonsoft.Json;

namespace O24OpenAPI.O24NCH.Models.Request.Telegram;

public class TelegramSendModel : NotificationRequestModel
{
    [JsonProperty("chat_id")]
    public string ChatId { get; set; }
    [JsonProperty("message")]
    public string Message { get; set; }
}
