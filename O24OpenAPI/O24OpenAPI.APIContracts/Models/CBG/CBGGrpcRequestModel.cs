namespace O24OpenAPI.APIContracts.Models.CBG;

public class CBGGrpcRequestModel
{
    public bool IsDigital { get; set; } = false;
    public O9Config o9config { get; set; } = new();
    public Dictionary<string, object> data { get; set; }
}
public class O9Config
{
    public string tx_code { get; set; }
    public string function { get; set; }
    public string action_type { get; set; }
    public string table_name { get; set; }
    public string id_field { get; set; }
}
