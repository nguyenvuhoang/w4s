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
    public string UserName { get; set; }
    public string Password { get; set; }
    public string LoginName { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
    public string Phone { get; set; }
    public int Gender { get; set; }
    public string Birthday { get; set; }
    public string Address { get; set; }
    public string Email { get; set; }
    public string UserCreated { get; set; }
    public string UserLevel { get; set; }
    public int PolicyId { get; set; }
    public List<string> UserGroup { get; set; }
    public string ContractNumber { get; set; }
    public string UserChannelId { get; set; }
    public string RoleChannel { get; set; }
    public string NotificationType { get; set; } = "MAIL";
    public string ContractType { get; set; }
    public string UserType { get; set; } = "0502";
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
        DateTime now = DateTime.UtcNow;
        string userId = null;
        string userCode = null;
        bool isUserAccountCreated = false;
        bool isUserPasswordCreated = false;
        bool isUserInRolesCreated = false;

        try
        {
            if (request.IsReverse)
            {
                await userInRoleRepository.DeleteByUserCodeAsync(userCode);
                await userPasswordRepository.DeletePasswordByUserIdAsync(userId);
                await userAccountRepository.DeleteUserByUserIdAsync(userId);
                return new UserResponseModel { };
            }

            // 1. Check username existence
            bool isUserExisting = await userAccountRepository.IsExist(request.UserName);
            if (isUserExisting)
            {
                throw await O24Exception.CreateAsync(
                    O24CTHResourceCode.Validation.UsernameIsExisting,
                    request.Language,
                    [request.UserName]
                );
            }

            // 2. Generate ID and hash password
            userId = Guid.NewGuid().ToString();
            userCode = userId;
            string password = request.Password ?? PasswordUtils.GenerateRandomPassword(8);
            string salt = PasswordUtils.GenerateRandomSalt();
            string hashPassword = O9Encrypt.sha_sha256(password, userCode);

            string userchannel = string.IsNullOrEmpty(request.ContractType)
                ? "BO"
                : (request.ContractType == "IND" ? "MB" : "AM");

            List<int> listofRoleUser = await userRightChannelRepository.GetListRoleIdByChannelAsync(
                userchannel
            );
            string roleArrayString =
                listofRoleUser != null && listofRoleUser.Count != 0
                    ? $"[{string.Join(",", listofRoleUser)}]"
                    : "[1]";
            // 3. Create UserAccount
            UserAccount userAccount = new()
            {
                ChannelId = userchannel,
                UserId = userId,
                UserName = request.UserName,
                UserCode = userCode,
                LoginName = request.UserName,
                RoleChannel = roleArrayString,
                Status = Common.ACTIVE,
                UserCreated = request.CurrentUserCode,
                IsSuperAdmin = false,
                BranchID = !string.IsNullOrWhiteSpace(request.CurrentBranchCode)
                    ? request.CurrentBranchCode
                    : "0",
                FirstName = request.FirstName,
                MiddleName = request.MiddleName,
                LastName = request.LastName,
                Email = request.Email,
                Gender = request.Gender,
                Address = request.Address,
                Phone = request.Phone,
                Birthday = DateTime.TryParse(request.Birthday, out DateTime birthday)
                    ? birthday
                    : null,
                PolicyID = request.PolicyId,
                UserLevel = int.Parse(request.UserLevel),
                CreatedOnUtc = now,
                IsShow = Constant.Code.ShowStatus.YES,
                ContractNumber = request.ContractNumber,
                IsFirstLogin = true,
                NotificationType = !string.IsNullOrWhiteSpace(request.NotificationType)
                    ? request.NotificationType
                    : "MAIL",
            };
            await userAccountRepository.AddAsync(userAccount);
            isUserAccountCreated = true;

            // 4. Create UserPassword
            UserPassword userPassword = new()
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
            List<int> listUserRole = await userRoleRepository.GetByRoleTypeAsync(request.UserType);

            List<UserInRole> userInRoles = listUserRole
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

            string[] nameParts = new[] { request.FirstName, request.MiddleName, request.LastName };
            string fullname = string.Join(
                " ",
                nameParts.Where(part => !string.IsNullOrWhiteSpace(part))
            );

            // 6. Build response
            UserResponseModel response = new()
            {
                DataTemplate = new
                {
                    userCode,
                    request.UserName,
                    request.Email,
                    fullname,
                    request.Phone,
                    password,
                },
                MimeEntities = [],
            };

            byte[] qrImageBytes = Utils.StringExtensions.GenerateQRCodeBytes(password);
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
                request.Language,
                [ex.Message]
            );
        }
    }
}
