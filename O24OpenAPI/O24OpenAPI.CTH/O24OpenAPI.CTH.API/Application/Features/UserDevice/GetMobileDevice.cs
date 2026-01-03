using LinKit.Core.Cqrs;
using LinqToDB;
using O24OpenAPI.APIContracts.Models.CTH;
using O24OpenAPI.CTH.API.Application.Constants;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;

namespace O24OpenAPI.CTH.API.Application.Features.UserDevice;

public class GetMobileDeviceQuery : IQuery<List<CTHUserNotificationModel>> { }

[CqrsHandler]
public class GetMobileDeviceQueryHandler(
    IUserDeviceRepository userDeviceRepository,
    IUserAccountRepository userAccountRepository
) : IQueryHandler<GetMobileDeviceQuery, List<CTHUserNotificationModel>>
{
    private readonly IUserDeviceRepository _userDeviceRepository = userDeviceRepository;
    private readonly IUserAccountRepository _userAccountRepository = userAccountRepository;

    public async Task<List<CTHUserNotificationModel>> HandleAsync(
        GetMobileDeviceQuery query,
        CancellationToken cancellationToken
    )
    {
        var q =
            from d in _userDeviceRepository.Table
            join u in _userAccountRepository.Table on d.UserCode equals u.UserCode into gj
            from ua in gj.DefaultIfEmpty()
            where
                d.ChannelId == Code.Channel.MB
                && d.Status == DeviceStatus.ACTIVE
                && !string.IsNullOrEmpty(d.PushId)
            select new CTHUserNotificationModel
            {
                UserCode = d.UserCode,
                PushId = d.PushId ?? string.Empty,
                PhoneNumber =
                    ua != null && !string.IsNullOrEmpty(ua.Phone) ? ua.Phone : string.Empty,
                UserDevice = d.DeviceId,
            };

        var result = await q.ToListAsync(token: cancellationToken);

        Console.WriteLine($"Count Mobile Device = {result.Count}");

        return result;
    }
}
