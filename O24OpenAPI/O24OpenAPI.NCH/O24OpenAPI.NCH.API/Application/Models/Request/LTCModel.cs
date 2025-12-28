using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.NCH.Models.Request;

public class LTCModel : BaseTransactionModel
{
    public string Userid { get; set; }
    public string Key { get; set; }
    public string Verson { get; set; }
    public string Msisdn { get; set; }
    public int Type { get; set; }
    public double Amount { get; set; }
    public string HeaderSMS { get; set; }
    public string Message { get; set; }
}
