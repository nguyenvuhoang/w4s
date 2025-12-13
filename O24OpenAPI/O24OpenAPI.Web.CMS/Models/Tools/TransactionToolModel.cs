using CsvHelper.Configuration.Attributes;

namespace O24OpenAPI.Web.CMS.Models.Tools;

public class TXFTagModel : BaseO24OpenAPIModel
{
    [Name("TXCODE")]
    public string TXCODE { get; set; }

    [Name("FTAG")]
    public string FTAG { get; set; }

    [Name("INTYPE")]
    public string INTYPE { get; set; }

    [Name("CAPTION")]
    public string CAPTION { get; set; }

    [Name("MAXLENGT")]
    public int? MAXLENGT { get; set; }

    [Name("MINWIDTH")]
    public int? MINWIDTH { get; set; }

    [Name("HEIGHT")]
    public int? HEIGHT { get; set; }

    [Name("MASK")]
    public string MASK { get; set; }

    [Name("FORMAT")]
    public string FORMAT { get; set; }

    [Name("ENABLE")]
    public int? ENABLE { get; set; }

    [Name("VISIBLE")]
    public int? VISIBLE { get; set; }

    [Name("EDITABLE")]
    public int? EDITABLE { get; set; }

    [Name("REQUIRE")]
    public int? REQUIRE { get; set; }

    [Name("CLEAR")]
    public int? CLEAR { get; set; }

    [Name("ISRATE")]
    public int? ISRATE { get; set; }

    [Name("TOOLTIP")]
    public string TOOLTIP { get; set; }

    [Name("TAG")]
    public string TAG { get; set; }

    [Name("FUN")]
    public string FUN { get; set; }

    [Name("MTDAT")]
    public string MTDAT { get; set; }

    [Name("HELP")]
    public string HELP { get; set; }

    [Name("RNDREF")]
    public string RNDREF { get; set; }

    [Name("FIDX")]
    public int? FIDX { get; set; }

    [Name("LIMIT")]
    public string LIMIT { get; set; }

    [Name("INOUT")]
    public string INOUT { get; set; }

    [Name("ISNULL")]
    public int? ISNULL { get; set; }

    [Name("FMAP")]
    public string FMAP { get; set; }

    [Name("GTXINV")]
    public string GTXINV { get; set; }

    [Name("ISTMPL")]
    public int? ISTMPL { get; set; }
}

public class RuleFunc
{
    [Name("TXCODE")]
    public string TXCODE { get; set; }

    [Name("FUNC")]
    public string FUNC { get; set; }

    [Name("RULE")]
    public string RULE { get; set; }

    [Name("QUERY")]
    public string QUERY { get; set; }

    [Name("ACTF")]
    public string ACTF { get; set; }

    [Name("RELF")]
    public string RELF { get; set; }

    [Name("SEAF")]
    public string SEAF { get; set; }

    [Name("MINVAL")]
    public string MINVAL { get; set; }

    [Name("MAXVAL")]
    public string MAXVAL { get; set; }

    [Name("VALUE")]
    public string VALUE { get; set; }

    [Name("ACTCDT")]
    public string ACTCDT { get; set; }

    [Name("RIDX")]
    public int? RIDX { get; set; }

    [Name("ERRNAME")]
    public string ERRNAME { get; set; }

    [Name("ISOK")]
    public int? ISOK { get; set; }

    [Name("ISCACHED")]
    public int? ISCACHED { get; set; }

    [Name("INUSE")]
    public int? INUSE { get; set; }
}
