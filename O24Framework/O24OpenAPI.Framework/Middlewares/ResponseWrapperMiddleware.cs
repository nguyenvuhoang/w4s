using System.Diagnostics;
using System.Text.Json;
using LinKit.Json.Runtime;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using O24OpenAPI.Contracts.Models;
using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.Framework.Middlewares;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseResponseWrapperExceptGrpc(this IApplicationBuilder app)
    {
        return app.UseWhen(
            ctx => !ctx.Request.ContentType?.StartsWith("application/grpc") == true,
            appBuilder =>
            {
                appBuilder.UseMiddleware<ResponseWrapperMiddleware>();
            }
        );
    }
}

public sealed class ResponseWrapperMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        var originalBody = context.Response.Body;

        await using MemoryStream memoryStream = new();
        context.Response.Body = memoryStream;
        var executionId = context.RequestServices.GetRequiredService<WorkContext>().ExecutionId;
        context.TraceIdentifier = executionId;
        try
        {
            await _next(context);

            stopwatch.Stop();

            if (!context.Response.ContentType?.Contains("application/json") ?? true)
            {
                memoryStream.Position = 0;
                await memoryStream.CopyToAsync(originalBody);
                return;
            }

            memoryStream.Position = 0;
            var bodyText = await new StreamReader(memoryStream).ReadToEndAsync();

            JsonElement? data = string.IsNullOrWhiteSpace(bodyText)
                ? null
                : bodyText.FromJson<JsonElement>();

            BaseResponse<JsonElement?> response = new()
            {
                Success = context.Response.StatusCode is >= 200 and < 300,
                ExecutionId = context.TraceIdentifier,
                TimeInMilliseconds = stopwatch.ElapsedMilliseconds,
                Status = context.Response.StatusCode.ToString(),
                Data = data,
            };

            context.Response.Body = originalBody;
            context.Response.ContentType = "application/json";

            await JsonSerializer.SerializeAsync(context.Response.Body, response);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();

            context.Response.Body = originalBody;
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status200OK;

            BaseResponse<object> errorResponse = new()
            {
                Success = false,
                ExecutionId = context.TraceIdentifier,
                TimeInMilliseconds = stopwatch.ElapsedMilliseconds,
                Status = "ERROR",
                ErrorMessage = ex.Message,
                Description = "Unhandled exception occurred",
                Data = null,
            };

            await JsonSerializer.SerializeAsync(context.Response.Body, errorResponse);
        }
    }
}
