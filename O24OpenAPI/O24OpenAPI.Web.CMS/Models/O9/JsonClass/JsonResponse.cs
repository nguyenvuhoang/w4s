using Jits.Neptune.Web.CMS.LogicOptimal9.Common;
using Newtonsoft.Json;

namespace O24OpenAPI.Web.CMS.Models.O9;

/// <summary>
/// The json response class
/// </summary>
public class JsonResponse
{
    /// <summary>
    ///
    /// </summary>
    public int R { get; set; }

    /// <summary>
    ///
    /// </summary>
    public object M { get; set; }

    /// <summary>
    ///
    /// </summary>
    public bool IsERROR()
    {
        return R == (int)EnmJsonResponse.E ? true : false;
    }

    /// <summary>
    ///
    /// </summary>
    public bool IsOK()
    {
        return R == (int)EnmJsonResponse.O ? true : false;
    }

    /// <summary>
    ///
    /// </summary>
    public bool IsWARN()
    {
        return R == (int)EnmJsonResponse.W ? true : false;
    }

    /// <summary>
    ///
    /// </summary>
    public string GetMessage()
    {
        if (M != null)
        {
            return M.ToString();
        }

        return string.Empty;
    }
}

public class JsonErrorName
{
    public string E { get; set; }
    public string P { get; set; }
}

public class JsonLoginResponse : JsonErrorName
{
    public string UUID { get; set; }
    public int USRID { get; set; }
    public string USRNAME { get; set; }
    public string STATUS { get; set; }
    public string LANG { get; set; }
    public int BRANCHID { get; set; }
    public string BRANCHCD { get; set; }
    public string BRNAME { get; set; }
    public int PWDCNT { get; set; }
    public string ISONLINE { get; set; }
    public string BUSDATE { get; set; }
    public int DEPRTID { get; set; }
    public string DEPRTCD { get; set; }
    public string LUSRCD { get; set; }
    public string COMCODE { get; set; }
    public string COMTYPE { get; set; }
    public string PWDRESET { get; set; }
    public string WSIP { get; set; }
    public string WSNAME { get; set; }
    public string BANKACTIVE { get; set; }
    public string TOKEN { get; set; }
}

public class LoginO9ResponseModel
{
    [JsonProperty("bankactive")]
    public string bankactive { get; set; }

    [JsonProperty("branchcd")]
    public string branchcd { get; set; }

    [JsonProperty("branchid")]
    public int branchid { get; set; }

    [JsonProperty("brname")]
    public string brname { get; set; }

    [JsonProperty("busdate")]
    public string busdate { get; set; }

    [JsonProperty("comcode")]
    public string comcode { get; set; }

    [JsonProperty("comtype")]
    public string comtype { get; set; }

    [JsonProperty("deprtcd")]
    public string deprtcd { get; set; }

    [JsonProperty("deprtid")]
    public int deprtid { get; set; }

    [JsonProperty("e")]
    public string e { get; set; }

    [JsonProperty("expdt")]
    public string expdt { get; set; }

    [JsonProperty("isonline")]
    public string isonline { get; set; }

    [JsonProperty("isvalidatepolicy")]
    public bool isvalidatepolicy { get; set; }

    [JsonProperty("lang")]
    public string lang { get; set; }

    [JsonProperty("lastdt")]
    public string lastdt { get; set; }

    [JsonProperty("lgname")]
    public string lgname { get; set; }

    [JsonProperty("mac")]
    public string mac { get; set; }

    [JsonProperty("menu")]
    public List<string> menu { get; set; }

    [JsonProperty("menuarc")]
    public List<string> menuarc { get; set; }

    [JsonProperty("minpwdlen")]
    public int minpwdlen { get; set; }

    [JsonProperty("policyid")]
    public int policyid { get; set; }

    [JsonProperty("pwdagemax")]
    public int pwdagemax { get; set; }

    [JsonProperty("pwdagemin")]
    public int pwdagemin { get; set; }

    [JsonProperty("pwdcnt")]
    public int pwdcnt { get; set; }

    [JsonProperty("pwdreset")]
    public string pwdreset { get; set; }

    [JsonProperty("pwdstr")]
    public string pwdstr { get; set; }

