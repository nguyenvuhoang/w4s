using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using O24OpenAPI.Contracts.Models;
using O24OpenAPI.Core.Domain;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

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

public sealed class ResponseWrapperMiddleware
{
    private readonly RequestDelegate _next;

    public ResponseWrapperMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.ContentType?.StartsWith("application/grpc") == true)
        {
            await _next(context);
            return;
        }

        Stopwatch stopwatch = Stopwatch.StartNew();
        Stream originalBody = context.Response.Body;

        await using MemoryStream buffer = new();
        context.Response.Body = buffer;

        try
        {
            await _next(context);
            stopwatch.Stop();

            if (context.Response.HasStarted)
            {
                buffer.Position = 0;
                await buffer.CopyToAsync(originalBody);
                return;
            }

            if (context.Response.StatusCode == StatusCodes.Status204NoContent)
            {
                context.Response.Body = originalBody;
                return;
            }

            if (context.Response.ContentType?.Contains("application/json") != true)
            {
                buffer.Position = 0;
                context.Response.Body = originalBody;
                await buffer.CopyToAsync(originalBody);
                return;
            }

            buffer.Position = 0;
            string bodyText = await new StreamReader(buffer, Encoding.UTF8).ReadToEndAsync();

            JsonElement? data = null;

            if (!string.IsNullOrWhiteSpace(bodyText))
            {
                try
                {
                    using JsonDocument doc = JsonDocument.Parse(bodyText);
                    data = doc.RootElement.Clone();
                }
                catch
                {
                    buffer.Position = 0;
                    context.Response.Body = originalBody;
                    await buffer.CopyToAsync(originalBody);
                    return;
                }
            }

            string executionId = context
                .RequestServices.GetRequiredService<WorkContext>()
                .ExecutionId;

            BaseResponse<JsonElement?> response = new()
            {
                Success = context.Response.StatusCode is >= 200 and < 300,
                ExecutionId = executionId,
                TimeInMilliseconds = stopwatch.ElapsedMilliseconds,
                Status = context.Response.StatusCode.ToString(),
                Data = data,
            };

            context.Response.Body = originalBody;
            context.Response.ContentType = "application/json";
            context.Response.ContentLength = null;

            await JsonSerializer.SerializeAsync(context.Response.Body, response);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();

            context.Response.Body = originalBody;

            if (context.Response.HasStarted)
                throw;

            context.Response.Clear();
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            BaseResponse<object> error = new()
            {
                Success = false,
                ExecutionId = context.TraceIdentifier,
                TimeInMilliseconds = stopwatch.ElapsedMilliseconds,
                Status = "ERROR",
                ErrorMessage = ex.Message,
                Description = "Unhandled exception occurred",
            };

            await JsonSerializer.SerializeAsync(context.Response.Body, error);
        }
    }
}
