using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.PMT.Domain.AggregatesModel.VNPayAggregate
{
    public class VNPayResponseCodeMap : BaseEntity
    {
        public VNPayResponseCodeMap() { }
        public string ResponseCode { get; set; } = default!;
        public string Description { get; set; } = default!;
    }
}
