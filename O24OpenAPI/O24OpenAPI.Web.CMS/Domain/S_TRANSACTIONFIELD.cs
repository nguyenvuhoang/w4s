namespace O24OpenAPI.Web.CMS.Domain;

public partial class S_TRANSACTIONFIELD : BaseEntity
{
    public string TransactionCode { get; set; }
    public string TransactionField { get; set; }
    public char FiledType { get; set; }
    public string Caption { get; set; }
    public int? MaxLength { get; set; }
    public int? MinWidth { get; set; }
    public int? Height { get; set; }
    public int? FunctionDecimal { get; set; }
    public string Mask { get; set; }
    public string Format { get; set; }
    public bool? Enable { get; set; }
    public bool? Visible { get; set; }
    public bool? Editable { get; set; }
    public bool? IsRequire { get; set; }
    public bool? Clear { get; set; }
    public bool? Israte { get; set; }
    public string Tag { get; set; }
    public int Index { get; set; }
    public string Limit { get; set; }
    public string InOut { get; set; }
    public char Map { get; set; }
}
