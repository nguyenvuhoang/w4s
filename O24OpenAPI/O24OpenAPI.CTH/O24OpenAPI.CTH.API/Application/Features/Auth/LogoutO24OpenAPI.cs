using LinKit.Core.Cqrs;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.Core.Constants;
using O24OpenAPI.CTH.API.Application.Constants;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Exceptions;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.CTH.API.Application.Features.Auth;

public class LogoutO24OpenAPICommand : BaseTransactionModel, ICommand<bool>
{
    /// <summary>
    /// Gets or sets the value of the login name
    /// </summary>
    public string LoginName { get; set; }


}

[CqrsHandler]
public class LogoutO24OpenAPIHandle(IUserAccountRepository userAccountRepository)
    : ICommandHandler<LogoutO24OpenAPICommand, bool>
{
    [WorkflowStep(WorkflowStepCode.CTH.WF_STEP_CTH_LOGOUT)]
    public async Task<bool> HandleAsync(
        LogoutO24OpenAPICommand request,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            string loginName = request.LoginName;
            string channelid = request.ChannelId;

            var userAccount =
                await userAccountRepository.GetByLoginNameandChannelAsync(loginName, channelid)
                ?? throw await O24Exception.CreateAsync(
                    O24CTHResourceCode.Validation.UsernameIsNotExist,
                    request.Language,
                    [loginName]
                );

            userAccount.IsLogin = false;
            await userAccountRepository.UpdateAsync(userAccount);
            return true;
        }
        catch (Exception ex)
        {
            await ex.LogErrorAsync();
            throw await O24Exception.CreateAsync(ResourceCode.Common.ServerError, request.Language);
        }
    }
}