    [JsonProperty("roleid")]
    public int roleid { get; set; }

    [JsonProperty("serid")]
    public string serid { get; set; }

    [JsonProperty("status")]
    public string status { get; set; }

    [JsonProperty("txdef")]
    public List<string> txdef { get; set; }

    [JsonProperty("usrac")]
    public Usrac usrac { get; set; }

    [JsonProperty("usrcd")]
    public string usrcd { get; set; }

    [JsonProperty("usrid")]
    public int usrid { get; set; }

    [JsonProperty("usrname")]
    public string usrname { get; set; }

    [JsonProperty("uuid")]
    public string uuid { get; set; }

    [JsonProperty("wsip")]
    public string wsip { get; set; }

    [JsonProperty("wsname")]
    public string wsname { get; set; }

    [JsonProperty("token")]
    public string token { get; set; }
    public string RefreshToken { get; set; }
    public string BranchStatus { get; set; }
}

public class Usrac
{
    [JsonProperty("usrid")]
    public int usrid { get; set; }

    [JsonProperty("usrcd")]
    public string usrcd { get; set; }

    [JsonProperty("urefid")]
    public string urefid { get; set; }

    [JsonProperty("usrname")]
    public string usrname { get; set; }

    [JsonProperty("lgname")]
    public string lgname { get; set; }

    [JsonProperty("branchid")]
    public int branchid { get; set; }

    [JsonProperty("deprtid")]
    public int deprtid { get; set; }

    [JsonProperty("position")]
    public Dictionary<string, int> position { get; set; }

    [JsonProperty("lang")]
    public string lang { get; set; }

    [JsonProperty("phone")]
    public string phone { get; set; }

    [JsonProperty("mphone")]
    public MPhone mphone { get; set; }

    [JsonProperty("status")]
    public string status { get; set; }

    [JsonProperty("pwdchg")]
    public string pwdchg { get; set; }

    [JsonProperty("pwdexp")]
    public string pwdexp { get; set; }

    [JsonProperty("pwdchgr")]
    public string pwdchgr { get; set; }

    [JsonProperty("wsreg")]
    public string wsreg { get; set; }

    [JsonProperty("pwdhis")]
    public int pwdhis { get; set; }

    [JsonProperty("pwdagemax")]
    public int pwdagemax { get; set; }

    [JsonProperty("pwdagemin")]
    public int pwdagemin { get; set; }

    [JsonProperty("minpwdlen")]
    public int minpwdlen { get; set; }

    [JsonProperty("pwdcplx")]
    public string pwdcplx { get; set; }

    [JsonProperty("lginfr")]
    public string lginfr { get; set; }

    [JsonProperty("lginto")]
    public string lginto { get; set; }

    [JsonProperty("timezn")]
    public int timezn { get; set; }

    [JsonProperty("numfmtt")]
    public string numfmtt { get; set; }

    [JsonProperty("numfmtd")]
    public string numfmtd { get; set; }

    [JsonProperty("datefmt")]
    public string datefmt { get; set; }

    [JsonProperty("ldatefmt")]
    public string ldatefmt { get; set; }

    [JsonProperty("timefmt")]
    public string timefmt { get; set; }

    [JsonProperty("lkoutdur")]
    public int lkoutdur { get; set; }

    [JsonProperty("lkoutthrs")]
    public int lkoutthrs { get; set; }

    [JsonProperty("resetlkout")]
    public int resetlkout { get; set; }

    [JsonProperty("policyid")]
    public int policyid { get; set; }

    [JsonProperty("expdt")]
    public string expdt { get; set; }

    [JsonProperty("udfield1")]
    public string udfield1 { get; set; }

    [JsonProperty("iscash")]
    public string iscash { get; set; }

    [JsonProperty("remark")]
    public string remark { get; set; }
}

public class MPhone
{
    [JsonProperty("T")]
    public string T { get; set; }

    [JsonProperty("C")]
    public string C { get; set; }

    [JsonProperty("F")]
    public string F { get; set; }

    [JsonProperty("E")]
    public string E { get; set; }

    [JsonProperty("H")]
    public string H { get; set; }

    [JsonProperty("W")]
    public string W { get; set; }

    [JsonProperty("O")]
    public string O { get; set; }
}
