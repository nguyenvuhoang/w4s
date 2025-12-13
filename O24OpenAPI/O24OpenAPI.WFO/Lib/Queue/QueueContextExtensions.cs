namespace O24OpenAPI.WFO.Lib.Queue;

public static class QueueContextExtensions
{
    public static IServiceCollection AddQueueContext(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        _ = QueueContext.Instance;

        return services;
    }
}
