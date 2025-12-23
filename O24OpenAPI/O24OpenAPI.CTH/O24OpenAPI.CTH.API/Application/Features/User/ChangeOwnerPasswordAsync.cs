using LinKit.Core.Cqrs;
using O24OpenAPI.Core;
using O24OpenAPI.CTH.API.Application.Constants;
using O24OpenAPI.CTH.API.Application.Utils;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Exceptions;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.Framework.Utils.O9;

namespace O24OpenAPI.CTH.API.Application.Features.User
{
    public class ChangeOwnerPasswordAsyncCommand : BaseTransactionModel, ICommand<bool>
    {
        public string OldPassword { get; set; }
        public string Password { get; set; }
    }
    [CqrsHandler]
    public class ChangeOwnerPasswordHandle(
        IUserAccountRepository userAccountRepository,
        IUserPasswordRepository userPasswordRepository,
        IUserSessionRepository userSessionRepository
    ) : ICommandHandler<ChangeOwnerPasswordAsyncCommand, bool>
    {
        [WorkflowStep("WF_STEP_CTH_CHANGE_OWNER_PW")]
        public async Task<bool> HandleAsync(
            ChangeOwnerPasswordAsyncCommand request,
            CancellationToken cancellationToken = default
        )
        {
            var userAccount =
                await userAccountRepository.GetByUserCodeAsync(request.CurrentUserCode)
                ?? throw await O24Exception.CreateAsync(
                    O24CTHResourceCode.Operation.ChangePasswordError,
                    request.Language
                );

            string hashPassword = O9Encrypt.sha_sha256(request.Password, userAccount.UserCode);

            var userPassword = await userPasswordRepository.GetByUserCodeAsync(
                userAccount.UserCode
            );

            if (userPassword == null)
            {
                throw new O24OpenAPIException(
                    $"This user {userAccount.LoginName} have no user password"
                );
            }
            else
            {
                bool isPasswordValid;
                try
                {
                    isPasswordValid = PasswordUtils.VerifyPassword(
                        usercode: userAccount.UserCode,
                        password: request.OldPassword,
                        storedHash: userPassword.Password,
                        storedSalt: userPassword.Salt
                    );
                }
                catch (Exception ex)
                {
                    await ex.LogErrorAsync();
                    isPasswordValid = false;
                }

                if (!isPasswordValid)
                {
                    throw new O24OpenAPIException("Current password is incorrect");
                }
                userPassword.Password = hashPassword;
                userPassword.UpdatedOnUtc = DateTime.UtcNow;
                await userPasswordRepository.UpdateAsync(userPassword);
                userAccount.IsFirstLogin = false;
                userAccount.UpdatedOnUtc = DateTime.UtcNow;
                userAccount.IsLogin = false;
                await userAccountRepository.UpdateAsync(userAccount);
            }
            await userSessionRepository.RevokeByLoginName(userAccount.LoginName);

            return true;
        }
    }
}
