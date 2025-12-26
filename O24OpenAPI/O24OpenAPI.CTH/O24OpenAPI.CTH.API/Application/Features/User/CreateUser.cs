using LinKit.Core.Cqrs;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.APIContracts.Models.DTS;
using O24OpenAPI.Core;
using O24OpenAPI.Core.Constants;
using O24OpenAPI.CTH.API.Application.Constants;
using O24OpenAPI.CTH.API.Application.Models;
using O24OpenAPI.CTH.API.Application.Utils;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Exceptions;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.Framework.Utils.O9;

namespace O24OpenAPI.CTH.API.Application.Features.User;

public class CreateUserCommand : BaseTransactionModel, ICommand<UserResponseModel>
{
    public CreateUserRequestModel Model { get; set; } = default!;
}

[CqrsHandler]
public class CreateUserHandle(
    IUserInRoleRepository userInRoleRepository,
    IUserPasswordRepository userPasswordRepository,
    IUserAccountRepository userAccountRepository,
    IUserRightChannelRepository userRightChannelRepository,
    IUserRoleRepository userRoleRepository
) : ICommandHandler<CreateUserCommand, UserResponseModel>
{
    [WorkflowStep(WorkflowStep.CTH.WF_STEP_CTH_CREATE_USER)]
    public async Task<UserResponseModel> HandleAsync(
        CreateUserCommand request,
        CancellationToken cancellationToken = default
    )
    {
        return await CreateUserAsync(request.Model);
    }

    public async Task<UserResponseModel> CreateUserAsync(CreateUserRequestModel model)
    {
        var now = DateTime.UtcNow;
        string userId = null;
        string userCode = null;
        bool isUserAccountCreated = false;
        bool isUserPasswordCreated = false;
        bool isUserInRolesCreated = false;

        try
        {
            if (model.IsReverse)
            {
                await userInRoleRepository.DeleteByUserCodeAsync(userCode);
                await userPasswordRepository.DeletePasswordByUserIdAsync(userId);
                await userAccountRepository.DeleteUserByUserIdAsync(userId);
                return new UserResponseModel { };
            }

            // 1. Check username existence
            var isUserExisting = await userAccountRepository.IsExist(model.UserName);
            if (isUserExisting)
            {
                throw await O24Exception.CreateAsync(
                    O24CTHResourceCode.Validation.UsernameIsExisting,
                    model.Language,
                    [model.UserName]
                );
            }

            // 2. Generate ID and hash password
            userId = Guid.NewGuid().ToString();
            userCode = userId;
            string password = model.Password ?? PasswordUtils.GenerateRandomPassword(8);
            string salt = PasswordUtils.GenerateRandomSalt();
            string hashPassword = O9Encrypt.sha_sha256(password, userCode);

            var userchannel = string.IsNullOrEmpty(model.ContractType)
                ? "BO"
                : (model.ContractType == "IND" ? "MB" : "AM");

            var listofRoleUser = await userRightChannelRepository.GetListRoleIdByChannelAsync(
                userchannel
            );
            var roleArrayString =
                listofRoleUser != null && listofRoleUser.Count != 0
                    ? $"[{string.Join(",", listofRoleUser)}]"
                    : "[1]";
            // 3. Create UserAccount
            var userAccount = new UserAccount
            {
                ChannelId = userchannel,
                UserId = userId,
                UserName = model.UserName,
                UserCode = userCode,
                LoginName = model.UserName,
                RoleChannel = roleArrayString,
                Status = Common.ACTIVE,
                UserCreated = model.CurrentUserCode,
                IsSuperAdmin = false,
                BranchID = !string.IsNullOrWhiteSpace(model.CurrentBranchCode)
                    ? model.CurrentBranchCode
                    : "0",
                FirstName = model.FirstName,
                MiddleName = model.MiddleName,
                LastName = model.LastName,
                Email = model.Email,
                Gender = model.Gender,
                Address = model.Address,
                Phone = model.Phone,
                Birthday = DateTime.TryParse(model.Birthday, out var birthday)
                    ? birthday
                    : null,
                PolicyID = model.PolicyId,
                UserLevel = int.Parse(model.UserLevel),
                CreatedOnUtc = now,
                IsShow = Constant.Code.ShowStatus.YES,
                ContractNumber = model.ContractNumber,
                IsFirstLogin = true,
                NotificationType = !string.IsNullOrWhiteSpace(model.NotificationType)
                    ? model.NotificationType
                    : "MAIL",
            };
            await userAccountRepository.AddAsync(userAccount);
            isUserAccountCreated = true;

            // 4. Create UserPassword
            var userPassword = new UserPassword
            {
                ChannelId = userchannel,
                UserId = userId,
                Password = hashPassword,
                Salt = salt,
                CreatedOnUtc = now,
            };
            await userPasswordRepository.AddAsync(userPassword);
            isUserPasswordCreated = true;

            // 5. Create UserInRole
            var listUserRole = await userRoleRepository.GetByRoleTypeAsync(model.UserType);

            var userInRoles = listUserRole
                .Select(
                    (roleId, index) =>
                        new UserInRole
                        {
                            UserCode = userCode,
                            IsMain = index == 0 ? "Y" : "N",
                            CreatedOnUtc = now,
                            RoleId = roleId,
                        }
                )
                .ToList();

            if (userInRoles.Count > 0)
            {
                await userInRoleRepository.BulkInsert(userInRoles);
                isUserInRolesCreated = true;
            }

            var nameParts = new[] { model.FirstName, model.MiddleName, model.LastName };
            var fullname = string.Join(
                " ",
                nameParts.Where(part => !string.IsNullOrWhiteSpace(part))
            );

            // 6. Build response
            var response = new UserResponseModel
            {
                DataTemplate = new
                {
                    userCode,
                    model.UserName,
                    model.Email,
                    fullname,
                    model.Phone,
                    password,
                },
                MimeEntities = [],
            };

            var qrImageBytes = Utils.StringExtensions.GenerateQRCodeBytes(password);
            response.MimeEntities.Add(
                new DTSMimeEntityModel
                {
                    ContentType = "image/png",
                    ContentId = "qr",
                    Base64 = Convert.ToBase64String(qrImageBytes),
                }
            );

            return response;
        }
        catch (O24OpenAPIException)
        {
            throw;
        }
        catch (Exception ex)
        {
            await ex.LogErrorAsync();

            try
            {
                if (isUserInRolesCreated)
                {
                    await userInRoleRepository.DeleteByUserCodeAsync(userCode);
                }

                if (isUserPasswordCreated)
                {
                    await userPasswordRepository.DeletePasswordByUserIdAsync(userId);
                }

                if (isUserAccountCreated)
                {
                    await userAccountRepository.DeleteUserByUserIdAsync(userId);
                }
            }
            catch (Exception reverseEx)
            {
                await reverseEx.LogErrorAsync();
            }

            throw await O24Exception.CreateAsync(
                ResourceCode.Common.ServerError,
                model.Language,
                [ex.Message]
            );
        }
    }
}
