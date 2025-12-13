using Newtonsoft.Json.Linq;

namespace O24OpenAPI.Web.CMS.Models.O9;

/// <summary>
/// Json Function messeges
/// </summary>
public class JsonFunction
{
    /// <summary>
    /// Transaction code
    /// </summary>
    public string TXCODE { get; set; }

    /// <summary>
    /// Transaction procedure
    /// </summary>
    public string TXPROC { get; set; }

    /// <summary>
    /// Transaction body
    /// </summary>
    public JObject TXBODY { get; set; }

    /// <summary>
    /// Transaction result
    /// </summary>
    public JObject RESULT { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public JsonFunction()
    {
    }

    /// <summary>
    /// Mapping
    /// </summary>
    public JsonFunction(JsonFunctionMapping jsonFunctionMapping)
    {
        if (jsonFunctionMapping != null)
        {
            TXCODE = jsonFunctionMapping.A;
            TXPROC = jsonFunctionMapping.B;
            TXBODY = jsonFunctionMapping.S;
            RESULT = jsonFunctionMapping.V;
        }
    }
}

/// <summary>
/// Json mapping
/// </summary>
public class JsonFunctionMapping
{
    /// <summary>
    /// Transaction code
    /// </summary>
    public string A { get; set; }

    /// <summary>
    /// Transaction procedure
    /// </summary>
    public string B { get; set; }

    /// <summary>
    /// Transaction body
    /// </summary>
    public JObject S { get; set; }

    /// <summary>
    /// Transaction result
    /// </summary>
    public JObject V { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public JsonFunctionMapping()
    {
    }

    /// <summary>
    /// Mapping
    /// </summary>
    public JsonFunctionMapping(JsonFunction jsonFunction)
    {
        if (jsonFunction != null)
        {
            A = jsonFunction.TXCODE;
            B = jsonFunction.TXPROC;
            S = jsonFunction.TXBODY;
            V = jsonFunction.RESULT;
        }
    }
}