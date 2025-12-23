using LinKit.Core.Cqrs;
using LinqToDB;
using Newtonsoft.Json.Linq;
using O24OpenAPI.Core;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.CTH.API.Application.Constants;
using O24OpenAPI.CTH.API.Application.Utils;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.CTH.Infrastructure.Configurations;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Exceptions;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.Framework.Utils.O9;

namespace O24OpenAPI.CTH.API.Application.Features.User;

public class ChangePasswordByO24UserCommand : BaseTransactionModel, ICommand<JToken>
{
    /// <summary>
    /// Gets or sets the value of the LoginName
    /// </summary>
    public string LoginName { get; set; }

    /// <summary>
    /// Gets or sets the value of the password
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// Gets or sets the value of the oldpassword
    /// </summary>
    public string NewPassword { get; set; }
}

[CqrsHandler]
public class ChangePasswordByO24UserHandle(
    IUserPasswordRepository userPasswordRepository,
    IUserAccountRepository userAccountRepository
) : ICommandHandler<ChangePasswordByO24UserCommand, JToken>
{
    [WorkflowStep("WF_STEP_CTH_CHANGE_PASSWORD")]
    public async Task<JToken> HandleAsync(
        ChangePasswordByO24UserCommand request,
        CancellationToken cancellationToken = default
    )
    {
        var userAccount =
            await GetLoginAccount(
                request.LoginName,
                password: request.Password,
                request.ChannelId,
                request.Language
            )
            ?? throw await O24Exception.CreateAsync(
                O24CTHResourceCode.Operation.ChangePasswordError,
                request.Language
            );
        string hashPassword = O9Encrypt.sha_sha256(request.NewPassword, userAccount.UserCode);

        var userPassword = await userPasswordRepository.GetByUserCodeAsync(
            userAccount.UserCode
        );

        if (userPassword == null)
        {
            throw new O24OpenAPIException(
                $"This user {request.LoginName} have no user password"
            );
        }
        else
        {
            userPassword.Password = hashPassword;
            userPassword.UpdatedOnUtc = DateTime.UtcNow;
            await userPasswordRepository.UpdateAsync(userPassword);
            userAccount.IsFirstLogin = false;
            userAccount.UpdatedOnUtc = DateTime.UtcNow;
            userAccount.IsLogin = false;
            await userAccountRepository.UpdateAsync(userAccount);
        }

        return JToken.FromObject(new { success = true });
    }

    private async Task<UserAccount> GetLoginAccount(
        string loginName,
        string password,
        string channelId,
        string language
    )
    {
        UserAccount user =
            await userAccountRepository
                .Table.Where(s =>
                    s.ChannelId == channelId
                    && s.LoginName == loginName
                    && s.Status != Common.DELETED
                )
                .FirstOrDefaultAsync()
            ?? throw await O24Exception.CreateAsync(
                O24CTHResourceCode.Validation.UsernameIsNotExist,
                language,
                [loginName]
            );

        UserPassword userPassword =
            await userPasswordRepository
                .Table.Where(s => s.ChannelId == channelId && s.UserId == user.UserId)
                .FirstOrDefaultAsync()
            ?? throw await O24Exception.CreateAsync(
                O24CTHResourceCode.Validation.PasswordDonotSetting,
                language,
                []
            );

        ControlHubSetting? setting = EngineContext.Current.Resolve<ControlHubSetting>();

        if (user.Status == Common.BLOCK && user.LockedUntil.HasValue)
        {
            if (user.LockedUntil > DateTime.UtcNow)
            {
                throw await O24Exception.CreateAsync(
                    O24CTHResourceCode.Operation.AccountLockedTemporarily,
                    language,
                    [loginName, user.LockedUntil.Value.ToString("HH:mm:ss")]
                );
            }
            else
            {
                user.Status = Common.ACTIVE;
                user.Failnumber = 0;
                user.LockedUntil = null;
                await userAccountRepository.UpdateAsync(user);
            }
        }

        bool isPasswordValid;
        try
        {
            isPasswordValid = PasswordUtils.VerifyPassword(
                usercode: user.UserCode,
                password: password,
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
            user.Failnumber++;

            if (user.Failnumber >= setting.MaxFailedAttempts)
            {
                user.Status = Common.BLOCK;
                user.LockedUntil = DateTime.UtcNow.AddMinutes(15);
                await userAccountRepository.UpdateAsync(user);

                throw await O24Exception.CreateAsync(
                    O24CTHResourceCode.Operation.AccountLockedTemporarily,
                    language,
                    [loginName, user.LockedUntil.Value.ToString("HH:mm:ss")]
                );
            }

            await userAccountRepository.UpdateAsync(user);

            throw await O24Exception.CreateAsync(
                O24CTHResourceCode.Validation.PasswordIncorrect,
                language,
                [user.Failnumber]
            );
        }

        user.Failnumber = 0;
        user.LockedUntil = null;
        await userAccountRepository.UpdateAsync(user);

        if (user.Status != Common.ACTIVE)
        {
            throw await O24Exception.CreateAsync(
                O24CTHResourceCode.Validation.AccountStatusInvalid,
                language,
                [user.LoginName]
            );
        }

        return user;
    }
}
