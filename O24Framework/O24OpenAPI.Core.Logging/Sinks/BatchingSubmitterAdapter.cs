using O24OpenAPI.Core.Logging.Abstractions;
using Serilog.Events;

namespace Serilog.Sinks.PeriodicBatching;

public class BatchingSubmitterAdapter(ILogSubmitter submitter) : IBatchedLogEventSink
{
    private readonly ILogSubmitter _submitter = submitter;

    public void Dispose()
    {
        _submitter.Dispose();
    }

    public async Task EmitBatchAsync(IEnumerable<LogEvent> batch)
    {
        await _submitter.SubmitAsync(batch);
    }

    public Task OnEmptyBatchAsync()
    {
        return Task.CompletedTask;
    }
}
