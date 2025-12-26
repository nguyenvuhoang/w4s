using Newtonsoft.Json;

namespace O24OpenAPI.Web.CMS.Domain;

public partial class S_USER_POLICY : BaseEntity
{
    [JsonProperty("policyID")]
    public int PolicyID { get; set; } // Thêm thuộc tính PolicyID

    [JsonProperty("serviceID")]
    public string ServiceID { get; set; } // Thêm thuộc tính ServiceID

    [JsonProperty("descr")]
    public string Descr { get; set; } // Thêm thuộc tính Descr

    [JsonProperty("isdefault")]
    public bool IsDefault { get; set; } // Thêm thuộc tính IsDefault

    [JsonProperty("efffrom")]
    public DateTime EffFrom { get; set; } // Thêm thuộc tính EffFrom

    [JsonProperty("efto")]
    public DateTime EffTo { get; set; } // Thêm thuộc tính EffTo

    [JsonProperty("pwdhis")]
    public int PwdHis { get; set; } // Thêm thuộc tính PwdHis

    [JsonProperty("pwdagemax")]
    public int PwdAgeMax { get; set; } // Thêm thuộc tính PwdAgeMax

    [JsonProperty("minpwlen")]
    public int MinPwdLen { get; set; } // Thêm thuộc tính MinPwdLen

    [JsonProperty("pwdcplx")]
    public bool PwdCplx { get; set; } // Thêm thuộc tính PwdCplx

    [JsonProperty("pwdcplxlc")]
    public bool PwdCplxLc { get; set; } // Thêm thuộc tính PwdCplxLc

    [JsonProperty("pwdcplxsc")]
    public bool PwdCplxSc { get; set; } // Thêm thuộc tính PwdCplxSc

    [JsonProperty("pwdcplxsn")]
    public bool PwdCplxSn { get; set; } // Thêm thuộc tính PwdCplxSn

    [JsonProperty("timelgirequire")]
    public bool TimeLineRequire { get; set; } // Thêm thuộc tính TimeLineRequire

    [JsonProperty("lginfr")]
    public string LginFr { get; set; } // Thêm thuộc tính LginFr

    [JsonProperty("lginTo")]
    public string LginTo { get; set; } // Thêm thuộc tính LginTo

    [JsonProperty("ilkoutthrs")]
    public string IlkOutThrs { get; set; } // Thêm thuộc tính IlkOutThrs

    [JsonProperty("resetlkout")]
    public string ResetLkOut { get; set; } // Thêm thuộc tính ResetLkOut

    [JsonProperty("usercreate")]
    public string UserCreate { get; set; } // Thêm thuộc tính UserCreate

    [JsonProperty("datecreate")]
    public DateTime DateCreate { get; set; } // Thêm thuộc tính DateCreate

    [JsonProperty("usermodify")]
    public string UserModify { get; set; } // Thêm thuộc tính UserModify

    [JsonProperty("datemodify")]
    public DateTime DateModify { get; set; } // Thêm thuộc tính DateModify

    [JsonProperty("contractID")]
    public string ContractID { get; set; } // Thêm thuộc tính ContractID

    public bool? IsBankEdit { get; set; }
    public bool? IsCorpEdit { get; set; }
    public bool? BaseOnPolicy { get; set; }
}
