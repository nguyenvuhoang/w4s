//using LinKit.Core.Cqrs;
//using LinqToDB;
//using O24OpenAPI.CTH.Constant;
//using O24OpenAPI.Framework.Models;
//using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
//using O24OpenAPI.CTH.API.Application.Constants;
//using O24OpenAPI.CTH.API.Application.Models.User;

//namespace O24OpenAPI.CTH.API.Application.Features.User
//{
//    public class GetUserByPhoneNumberASyncCommnad: BaseTransactionModel, ICommand<UserInfoModel>
//    {
//        public string PhoneNumber { get; set; }
//    }

//    public class GetUserByPhoneNumberASyncHandler(IUserAccountRepository userAccountRepository, IUserDeviceRepository userDeviceRepository) : ICommandHandler<GetUserByPhoneNumberASyncCommnad, UserInfoModel>
//    {
//        public async Task<UserInfoModel> HandleAsync(GetUserByPhoneNumberASyncCommnad request, CancellationToken cancellationToken = default)
//        {
//        try
//            {
//                ConsoleUtil.WriteInfo(
//                    $"[GetUserByPhoneNumberASync] Start processing for phone number: {request.PhoneNumber}"
//                );
//                var user =
//                    await userAccountRepository
//                        .Table.Where(s =>
//                            s.Phone == request.PhoneNumber
//                            && s.Status != Common.DELETED
//                            && s.ChannelId == Code.Channel.MB
//                        )
//                        .FirstOrDefaultAsync()
//                    ?? throw await O24Exception.CreateAsync(
//                        O24CTHResourceCode.Validation.PhoneNumberIsExisting,
//                        request.Language,
//                        [request.PhoneNumber]
//                    );

//                var userDevice = await userDeviceRepository.GetByUserCodeAsync(user.UserCode);
//                var userInfo = new UserInfoModel
//                {
//                    UserId = user.UserId,
//                    UserCode = user.UserCode,
//                    LoginName = user.LoginName,
//                    FullName = $"{user.FirstName} {user.MiddleName} {user.LastName}",
//                    Email = user.Email,
//                    PhoneNumber = user.Phone,
//                    ChannelId = user.ChannelId,
//                    UserDeviceId = userDevice?.DeviceId,
//                    UserPushId = userDevice?.PushId,
//                };
//                return userInfo;
//            }
//            catch (O24Exception)
//            {
//                throw;
//            }
//            catch (Exception ex)
//            {
//                await ex.LogErrorAsync();
//                throw await O24Exception.CreateAsync(
//                    ResourceCode.Common.SystemError,
//                    request.Language,
//                    ex
//                );
//            }
//        }
//    }
//}
