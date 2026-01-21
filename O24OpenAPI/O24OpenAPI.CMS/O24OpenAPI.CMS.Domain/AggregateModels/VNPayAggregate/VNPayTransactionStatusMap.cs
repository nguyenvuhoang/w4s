namespace O24OpenAPI.CMS.Domain.AggregateModels.VNPayAggregate
{
    public class VNPayTransactionStatusMap : BaseEntity
    {
        public VNPayTransactionStatusMap()
        { }
        public string StatusCode { get; set; } = string.Empty;
        public string StatusMessage { get; set; } = string.Empty;
    }
}
