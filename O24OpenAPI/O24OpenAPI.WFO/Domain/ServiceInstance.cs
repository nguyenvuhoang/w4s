using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.WFO.Domain;

public partial class ServiceInstance : BaseEntity
{
    public string InstanceID { get; set; }
    public string GrpcUrl { get; set; }
    public long GrpcTimeout { get; set; }
    public string ServiceCode { get; set; }
    public string ServiceHandleName { get; set; }
    public string EventQueueName { get; set; }
    public string CommandQueueName { get; set; }
    public long ConcurrentLimit { get; set; } = short.MaxValue;
    public string AssemblyName { get; set; }
    public string Status { get; set; } = "A";

    public void SetServiceInstance(ServiceInstance serviceInstance)
    {
        InstanceID = serviceInstance.InstanceID;
        GrpcUrl = serviceInstance.GrpcUrl;
        GrpcTimeout = serviceInstance.GrpcTimeout;
        ServiceCode = serviceInstance.ServiceCode;
        ServiceHandleName = serviceInstance.ServiceHandleName;
        EventQueueName = serviceInstance.EventQueueName;
        CommandQueueName = serviceInstance.CommandQueueName;
        ConcurrentLimit = serviceInstance.ConcurrentLimit;
        AssemblyName = serviceInstance.AssemblyName;
        Status = serviceInstance.Status;
    }
}
