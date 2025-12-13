namespace O24OpenAPI.Web.CMS.Models.Digital;

/// <summary>
/// The templatetransfermodel class
/// </summary>
/// <seealso cref="BaseTransactionModel"/>
/// <seealso cref="ITEMPLATETRANSFER"/>
public class D_TEMPLATETRANSFERModel : BaseTransactionModel, ITEMPLATETRANSFER
{
    /// <summary>
    /// Gets or sets the value of the application code
    /// </summary>
    public string ApplicationCode { get; set; }

    /// <summary>
    /// Gets or sets the value of the template code
    /// </summary>
    public string TemplateCode { get; set; }

    /// <summary>
    /// Gets or sets the value of the template name
    /// </summary>
    public string TemplateName { get; set; }

    /// <summary>
    /// Gets or sets the value of the sender account
    /// </summary>
    public string SenderAccount { get; set; }

    /// <summary>
    /// Gets or sets the value of the receiver account
    /// </summary>
    public string ReceiverAccount { get; set; }

    /// <summary>
    /// Gets or sets the value of the amount
    /// </summary>
    public float Amount { get; set; }

    /// <summary>
    /// Gets or sets the value of the currency code
    /// </summary>
    public string CurrencyCode { get; set; }

    /// <summary>
    /// Gets or sets the value of the user code
    /// </summary>
    public string UserCode { get; set; }

    /// <summary>
    /// Gets or sets the value of the execute now
    /// </summary>
    public string ExecuteNow { get; set; }

    /// <summary>
    /// Gets or sets the value of the execute date
    /// </summary>
    public DateTime? ExecuteDate { get; set; }

    /// <summary>
    /// Gets or sets the value of the charge fee
    /// </summary>
    public string ChargeFee { get; set; }

    /// <summary>
    /// Gets or sets the value of the city code
    /// </summary>
    public string CityCode { get; set; }

    /// <summary>
    /// Gets or sets the value of the country code
    /// </summary>
    public string CountryCode { get; set; }

    /// <summary>
    /// Gets or sets the value of the identify no
    /// </summary>
    public string IdentifyNo { get; set; }

    /// <summary>
    /// Gets or sets the value of the issue date
    /// </summary>
    public DateTime? IssueDate { get; set; }

    /// <summary>
    /// Gets or sets the value of the issue place
    /// </summary>
    public string IssuePlace { get; set; }

    /// <summary>
    /// Gets or sets the value of the receiver name
    /// </summary>
    public string ReceiverName { get; set; }

    /// <summary>
    /// Gets or sets the value of the bank code
    /// </summary>
    public string BankCode { get; set; }

    /// <summary>
    /// Gets or sets the value of the sender name
    /// </summary>
    public string SenderName { get; set; }

    /// <summary>
    /// Gets or sets the value of the receiver add
    /// </summary>
    public string ReceiverAdd { get; set; }

    /// <summary>
    /// Gets or sets the value of the branch
    /// </summary>
    public string Branch { get; set; }

    /// <summary>
    /// Gets or sets the value of the branch desc
    /// </summary>
    public string BranchDesc { get; set; }

    /// <summary>
    /// Gets or sets the value of the receiver id
    /// </summary>
    public string ReceiverID { get; set; }
}

/// <summary>
/// The itemplatetransfer interface
/// </summary>

public interface ITEMPLATETRANSFER
{
    /// <summary>
    /// Gets or sets the value of the application code
    /// </summary>
    public string ApplicationCode { get; set; }

    /// <summary>
    /// Gets or sets the value of the template code
    /// </summary>
    public string TemplateCode { get; set; }

    /// <summary>
    /// Gets or sets the value of the template name
    /// </summary>
    public string TemplateName { get; set; }

    /// <summary>
    /// Gets or sets the value of the description
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the value of the sender account
    /// </summary>
    public string SenderAccount { get; set; }

    /// <summary>
    /// Gets or sets the value of the receiver account
    /// </summary>
    public string ReceiverAccount { get; set; }

    /// <summary>
    /// Gets or sets the value of the amount
    /// </summary>
    public float Amount { get; set; }

    /// <summary>
    /// Gets or sets the value of the currency code
    /// </summary>
    public string CurrencyCode { get; set; }

    /// <summary>
    /// Gets or sets the value of the transaction code
    /// </summary>
    public string TransactionCode { get; set; }

    /// <summary>
    /// Gets or sets the value of the user code
    /// </summary>
    public string UserCode { get; set; }

    /// <summary>
    /// Gets or sets the value of the execute now
    /// </summary>
    public string ExecuteNow { get; set; }

    /// <summary>
    /// Gets or sets the value of the execute date
    /// </summary>
    public DateTime? ExecuteDate { get; set; }

    /// <summary>
    /// Gets or sets the value of the charge fee
    /// </summary>
    public string ChargeFee { get; set; }

    /// <summary>
    /// Gets or sets the value of the city code
    /// </summary>
    public string CityCode { get; set; }

    /// <summary>
    /// Gets or sets the value of the country code
    /// </summary>
    public string CountryCode { get; set; }

    /// <summary>
    /// Gets or sets the value of the identify no
    /// </summary>
    public string IdentifyNo { get; set; }

    /// <summary>
    /// Gets or sets the value of the issue date
    /// </summary>
    public DateTime? IssueDate { get; set; }

    /// <summary>
    /// Gets or sets the value of the issue place
    /// </summary>
    public string IssuePlace { get; set; }

    /// <summary>
    /// Gets or sets the value of the receiver name
    /// </summary>
    public string ReceiverName { get; set; }

    /// <summary>
    /// Gets or sets the value of the bank code
    /// </summary>
    public string BankCode { get; set; }

    /// <summary>
    /// Gets or sets the value of the sender name
    /// </summary>
    public string SenderName { get; set; }

    /// <summary>
    /// Gets or sets the value of the receiver add
    /// </summary>
    public string ReceiverAdd { get; set; }

    /// <summary>
    /// Gets or sets the value of the branch
    /// </summary>
    public string Branch { get; set; }

    /// <summary>
    /// Gets or sets the value of the branch desc
    /// </summary>
    public string BranchDesc { get; set; }

    /// <summary>
    /// Gets or sets the value of the receiver id
    /// </summary>
    public string ReceiverID { get; set; }
}
