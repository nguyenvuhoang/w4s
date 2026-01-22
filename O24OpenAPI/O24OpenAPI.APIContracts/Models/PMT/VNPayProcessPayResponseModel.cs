namespace O24OpenAPI.APIContracts.Models.PMT
{
    public class VNPayProcessPayResponseModel
    {
        public string PaymentUrl { get; set; } = default!;
        public string TransactionRef { get; set; } = default!;
        public string TransactionDate { get; set; } = default!;
    }
}
