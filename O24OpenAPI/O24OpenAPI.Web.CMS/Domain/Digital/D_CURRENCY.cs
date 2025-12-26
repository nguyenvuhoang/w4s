namespace O24OpenAPI.Web.CMS.Domain;

public partial class D_CURRENCY : BaseEntity
{
    public string CurrencyCode { get; set; }

    public string SystemCurrencyCode { get; set; }

    public string CurrencyNumber { get; set; }

    public string CurrencyName { get; set; }

    public string MasterName { get; set; }

    public string Desc { get; set; }

    public int DecimalDigits { get; set; }

    public int RoundingDigit { get; set; }

    public string Status { get; set; }

    public int Order { get; set; }

    public string UserCreated { get; set; }

    public DateTime? DateCreated { get; set; }

    public string UserModified { get; set; }

    public DateTime? LastModified { get; set; }

    public string UserApproved { get; set; }

    public DateTime? DateApproved { get; set; }
}
