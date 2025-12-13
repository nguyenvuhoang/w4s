using Jits.Neptune.Web.CMS.LogicOptimal9.Common;
using Newtonsoft.Json.Linq;

namespace O24OpenAPI.Web.CMS.Models.O9;

public class TransactionResponse
{
    /// <summary>
    /// 
    /// </summary>
    public int ERRORCODE { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string ERRORDESC { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public object RESULT { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public void SetCode(CodeDescription code)
    {
        ERRORCODE = code.ERRORCODE;
        ERRORDESC = code.ERRORDESC;
    }

    /// <summary>
    /// 
    /// </summary>
    public void SetResult(JObject value)
    {
        RESULT = value;
    }

    /// <summary>
    /// 
    /// </summary>
    public TransactionResponse()
    {
        ERRORCODE = -1;
        ERRORDESC = "";
        RESULT = new object();
    }
    /// <summary>
    /// 
    /// </summary>
    public TransactionResponse(CodeDescription code)
    {
        ERRORCODE = code.ERRORCODE;
        ERRORDESC = code.ERRORDESC;
        RESULT = new object(); 
    }

    /// <summary>
    /// 
    /// </summary>
    public TransactionResponse(CodeDescription code, JObject jsresult)
    {
        ERRORCODE = code.ERRORCODE;
        ERRORDESC = code.ERRORDESC;
        RESULT = jsresult;
    }
}