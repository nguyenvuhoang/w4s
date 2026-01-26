using LinKit.Core.Cqrs;
using LinKit.Core.Endpoints;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.Core;
using O24OpenAPI.CTH.API.Application.Utils;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.Framework.Utils.O9;

namespace O24OpenAPI.CTH.API.Application.Features.User;

[ApiEndpoint(ApiMethod.Post, "/api/supperadmin/create", Summary = "Create Supper Admin")]
public class CreateSupperAdminCommand : BaseTransactionModel, ICommand<bool?>
{
    public string Reference { get; set; }
    public string UserId { get; set; }
    public string LoginName { get; set; }
    public string UserCode { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string BranchCode { get; set; }
    public bool IsSupperAdmin { get; set; }
    public string DeviceId { get; set; } = string.Empty;
    public string DeviceType { get; set; }
    public string IpAddress { get; set; }
    public string UserAgent { get; set; }
    public string RoleChannel { get; set; }
    public bool IsO24ManageUser { get; set; } = true;
    public string PushId { get; set; }
    public string OsVersion { get; set; }
    public string AppVersion { get; set; }
    public string DeviceName { get; set; }
    public string Brand { get; set; }
    public bool IsEmulator { get; set; }
    public bool IsRootedOrJailbroken { get; set; }
    public string Modelname { get; set; } = string.Empty;
    public bool IsResetDevice { get; set; } = false;
    public string CoreToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public string Network { get; set; } = string.Empty;
    public string Memory { get; set; } = string.Empty;
}

[CqrsHandler]
public class CreateSupperAdminHandler(
    ISupperAdminRepository supperAdminRepository,
    IUserAccountRepository userAccountRepository,
    IUserPasswordRepository userPasswordRepository
) : ICommandHandler<CreateSupperAdminCommand, bool?>
{
    [WorkflowStep(WorkflowStepCode.CTH.WF_STEP_CTH_CREATE_SADMIN)]
    public async Task<bool?> HandleAsync(
        CreateSupperAdminCommand request,
        CancellationToken cancellationToken = default
    )
    {
        SupperAdmin exited = await supperAdminRepository.IsExit();
        if (exited != null)
        {
            throw new O24OpenAPIException("Supper Admin already exits.");
        }

        //(string hashedPass, string salt) = PasswordUtils.HashPassword(request.Password);
        string hashPassword = O9Encrypt.sha_sha256(request.Password, request.LoginName);
        string salt = PasswordUtils.GenerateRandomSalt();

        string userId = Guid.NewGuid().ToString();
        SupperAdmin sAdmin = new()
        {
            UserId = userId,
            LoginName = request.LoginName,
            PasswordHash = hashPassword,
        };
        UserAccount userAccount = new()
        {
            ChannelId = request.ChannelId,
            UserId = userId,
            UserName = request.LoginName,
            UserCode = Guid.NewGuid().ToString(),
            LoginName = request.LoginName,
            RoleChannel = "[1]",
            Status = "A",
            UserCreated = request.LoginName,
            IsSuperAdmin = true,
            BranchID = "0",
        };
        UserPassword userPassword = new()
        {
            ChannelId = request.ChannelId,
            UserId = sAdmin.UserId,
            Password = hashPassword,
            Salt = salt,
        };

        await userAccountRepository.InsertAsync(userAccount);
        await supperAdminRepository.InsertAsync(sAdmin);
        await userPasswordRepository.InsertAsync(userPassword);
        return true;
    }
}
