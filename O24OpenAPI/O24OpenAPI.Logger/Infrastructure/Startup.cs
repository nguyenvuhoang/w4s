using MediatR;
using O24OpenAPI.Core.Abstractions;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Framework.Domain.Logging;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.Logger.Domain;
using O24OpenAPI.Logger.GrpcServices;
using O24OpenAPI.Logger.Services;
using O24OpenAPI.Logger.Services.CommandHandler;
using O24OpenAPI.Logger.Services.Interfaces;
using O24OpenAPI.Logger.Services.QueryHandler;

namespace O24OpenAPI.Logger.Infrastructure;

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
            endpoints.MapGrpcService<LOGGrpcService>();
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
        // services.AddScoped<IServiceLogService, ServiceLogService>();
        // services.AddScoped<IHttpLogService, HttpLogService>();
        services.AddScoped<ILogService<HttpLog>, HttpLogService>();
        services.AddScoped<ILogService<ServiceLog>, ServiceLogService>();

        services.AddScoped<ILogService<WorkflowLog>, WorkflowLogService>();
        services.AddScoped<ILogService<WorkflowStepLog>, WorkflowStepLogService>();

        // services.AddScoped<LogCommandHandler<HttpLog>>();
        // services.AddScoped<LogCommandHandler<ServiceLog>>();

        services.AddScoped<IRequestHandler<LogCommand<HttpLog>>, LogCommandHandler<HttpLog>>();
        services.AddScoped<
            IRequestHandler<LogCommand<ServiceLog>>,
            LogCommandHandler<ServiceLog>
        >();
        services.AddScoped<
            IRequestHandler<LogCommand<WorkflowLog>>,
            LogCommandHandler<WorkflowLog>
        >();
        services.AddScoped<
            IRequestHandler<LogCommand<WorkflowStepLog>>,
            LogCommandHandler<WorkflowStepLog>
        >();

        services.AddScoped<
            IRequestHandler<SimpleSearchQuery<ServiceLog>, PagedModel>,
            SimpleSearchQueryHandler<ServiceLog>
        >();

        services.AddScoped<
            IRequestHandler<SimpleSearchQuery<HttpLog>, PagedModel>,
            SimpleSearchQueryHandler<HttpLog>
        >();

        services.AddScoped<
            IRequestHandler<SimpleSearchQuery<WorkflowLog>, PagedModel>,
            SimpleSearchQueryHandler<WorkflowLog>
        >();
        services.AddScoped<
            IRequestHandler<SimpleSearchQuery<WorkflowStepLog>, PagedModel>,
            SimpleSearchQueryHandler<WorkflowStepLog>
        >();
        services.AddScoped<
            IRequestHandler<ViewDetailQuery<WorkflowStepLog>, WorkflowStepLog>,
            ViewDetailQueryHandler<WorkflowStepLog>
        >();
        services.AddScoped<IWorkflowStepLogService, WorkflowStepLogService>();
        services.AddScoped<IApplicationLogService, ApplicationLogService>();
    }
}
