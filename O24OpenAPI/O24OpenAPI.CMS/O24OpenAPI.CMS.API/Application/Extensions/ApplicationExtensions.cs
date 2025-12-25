using O24OpenAPI.CMS.API.Application.Models.ContextModels;
using O24OpenAPI.CMS.API.Application.Services.Interfaces;
using O24OpenAPI.CMS.API.Application.Services.QR;
using O24OpenAPI.CMS.API.Application.Services.Services;
using O24OpenAPI.Framework.Abstractions;
using O24OpenAPI.Framework.Domain.Logging;

namespace O24OpenAPI.CMS.API.Application.Extensions;

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

        string? appVersion = configuration["AppVersion"];

        //common
        services.AddScoped<ICommonService, CommonService>();

        //services
        services.AddSingleton<IMemoryCacheService, MemoryCacheService>();
        services.AddSingleton<SignalHubService, SignalHubService>();
        services.AddScoped<ICMSSettingService, CMSSettingService>();
        services.AddScoped<ILogServiceService, LogServiceService>();
        services.AddScoped<JWebUIObjectContextModel>();
        // services.AddScoped<DataMigration>();
        services.AddScoped<IMailConfigService, MailConfigService>();
        services.AddScoped<IMailTemplateService, MailTemplateService>();
        services.AddScoped<ISendMailService, SendMailService>();
        services.AddScoped<IRaiseErrorService, RaiseErrorService>();
        services.AddScoped<IValidationService, ValidationService>();
        services.AddScoped<ILoadFormService, LoadFormService>();
        services.AddScoped<IFormService, FormService>();
        services.AddScoped<IParaServerService, ParaServerService>();
        services.AddScoped<IDbFunctionService, DbFunctionService>();
        services.AddScoped<HttpLog>();
        services.AddScoped<IQRService, QRService>();

        services.AddScoped<ITranslationService, LoadTranslationService>();
        services.AddScoped<IFormFieldDefinitionService, FormFieldDefinitionService>();
        services.AddScoped<ICoreAPIService, CoreAPIService>();
        services.AddScoped<ISignalHubBusinessService, SignalHubBusinessService>();
        services.AddLinKitCqrs("cms");
        services.AddLinKitDependency();
        services.AddSingleton<IWorkflowStepInvoker, Workflow.Generated.WorkflowStepInvoker>();
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
            endpoints.MapControllers();
        });
    }

    /// <summary>
    /// Gets order of this dependency registrar implementation
    /// </summary>
    public int Order => 2000;
}
