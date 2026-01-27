using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Yarp.ReverseProxy.Transforms;

namespace O24OpenAPI.CMS.Infrastructure.Extensions;

public static class YarpConfiguration
{
    public static IServiceCollection AddYarpWithTransforms(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services
            .AddReverseProxy()
            .LoadFromConfig(configuration.GetSection("ReverseProxy"))
            .ConfigureHttpClient((context, handler) =>
            {
                handler.SslOptions.RemoteCertificateValidationCallback =
                    (_, _, _, _) => true;
            })
            .AddTransforms(builderContext =>
            {
                builderContext.AddRequestTransform(context =>
                {
                    if (context.Path.StartsWithSegments("/api/chat"))
                    {
                        IHttpResponseBodyFeature? bufferingFeature =
                            context.HttpContext.Features.Get<IHttpResponseBodyFeature>();
                        bufferingFeature?.DisableBuffering();

                        context.ProxyRequest.Headers.TryAddWithoutValidation(
                            "Accept",
                            "text/event-stream"
                        );
                    }

                    return ValueTask.CompletedTask;
                });

                builderContext.AddResponseTransform(context =>
                {
                    if (context.HttpContext.Request.Path.StartsWithSegments("/api/chat"))
                    {
                        IHttpResponseBodyFeature? bufferingFeature =
                            context.HttpContext.Features.Get<IHttpResponseBodyFeature>();
                        bufferingFeature?.DisableBuffering();
                    }

                    return ValueTask.CompletedTask;
                });
            });

        return services;
    }
}
