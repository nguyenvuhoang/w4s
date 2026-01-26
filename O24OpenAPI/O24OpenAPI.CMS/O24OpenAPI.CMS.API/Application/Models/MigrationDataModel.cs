using CsvHelper.Configuration.Attributes;

namespace O24OpenAPI.CMS.API.Application.Models;

public class S_USRCMD
{
    [Name("CMDID")]
    public string CMDID { get; set; }

    [Name("PARENTID")]
    public string PARENTID { get; set; }

    [Name("CMDTYPE")]
    public string CMDTYPE { get; set; }

    [Name("CAPTION")]
    public string CAPTION { get; set; }

    [Name("CMDNAME")]
    public string CMDNAME { get; set; }

    [Name("CMDVALUE")]
    public string CMDVALUE { get; set; }

    [Name("ICON")]
    public string ICON { get; set; }

    [Name("CMDLVL")]
    public string CMDLVL { get; set; }

    [Name("ISVISIBLE")]
    public int ISVISIBLE { get; set; }

    [Name("ISENABLE")]
    public int ISENABLE { get; set; }

    [Name("ACTION")]
    public string ACTION { get; set; }

    [Name("CMDIDX")]
    public int CMDIDX { get; set; }

    [Name("STATUS")]
    public string STATUS { get; set; }

    [Name("HKCMD")]
    public string HKCMD { get; set; }

    [Name("URI")]
    public string URI { get; set; }

    [Name("UCTID")]
    public string UCTID { get; set; }
}
