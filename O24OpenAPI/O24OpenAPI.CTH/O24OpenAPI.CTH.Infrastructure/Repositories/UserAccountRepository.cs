using LinKit.Core.Abstractions;
using LinqToDB;
using O24OpenAPI.Core;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Core.Events;
using O24OpenAPI.CTH.API.Application.Utils;
using O24OpenAPI.CTH.Constant;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Data;
using O24OpenAPI.Framework.Exceptions;
using O24OpenAPI.Framework.Extensions;

namespace O24OpenAPI.CTH.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class UserAccountRepository(
    IEventPublisher eventPublisher,
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
)
    : EntityRepository<UserAccount>(eventPublisher, dataProvider, staticCacheManager),
        IUserAccountRepository
{
    public Task<IPagedList<UserAccount>> GetAllPaged(
        Func<IQueryable<UserAccount>, Task<IQueryable<UserAccount>>> func = null,
        int pageIndex = 0,
        int pagSize = int.MaxValue,
        bool getOnlyTotalCount = false
    )
    {
        throw new NotImplementedException();
    }

    public async Task<UserAccount> GetByLoginNameAndEmailAsync(
        string loginName,
        string email,
        string phonenumber
    )
    {
        return await Table
            .Where(s => s.LoginName == loginName && s.Email == email && s.Phone == phonenumber)
            .FirstOrDefaultAsync();
    }

    public async Task<UserAccount> GetLoginAccount(
        string loginName,
        string password,
        string channelId,
        string language
    )
    {
        UserAccount user =
            await Table
                .Where(s =>
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

        var MaxFailedAttempts = 5;

        UserAccount userPassword =
            await Table
                .Where(s => s.ChannelId == channelId && s.UserId == user.UserId)
                .FirstOrDefaultAsync()
            ?? throw await O24Exception.CreateAsync(
                O24CTHResourceCode.Validation.PasswordDonotSetting,
                language,
                []
            );

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
                await UpdateAsync(user);
            }
        }

        bool isPasswordValid;
        try
        {
            isPasswordValid = PasswordUtils.VerifyPassword(
                usercode: user.UserCode,
                password: password,
                storedHash: user.UserCode,
                storedSalt: password
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

            if (user.Failnumber >= MaxFailedAttempts)
            {
                user.Status = Common.BLOCK;
                user.LockedUntil = DateTime.UtcNow.AddMinutes(15);
                await UpdateAsync(user);

                throw await O24Exception.CreateAsync(
                    O24CTHResourceCode.Operation.AccountLockedTemporarily,
                    language,
                    [loginName, user.LockedUntil.Value.ToString("HH:mm:ss")]
                );
            }

            await UpdateAsync(user);

            throw await O24Exception.CreateAsync(
                O24CTHResourceCode.Validation.PasswordIncorrect,
                language,
                [user.Failnumber]
            );
        }

        user.Failnumber = 0;
        user.LockedUntil = null;
        await UpdateAsync(user);

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

    public async Task<int> GetNextRoleIdAsync()
    {
        var maxId = await Table.Select(x => (int?)x.UserLevel).MaxAsync();

        return (maxId ?? 0) + 1;
    }

    public async Task UpdateAsync(UserAccount userAccount)
    {
        await Update(userAccount);
    }

    public async Task<UserAccount> GetByUserCodeAsync(string userCode)
    {
        return await Table.Where(s => s.UserCode == userCode).FirstOrDefaultAsync();
    }
}
