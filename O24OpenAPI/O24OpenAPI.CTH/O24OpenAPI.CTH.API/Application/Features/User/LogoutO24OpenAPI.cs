using LinKit.Core.Cqrs;
using O24OpenAPI.Client.Scheme.Workflow;
using O24OpenAPI.Core.Constants;
using O24OpenAPI.CTH.API.Application.Constants;
using O24OpenAPI.CTH.API.Application.Models;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Exceptions;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.CTH.API.Application.Features.User
{
    public class LogoutO24OpenAPICommand : BaseTransactionModel, ICommand<bool>
    {
        public LogoutO24OpenAPIRequestModel Model { get; set; } = default!;
    }

    [CqrsHandler]
    public class LogoutO24OpenAPIHandle(IUserAccountRepository userAccountRepository)
        : ICommandHandler<LogoutO24OpenAPICommand, bool>
    {
        [WorkflowStep("WF_STEP_CTH_LOGOUT")]
        public async Task<bool> HandleAsync(
            LogoutO24OpenAPICommand request,
            CancellationToken cancellationToken = default
        )
        {
            return await LogoutAsync(request.Model);
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
                throw await O24Exception.CreateAsync(
                    ResourceCode.Common.ServerError,
                    model.Language
                );
            }
        }
    }
}
