namespace O24OpenAPI.Web.CMS.Models.Digital;

public interface IRECEIVERLIST
{
    /// <summary>
    /// Gets or sets the value of the code
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Gets or sets the value of the user id
    /// </summary>
    public string UserCode { get; set; }

    /// <summary>
    /// Gets or sets the value of the receiver name
    /// </summary>
    public string ReceiverName { get; set; }

    /// <summary>
    /// Gets or sets the value of the acct no
    /// </summary>
    public string AcctNo { get; set; }

    /// <summary>
    /// Gets or sets the value of the transfer type
    /// </summary>
    public string TransferType { get; set; }

    /// <summary>
    /// Gets or sets the value of the license
    /// </summary>
    public string License { get; set; }

    /// <summary>
    /// Gets or sets the value of the issue place
    /// </summary>
    public string IssuePlace { get; set; }

    /// <summary>
    /// Gets or sets the value of the issue date
    /// </summary>
    public DateTime? IssueDate { get; set; }

    /// <summary>
    /// Gets or sets the value of the description
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the value of the status
    /// </summary>
    public string Status { get; set; }

    /// <summary>
    /// Gets or sets the value of the address
    /// </summary>
    public string Address { get; set; }

    /// <summary>
    /// Gets or sets the value of the city code
    /// </summary>
    public string CityCode { get; set; }

    /// <summary>
    /// Gets or sets the value of the bank id
    /// </summary>
    public string BankId { get; set; }

    /// <summary>
    /// Gets or sets the value of the branch
    /// </summary>
    public string Branch { get; set; }

    /// <summary>
    /// Gets or sets the value of the branch desc
    /// </summary>
    public string BranchDesc { get; set; }
}

public class D_RECEIVERLISTModel : BaseTransactionModel, IRECEIVERLIST
{
    /// <summary>
    /// Gets or sets the value of the code
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Gets or sets the value of the user id
    /// </summary>
    public string UserCode { get; set; }

    /// <summary>
    /// Gets or sets the value of the receiver name
    /// </summary>
    public string ReceiverName { get; set; }

    /// <summary>
    /// Gets or sets the value of the acct no
    /// </summary>
    public string AcctNo { get; set; }

    /// <summary>
    /// Gets or sets the value of the transfer type
    /// </summary>
    public string TransferType { get; set; }

    /// <summary>
    /// Gets or sets the value of the license
    /// </summary>
    public string License { get; set; }

    /// <summary>
    /// Gets or sets the value of the issue place
    /// </summary>
    public string IssuePlace { get; set; }

    /// <summary>
    /// Gets or sets the value of the issue date
    /// </summary>
    public DateTime? IssueDate { get; set; }

    /// <summary>
    /// Gets or sets the value of the description
    /// </summary>
    public new string Description { get; set; }

    /// <summary>
    /// Gets or sets the value of the status
    /// </summary>
    public string Status { get; set; }

    /// <summary>
    /// Gets or sets the value of the address
    /// </summary>
    public string Address { get; set; }

    /// <summary>
    /// Gets or sets the value of the city code
    /// </summary>
    public string CityCode { get; set; }

    /// <summary>
    /// Gets or sets the value of the bank id
    /// </summary>
    public string BankId { get; set; }

    /// <summary>
    /// Gets or sets the value of the branch
    /// </summary>
    public string Branch { get; set; }

    /// <summary>
    /// Gets or sets the value of the branch desc
    /// </summary>
    public string BranchDesc { get; set; }
}
