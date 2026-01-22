namespace O24OpenAPI.CMS.API.Models.VNPay
{
    public class VNPayProcessPayModel : BaseTransactionModel
    {
        public decimal Amount { get; set; }
        public string TransactionDescription { get; set; } = default!;
        public string LanguageCode { get; set; } = "vn";
        public string VNPayTransactionDate { get; set; }
    }
}
