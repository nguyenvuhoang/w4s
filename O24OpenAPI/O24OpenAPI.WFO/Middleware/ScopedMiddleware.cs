namespace O24OpenAPI.WFO.Middleware;

public class ScopedMiddleware
{
    private readonly RequestDelegate _next;

    public ScopedMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        using (var scope = context.RequestServices.CreateScope())
        {
            // Các service scoped có thể được resolve ở đây
            await _next(context);
        }
    }
}
