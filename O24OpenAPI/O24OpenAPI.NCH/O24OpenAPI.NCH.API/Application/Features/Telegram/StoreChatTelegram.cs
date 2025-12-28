using LinKit.Core.Cqrs;
using LinqToDB;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.NCH.Domain.AggregatesModel.TelegramAggregate;
using O24OpenAPI.NCH.Models.Request.Telegram;

namespace O24OpenAPI.NCH.API.Application.Features.Telegram;

public class StoreChatTelegramCommand : BaseTransactionModel, ICommand<bool>
{
    public string UserCode { get; set; }
    public string PhoneNumber { get; set; }
    public string ChatId { get; set; }
    public string TelegramUsername { get; set; }
    public string Fullname { get; set; }
    public string LanguageCode { get; set; }
    public bool? IsBot { get; set; }
    public DateTime CreatedOnUtc { get; set; }
}

[CqrsHandler]
public class StoreChatTelegramHandle(ITelegramChatMappingRepository repo)
    : ICommandHandler<StoreChatTelegramCommand, bool>
{
    public async Task<bool> HandleAsync(
        StoreChatTelegramCommand request,
        CancellationToken cancellationToken = default
    )
    {
        if (
            string.IsNullOrWhiteSpace(request.UserCode) || string.IsNullOrWhiteSpace(request.ChatId)
        )
        {
            return false;
        }

        var existing = await repo
            .Table.Where(x => x.UserCode == request.UserCode || x.ChatId == request.ChatId)
            .FirstOrDefaultAsync();

        if (existing != null)
        {
            existing.ChatId = request.ChatId;
            existing.PhoneNumber = request.PhoneNumber;
            existing.TelegramUsername = request.TelegramUsername;
            existing.Fullname = request.Fullname;
            existing.LanguageCode = request.LanguageCode;
            existing.IsBot = request.IsBot;
            existing.UpdatedOnUtc = DateTime.UtcNow;
            await repo.Update(existing);
        }
        else
        {
            var entity = new TelegramChatMapping
            {
                UserCode = request.UserCode,
                PhoneNumber = request.PhoneNumber,
                ChatId = request.ChatId,
                TelegramUsername = request.TelegramUsername,
                Fullname = request.Fullname,
                LanguageCode = request.LanguageCode,
                IsBot = request.IsBot,
                CreatedOnUtc = DateTime.UtcNow,
            };
            await repo.Insert(entity);
        }

        return true;
    }
}
