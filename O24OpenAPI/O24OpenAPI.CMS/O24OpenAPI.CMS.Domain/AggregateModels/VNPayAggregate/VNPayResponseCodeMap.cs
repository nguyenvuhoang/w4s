namespace O24OpenAPI.CMS.Domain.AggregateModels.VNPayAggregate
{
    public class VNPayResponseCodeMap : BaseEntity
    {
        public VNPayResponseCodeMap() { }
        public string ResponseCode { get; set; } = default!;
        public string Description { get; set; } = default!;
    }
}
