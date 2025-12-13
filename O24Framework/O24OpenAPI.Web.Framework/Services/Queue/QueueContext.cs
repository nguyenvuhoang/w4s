using O24OpenAPI.Core.Configuration;
using O24OpenAPI.Core.Events;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.O24OpenAPIClient;
using O24OpenAPI.O24OpenAPIClient.Events;
using O24OpenAPI.O24OpenAPIClient.Events.EventData;
using O24OpenAPI.O24OpenAPIClient.Scheme.Workflow;
using O24OpenAPI.Web.Framework.Services.Events;
using System.Runtime.CompilerServices;

namespace O24OpenAPI.Web.Framework.Services.Queue;

/// <summary>
/// The queue context class
/// </summary>
public class QueueContext
{
    /// <summary>
    /// Get the singleton Queue Client
    /// </summary>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public static QueueClient GetInstance()
    {
        if (!Singleton<O24OpenAPIConfiguration>.Instance.ConnectToWFO)
        {
            return null;
        }
        try
        {
            if (Singleton<QueueClient>.Instance != null)
            {
                return Singleton<QueueClient>.Instance;
            }
            QueueClient queueClient = new QueueClient();
            queueClient.WorkflowDelivering += WorkflowDelivering;
            queueClient.WorkflowExecutionEvent += WorkflowEvent;
            queueClient.ServiceToServiceEvent += _ServiceToServiceEvent;
            queueClient.LogEvent += _LogEvent;
            queueClient.EntityEvent += _EntityEvent;
            queueClient.CdcEvent += CdcEvent;
            queueClient.StepEvent += LogWorkflowStepEvent;
            queueClient.AutoConfigure().GetAwaiter().GetResult();
            Singleton<QueueClient>.Instance = queueClient;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Queue client initialization failed: " + ex.Message);
            Singleton<QueueClient>.Instance = null;
        }
        return Singleton<QueueClient>.Instance;
    }

    /// <summary>
    /// Workflows the delivering using the specified workflow
    /// </summary>
    /// <param name="workflow">The workflow</param>
    private static async Task WorkflowDelivering(WFScheme workflow)
    {
        workflow = await O24OpenAPIServiceManager.ConsumeWorkflow1(workflow);
        await Singleton<QueueClient>.Instance.ReplyWorkflow(workflow);
    }

    /// <summary>
    /// Workflows the event using the specified event data
    /// </summary>
    /// <param name="eventData">The event data</param>
    private static async Task WorkflowEvent(
        O24OpenAPIEvent<WorkflowExecutionEventData> eventData
    )
    {
        IEventPublisher eventPublisher = EngineContext.Current.Resolve<IEventPublisher>();
        WorkflowEvent evt = new(eventData);
        await eventPublisher.PublishWorkflowEvent(evt);
    }

    /// <summary>
    /// Services the to service event using the specified event data
    /// </summary>
    /// <param name="eventData">The event data</param>
    private static async Task _ServiceToServiceEvent(
        O24OpenAPIEvent<ServiceToServiceEventData> eventData
    )
    {
        IEventPublisher eventPublisher = EngineContext.Current.Resolve<IEventPublisher>();
        WorkflowServiceToServiceEvent evt = new(eventData);
        await eventPublisher.WorkflowServiceToServiceEventUpdate(evt);
    }

    /// <summary>
    /// Logs the event using the specified event data
    /// </summary>
    /// <param name="eventData">The event data</param>
    private static async Task _LogEvent(O24OpenAPIEvent<LogEventData> eventData)
    {
        IEventPublisher eventPublisher = EngineContext.Current.Resolve<IEventPublisher>();
        LogEvent evt = new(eventData);
        await eventPublisher.LogEventUpdate(evt);
    }

    /// <summary>
    /// Entities the event using the specified e
    /// </summary>
    /// <param name="e">The </param>
    private static async Task _EntityEvent(O24OpenAPIEvent<EntityEvent> e)
    {
        IEventPublisher eventPublisher = EngineContext.Current.Resolve<IEventPublisher>();
        await eventPublisher.EntityEventUpdate(e.EventData.data);
    }

    /// <summary>
    /// Cdcs the event using the specified e
    /// </summary>
    /// <param name="e">The </param>
    private static async Task CdcEvent(O24OpenAPIEvent<CDCEvent> e)
    {
        IEventPublisher eventPublisher = EngineContext.Current.Resolve<IEventPublisher>();
        await eventPublisher.CDCEventUpdate(e.EventData.data);
    }

    private static async Task LogWorkflowStepEvent(
        O24OpenAPIEvent<StepExecutionEvent> eventData
    )
    {
        IEventPublisher eventPublisher = EngineContext.Current.Resolve<IEventPublisher>();
        await eventPublisher.LogWorkflowStepUpdate(eventData.EventData.data);
    }
}
