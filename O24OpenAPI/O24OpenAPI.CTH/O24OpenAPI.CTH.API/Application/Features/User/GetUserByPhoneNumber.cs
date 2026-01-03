using LinKit.Core.Cqrs;
using LinqToDB;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.Core.Constants;
using O24OpenAPI.CTH.API.Application.Constants;
using O24OpenAPI.CTH.API.Application.Models;
using O24OpenAPI.CTH.API.Application.Models.User;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Exceptions;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Infrastructure.Mapper.Extensions;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.Framework.Utils;

namespace O24OpenAPI.CTH.API.Application.Features.User;

public class GetUserByPhoneNumberCommand : BaseTransactionModel, ICommand<UserInfoModel>
{
    public string PhoneNumber { get; set; }
}

[CqrsHandler]
public class GetUserByPhoneNumberHandle(
    IUserAccountRepository userAccountRepository,
    IUserDeviceRepository userDeviceRepository
) : ICommandHandler<GetUserByPhoneNumberCommand, UserInfoModel>
{
    [WorkflowStep(WorkflowStepCode.CTH.WF_STEP_CTH_GET_USER_BY_PHONE)]
    public async Task<UserInfoModel> HandleAsync(
        GetUserByPhoneNumberCommand request,
        CancellationToken cancellationToken = default
    )
    {
        var model = request.ToModel<UserWithPhoneNumber>();
        return await GetUserByPhoneNumberASync(model);
    }

    public async Task<UserInfoModel> GetUserByPhoneNumberASync(UserWithPhoneNumber model)
    {
        try
        {
            ConsoleUtil.WriteInfo(
                $"[GetUserByPhoneNumberASync] Start processing for phone number: {model.PhoneNumber}"
            );
            var user =
                await userAccountRepository
                    .Table.Where(s =>
                        s.Phone == model.PhoneNumber
                        && s.Status != Common.DELETED
                        && s.ChannelId == Code.Channel.MB
                    )
                    .FirstOrDefaultAsync()
                ?? throw await O24Exception.CreateAsync(
                    O24CTHResourceCode.Validation.PhoneNumberIsExisting,
                    model.Language,
                    [model.PhoneNumber]
                );

            var userDevice = await userDeviceRepository.GetByUserCodeAsync(user.UserCode);
            var userInfo = new UserInfoModel
            {
                UserId = user.UserId,
                UserCode = user.UserCode,
                LoginName = user.LoginName,
                FullName = $"{user.FirstName} {user.MiddleName} {user.LastName}",
                Email = user.Email,
                PhoneNumber = user.Phone,
                ChannelId = user.ChannelId,
                UserDeviceId = userDevice?.DeviceId,
                UserPushId = userDevice?.PushId,
            };
            return userInfo;
        }
        catch (O24Exception)
        {
            throw;
        }
        catch (Exception ex)
        {
            await ex.LogErrorAsync();
            throw await O24Exception.CreateAsync(
                ResourceCode.Common.SystemError,
                model.Language,
                ex
            );
        }
    }
}
