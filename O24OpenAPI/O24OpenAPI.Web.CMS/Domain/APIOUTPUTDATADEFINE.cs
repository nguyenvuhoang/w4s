namespace O24OpenAPI.Web.CMS.Domain;

public class APIOUTPUTDATADEFINE : BaseEntity
{
    public string SOURCEID { get; set; }
    public string DESTID { get; set; }
    public string TRANCODE { get; set; }
    public bool ONLINE { get; set; }
    public int FIELDNO { get; set; }
    public string FIELDDESC { get; set; }
    public string FIELDSTYLE { get; set; }
    public string FIELDNAME { get; set; }
    public string VALUESTYLE { get; set; }
    public string VALUENAME { get; set; }
    public string FORMATTYPE { get; set; }
    public string FORMATOBJECT { get; set; }
    public string FORMATFUNCTION { get; set; }
    public string FORMATPARAM { get; set; }
    public string DATASOURCE { get; set; }
    public string DATATABLE { get; set; }
    public string FIELDKEY { get; set; }
}
