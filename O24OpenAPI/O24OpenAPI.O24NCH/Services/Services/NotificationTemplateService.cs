using O24OpenAPI.Data.System.Linq;
using O24OpenAPI.O24NCH.Domain;
using O24OpenAPI.O24NCH.Services.Interfaces;

namespace O24OpenAPI.O24NCH.Services.Services;

public class NotificationTemplateService(
    IRepository<NotificationTemplate> templateRepo
) : INotificationTemplateService
{
    private readonly IRepository<NotificationTemplate> _templateRepo = templateRepo;

    public async Task<NotificationTemplate> GetByTemplateIdAsync(string templateId)
    {
        return await _templateRepo.Table.FirstOrDefaultAsync(x => x.TemplateID == templateId);
    }

    public async Task<IReadOnlyList<NotificationTemplate>> GetByTemplateIdsAsync(IEnumerable<string> templateIds)
    {
        var ids = templateIds?.Distinct().ToList() ?? [];
        if (ids.Count == 0)
        {
            return [];
        }

        return await _templateRepo.Table
            .Where(x => ids.Contains(x.TemplateID))
            .ToListAsync();
    }

}
