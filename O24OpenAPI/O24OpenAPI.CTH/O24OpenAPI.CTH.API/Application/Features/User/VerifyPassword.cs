using LinKit.Core.Cqrs;
using LinqToDB;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.CTH.API.Application.Utils;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.CTH.API.Application.Features.User;

public class VerifyPasswordCommand : BaseTransactionModel, ICommand<bool>
{
    public string Password { get; set; }
    public string UserCode { get; set; }
}

[CqrsHandler]
public class VerifyPasswordHandle(IUserPasswordRepository userPasswordRepository)
    : ICommandHandler<VerifyPasswordCommand, bool>
{
    [WorkflowStep(WorkflowStepCode.CTH.WF_STEP_CTH_VERIFY_PASSWORD)]
    public async Task<bool> HandleAsync(
        VerifyPasswordCommand request,
        CancellationToken cancellationToken = default
    )
    {
        UserPassword userInfo = await userPasswordRepository
            .Table.Where(s => s.ChannelId == request.ChannelId && s.UserId == request.UserCode)
            .FirstOrDefaultAsync();
        if (userInfo == null)
        {
            return false;
        }

        bool isPasswordValid;
        try
        {
            isPasswordValid = PasswordUtils.VerifyPassword(
                usercode: request.UserCode,
                password: request.Password,
                storedHash: userInfo.Password,
                storedSalt: userInfo.Salt
            );
        }
        catch (Exception ex)
        {
            await ex.LogErrorAsync();
            isPasswordValid = false;
        }

        return isPasswordValid;
    }
}
