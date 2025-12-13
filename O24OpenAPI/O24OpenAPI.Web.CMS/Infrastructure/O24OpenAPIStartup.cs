using O24OpenAPI.Web.CMS.GrpcServices;
using O24OpenAPI.Web.CMS.Models.ContextModels;
using O24OpenAPI.Web.CMS.Services.Factory;
using O24OpenAPI.Web.CMS.Services.Interfaces;
using O24OpenAPI.Web.CMS.Services.Interfaces.CBS;
using O24OpenAPI.Web.CMS.Services.Interfaces.Digital;
using O24OpenAPI.Web.CMS.Services.Interfaces.Media;
using O24OpenAPI.Web.CMS.Services.NeptuneService;
using O24OpenAPI.Web.CMS.Services.O9Service;
using O24OpenAPI.Web.CMS.Services.QR;
using O24OpenAPI.Web.CMS.Services.Services;
using O24OpenAPI.Web.CMS.Services.Services.Digital;
using O24OpenAPI.Web.CMS.Services.Services.Logging;
using O24OpenAPI.Web.CMS.Services.Services.Media;
using O24OpenAPI.Web.CMS.Services.Services.Portal;
using O24OpenAPI.Web.Framework.Domain.Logging;
using Stimulsoft.Report;

namespace O24OpenAPI.Web.CMS.Infrastructure;

