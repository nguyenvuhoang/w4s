namespace O24OpenAPI.APIContracts.Models.NCH;

public class SMSLoanAlertModel
{
    public string? AccountNumber { get; set; }
    public string? AccountName { get; set; }
    public DateTime DueDate { get; set; }
    public decimal TotalPayment { get; set; }
    public string? CurrencyCode { get; set; }
    public string? PhoneNumber { get; set; }
    public string? NotificationType { get; set; }
    public string? Status { get; set; }
    public string? Description { get; set; }
}
