using O24OpenAPI.WFO.Lib.Queue;

namespace O24OpenAPI.WFO.Infrastructure;

public static class RegisterQueue
{
    public static void AddQueue(this IApplicationBuilder app)
    {
        _ = QueueContext.Instance;
    }
}
