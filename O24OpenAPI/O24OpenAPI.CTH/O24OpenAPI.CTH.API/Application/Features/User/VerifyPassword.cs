using LinKit.Core.Cqrs;
using LinqToDB;
using O24OpenAPI.Client.Scheme.Workflow;
using O24OpenAPI.CTH.API.Application.Models;
using O24OpenAPI.CTH.API.Application.Utils;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.CTH.API.Application.Features.User
{
    public class VerifyPasswordCommand : BaseTransactionModel, ICommand<bool>
    {
        public VerifyPasswordModel Model { get; set; } = default!;
    }

    [CqrsHandler]
    public class VerifyPasswordHandle(IUserPasswordRepository userPasswordRepository)
        : ICommandHandler<VerifyPasswordCommand, bool>
    {
        [WorkflowStep("WF_STEP_CTH_VERIFY_PASSWORD")]
        public async Task<bool> HandleAsync(
            VerifyPasswordCommand request,
            CancellationToken cancellationToken = default
        )
        {
            return await VerifyPasswordAsync(request.Model);
        }

        public async Task<bool> VerifyPasswordAsync(VerifyPasswordModel model)
        {
            var userInfo = await userPasswordRepository
                .Table.Where(s => s.ChannelId == model.ChannelId && s.UserId == model.UserCode)
                .FirstOrDefaultAsync();
            if (userInfo == null)
            {
                return false;
            }

            bool isPasswordValid;
            try
            {
                isPasswordValid = PasswordUtils.VerifyPassword(
                    usercode: model.UserCode,
                    password: model.Password,
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
}
