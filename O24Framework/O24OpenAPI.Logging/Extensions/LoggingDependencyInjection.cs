using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using O24OpenAPI.Core.Configuration;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Logging.Abstractions;
using O24OpenAPI.Logging.Formatters;
using O24OpenAPI.Logging.Handlers;
using O24OpenAPI.Logging.Interceptors;
using O24OpenAPI.Logging.Middlewares;
using Serilog;
using Serilog.Filters;
using Serilog.Sinks.PeriodicBatching;

namespace O24OpenAPI.Logging.Extensions;

public static class LoggingDependencyInjection
{
    public static WebApplicationBuilder AddO24Logging(this WebApplicationBuilder builder)
    {
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddSingleton<GrpcLoggingInterceptor>();
        builder.Services.AddTransient<HttpLoggingHandler>();
        builder.Services.AddSingleton<GrpcClientLoggingInterceptor>();

        builder.Host.UseSerilog(
            (context, serviceProvider, loggerConfiguration) =>
            {
                var serviceName =
                    context.Configuration.GetValue<string>("O24Logging:ServiceName")
                    ?? Singleton<O24OpenAPIConfiguration>.Instance.YourServiceID;
                var sinkOptions = context
                    .Configuration.GetSection("O24Logging:Sink")
                    .Get<SinkOptions>();
                if (sinkOptions == null)
                {
                    try
                    {
                        sinkOptions =
                            Singleton<AppSettings>.Instance.Get<SinkOptions>() ?? new SinkOptions();
                    }
                    catch
                    {
                        sinkOptions = new SinkOptions();
                    }
                }

                loggerConfiguration
                    .ReadFrom.Configuration(context.Configuration)
                    .Enrich.FromLogContext()
                    .Enrich.WithProperty("ServiceName", serviceName);

                bool writeBusinessToFile =
                    sinkOptions.Mode.Equals("hybrid", StringComparison.CurrentCultureIgnoreCase)
                    || sinkOptions.Mode.Equals(
                        "fileonly",
                        StringComparison.CurrentCultureIgnoreCase
                    );
                bool sendRemote =
                    sinkOptions.Mode.Equals("hybrid", StringComparison.CurrentCultureIgnoreCase)
                    || sinkOptions.Mode.Equals(
                        "remoteonly",
                        StringComparison.CurrentCultureIgnoreCase
                    );

                #region System Logs

                if (sinkOptions.SystemLogToFile)
                {
                    loggerConfiguration.WriteTo.Logger(lc =>
                        lc.Filter.ByExcluding(Matching.WithProperty("LogType"))
                            .WriteTo.Map(
                                logEvent =>
                                {
                                    var direction =
                                        (
                                            logEvent.Properties.TryGetValue("Direction", out var d)
                                            && d is Serilog.Events.ScalarValue svd
                                        )
                                            ? svd.Value?.ToString()
                                            : "default";
                                    return $"system/{direction}".ToLower();
                                },
                                (key, wt) =>
                                {
                                    var path = Path.Combine("logs", key, "log-.txt");
                                    wt.Async(a =>
                                        a.File(
                                            new CustomTextFormatter(),
                                            path,
                                            rollingInterval: RollingInterval.Day,
                                            retainedFileCountLimit: sinkOptions.RetainedFileCountLimit
                                        )
                                    );
                                }
                            )
                    );
                    Console.WriteLine("[O24Logging] System logs are written to file.");
                }
                else
                {
                    loggerConfiguration.WriteTo.Logger(lc =>
                        lc.Filter.ByExcluding(Matching.WithProperty("LogType"))
                            .WriteTo.Console(new CustomTextFormatter())
                    );
                    Console.WriteLine("[O24Logging] System logs are written to console.");
                }

                #endregion

                #region Business Logs

                if (writeBusinessToFile)
                {
                    loggerConfiguration.WriteTo.Logger(lc =>
                        lc.Filter.ByIncludingOnly(Matching.WithProperty("LogType"))
                            .WriteTo.Map(
                                logEvent =>
                                {
                                    var logType =
                                        (
                                            logEvent.Properties.TryGetValue("LogType", out var lt)
                                            && lt is Serilog.Events.ScalarValue svlt
                                        )
                                            ? svlt.Value?.ToString()
                                            : "business";
                                    var direction =
                                        (
                                            logEvent.Properties.TryGetValue("Direction", out var d)
                                            && d is Serilog.Events.ScalarValue svd
                                        )
                                            ? svd.Value?.ToString()
                                            : "default";
                                    return $"{logType}/{direction}".ToLower();
                                },
                                (key, wt) =>
                                {
                                    var path = Path.Combine("logs", key, "log-.txt");
                                    wt.Async(a =>
                                        a.File(
                                            new CustomTextFormatter(),
                                            path,
                                            rollingInterval: RollingInterval.Day,
                                            retainedFileCountLimit: sinkOptions.RetainedFileCountLimit
                                        )
                                    );
                                }
                            )
                    );
                    Console.WriteLine("[O24Logging] Business logs are written to file.");
                }

                #endregion

                #region Remote Logs

                if (sendRemote && !string.IsNullOrEmpty(sinkOptions.Type))
                {
                    var submitter = serviceProvider.GetKeyedService<ILogSubmitter>(
                        sinkOptions.Type
                    );
                    if (submitter != null)
                    {
                        loggerConfiguration.WriteTo.Logger(lc =>
                            lc.Filter.ByIncludingOnly(Matching.WithProperty("LogType"))
                                .WriteTo.Sink(
                                    new PeriodicBatchingSink(
                                        new BatchingSubmitterAdapter(submitter),
                                        new PeriodicBatchingSinkOptions
                                        {
                                            BatchSizeLimit = sinkOptions.BatchSizeLimit,
                                            Period = TimeSpan.FromSeconds(
                                                sinkOptions.PeriodSeconds
                                            ),
                                            EagerlyEmitFirstEvent = true,
                                        }
                                    )
                                )
                        );
                        Console.WriteLine(
                            $"[O24Logging] Remote sink is enabled for Business/Communication logs. Type: {sinkOptions.Type.ToUpper()}."
                        );
                    }
                    else
                    {
                        Console.WriteLine(
                            $"[ERROR][O24Logging] Remote sink mode enabled but no implementation for ILogSubmitter is registered. Skipping remote sink."
                        );
                    }
                }

                #endregion
            }
        );

        return builder;
    }

    public static IApplicationBuilder UseO24Logging(this IApplicationBuilder app)
    {
        app.UseMiddleware<CorrelationIdMiddleware>();
        app.UseMiddleware<RestApiLoggingMiddleware>();
        return app;
    }
}

public class SinkOptions : IConfig
{
    public string Mode { get; set; } = "hybrid";
    public string Type { get; set; } = "rabbitmq";
    public bool SystemLogToFile { get; set; } = false;
    public int BatchSizeLimit { get; set; } = 50;
    public int PeriodSeconds { get; set; } = 2;
    public int RetainedFileCountLimit { get; set; } = 7;
}
