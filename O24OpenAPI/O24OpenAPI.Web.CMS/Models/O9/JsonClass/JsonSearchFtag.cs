using Jits.Neptune.Web.CMS.LogicOptimal9.Common;
using Newtonsoft.Json.Linq;

namespace O24OpenAPI.Web.CMS.Models.O9;

public class JsonSearchFtag
{
    /// <summary>
    ///
    /// </summary>
    public string FTAG { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string FTYPE { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string INPTYPE { get; set; }

    /// <summary>
    ///
    /// </summary>
    public int? MAXLENGTH { get; set; }

    /// <summary>
    ///
    /// </summary>
    public int MINWIDTH { get; set; }

    /// <summary>
    ///
    /// </summary>
    public int HEIGHT { get; set; }

    /// <summary>
    ///
    /// </summary>
    public int FDEC { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string MASK { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string FORMAT { get; set; }

    /// <summary>
    ///
    /// </summary>
    public JObject CAPTION { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string OPRT { get; set; }

    /// <summary>
    ///
    /// </summary>
    public bool VISIBLE { get; set; }

    /// <summary>
    ///
    /// </summary>
    public bool INCDT { get; set; }

    /// <summary>
    ///
    /// </summary>
    public bool INSELECT { get; set; }

    /// <summary>
    ///
    /// </summary>
    public bool ISPK { get; set; }

    /// <summary>
    ///
    /// </summary>
    public bool INMSG { get; set; }

    /// <summary>
    ///
    /// </summary>
    public int ORD { get; set; }

    /// <summary>
    ///
    /// </summary>
    public int FIDX { get; set; }

    /// <summary>
    ///
    /// </summary>
    public object FVAL { get; set; }

    /// <summary>
    /// ///
    /// </summary>
    public string OPRTVAL { get; set; }

    /// <summary>
    ///
    /// </summary>
    public JArray ARRFVAL { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string ORAND { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string FDEFAULT { get; set; }

    /// <summary>
    /// field in select query
    /// </summary>
    public string FSELECT
    {
        get
        {
            if (this.FTYPE.ToUpper().Equals("C"))
            {
                return this.FTAG;
            }
            else if (this.FTYPE.ToUpper().Equals("J"))
            {
                return $"O9UTIL.GET_FLAT_JSON_VALUE({FTAG},'{GlobalVariable.O9_GLOBAL_LANG}') AS {FTAG}";
            }
            else if (this.FTYPE.ToUpper().Equals("V"))
            {
                return "TO_CHAR(" + this.FTAG + "')";
            }
            return this.FTAG;
        }
    }

    /// <summary>
    /// field in where query
    /// </summary>
    public string FWHERE
    {
        get
        {
            if (this.FTYPE.ToUpper().Equals("C"))
            {
                return this.FTAG;
            }
            else if (this.FTYPE.ToUpper().Equals("J"))
            {
                return $"O9UTIL.GET_FLAT_JSON_VALUE({FTAG},'{GlobalVariable.O9_GLOBAL_LANG}')";
            }
            else if (this.FTYPE.ToUpper().Equals("V"))
            {
                return "TO_CHAR(" + this.FTAG + "')";
            }
            return this.FTAG;
        }
    }

    /// <summary>
    /// Description: Add or-and to search ftag
    /// </summary>
    public void AddOrAndToSearchFtag(string strOrAnd = "")
    {
        if (this.INCDT)
        {
            if (string.IsNullOrEmpty(this.ORAND))
            {
                if (!string.IsNullOrEmpty(strOrAnd))
                {
                    this.ORAND = strOrAnd;
                }
                else
                {
                    this.ORAND = O9Constants.O9_CONSTANT_OR;
                }
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    public string GenOrderString(string stringOrder)
    {
        if (this.ORD == 1)
        {
            stringOrder += this.FTAG + O9Constants.O9_CONSTANT_COMMA;
        }
        else if (this.ORD == 2)
        {
            stringOrder += this.FTAG + " DESC" + O9Constants.O9_CONSTANT_COMMA;
        }
        return stringOrder;
    }
}
