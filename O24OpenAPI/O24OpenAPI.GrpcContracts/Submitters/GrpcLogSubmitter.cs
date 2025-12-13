using O24OpenAPI.Core.Logging.Abstractions;
using O24OpenAPI.Core.Logging.Extensions;
using O24OpenAPI.GrpcContracts.GrpcClientServices.LOG;
using Serilog.Events;

namespace O24OpenAPI.GrpcContracts.Submitters;

public class GrpcLogSubmitter(ILOGGrpcClientService logGrpcClientService) : ILogSubmitter
{
    private readonly ILOGGrpcClientService _logGrpcClientService = logGrpcClientService;

    public async Task SubmitAsync(IEnumerable<LogEvent> logEvents)
    {
        if (!logEvents.Any())
        {
            return;
        }

        try
        {
            foreach (var logEvent in logEvents)
            {
                var dto = logEvent.ToLogEntryModel();
                await _logGrpcClientService.SubmitLogAsync(dto);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"[FATAL][GrpcLogSubmitter] Failed to submit logs via gRPC. Error: {ex.Message}"
            );
        }
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}
