using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.WFO.Domain;

public partial class ServiceDef : BaseEntity
{
    public string ServiceID { get; set; }
    public string ServiceCode { get; set; }
    public string ServiceName { get; set; }
    public string ServiceType { get; set; }
    public string Status { get; set; }
    public string AcceptTime { get; set; }
    public string GrpcStatus { get; set; }
    public long GrpcTimeout { get; set; }
    public string GrpcUrl { get; set; }
    public long ConcurrentLimit { get; set; }
}
