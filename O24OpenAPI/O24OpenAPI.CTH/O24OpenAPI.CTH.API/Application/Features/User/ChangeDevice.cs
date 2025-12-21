//using LinKit.Core.Cqrs;
//using O24OpenAPI.CTH.Constant;
//using O24OpenAPI.Framework.Models;
//using O24OpenAPI.CTH.Models;
//using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
//using O24OpenAPI.CTH.API.Application.Constants;
//using O24OpenAPI.CTH.API.Application.Models;

//namespace O24OpenAPI.CTH.API.Application.Features.User
//{
//    public class ChangeDeviceCommnad: BaseTransactionModel, ICommand<AuthResponseModel>
//    {
//        public string UserCode { get; set; }
//        public string Phone { get; set; }
//        public DateTime DOB { get; set; }
//        public string LicenseType { get; set; }
//        public string LicenseId { get; set; }
//        public string DeviceId { get; set; }
//        public string DeviceType { get; set; }
//        public string IpAddress { get; set; }
//        public string Modelname { get; set; }
//        public string UserAgent { get; set; }
//        public string PushId { get; set; }
//        public string OsVersion { get; set; }
//        public string AppVersion { get; set; }
//        public string DeviceName { get; set; }
//        public string Brand { get; set; }
//        public bool IsEmulator { get; set; }
//        public bool IsRootedOrJailbroken { get; set; }
//    }

//    public class ChangeDeviceHandler(IUserDeviceRepository userDeviceRepository, IAuthSessionRepository authSessionRepository) : ICommandHandler<ChangeDeviceCommnad, AuthResponseModel>
//    {
//        public async Task<AuthResponseModel> HandleAsync(ChangeDeviceCommnad request, CancellationToken cancellationToken = default)
//        {
//        var userAccount =
//                await GetByUserCodeAsync(request.UserCode)
//                ?? throw await O24Exception.CreateAsync(
//                    O24CTHResourceCode.Validation.UsernameIsExisting,
//                    request.Language
//                );

//            if (userAccount.Phone != request.Phone)
//            {
//                throw await O24Exception.CreateAsync(
//                    O24CTHResourceCode.Validation.PhoneNumberIsNotExisting,
//                    request.Language,
//                    [request.Phone]
//                );
//            }

//            if (string.IsNullOrEmpty(userAccount.ContractNumber))
//            {
//                throw await O24Exception.CreateAsync(
//                    O24CTHResourceCode.Validation.ContractNumberIsNotExisting,
//                    request.Language,
//                    [userAccount.Phone]
//                );
//            }

//            try
//            {
//                await userDeviceRepository.EnsureUserDeviceAsync(
//                    userCode: userAccount.UserCode,
//                    loginName: userAccount.LoginName,
//                    deviceId: request.DeviceId + request.Modelname ?? "",
//                    deviceType: request.DeviceType,
//                    userAgent: request.UserAgent,
//                    ipAddress: request.IpAddress,
//                    channelId: request.ChannelId,
//                    pushId: request.PushId,
//                    osVersion: request.OsVersion,
//                    appVersion: request.AppVersion,
//                    deviceName: request.DeviceName,
//                    brand: request.Brand,
//                    isEmulator: request.IsEmulator,
//                    isRooted: request.IsRootedOrJailbroken,
//                    language: request.Language,
//                    isResetDevice: true
//                );
//            }
//            catch (Exception ex)
//            {
//                await ex.LogErrorAsync();
//                throw await O24Exception.CreateAsync(
//                    O24CTHResourceCode.Operation.ChangeDeviceError,
//                    request.Language
//                );
//            }

//            var context = new LoginContextModel
//            {
//                DeviceId = request.DeviceId,
//                Modelname = request.Modelname,
//                RoleChannel = userAccount.RoleChannel,
//                IpAddress = request.IpAddress,
//                ChannelId = request.ChannelId,
//                Reference = request.DeviceId + request.Modelname ?? "",
//            };

//            userAccount.LastLoginTime = DateTime.Now;
//            userAccount.UUID = Guid.NewGuid().ToString();
//            userAccount.Failnumber = 0;
//            userAccount.IsLogin = true;

//            await UpdateAsync(userAccount);

//            return await authSessionRepository.CreateTokenAndSessionAsync(userAccount, context);
//        }
//    }
//}
