using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.NCH.Domain.AggregatesModel.TelegramAggregate;

public interface ITelegramChatMappingRepository : IRepository<TelegramChatMapping>
{
    Task<TelegramChatMapping?> GetByChatIdAsync(string chatId);
}
