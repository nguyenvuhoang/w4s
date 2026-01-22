namespace O24OpenAPI.APIContracts.Models.PMT
{
    public class VNPayProcessReturnModel
    {
        public string TransactionStatus { get; set; } = string.Empty;
        public string TransactionRef { get; set; } = default!;
        public string TransactionStatusMessage { get; set; } = default!;
        public string ResponseCodeStatus { get; set; } = default!;
        public string ResponseCodeMessage { get; set; } = default!;
        public string TransactionDate { get; set; } = default!;
    }
}
