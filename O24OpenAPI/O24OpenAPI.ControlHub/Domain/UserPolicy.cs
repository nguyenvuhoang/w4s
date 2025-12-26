using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.ControlHub.Domain;

public partial class UserPolicy : BaseEntity
{
    public int PolicyCode { get; set; }
    public string ServiceID { get; set; } = string.Empty;
    public string Descr { get; set; }
    public bool? IsDefault { get; set; }
    public DateTime EfFrom { get; set; }
    public DateTime? EfTo { get; set; }
    public int PwdHis { get; set; }
    public int PwdAgeMax { get; set; }
    public int MinPwdLen { get; set; }
    public bool PwdCplx { get; set; }
    public bool PwdCplxLc { get; set; }
    public bool PwdCplxUc { get; set; }
    public bool PwdCplxSc { get; set; }
    public bool PwdCplxSn { get; set; }
    public bool TimeLginRequire { get; set; }
    public string LginFr { get; set; } = string.Empty;
    public string LginTo { get; set; } = string.Empty;
    public string LlkOutThrs { get; set; } = string.Empty;
    public string ResetLkOut { get; set; } = string.Empty;
    public string UserCreate { get; set; }
    public DateTime? DateCreate { get; set; }
    public string UserModify { get; set; }
    public DateTime? DateModify { get; set; }
    public string ContractID { get; set; }
    public bool? IsBankEdit { get; set; } = false;
    public bool? IsCorpEdit { get; set; } = false;
    public int? BaseOnPolicy { get; set; } = 0;
}
