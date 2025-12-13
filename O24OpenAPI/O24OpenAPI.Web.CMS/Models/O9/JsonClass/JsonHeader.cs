namespace O24OpenAPI.Web.CMS.Models.O9;

/// <summary>
/// Json Header messeges
/// </summary>
public class JsonHeader
{
    /// <summary>
    /// Transaction code
    /// </summary>
    public string TXCODE { get; set; }
    /// <summary>
    /// Transaction date
    /// </summary>
    public string TXDT { get; set; }
    /// <summary>
    /// Transaction number
    /// </summary>
    public string TXREFID { get; set; }
    /// <summary>
    /// working date
    /// </summary>
    public string VALUEDT { get; set; }
    /// <summary>
    /// branch id
    /// </summary>
    public int BRANCHID { get; set; }
    /// <summary>
    /// user id
    /// </summary>
    public int USRID { get; set; }
    /// <summary>
    /// language
    /// </summary>
    public string LANG { get; set; }
    /// <summary>
    /// user ws
    /// </summary>
    public string USRWS { get; set; }
    /// <summary>
    /// user approved
    /// </summary>
    public object APUSER { get; set; }
    /// <summary>
    /// user approved IP
    /// </summary>
    public string APUSRIP { get; set; }
    /// <summary>
    /// user approved ws
    /// </summary>
    public string APUSRWS { get; set; }
    /// <summary>
    /// approved date
    /// </summary>
    public string APDT { get; set; }
    /// <summary>
    /// Transaction status
    /// </summary>
    public string STATUS { get; set; }
    /// <summary>
    /// is reverse: N, Y = reverse
    /// </summary>
    public string ISREVERSE { get; set; }
    /// <summary>
    /// HO branch id
    /// </summary>
    public int? HBRANCHID { get; set; }
    /// <summary>
    /// branch id
    /// </summary>
    public int? RBRANCHID { get; set; }
    /// <summary>
    /// Approved reason
    /// </summary>
    public string APREASON { get; set; }
    /// <summary>
    /// voucher
    /// </summary>
    public string PRN { get; set; }
    /// <summary>
    /// id
    /// </summary>
    public string ID { get; set; }
}


/// <summary>
/// Header Mapping
/// </summary>
public class JsonHeaderMapping
{
    /// <summary>
    /// 
    /// </summary>
    public string A { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string B { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string C { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string D { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int E { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int F { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string G { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string H { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public object I { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string J { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string K { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string L { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string M { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string N { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int? O { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int? P { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string Q { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string R { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string ID { get; set; }
}
