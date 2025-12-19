using O24OpenAPI.APIContracts.Models.CTH;
using O24OpenAPI.ControlHub.Config;
using O24OpenAPI.ControlHub.Constant;
using O24OpenAPI.ControlHub.Domain;
using O24OpenAPI.ControlHub.Models;
using O24OpenAPI.ControlHub.Models.Roles;
using O24OpenAPI.ControlHub.Models.User;
using O24OpenAPI.ControlHub.Services.Interfaces;
using O24OpenAPI.ControlHub.Utils;
using O24OpenAPI.Core;
using O24OpenAPI.Core.Constants;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Data.System.Linq;
using O24OpenAPI.Framework.Exceptions;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Infrastructure.Mapper.Extensions;
using O24OpenAPI.Framework.Utils;
using O24OpenAPI.Framework.Utils.O9;
using static O24OpenAPI.ControlHub.Constant.Code;

namespace O24OpenAPI.ControlHub.Services;

/// <summary>
///
/// </summary>
/// <param name="repository"></param>
/// <param name="passwordRepository"></param>
/// <param name="userInRoleRepository"></param>
/// <param name="userInRoleService"></param>
/// <param name="userRoleService"></param>
/// <param name="userDeviceService"></param>
/// <param name="authSessionService"></param>
public class UserAccountService(
    IRepository<UserAccount> repository,
    IRepository<UserPassword> passwordRepository,
    IRepository<UserInRole> userInRoleRepository,
    IUserInRoleService userInRoleService,
    IUserRoleService userRoleService,
    IUserDeviceService userDeviceService,
    IAuthSessionService authSessionService,
    IUserCommandService userCommandService,
    IRepository<UserRight> userRightRepository,
    IUserPasswordService userPasswordService,
    IUserAvatarService userAvatarService
) : IUserAccountService
{
    /// <summary>
    /// The repository
    /// </summary>
    private readonly IRepository<UserAccount> _repository = repository;

    /// <summary>
    /// The password repository
    /// </summary>
    private readonly IRepository<UserPassword> _passwordRepository = passwordRepository;

    /// <summary>
    /// Role of User
    /// </summary>
    private readonly IRepository<UserInRole> _userInRole = userInRoleRepository;

    /// <summary>
    /// User Right Repository
    /// </summary>
    private readonly IRepository<UserRight> _userRightRepository = userRightRepository;

    /// <summary>
    /// User in Role Service
    /// </summary>
    private readonly IUserInRoleService _userInRoleService = userInRoleService;

    /// <summary>
    /// User Role Service
    /// </summary>
    private readonly IUserRoleService _userRoleService = userRoleService;

    /// <summary>
    /// User Device Service
    /// </summary>
    private readonly IUserDeviceService _userDeviceService = userDeviceService;

    /// <summary>
    /// Authen Session Service
    /// </summary>
    private readonly IAuthSessionService _authSessionService = authSessionService;

    /// <summary>
    /// User Command Service
    /// </summary>
    private readonly IUserCommandService _userCommandService = userCommandService;

    /// <summary>
    /// User in Role Service
    /// </summary>
    private readonly IUserPasswordService _userPasswordService = userPasswordService;

    /// <summary>
    /// User Avatar Service
    /// </summary>
    private readonly IUserAvatarService _userAvatarService = userAvatarService;

    public async Task<UserAccount> GetByUserIdAsync(string userId)
    {
        return await _repository.Table.Where(s => s.UserId == userId).FirstOrDefaultAsync();
    }

    /// <summary>
    /// Get User by code
    /// </summary>
    /// <param name="userCode"></param>
    /// <returns></returns>
    public async Task<UserAccount> GetByUserCodeAsync(string userCode)
    {
        return await _repository.Table.Where(s => s.UserCode == userCode).FirstOrDefaultAsync();
    }

    /// <summary>
    /// Get User by Phone Number
    /// </summary>
    /// <param name="phone"></param>
    /// <returns></returns>
    public async Task<UserAccount> GetByPhoneNumberAsync(string phone)
    {
        return await _repository.Table.Where(s => s.Phone == phone).FirstOrDefaultAsync();
    }

    /// <summary>
    /// Get User by Login name
    /// </summary>
    /// <param name="loginName"></param>
    /// <returns></returns>
    public async Task<UserAccount> GetByLoginNameAsync(string loginName)
    {
        return await _repository.Table.Where(s => s.LoginName == loginName).FirstOrDefaultAsync();
    }

    /// <summary>
    /// Get User by Login name and Channel
    /// </summary>
    /// <param name="loginName"></param>
    /// <returns></returns>
    public async Task<UserAccount> GetByLoginNameandChannelAsync(string loginName, string channelid)
    {
        return await _repository
            .Table.Where(s => s.LoginName == loginName && s.ChannelId == channelid)
            .FirstOrDefaultAsync();
    }

    /// <summary>
    /// Get User by Login name and Email
    /// </summary>
    /// <param name="loginName"></param>
    /// <param name="email"></param>
    /// <param name="phonenumber"></param>
    /// <returns></returns>
    public async Task<UserAccount> GetByLoginNameAndEmailAsync(
        string loginName,
        string email,
        string phonenumber
    )
    {
        return await _repository
            .Table.Where(s =>
                s.LoginName == loginName && s.Email == email && s.Phone == phonenumber
            )
            .FirstOrDefaultAsync();
    }

    /// <summary>
    /// Update User Account
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public async Task UpdateAsync(UserAccount entity)
    {
        await _repository.Update(entity);
    }

    /// <summary>
    /// Gets the or add user account using the specified user id
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <param name="loginName">The login name</param>
    /// <param name="password">The password</param>
    /// <param name="channelId">The channel id</param>
    /// <exception cref="O24OpenAPIException">Invalid password.</exception>
    /// <exception cref="O24OpenAPIException">User password not found</exception>
    /// <returns>The user account</returns>
    public async Task<UserAccount> GetLoginAccount(
        string loginName,
        string password,
        string channelId,
        string language
    )
    {
        var user =
            await _repository
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

        var userPassword =
            await _passwordRepository
                .Table.Where(s => s.ChannelId == channelId && s.UserId == user.UserId)
                .FirstOrDefaultAsync()
            ?? throw await O24Exception.CreateAsync(
                O24CTHResourceCode.Validation.PasswordDonotSetting,
                language,
                []
            );

        var setting = EngineContext.Current.Resolve<ControlHubSetting>();

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

    public async Task<UserAccount> AddAsync(UserAccount user)
    {
        return await _repository.InsertAsync(user);
    }

    /// <summary>
    /// List Role Of User
    /// </summary>
    /// <param name="usercode"></param>
    /// <returns></returns>
    public async Task<List<UserInRole>> ListOfRole(string usercode)
    {
        var listOfRole = await _userInRole.Table.Where(s => s.UserCode == usercode).ToListAsync();
        return listOfRole;
    }

    /// <summary>
    /// Get UserCode By Contract Number
    /// </summary>
    /// <param name="contractnumber"></param>
    /// <returns></returns>
    public async Task<string> GetUserCodeByContractNumber(string contractnumber)
    {
        string userCode = string.Empty;
        var userAccount = await _repository
            .Table.Where(s => s.ContractNumber == contractnumber)
            .FirstOrDefaultAsync();
        if (userAccount != null)
        {
            userCode = userAccount.UserCode;
        }
        return userCode;
    }

    public async Task<bool> IsExist(string userName)
    {
        var useraccount = await _repository
            .Table.Where(s => s.UserName == userName)
            .FirstOrDefaultAsync();
        return useraccount != null;
    }

    public async Task<IPagedList<UserAccount>> GetUserByRoleIdASync(ModelWithRoleId model)
    {
        var userList = await _userInRoleService.GetUserInRolesByRoleIdAsync(model.RoleId);
        if (userList == null || userList.Count == 0)
        {
            throw await O24Exception.CreateAsync(
                O24CTHResourceCode.Validation.UserNotFoundByRoleId,
                model.Language,
                [model.RoleId.ToString()]
            );
        }

        var userCodes = userList.Select(u => u.UserCode).ToList();

        var users = await _repository
            .Table.Where(s => userCodes.Contains(s.UserCode))
            .ToListAsync();

        var pageList = await users.AsQueryable().ToPagedList(model.PageIndex, model.PageSize);
        return pageList;
    }

    public async Task<bool> UpdateUserRoleASync(UpdateUserRoleModel model)
    {
        if (model.IsAssign)
        {
            var toInsert = new List<UserInRole>();

            foreach (var userCode in model.ListOfUser)
            {
                var exists = await _userInRole
                    .Table.Where(u => u.UserCode == userCode && u.RoleId == model.RoleId)
                    .FirstOrDefaultAsync();

                if (exists == null)
                {
                    toInsert.Add(
                        new UserInRole
                        {
                            UserCode = userCode,
                            RoleId = model.RoleId,
                            IsMain = ShowStatus.YES,
                            CreatedOnUtc = DateTime.UtcNow,
                            UpdatedOnUtc = DateTime.UtcNow,
                        }
                    );
                }
            }

            if (toInsert.Count != 0)
            {
                await _userInRoleService.BulkInsert(toInsert);
            }
        }
        else
        {
            // Bulk delete users from role
            var toDelete = await _userInRole
                .Table.Where(u => model.ListOfUser.Contains(u.UserCode) && u.RoleId == model.RoleId)
                .ToListAsync();

            if (toDelete.Count != 0)
            {
                await _userInRoleService.DeleteBulkAsync(toDelete);
            }
        }

        return true;
    }

    /// <summary>
    /// Create user role asynchronously.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<bool> CreateUserRoleAsync(CreateUserRoleModel model)
    {
        var roleId = await _userRoleService.GetNextRoleIdAsync();

        var newUserRole = new UserRole
        {
            RoleId = roleId,
            RoleName = model.RoleName,
            RoleDescription = model.RoleDescription,
            UserType = Code.UserType.BO,
            UserCreated = model.CurrentUserCode,
            DateCreated = DateTime.UtcNow,
            ServiceID = Code.UserType.BO,
            RoleType = model.RoleType,
            Status = Code.ShowStatus.ACTIVE,
            IsShow = Code.ShowStatus.YES,
            SortOrder = roleId,
        };

        var inserted = await _userRoleService.AddAsync(newUserRole);
        if (inserted == null)
        {
            return false;
        }

        var parentCommandIds = await _userCommandService.GetListCommandParentAsync(model.ChannelId);
        if (parentCommandIds == null || parentCommandIds.Count == 0)
        {
            return true;
        }

        var existedCmdIds = await _userRightRepository
            .Table.Where(r => r.RoleId == roleId && parentCommandIds.Contains(r.CommandId))
            .Select(r => r.CommandId)
            .Distinct()
            .ToListAsync();

        var now = DateTime.UtcNow;
        var toInsert = parentCommandIds
            .Except(existedCmdIds)
            .Distinct()
            .Select(pid => new UserRight
            {
                RoleId = roleId,
                CommandId = pid,
                CommandIdDetail = Common.ACTIVE,
                Invoke = 1,
                Approve = 1,
                CreatedOnUtc = now,
                UpdatedOnUtc = now,
            })
            .ToList();

        if (toInsert.Count > 0)
        {
            await _userRightRepository.BulkInsert(toInsert);
        }

        return true;
    }

    public async Task<UpdateUserResponseModel> UpdateUserAsync(UpdateUserRequestModel model)
    {
        var entity =
            await _repository.GetById(model.Id)
            ?? throw await O24Exception.CreateAsync(ResourceCode.Common.NotExists, model.Language);

        var originalEntity = entity.Clone();

        model.ToEntityNullable(entity);

        entity.UpdatedOnUtc = DateTime.UtcNow;

        await _repository.Update(entity);

        return UpdateUserResponseModel.FromUpdatedEntity(entity, originalEntity);
    }

    public async Task<bool> IsPhoneNumberExistingAsync(UserAccountPhoneModel model)
    {
        var existingPhonenumber = await _repository
            .Table.Where(c => c.Status != "D")
            .FirstOrDefaultAsync(c => c.Phone == model.PhoneNumber.Trim());

        if (existingPhonenumber != null)
        {
            throw await O24Exception.CreateAsync(
                O24CTHResourceCode.Validation.PhoneNumberIsExisting,
                model.Language,
                [model.PhoneNumber]
            );
        }
        return true;
    }

    public async Task<bool> IsUserNameExistingAsync(DefaultModel model)
    {
        var existingUserName = await _repository.Table.FirstOrDefaultAsync(c =>
            c.UserName == model.UserName.Trim() && c.UserCode == model.UserCode.Trim()
        );

        if (existingUserName != null)
        {
            throw await O24Exception.CreateAsync(
                O24CTHResourceCode.Validation.UsernameIsNotExist,
                model.Language,
                [model.UserName]
            );
        }
        return true;
    }

    public async Task<AuthResponseModel> ChangeDeviceAsync(UserAccountChangeDeviceModel model)
    {
        var userAccount =
            await GetByUserCodeAsync(model.UserCode)
            ?? throw await O24Exception.CreateAsync(
                O24CTHResourceCode.Validation.UsernameIsExisting,
                model.Language
            );

        if (userAccount.Phone != model.Phone)
        {
            throw await O24Exception.CreateAsync(
                O24CTHResourceCode.Validation.PhoneNumberIsNotExisting,
                model.Language,
                [model.Phone]
            );
        }

        if (string.IsNullOrEmpty(userAccount.ContractNumber))
        {
            throw await O24Exception.CreateAsync(
                O24CTHResourceCode.Validation.ContractNumberIsNotExisting,
                model.Language,
                [userAccount.Phone]
            );
        }

        try
        {
            await _userDeviceService.EnsureUserDeviceAsync(
                userCode: userAccount.UserCode,
                loginName: userAccount.LoginName,
                deviceId: model.DeviceId + model.Modelname ?? "",
                deviceType: model.DeviceType,
                userAgent: model.UserAgent,
                ipAddress: model.IpAddress,
                channelId: model.ChannelId,
                pushId: model.PushId,
                osVersion: model.OsVersion,
                appVersion: model.AppVersion,
                deviceName: model.DeviceName,
                brand: model.Brand,
                isEmulator: model.IsEmulator,
                isRooted: model.IsRootedOrJailbroken,
                language: model.Language,
                isResetDevice: true
            );
        }
        catch (Exception ex)
        {
            await ex.LogErrorAsync();
            throw await O24Exception.CreateAsync(
                O24CTHResourceCode.Operation.ChangeDeviceError,
                model.Language
            );
        }

        var context = new LoginContextModel
        {
            DeviceId = model.DeviceId,
            Modelname = model.Modelname,
            RoleChannel = userAccount.RoleChannel,
            IpAddress = model.IpAddress,
            ChannelId = model.ChannelId,
            Reference = model.DeviceId + model.Modelname ?? "",
        };

        userAccount.LastLoginTime = DateTime.Now;
        userAccount.UUID = Guid.NewGuid().ToString();
        userAccount.Failnumber = 0;
        userAccount.IsLogin = true;

        await UpdateAsync(userAccount);

        return await _authSessionService.CreateTokenAndSessionAsync(userAccount, context);
    }

    public async Task<bool> VerifyPasswordAsync(VerifyPasswordModel model)
    {
        var userInfo = await _passwordRepository
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

    /// <summary>
    /// Delete User Password by UserId
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task DeleteUserByUserIdAsync(string userId)
    {
        var entity = await _repository.Table.FirstOrDefaultAsync(x => x.UserId == userId);

        if (entity != null)
        {
            await _repository.Delete(entity);
        }
    }

    /// <summary>
    /// Delete User by Contract Number
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<bool> DeleteUserByContractAsync(DeleteUserContractModel model)
    {
        if (model == null || string.IsNullOrWhiteSpace(model.ContractNumber))
        {
            return false;
        }

        var entity = await _repository.Table.FirstOrDefaultAsync(x =>
            x.ContractNumber == model.ContractNumber
        );

        if (entity == null)
        {
            return false;
        }

        var previousStatus = entity.Status;

        try
        {
            entity.Status = Common.PENDINGFORDELETE;
            await _repository.Update(entity);
            return true;
        }
        catch (Exception ex)
        {
            if (model.IsReverse)
            {
                entity.Status = previousStatus;
                await _repository.Update(entity);
            }

            await ex.LogErrorAsync();

            return false;
        }
    }

    /// <summary>
    /// Transition User Status Asynchronously
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<bool> TransitionUserStatusAsync(TransitionUserStatusModel model)
    {
        if (model == null || string.IsNullOrWhiteSpace(model.ContractNumber))
        {
            return false;
        }

        var entity = await _repository.Table.FirstOrDefaultAsync(x =>
            x.ContractNumber == model.ContractNumber
        );

        if (entity == null)
        {
            return false;
        }

        var previousStatus = entity.Status;

        try
        {
            entity.Status = model.Status;
            await _repository.Update(entity);
            return true;
        }
        catch (Exception ex)
        {
            if (model.IsReverse)
            {
                entity.Status = previousStatus;
                await _repository.Update(entity);
            }

            await ex.LogErrorAsync();

            return false;
        }
    }

    /// <summary>
    /// Delete User By Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<bool> DeleteUserById(DeleteUserModel model)
    {
        var entity = await _repository.Table.FirstOrDefaultAsync(x => x.Id == model.Id);

        if (entity != null)
        {
            if (entity.LoginName == model.CurrentLoginname)
            {
                throw await O24Exception.CreateAsync(
                    O24CTHResourceCode.Operation.DeleteOwnUserError,
                    model.Language
                );
            }
            await _userInRoleService.DeleteByUserCodeAsync(entity.UserId);
            await _userPasswordService.DeletePasswordByUserIdAsync(entity.UserId);
            await DeleteUserByUserIdAsync(entity.UserId);
            return true;
        }
        return false;
    }

    public async Task<bool> SyncUserInfoAsync(SyncUserInfoModel model)
    {
        if (model == null)
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(model.ContractNumber))
        {
            return false;
        }

        var user = await GetUserByContractNumber(model.ContractNumber.Trim());
        if (user == null)
        {
            return false;
        }

        var newPhone = NormalizePhone(model.PhoneNumber);
        if (string.IsNullOrWhiteSpace(newPhone))
        {
            return false;
        }

        if (string.Equals(user.Phone, newPhone, StringComparison.Ordinal))
        {
            return true;
        }

        user.Phone = newPhone;
        user.UpdatedOnUtc = DateTime.UtcNow;
        user.UserModified = model.CurrentUserCode ?? "SYSTEM";

        await _repository.Update(user);
        return true;
    }

    private static string NormalizePhone(string? input)
    {
        var v = (input ?? string.Empty).Trim();
        if (v.Length == 0)
        {
            return string.Empty;
        }

        v = v.Replace(" ", "").Replace("-", "").Replace(".", "").Replace("(", "").Replace(")", "");

        return v;
    }

    public Task<UserAccount> GetUserByContractNumber(string contractnumber)
    {
        if (string.IsNullOrWhiteSpace(contractnumber))
        {
            throw new ArgumentNullException(nameof(contractnumber));
        }

        return _repository.Table.FirstOrDefaultAsync(s => s.ContractNumber == contractnumber);
    }

    public async Task<bool> UnBlockUserAsync(UnblockUserModel model)
    {
        var entity = await _repository.Table.FirstOrDefaultAsync(x => x.UserName == model.UserName);

        if (entity != null)
        {
            entity.Status = Common.ACTIVE;
            entity.Failnumber = 0;
            entity.LockedUntil = null;
            entity.UpdatedOnUtc = DateTime.UtcNow;
            await _repository.Update(entity);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Get User Push ID by Contract Number
    /// </summary>
    /// <param name="contractNumber"></param>
    /// <returns></returns>
    public async Task<CTHUserPushModel> GetUserPushIdByContractNumberAsync(string contractNumber)
    {
        var userInfo = await GetUserCodeByContractNumber(contractNumber);
        if (string.IsNullOrWhiteSpace(userInfo))
        {
            return null;
        }

        if (await _userDeviceService.GetByUserCodeAsync(userInfo) is { } userDevice)
        {
            return new CTHUserPushModel
            {
                UserCode = userInfo,
                PushID = userDevice.PushId ?? string.Empty,
                UserDeviceID = userDevice.DeviceId ?? string.Empty,
            };
        }

        return null;
    }

    /// <summary>
    /// Update User Avatar Asynchronously
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<bool> UpdateUserAvatarAsync(UpdateUserAvatarRequestModel model)
    {
        try
        {
            var entity = await _userAvatarService.GetByUserCodeAsync(model.UserCode);

            if (entity == null)
            {
                entity = new UserAvatar
                {
                    UserCode = model.UserCode,
                    ImageUrl = model.AvatarUrl,
                    DateInsert = DateTime.UtcNow,
                };

                await _userAvatarService.AddAsync(entity);
            }
            else
            {
                entity.ImageUrl = model.AvatarUrl;
                await _userAvatarService.UpdateAsync(entity);
            }

            return true;
        }
        catch (O24Exception)
        {
            throw;
        }
        catch (Exception ex)
        {
            await ex.LogErrorAsync(
                $"[UpdateUserAvatarAsync] Error for {model.UserCode}: {ex.Message}"
            );
            throw await O24Exception.CreateAsync(
                ResourceCode.Common.SystemError,
                model.Language,
                ex
            );
        }
    }

    /// <summary>
    /// Scan Password
    /// </summary>
    /// <returns></returns>
    /// <summary>
    /// Scan Password:
    /// - Duyệt toàn bộ UserAccount theo trang
    /// - Hash = O9Encrypt.sha_sha256(password, UserCode)
    /// - Upsert vào UserPassword theo (UserId, ChannelId)
    /// </summary>
    public async Task<ScanResult> ScanPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            password = "123456";
        }

        var result = new ScanResult();
        var now = DateTime.UtcNow;

        const int deleteBatchSize = 10_000;
        while (true)
        {
            var batchToDelete = await _passwordRepository
                .Table.OrderBy(x => x.Id)
                .Take(deleteBatchSize)
                .ToListAsync();

            if (batchToDelete.Count == 0)
            {
                break;
            }

            await _passwordRepository.BulkDelete(batchToDelete);
        }

        const int pageSize = 2000;
        var pageIndex = 0;

        while (true)
        {
            var users = await _repository
                .Table.Where(u =>
                    !string.IsNullOrEmpty(u.UserId) && !string.IsNullOrEmpty(u.UserCode)
                )
                .OrderBy(u => u.Id)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();

            if (users.Count == 0)
            {
                break;
            }

            var batch = users
                .Select(u => new UserPassword
                {
                    UserId = u.UserId,
                    ChannelId = u.ChannelId,
                    Password = O9Encrypt.sha_sha256(password, u.UserCode),
                    Salt = u.UserCode,
                    CreatedOnUtc = now,
                    UpdatedOnUtc = now,
                })
                .ToList();

            try
            {
                if (batch.Count > 0)
                {
                    await _passwordRepository.BulkInsert(batch);
                }

                result.TotalMigrated += batch.Count;
            }
            catch (Exception ex)
            {
                result.TotalFailed += batch.Count;
                result.Errors.Add($"Page {pageIndex + 1}: {ex.Message}");
            }

            pageIndex++;
        }

        return result;
    }

    /// <summary>
    ///  Scan Role:
    /// </summary>
    /// <param name="role"></param>
    /// <returns></returns>
    public async Task<ScanResult> ScanRole(int role)
    {
        if (role == 0)
        {
            role = 1;
        }

        var result = new ScanResult();
        var now = DateTime.UtcNow;

        const int deleteBatchSize = 10_000;
        while (true)
        {
            var batchToDelete = await _userInRole
                .Table.OrderBy(x => x.Id)
                .Take(deleteBatchSize)
                .ToListAsync();

            if (batchToDelete.Count == 0)
            {
                break;
            }

            await _userInRole.BulkDelete(batchToDelete);
        }

        const int pageSize = 2000;
        var pageIndex = 0;

        while (true)
        {
            var users = await _repository
                .Table.Where(u =>
                    !string.IsNullOrEmpty(u.UserId) && !string.IsNullOrEmpty(u.UserCode)
                )
                .OrderBy(u => u.Id)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();

            if (users.Count == 0)
            {
                break;
            }

            var batch = users
                .Select(u => new UserInRole
                {
                    RoleId = role,
                    UserCode = u.UserCode,
                    IsMain = ShowStatus.YES,
                    CreatedOnUtc = now,
                    UpdatedOnUtc = now,
                })
                .ToList();

            try
            {
                if (batch.Count > 0)
                {
                    await _userInRole.BulkInsert(batch);
                }

                result.TotalMigrated += batch.Count;
            }
            catch (Exception ex)
            {
                result.TotalFailed += batch.Count;
                result.Errors.Add($"Page {pageIndex + 1}: {ex.Message}");
            }

            pageIndex++;
        }

        return result;
    }

    /// <summary>
    /// Get User by Phone Number Asynchronously
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<UserInfoModel> GetUserByPhoneNumberASync(UserWithPhoneNumber model)
    {
        try
        {
            ConsoleUtil.WriteInfo(
                $"[GetUserByPhoneNumberASync] Start processing for phone number: {model.PhoneNumber}"
            );
            var user =
                await _repository
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

            var userDevice = await _userDeviceService.GetByUserCodeAsync(user.UserCode);
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

    /// <summary>
    /// Get User by code
    /// </summary>
    /// <param name="userCode"></param>
    /// <returns></returns>
    public async Task<string> CheckUserHasEmail(DefaultModel model)
    {
        var user = await _repository
            .Table.Where(s => s.UserCode == model.UserCode)
            .FirstOrDefaultAsync();
        return string.IsNullOrWhiteSpace(user?.Email)
            ? throw new O24OpenAPIException("This user does not have an email!")
            : user.Email;
    }
}
