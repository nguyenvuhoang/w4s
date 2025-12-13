using Newtonsoft.Json.Linq;

namespace O24OpenAPI.Web.CMS.Models.O9;

/// <summary>
/// Json Posting
/// </summary>
public class JsonPosting
{
    /// <summary>
    /// Account Number
    /// </summary>
    public JArray ACNO { get; set; }

    /// <summary>
    /// Branch account number
    /// </summary>
    public JArray BACNO { get; set; }

    /// <summary>
    /// Currency code
    /// </summary>
    public JArray CCRCD { get; set; }

    /// <summary>
    /// Action: D = Debit, C=Credit
    /// </summary>
    public JArray ACTION { get; set; }

    /// <summary>
    /// Amount
    /// </summary>
    public JArray AMT { get; set; }

    /// <summary>
    /// Posting group
    /// </summary>
    public JArray ACGRP { get; set; }

    /// <summary>
    /// Posting index
    /// </summary>
    public JArray ACIDX { get; set; }

    /// <summary>
    /// Voucher
    /// </summary>
    public JArray PRN { get; set; }

    /// <summary>
    /// Account name
    /// </summary>
    public JArray ACNAME { get; set; }

    /// <summary>
    ///
    /// </summary>
    public JArray BACNO2 { get; set; }

    /// <summary>
    ///
    /// </summary>
    public JArray BACNAME2 { get; set; }

    /// <summary>
    ///
    /// </summary>
    public JsonPosting() { }

    /// <summary>
    /// Mapping
    /// </summary>
    public JsonPosting(JsonPostingMapping jsonPostingMapping)
    {
        if (jsonPostingMapping != null)
        {
            ACNO = jsonPostingMapping.A;
            BACNO = jsonPostingMapping.B;
            CCRCD = jsonPostingMapping.C;
            ACTION = jsonPostingMapping.D;
            AMT = jsonPostingMapping.E;
            ACGRP = jsonPostingMapping.F;
            ACIDX = jsonPostingMapping.G;
            PRN = jsonPostingMapping.H;
            ACNAME = jsonPostingMapping.I;
            BACNO2 = jsonPostingMapping.J;
            BACNAME2 = jsonPostingMapping.K;
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public JArray ToJArray()
    {
        JArray jsArray = new();
        try
        {
            for (int i = 0; i < this.ACNO.Count; i++)
            {
                JObject js =
                    new()
                    {
                        { "acno", ACNO != null ? this.ACNO[i] : "" },
                        { "bacno", BACNO != null ? this.BACNO[i] : "" },
                        { "ccrcd", CCRCD != null ? this.CCRCD[i] : "" },
                        { "action", ACTION != null ? this.ACTION[i] : "" },
                        { "amt", AMT != null ? this.AMT[i] : "" },
                        { "acgrp", ACGRP != null ? this.ACGRP[i] : "" },
                        { "acidx", ACIDX != null ? this.ACIDX[i] : "" },
                        { "prn", PRN != null ? this.PRN[i] : "" },
                        { "acname", ACNAME != null ? this.ACNAME[i] : "" },
                        { "bacno2", BACNO2 != null ? this.BACNO2[i] : "" },
                        { "bacname2", BACNAME2 != null ? this.BACNAME2[i] : "" },
                    };
                jsArray.Add(js);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.StackTrace);
        }

        return jsArray;
    }
}

/// <summary>
/// Mapping
/// </summary>
public class JsonPostingMapping
{
    /// <summary>
    /// Account Number
    /// </summary>
    public JArray A { get; set; }

    /// <summary>
    /// B Account Number
    /// </summary>
    public JArray B { get; set; }

    /// <summary>
    /// Currency Code
    /// </summary>
    public JArray C { get; set; }

    /// <summary>
    /// Action D=Debit, C= Credit
    /// </summary>
    public JArray D { get; set; }

    /// <summary>
    /// Amount
    /// </summary>
    public JArray E { get; set; }

    /// <summary>
    /// posting group
    /// </summary>
    public JArray F { get; set; }

    /// <summary>
    /// posting index
    /// </summary>
    public JArray G { get; set; }

    /// <summary>
    /// voucher
    /// </summary>
    public JArray H { get; set; }

    /// <summary>
    /// Account name
    /// </summary>
    public JArray I { get; set; }

    /// <summary>
    ///
    /// </summary>
    public JArray J { get; set; }

    /// <summary>
    ///
    /// </summary>
    public JArray K { get; set; }

    /// <summary>
    ///
    /// </summary>
    public JsonPostingMapping() { }

    /// <summary>
    ///
    /// </summary>
    public JsonPostingMapping(JsonPosting jsonPosting)
    {
        if (jsonPosting != null)
        {
            A = jsonPosting.ACNO;
            B = jsonPosting.BACNO;
            C = jsonPosting.CCRCD;
            D = jsonPosting.ACTION;
            E = jsonPosting.AMT;
            F = jsonPosting.ACGRP;
            G = jsonPosting.ACIDX;
            H = jsonPosting.PRN;
            I = jsonPosting.ACNAME;
            J = jsonPosting.BACNO2;
            K = jsonPosting.BACNAME2;
        }
    }
}
