using System.Collections.Concurrent;
using O24OpenAPI.Client;
using O24OpenAPI.Client.Events;
using O24OpenAPI.Client.Events.EventData;
using O24OpenAPI.Client.Scheme.Workflow;
using O24OpenAPI.Core.Helper;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.WFO.Services.Interfaces;

namespace O24OpenAPI.WFO.Lib.Queue;

public class QueueContext : IDisposable
{
    private static readonly object _lock = new();
    private static QueueContext _instance;
    private readonly QueueClient _queueClient;
    private readonly ConcurrentDictionary<string, TaskCompletionSource<WFScheme>> _pendingResponses;

    private QueueContext()
    {
        _queueClient = InitializeQueueClient();
        _pendingResponses = new ConcurrentDictionary<string, TaskCompletionSource<WFScheme>>();
    }

    public static QueueContext Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    _instance ??= new QueueContext();
                }
            }
            return _instance;
        }
    }

    private QueueClient InitializeQueueClient()
    {
        try
        {
            if (Singleton<QueueClient>.Instance != null)
            {
                return Singleton<QueueClient>.Instance;
            }

            var queueClient = new QueueClient();

            queueClient.WorkflowDelivering += HandleWorkflowResponse;
            queueClient.StepEvent += LogWorkflowStepEvent;

            lock (_lock)
            {
                queueClient.AutoConfigure().GetAwaiter().GetResult();
            }
            Singleton<QueueClient>.Instance = queueClient;
            return Singleton<QueueClient>.Instance;
        }
        catch
        {
            return null;
        }
    }

    private static async Task LogWorkflowStepEvent(O24OpenAPIEvent<StepExecutionEvent> eventData)
    {
        var service = EngineContext.Current.Resolve<IWorkflowStepInfoService>();
        await service.UpdateShouldNotAwaitStepInfo(eventData.EventData.data.WFScheme);
    }

    public async Task<WFScheme> SendAndWaitResponse(string queueName, WFScheme workflow)
    {
        if (_queueClient == null)
        {
            throw new InvalidOperationException("QueueClient not initialized");
        }

        var executionId = workflow.request.request_header.execution_id;
        var tcs = new TaskCompletionSource<WFScheme>();

        _pendingResponses.TryAdd(executionId, tcs);

        try
        {
            await _queueClient.SendWorkflow(
                queueName,
                System.Text.Json.JsonSerializer.Serialize(workflow, JsonOptions.IgnoreCycles)
            );

            using var cts = new CancellationTokenSource(
                TimeSpan.FromMilliseconds(workflow.request.request_header.step_timeout)
            );
            return await tcs.Task.WaitAsync(cts.Token);
        }
        finally
        {
            _pendingResponses.TryRemove(executionId, out _);
        }
    }

    public async Task SendWorkflow(string queueName, WFScheme workflow)
    {
        if (_queueClient == null)
        {
            throw new InvalidOperationException("QueueClient not initialized");
        }

        try
        {
            // Gửi message
            await _queueClient.SendWorkflow(
                queueName,
                System.Text.Json.JsonSerializer.Serialize(workflow)
            );
        }
        finally { }
    }

    public async Task SendMessageAsync(string queueName, object message, string messageType)
    {
        if (_queueClient == null)
        {
            throw new InvalidOperationException("QueueClient not initialized");
        }

        try
        {
            await _queueClient.SendMessage(queueName, message, messageType);
        }
        finally { }
    }

    private async Task HandleWorkflowResponse(WFScheme response)
    {
        await Task.CompletedTask;
        var executionId = response.request.request_header.execution_id;

        if (_pendingResponses.TryGetValue(executionId, out var tcs))
        {
            tcs.TrySetResult(response);
        }
    }

    public void Dispose()
    {
        _queueClient?.Dispose();
    }
}
