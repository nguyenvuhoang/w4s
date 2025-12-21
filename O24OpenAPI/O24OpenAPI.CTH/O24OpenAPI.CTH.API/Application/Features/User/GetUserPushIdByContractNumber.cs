//using LinKit.Core.Cqrs;
//using O24OpenAPI.Framework.Models;
//using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;

//namespace O24OpenAPI.CTH.API.Application.Features.User
//{
//    public class GetUserPushIdByContractNumberCommnad: BaseTransactionModel, ICommand<CTHUserPushModel>
//    {

//    }

//    public class GetUserPushIdByContractNumberHandler(IUserDeviceRepository userDeviceRepository) : ICommandHandler<GetUserPushIdByContractNumberCommnad, CTHUserPushModel>
//    {
//        public async Task<CTHUserPushModel> HandleAsync(GetUserPushIdByContractNumberCommnad request, CancellationToken cancellationToken = default)
//        {
//        var userInfo = await GetUserCodeByContractNumber(contractNumber);
//            if (string.IsNullOrWhiteSpace(userInfo))
//            {
//                return null;
//            }

//            if (await userDeviceRepository.GetByUserCodeAsync(userInfo) is { } userDevice)
//            {
//                return new CTHUserPushModel
//                {
//                    UserCode = userInfo,
//                    PushID = userDevice.PushId ?? string.Empty,
//                    UserDeviceID = userDevice.DeviceId ?? string.Empty,
//                };
//            }

//            return null;
//        }
//    }
//}
