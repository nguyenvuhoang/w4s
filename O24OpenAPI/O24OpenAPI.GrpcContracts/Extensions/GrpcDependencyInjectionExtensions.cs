using LinKit.Core.Cqrs;
using Microsoft.Extensions.DependencyInjection;
using O24OpenAPI.Core.Configuration;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Generated.Mediator;
using O24OpenAPI.Grpc.WFO;
using O24OpenAPI.GrpcContracts.Configuration;
using O24OpenAPI.GrpcContracts.Factory;
using O24OpenAPI.GrpcContracts.GrpcClient;
using O24OpenAPI.GrpcContracts.GrpcClientServices.CBG;
using O24OpenAPI.GrpcContracts.GrpcClientServices.CTH;
using O24OpenAPI.GrpcContracts.GrpcClientServices.DTS;
using O24OpenAPI.GrpcContracts.GrpcClientServices.DWH;
using O24OpenAPI.GrpcContracts.GrpcClientServices.LOG;
using O24OpenAPI.GrpcContracts.GrpcClientServices.NCH;
using O24OpenAPI.GrpcContracts.GrpcClientServices.PMT;
using O24OpenAPI.GrpcContracts.GrpcClientServices.WFO;
using O24OpenAPI.Logging.Helpers;

namespace O24OpenAPI.GrpcContracts.Extensions;

public static class GrpcDependencyInjectionExtensions
{
    public static void AddGrpcContracts(this IServiceCollection services)
    {
        services.AddKeyedScoped<IMediator, GrpcMediator>("grpc");
        services.AddSingleton<IGrpcClientFactory, GrpcClientFactory>();
        services.AddSingleton(typeof(IGrpcClient<>), typeof(ClientGrpc<>));
        services.AddLinKitDependency();

        services.AddScoped<IWFOGrpcClientService, WFOGrpcClientService>();
        services.AddScoped<ICTHGrpcClientService, CTHGrpcClientService>();
        services.AddScoped<IPMTGrpcClientService, PMTGrpcClientService>();
        services.AddScoped<ICTHGrpcClientService, CTHGrpcClientService>();
        services.AddScoped<ICBGGrpcClientService, CBGGrpcClientService>();
        services.AddScoped<IDTSGrpcClientService, DTSGrpcClientService>();
        services.AddScoped<INCHGrpcClientService, NCHGrpcClientService>();
        services.AddScoped<IDWHGrpcClientService, DWHGrpcClientService>();
        services.AddScoped<ILOGGrpcClientService, LOGGrpcClientService>();

        // services.AddSingleton<ILogSubmitter, GrpcLogSubmitter>();

        #region Register Grpc

        try
        {
            O24OpenAPIConfiguration o24Config =
                Singleton<O24OpenAPIConfiguration>.Instance
                ?? throw new Exception(
                    "O24OpenAPIConfiguration is not initialized when regitry grpc."
                );
            var grpcClientsConfig = new GrpcClientsConfig
            {
                { typeof(WFOGrpcService).Name, o24Config.WFOGrpcURL },
            };
            Singleton<GrpcClientsConfig>.Instance = grpcClientsConfig;
            if (o24Config.ConnectToWFO && !o24Config.YourServiceID.EqualsOrdinalIgnoreCase("WFO"))
            {
                ServiceProvider serviceProvider = services.BuildServiceProvider();
                using IServiceScope scope = serviceProvider.CreateScope();
                AsyncScope.Scope = scope;
                IWFOGrpcClientService wfoGrpcClientService =
                    serviceProvider.GetRequiredService<IWFOGrpcClientService>();
                var grpcService = $"{o24Config.YourServiceID}GrpcService";
                wfoGrpcClientService
                    .RegisterServiceGrpcEndpointAsync(
                        serviceCode: o24Config.YourServiceID,
                        serviceHandleName: grpcService,
                        grpcEndpointURL: o24Config.YourGrpcURL,
                        instanceID: o24Config.YourInstanceID,
                        serviceAssemblyName: o24Config.AssemblyMigration
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
