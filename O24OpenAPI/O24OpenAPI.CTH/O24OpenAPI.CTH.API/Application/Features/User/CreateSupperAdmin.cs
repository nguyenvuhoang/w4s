using LinKit.Core.Cqrs;
using LinKit.Core.Endpoints;
using O24OpenAPI.Core;
using O24OpenAPI.CTH.API.Application.Utils;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Framework.Constants;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.CTH.API.Application.Features.User;

[ApiEndpoint(ApiMethod.Post, "/api/supperadmin/create", Summary = "Create Supper Admin")]
public class CreateSupperAdminCommand : BaseTransactionModel, ICommand<bool?>
{
    /// <summary>
    /// Gets or sets the value of the reference
    /// </summary>
    public string Reference { get; set; }

    /// <summary>
    /// Gets or sets the value of the user id
    /// </summary>
    public string UserId { get; set; }

    /// <summary>
    /// Gets or sets the value of the login name
    /// </summary>
    public string LoginName { get; set; }

    public string UserCode { get; set; }
    public string UserName { get; set; }

    /// <summary>
    /// Gets or sets the value of the password
    /// </summary>
    public string Password { get; set; }
    public string BranchCode { get; set; }

    /// <summary>
    /// Gets or sets the value of the is supper admin
    /// </summary>
    public bool IsSupperAdmin { get; set; }

    /// <summary>
    /// /// Gets or sets the value of the device
    /// </summary>
    public string DeviceId { get; set; } = string.Empty;

    /// <summary>
    /// /// Gets or sets the value of the device
    /// </summary>
    public string DeviceType { get; set; }

    /// <summary>
    /// Gets or sets the value of the ip address
    /// </summary>
    public string IpAddress { get; set; }

    /// <summary>
    /// User Agent
    /// </summary>
    public string UserAgent { get; set; }

    /// <summary>
    /// Gets or sets the value of the roles
    /// </summary>
    public string RoleChannel { get; set; }
    public bool IsO24ManageUser { get; set; } = true;

    /// <summary>
    /// Gets or sets the value of the PushId
    /// </summary>
    public string PushId { get; set; }

    /// <summary>
    /// OSVERSION
    /// </summary>
    public string OsVersion { get; set; }

    /// <summary>
    /// App Version
    /// </summary>
    public string AppVersion { get; set; }

    /// <summary>
    /// Device Name
    /// </summary>
    public string DeviceName { get; set; }

    /// <summary>
    /// Brand
    /// </summary>
    public string Brand { get; set; }

    /// <summary>
    /// IsEmulator
    /// </summary>
    public bool IsEmulator { get; set; }

    /// <summary>
    /// IsRootedOrJailbroken
    /// </summary>
    public bool IsRootedOrJailbroken { get; set; }

    /// <summary>
    /// Modelname
    /// </summary>
    public string Modelname { get; set; } = string.Empty;

    /// <summary>
    /// IsResetDevice
    /// </summary>
    public bool IsResetDevice { get; set; } = false;

    /// <summary>
    /// Core Token
    /// </summary>
    public string CoreToken { get; set; } = string.Empty;

    /// <summary>
    /// Refresh Token
    /// </summary>
    public string RefreshToken { get; set; } = string.Empty;

    /// <summary>
    /// Network
    /// </summary>
    public string Network { get; set; } = string.Empty;

    /// <summary>
    /// Memory
    /// </summary>
    public string Memory { get; set; } = string.Empty;
}

[CqrsHandler]
public class CreateSupperAdminHandler(
    ISupperAdminRepository supperAdminRepository,
    IUserAccountRepository userAccountRepository, IUserPasswordRepository userPasswordRepository
) : ICommandHandler<CreateSupperAdminCommand, bool?>
{
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

        (string hashedPass, string salt) = PasswordUtils.HashPassword(request.Password);
        string userId = Guid.NewGuid().ToString();
        SupperAdmin sAdmin = new()
        {
            UserId = userId,
            LoginName = request.LoginName,
            PasswordHash = hashedPass,
        };
        UserAccount userAccount = new()
        {
            ChannelId = ChannelId.Portal,
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
            Password = hashedPass,
            Salt = salt
        };

        await userAccountRepository.InsertAsync(userAccount);
        await supperAdminRepository.InsertAsync(sAdmin);
        await userPasswordRepository.InsertAsync(userPassword);
        return true;
    }
}
