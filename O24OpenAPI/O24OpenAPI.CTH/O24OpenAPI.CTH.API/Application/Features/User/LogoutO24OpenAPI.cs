using LinKit.Core.Cqrs;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.Core.Constants;
using O24OpenAPI.CTH.API.Application.Constants;
using O24OpenAPI.CTH.API.Application.Models;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Exceptions;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Infrastructure.Mapper.Extensions;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.CTH.API.Application.Features.User;

public class LogoutO24OpenAPICommand : BaseTransactionModel, ICommand<bool>
{
    /// <summary>
    /// Gets or sets the value of the login name
    /// </summary>
    public string LoginName { get; set; }

    /// <summary>
    /// /// Gets or sets the value of the device
    /// </summary>
    public string DeviceId { get; set; }

    /// <summary>
    /// /// Gets or sets the value of the device
    /// </summary>
    public string DeviceType { get; set; }

    /// <summary>
    /// Gets or sets the value of the ip address
    /// </summary>
    public string IpAddress { get; set; }

    /// <summary>
    /// User Agent
    /// </summary>
    public string UserAgent { get; set; }

    /// <summary>
    /// OsVersion
    /// </summary>
    public string OsVersion { get; set; }

    /// <summary>
    /// App Version
    /// </summary>
    public string AppVersion { get; set; }

    /// <summary>
    /// Device Name
    /// </summary>
    public string DeviceName { get; set; }

    /// <summary>
    /// Brand
    /// </summary>
    public string Brand { get; set; }

    /// <summary>
    /// IsEmulator
    /// </summary>
    public bool IsEmulator { get; set; }

    /// <summary>
    /// IsRootedOrJailbroken
    /// </summary>
    public bool IsRootedOrJailbroken { get; set; }

    /// <summary>
    /// Modelname
    /// </summary>
    public string Modelname { get; set; } = string.Empty;
}

[CqrsHandler]
public class LogoutO24OpenAPIHandle(IUserAccountRepository userAccountRepository)
    : ICommandHandler<LogoutO24OpenAPICommand, bool>
{
    [WorkflowStep(WorkflowStep.CTH.WF_STEP_CTH_LOGOUT)]
    public async Task<bool> HandleAsync(
        LogoutO24OpenAPICommand request,
        CancellationToken cancellationToken = default
    )
    {
        var model = request.ToModel<LogoutO24OpenAPIRequestModel>();
        return await LogoutAsync(model);
    }

    public async Task<bool> LogoutAsync(LogoutO24OpenAPIRequestModel model)
    {
        try
        {
            var loginName = model.LoginName;
            var channelid = model.ChannelId;

            var userAccount =
                await userAccountRepository.GetByLoginNameandChannelAsync(loginName, channelid)
                ?? throw await O24Exception.CreateAsync(
                    O24CTHResourceCode.Validation.UsernameIsNotExist,
                    model.Language,
                    [loginName]
                );

            userAccount.IsLogin = false;
            await userAccountRepository.UpdateAsync(userAccount);
            return true;
        }
        catch (Exception ex)
        {
            await ex.LogErrorAsync();
            throw await O24Exception.CreateAsync(ResourceCode.Common.ServerError, model.Language);
        }
    }
}
