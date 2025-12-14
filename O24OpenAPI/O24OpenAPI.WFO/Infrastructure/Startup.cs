using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Web.Framework.Services.Grpc;
using O24OpenAPI.WFO.GrpcServices;
using O24OpenAPI.WFO.Models;
using O24OpenAPI.WFO.Services;
using O24OpenAPI.WFO.Services.Interfaces;

namespace O24OpenAPI.WFO.Infrastructure;

public class Startup : IO24OpenAPIStartup
{
    public int Order => 2000;

    public void Configure(IApplicationBuilder application)
    {
        application.UseRouting();
        application.UseEndpoints(endpoints =>
        {
            endpoints.MapGrpcService<WFOGrpcService>();
            endpoints.MapControllers();
        });
    }

    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IServiceInstanceService, ServiceInstanceService>();
        services.AddScoped<IWorkflowService, WorkflowService>();
        services.AddScoped<IWorkflowExecutionService, WorkflowExecutionService>();
        services.AddScoped<WorkflowExecution>();
        services.AddScoped<IWorkflowInfoService, WorkflowInfoService>();
        services.AddScoped<IWorkflowStepInfoService, WorkflowStepInfoService>();
        services.AddScoped<IWorkflowExecutionGrpc, WorkflowExecutionService>();
        services.AddScoped<IWorkflowDefService, WorkflowDefService>();
        services.AddScoped<IWorkflowStepService, WorkflowStepService>();
    }
}
