//using LinKit.Core.Cqrs;
//using LinqToDB;
//using O24OpenAPI.CTH.Constant;
//using O24OpenAPI.Framework.Models;
//using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
//using O24OpenAPI.CTH.API.Application.Constants;

//namespace O24OpenAPI.CTH.API.Application.Features.User
//{
//    public class GetMobileDeviceCommnad: BaseTransactionModel, ICommand<virtual Task<List<CTHUserNotificationModel>>>
//    {

//    }

//    public class GetMobileDeviceHandler(IUserDeviceRepository userDeviceRepository, IUserAccountRepository userAccountRepository) : ICommandHandler<GetMobileDeviceCommnad, virtual Task<List<CTHUserNotificationModel>>>
//    {
//        public async Task<virtual Task<List<CTHUserNotificationModel>>> HandleAsync(GetMobileDeviceCommnad request, CancellationToken cancellationToken = default)
//        {
//        var query =
//                from d in userDeviceRepository.Table
//                join u in userAccountRepository.Table on d.UserCode equals u.UserCode into gj
//                from ua in gj.DefaultIfEmpty()
//                where
//                    d.ChannelId == Code.Channel.MB
//                    && d.Status == DeviceStatus.ACTIVE
//                    && !string.IsNullOrEmpty(d.PushId)
//                select new CTHUserNotificationModel
//                {
//                    UserCode = d.UserCode,
//                    PushId = d.PushId ?? string.Empty,
//                    PhoneNumber =
//                        ua != null && !string.IsNullOrEmpty(ua.Phone) ? ua.Phone : string.Empty,
//                    UserDevice = d.DeviceId,
//                };

//            var result = await query.ToListAsync();

//            ConsoleUtil.WriteInfo($"Count Mobile Device = {result.Count}");

//            return result;
//        }
//    }
//}
