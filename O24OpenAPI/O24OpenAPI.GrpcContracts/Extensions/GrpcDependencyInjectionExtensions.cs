using Microsoft.Extensions.DependencyInjection;
using O24OpenAPI.Core.Configuration;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Grpc.WFO;
using O24OpenAPI.GrpcContracts.Configuration;
using O24OpenAPI.GrpcContracts.Factory;
using O24OpenAPI.GrpcContracts.GrpcClient;
using O24OpenAPI.GrpcContracts.GrpcClientServices.CBG;
using O24OpenAPI.GrpcContracts.GrpcClientServices.CMS;
using O24OpenAPI.GrpcContracts.GrpcClientServices.CTH;
using O24OpenAPI.GrpcContracts.GrpcClientServices.DTS;
using O24OpenAPI.GrpcContracts.GrpcClientServices.DWH;
using O24OpenAPI.GrpcContracts.GrpcClientServices.LOG;
using O24OpenAPI.GrpcContracts.GrpcClientServices.NCH;
using O24OpenAPI.GrpcContracts.GrpcClientServices.TEL;
using O24OpenAPI.GrpcContracts.GrpcClientServices.WFO;
using O24OpenAPI.Logging.Helpers;

namespace O24OpenAPI.GrpcContracts.Extensions;

public static class GrpcDependencyInjectionExtensions
{
    public static void AddGrpcContracts(this IServiceCollection services)
    {
        services.AddSingleton<IGrpcClientFactory, GrpcClientFactory>();
        services.AddSingleton(typeof(IGrpcClient<>), typeof(ClientGrpc<>));
        services.AddLinKitDependency();

        services.AddScoped<IWFOGrpcClientService, WFOGrpcClientService>();
        services.AddScoped<ICTHGrpcClientService, CTHGrpcClientService>();
        services.AddScoped<ICMSGrpcClientService, CMSGrpcClientService>();
        services.AddScoped<ICTHGrpcClientService, CTHGrpcClientService>();
        services.AddScoped<ICBGGrpcClientService, CBGGrpcClientService>();
        services.AddScoped<IDTSGrpcClientService, DTSGrpcClientService>();
        services.AddScoped<INCHGrpcClientService, NCHGrpcClientService>();
        services.AddScoped<ITELGrpcClientService, TELGrpcClientService>();
        services.AddScoped<IDWHGrpcClientService, DWHGrpcClientService>();
        services.AddScoped<ILOGGrpcClientService, LOGGrpcClientService>();

        // services.AddSingleton<ILogSubmitter, GrpcLogSubmitter>();

        #region Register Grpc

        try
        {
            var grpcClientsConfig = new GrpcClientsConfig
            {
                {
                    typeof(WFOGrpcService).Name,
                    Singleton<O24OpenAPIConfiguration>.Instance.WFOGrpcURL
                },
            };
            Singleton<GrpcClientsConfig>.Instance = grpcClientsConfig;
            if (
                Singleton<O24OpenAPIConfiguration>.Instance.ConnectToWFO
                && Singleton<O24OpenAPIConfiguration>.Instance.YourServiceID != "WFO"
            )
            {
                var serviceProvider = services.BuildServiceProvider();
                using var scope = serviceProvider.CreateScope();
                AsyncScope.Scope = scope;
                var wfoGrpcClientService =
                    serviceProvider.GetRequiredService<IWFOGrpcClientService>();
                var grpcService =
                    $"{Singleton<O24OpenAPIConfiguration>.Instance.YourServiceID}GrpcService";
                wfoGrpcClientService
                    .RegisterServiceGrpcEndpointAsync(
                        serviceCode: Singleton<O24OpenAPIConfiguration>.Instance.YourServiceID,
                        serviceHandleName: grpcService,
                        grpcEndpointURL: Singleton<O24OpenAPIConfiguration>.Instance.YourGrpcURL,
                        instanceID: Singleton<O24OpenAPIConfiguration>.Instance.YourInstanceID,
                        serviceAssemblyName: Singleton<O24OpenAPIConfiguration>
                            .Instance
                            .AssemblyMigration
                    )
                    .GetAsyncResult();
            }
        }
        catch (Exception ex)
        {
            BusinessLogHelper.Error(ex, "Failed to register gRPC service with WFO.");
            Console.WriteLine("Failed to register gRPC service with WFO. Exception: " + ex.Message);
        }

        #endregion
    }
}
