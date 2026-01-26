//using LinKit.Core.Cqrs;
//using MimeKit;
//using Newtonsoft.Json;
//using O24OpenAPI.Framework.Models;
//using System.Text;
//using System.Text.Json;

//namespace O24OpenAPI.NCH.API.Application.Features.Telegram;

//public class SendTelegramMessageCommand : BaseTransactionModel, ICommand<bool>
//{
//    [JsonProperty("chat_id")]
//    public string ChatId { get; set; }

//    [JsonProperty("message")]
//    public string Message { get; set; }
//    public string UserCode { get; set; }
//    public string PhoneNumber { get; set; }
//    public string Purpose { get; set; }
//    public string NotificationType { get; set; }
//    public string Email { get; set; }
//    public Dictionary<string, object> SenderData { get; set; }
//    public Dictionary<string, object> DataTemplate { get; set; } = [];
//    public List<string> AttachmentBase64Strings { get; set; } = [];
//    public List<string> AttachmentFilenames { get; set; } = [];
//    public List<MimeEntity> MimeEntities { get; set; } = [];
//    public List<int> FileIds { get; set; } = [];
//}

//[CqrsHandler]
//public class SendTelegramMessageHandle()
//    : ICommandHandler<SendTelegramMessageCommand, bool>
//{
//    public async Task<bool> HandleAsync(
//        SendTelegramMessageCommand request,
//        CancellationToken cancellationToken = default
//    )
//    {
//        if (string.IsNullOrEmpty(request.ChatId) || string.IsNullOrEmpty(request.Message))
//        {
//            return false;
//        }

//        string _botToken = "8197855911:AAHId-nVPso_dkkBTsE5WHO-Ae0dp9HzMpE";
//        string requestUrl = $"https://api.telegram.org/bot{_botToken}/sendMessage";

//        var payload = new
//        {
//            chat_id = request.ChatId,
//            text = request.Message,
//            parse_mode = "Markdown",
//        };
//        var content = new StringContent(
//            System.Text.Json.JsonSerializer.Serialize(payload),
//            Encoding.UTF8,
//            "application/json"
//        );
//        var response = await httpClient.PostAsync(requestUrl, content);

//        return response.IsSuccessStatusCode;
//    }
//}
