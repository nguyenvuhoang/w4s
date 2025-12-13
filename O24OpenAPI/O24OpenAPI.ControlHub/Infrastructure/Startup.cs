using O24OpenAPI.ControlHub.GrpcServices;
using O24OpenAPI.ControlHub.Services;
using O24OpenAPI.ControlHub.Services.Interfaces;
using O24OpenAPI.Core.Infrastructure;

namespace O24OpenAPI.ControlHub.Infrastructure;

/// <summary>
/// The startup class
/// </summary>
/// <seealso cref="IO24OpenAPIStartup"/>
public class Startup : IO24OpenAPIStartup
{
    /// <summary>
    /// Gets the value of the order
    /// </summary>
    public int Order => 2000;

    /// <summary>
    /// Configures the application
    /// </summary>
    /// <param name="application">The application</param>
    public void Configure(IApplicationBuilder application)
    {
        application.UseRouting();
        application.UseEndpoints(endpoints =>
        {
            endpoints.MapGrpcService<CTHGrpcService>();
            endpoints.MapControllers();
        });
    }

    /// <summary>
    /// Configures the services using the specified services
    /// </summary>
    /// <param name="services">The services</param>
    /// <param name="configuration">The configuration</param>
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUserSessionService, UserSessionService>();
        services.AddScoped<IUserAccountService, UserAccountService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ISupperAdminService, SupperAdminService>();
        services.AddScoped<IUserRightService, UserRightService>();
        services.AddScoped<IUserCommandService, UserCommandService>();
        services.AddScoped<IUserDeviceService, UserDeviceService>();
        services.AddScoped<IUserPasswordService, UserPasswordService>();
        services.AddScoped<IUserAuthenService, UserAuthenService>();
        services.AddScoped<IAvatarMigrationService, AvatarMigrationService>();
        services.AddScoped<IUserAvatarService, UserAvatarService>();
        services.AddScoped<IControlHubService, ControlHubService>();
        services.AddScoped<IUserInRoleService, UserInRoleService>();
        services.AddScoped<IRoleProfileService, RoleProfileService>();
        services.AddScoped<IUserRoleService, UserRoleService>();
        services.AddScoped<IAuthSessionService, AuthSessionService>();
        services.AddScoped<INotificationBuilder, NotificationBuilder>();
        services.AddScoped<IUserLimitService, UserLimitService>();
        services.AddScoped<ICalendarService, CalendarService>();
        services.AddScoped<IBankService, BankService>();
        services.AddScoped<IChannelService, ChannelService>();
        services.AddScoped<IUserAgreementService, UserAgreementService>();
        services.AddScoped<IUserBannerService, UserBannerService>();
    }
}
