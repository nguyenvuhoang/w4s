using O24OpenAPI.Client;
using O24OpenAPI.Client.Events;
using O24OpenAPI.Client.Events.EventData;
using O24OpenAPI.Client.Scheme.Workflow;
using O24OpenAPI.Core.Helper;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Logging.Helpers;
using O24OpenAPI.WFO.Infrastructure.Abstractions;
using System.Collections.Concurrent;

namespace O24OpenAPI.WFO.Infrastructure.Services.Queue;

public class QueueContext : IDisposable
{
    private static readonly Lock _lock = new();
    private static QueueContext? _instance;
    private readonly QueueClient? _queueClient;
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

    private QueueClient? InitializeQueueClient()
    {
        try
        {
            if (Singleton<QueueClient>.Instance != null)
            {
                return Singleton<QueueClient>.Instance;
            }

            QueueClient queueClient = new();

            queueClient.WorkflowDelivering += HandleWorkflowResponse;
            queueClient.StepEvent += LogWorkflowStepEvent;

            lock (_lock)
            {
                queueClient.AutoConfigure().GetAwaiter().GetResult();
            }
            Singleton<QueueClient>.Instance = queueClient;
            return Singleton<QueueClient>.Instance;
        }
        catch (Exception ex)
        {
            BusinessLogHelper.Error(ex, "Failed to initialize QueueClient");
            return null;
        }
    }

    private static async Task LogWorkflowStepEvent(O24OpenAPIEvent<StepExecutionEvent> eventData)
    {
        IUpdateShouldNotAwaitStepInfoHandler? service =
            EngineContext.Current.Resolve<IUpdateShouldNotAwaitStepInfoHandler>();
        if (service is null)
        {
            BusinessLogHelper.Warning("IUpdateShouldNotAwaitStepInfoHandler is not registried");
        }
        else
        {
            await service.UpdateShouldNotAwaitStepInfo(eventData.EventData.data.WFScheme);
        }
    }

    public async Task<WFScheme> SendAndWaitResponse(string queueName, WFScheme workflow)
    {
        if (_queueClient == null)
        {
            throw new InvalidOperationException("QueueClient not initialized");
        }

        string executionId = workflow.request.request_header.execution_id;
        TaskCompletionSource<WFScheme> tcs = new();

        _pendingResponses.TryAdd(executionId, tcs);

        try
        {
            await _queueClient.SendWorkflow(
                queueName,
                System.Text.Json.JsonSerializer.Serialize(workflow, JsonOptions.IgnoreCycles)
            );

            using CancellationTokenSource cts = new(
                TimeSpan.FromMilliseconds(workflow.request.request_header.step_timeout)
            );
            return await tcs.Task.WaitAsync(cts.Token);
        }
        catch (OperationCanceledException)
        {
            throw new TimeoutException(
                $"Timeout waiting for response for execution ID: {executionId}"
            );
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
        string executionId = response.request.request_header.execution_id;

        if (_pendingResponses.TryGetValue(executionId, out TaskCompletionSource<WFScheme>? tcs))
        {
            tcs.TrySetResult(response);
        }
    }

    public void Dispose()
    {
        _queueClient?.Dispose();
        GC.SuppressFinalize(this);
    }
}
