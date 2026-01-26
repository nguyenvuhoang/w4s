using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.PMT.Domain.AggregatesModel.VNPayAggregate
{
    public class VNPayTransactionStatusMap : BaseEntity
    {
        public VNPayTransactionStatusMap()
        { }
        public string StatusCode { get; set; } = string.Empty;
        public string StatusMessage { get; set; } = string.Empty;
    }
}