public class O24OpenAPIStartup : IO24OpenAPIStartup
{
    public virtual void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(
                "CORSPolicy",
                builder =>
                    builder
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .SetIsOriginAllowed((hosts) => true)
            );
        });

        var appVersion = configuration["AppVersion"];

        //common
        services.AddScoped<ICommonService, CommonService>();

        //services
        services.AddScoped<IWebChannelService, WebChannelService>();
        services.AddSingleton<IMemoryCacheService, MemoryCacheService>();
        services.AddSingleton<SignalHubService, SignalHubService>();
        services.AddScoped<IWorkflowService, WorkflowService>();
        services.AddScoped<ICMSSettingService, CMSSettingService>();
        services.AddScoped<IUserSessionsService, UserSessionsService>();
        services.AddScoped<ICallMapService, CallMapService>();
        services.AddScoped<ILearnApiService, LearnApiService>();
        services.AddScoped<IBaseO9WorkflowService, BaseO9WorkflowService>();
        services.AddScoped<IFOService, FOService>();
        services.AddScoped<IBaseService, BaseService>();
        services.AddScoped<IPostService, PostService>();
        services.AddScoped<IO9ClientService, O9ClientService>();
        services.AddScoped<IMappingService, MappingService>();
        services.AddScoped<IApiService, ApiService>();
        services.AddScoped<ILogServiceService, LogServiceService>();
        services.AddScoped<IAuthenticateService, AuthenticateService>();
        services.AddScoped<ISmartOTPService, SmartOTPService>();
        services.AddScoped<IWebPortalService, WebPortalService>();
        services.AddScoped<JWebUIObjectContextModel>();
        // services.AddScoped<DataMigration>();
        services.AddScoped<IMailConfigService, MailConfigService>();
        services.AddScoped<IMailTemplateService, MailTemplateService>();
        services.AddScoped<ISendMailService, SendMailService>();
        services.AddScoped<ID_BANKService, D_BANKService>();
        services.AddScoped<ID_RECEIVERLISTService, D_RECEIVERLISTService>();
        services.AddScoped<ID_TEMPLATETRANSFERService, D_TEMPLATETRANSFERService>();
        services.AddScoped<IRaiseErrorService, RaiseErrorService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<IDeviceService, DeviceService>();
        services.AddScoped<IBranchService, BranchService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<INotificationTemplateService, NotificationTemplateService>();
        services.AddScoped<ICountryService, CountryService>();
        services.AddScoped<IRemittancePurposesService, RemittancePurposesService>();
        services.AddScoped<IReasonsDefinitionService, ReasonsDefinitionService>();
        services.AddScoped<IFeeService, FeeService>();
        services.AddScoped<IFeeTypeService, FeeTypeService>();
        services.AddScoped<ID_ServiceService, D_ServiceService>();
        services.AddScoped<ISecurityQuestionService, SecurityQuestionService>();
        services.AddScoped<IO9ClientService, O9ClientService>();
        services.AddScoped<IUserCommandService, UserCommandService>();
        services.AddScoped<Services.O9Service.O9AuthenticateService>();
        services.AddScoped<ICoreServiceFactory, CoreServiceFactory>();
        services.AddScoped<IUserFavoriteFeatureService, UserFavoriteFeatureService>();
        services.AddScoped<IFavoriteFeatureService, FavoriteFeatureService>();
        services.AddScoped<ISavingProductService, SavingProductService>();
        services.AddScoped<ICityService, CityService>();
        services.AddScoped<ICurrencyService, CurrencyService>();
        services.AddScoped<ICardService, CardService>();
        services.AddScoped<ICardServiceService, CardServiceService>();
        services.AddScoped<ICardUserService, CardUserService>();
        services.AddScoped<IUserPortalService, UserPortalService>();
        services.AddScoped<ICodeListService, CodeListService>();
        services.AddScoped<IRewardService, RewardService>();
        services.AddScoped<IRequestRewardService, RequestRewardService>();
        services.AddScoped<IValidationService, ValidationService>();
        services.AddScoped<IUserRewardService, UserRewardService>();
        services.AddScoped<ILoadFormService, LoadFormService>();
        services.AddScoped<IFormService, FormService>();
        services.AddScoped<IUserInRoleService, UserInRoleService>();
        services.AddScoped<IWorkflowStepService, WorkflowStepService>();
        services.AddScoped<IWorkflowDefinitionService, WorkflowDefinitionService>();
        services.AddScoped<IParaServerService, ParaServerService>();
        services.AddScoped<IDataService, DataService>();
        services.AddScoped<IDbFunctionService, DbFunctionService>();
        services.AddScoped<IWorkflowStepLogService, WorkflowStepLogService>();
        services.AddScoped<IRoleProfileService, RoleProfileService>();
        services.AddScoped<IUserAccountService, UserAccountService>();
        services.AddScoped<HttpLog>();
        services.AddScoped<IUserRightService, UserRightService>();
        services.AddScoped<IQRService, QRService>();

        //Core
        services.AddScoped<O9JournalService>();
        services.AddScoped<INeptuneCBSService, NeptuneCBSService>();
        services.RegisterO9CBService();

        //CrossService
        services.AddScoped<ICrossWorkflowService, CrossWorkflowService>();

        // report instance
        services.AddScoped<StiReport>();

        services.AddScoped<ITranslationService, LoadTranslationService>();
        services.AddScoped<IFormFieldDefinitionService, FormFieldDefinitionService>();
        services.AddScoped<IMediaService, MediaService>();
        services.AddScoped<IAppTypeConfigService, AppTypeConfigService>();
        services.AddScoped<IAppLanguageConfigService, AppLanguageConfigService>();
        services.AddScoped<ICoreAPIService, CoreAPIService>();
        services.AddScoped<ISignalHubBusinessService, SignalHubBusinessService>();
        services.AddScoped<IFileStorageService, S3FileStorageService>();


    }

    /// <summary>
    /// Configure the using of added middleware
    /// </summary>
    /// <param name="application">Builder for configuring an application's request pipeline</param>
    public void Configure(IApplicationBuilder application)
    {
        application.UseCors("CORSPolicy");
        application.UseRouting();
        application.UseEndpoints(endpoints =>
        {
            endpoints.MapGrpcService<CMSGrpcService>();
            endpoints.MapControllers();
        });
    }

    /// <summary>
    /// Gets order of this dependency registrar implementation
    /// </summary>
    public int Order => 2000;
}

public static class RegisterCB
{
    public static void RegisterO9CBService(this IServiceCollection services)
    {
        services.AddScoped<ICBService, O9CBService>();
    }
}
