//using Microsoft.AspNetCore.Builder;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using O24OpenAPI.Core.Infrastructure;
//using O24OpenAPI.Grpc.NCH;


//namespace O24OpenAPI.NCH.Infrastructure;

//public class O24OpenAPIStartup : IO24OpenAPIStartup
//{
//    public int Order => 2000;

//    public void Configure(IApplicationBuilder application)
//    {
//        application.UseCors("CORSPolicy");
//        application.UseRouting();
//        application.UseEndpoints(endpoints =>
//        {
//            endpoints.MapGrpcService<NCHGrpcService>();
//            endpoints.MapControllers();
//        });
//    }

//    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
//    {
//        //var setting = EngineContext.Current.Resolve<O24NCHSetting>();
//        services.AddScoped<IRaiseErrorService, RaiseErrorService>();
//        services.AddScoped<ISMSService, SMSService>();
//        services.AddScoped<ISMSProviderService, SMSProviderService>();
//        services.AddScoped<IFirebaseService, FirebaseService>();
//        services.AddScoped<INotificationTemplateService, NotificationTemplateService>();
//        services.AddScoped<INotificationService, NotificationService>();
//        services.AddScoped<IEmailService, EmailService>();
//        services.AddScoped<IMailConfigService, MailConfigService>();
//        services.AddScoped<IMailTemplateService, MailTemplateService>();
//        services.AddScoped<ISendMailService, SendMailService>();
//        services.AddScoped<ITelegramService, TelegramService>();
//        services.AddScoped<IUserNotificationsService, UserNotificationsService>();
//        services
//            .AddHttpClient("tgwebhook")
//            .RemoveAllLoggers()
//            .AddTypedClient<ITelegramBotClient>(httpClient => new TelegramBotClient(
//                "8197855911:AAHId-nVPso_dkkBTsE5WHO-Ae0dp9HzMpE",
//                httpClient
//            ));
//        services.AddScoped<UpdateHandler>();
//        services.AddScoped<IMailSendOutService, MailSendOutService>();
//        services.AddScoped<ISMSTemplateService, SMSTemplateService>();
//        services.AddScoped<ISMSLoanAlertService, SMSLoanAlertService>();
//    }
//}
