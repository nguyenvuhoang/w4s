using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using CsvHelper.TypeConversion;
using Newtonsoft.Json;

namespace O24OpenAPI.CMS.API.Application.Models;

/// <summary>
/// MultiCaption
/// </summary>
public class MultiCaption : BaseO24OpenAPIModel
{
    /// <summary>
    /// Vietnamese
    /// </summary>
    [JsonProperty("vi")]
    public string Vietnamese { get; set; } = null;

    /// <summary>
    /// English
    /// </summary>
    [JsonProperty("en")]
    public string English { get; set; } = null;

    /// <summary>
    /// ThaiLan
    /// </summary>
    [JsonProperty("thai")]
    public string ThaiLand { get; set; } = null;

    /// <summary>
    /// Lao
    /// </summary>
    [JsonProperty("lao")]
    public string Lao { get; set; } = null;

    /// <summary>
    /// Cambodia
    /// </summary>
    [JsonProperty("cam")]
    public string Cambodia { get; set; } = null;

    /// <summary>
    /// Myanmar
    /// </summary>
    [JsonProperty("mya")]
    public string Myanmar { get; set; } = null;
}

/// <summary>
/// codelist create model
/// </summary>
public partial class CodeListInsertModel : BaseTransactionModel
{
    /// <summary>
    /// codelist create model constructor
    /// </summary>
    public CodeListInsertModel() { }

    /// <summary>
    /// Code Id
    /// </summary>
    [JsonProperty("codeid")]
    public string CodeId { get; set; }

    /// <summary>
    /// Code Name
    /// </summary>
    [JsonProperty("codename")]
    public string CodeName { get; set; }

    /// <summary>
    /// caption
    /// </summary>
    [JsonProperty("caption")]
    public string Caption { get; set; }

    /// <summary>
    /// mcaption
    /// </summary>
    [JsonProperty("mcaption")]
    public MultiCaption Mcaption { get; set; }

    /// <summary>
    /// Code Group
    /// </summary>
    [JsonProperty("codegroup")]
    public string CodeGroup { get; set; }

    /// <summary>
    /// Code Index
    /// </summary>
    [JsonProperty("codeindex")]
    public int? CodeIndex { get; set; }

    /// <summary>
    /// Code Value
    /// </summary>
    [JsonProperty("codevalue")]
    public string CodeValue { get; set; }

    /// <summary>
    /// Ftag
    /// </summary>
    [JsonProperty("ftag")]
    public string Ftag { get; set; }

    /// <summary>
    /// Visible
    /// </summary>
    [JsonProperty("visible")]
    public bool Visible { get; set; }
}

/// <summary>
/// codelist search model
/// </summary>
public partial class CodeListAdvancedSearchRequestModel : BaseTransactionModel
{
    /// <summary>
    /// codelist search model constructor
    /// </summary>
    public CodeListAdvancedSearchRequestModel()
    {
        this.PageSize = int.MaxValue;
    }

    /// <summary>
    /// cdid
    /// </summary>
    [JsonProperty("codeid")]
    public string CodeId { get; set; }

    /// <summary>
    /// cdname
    /// </summary>
    [JsonProperty("codename")]
    public string CodeName { get; set; }

    /// <summary>
    /// caption
    /// </summary>
    [JsonProperty("caption")]
    public string Caption { get; set; }

    /// <summary>
    /// cdgrp
    /// </summary>
    [JsonProperty("codegroup")]
    public string CodeGroup { get; set; }

    /// <summary>
    /// cdidxFrom
    /// </summary>
    [JsonProperty("codeindex")]
    public int? CodeIndex { get; set; }

    /// <summary>
    /// ftag
    /// </summary>
    [JsonProperty("ftag")]
    public string Ftag { get; set; }

    /// <summary>
    /// Visible
    /// </summary>
    [JsonProperty("visible")]
    public bool Visible { get; set; }

    /// <summary>
    /// PageIndex
    /// </summary>
    public int PageIndex { get; set; } = 0;

    /// <summary>
    /// PageSize
    /// </summary>
    public int PageSize { get; set; } = int.MaxValue;
}

/// <summary>
/// CodeListSearchResponeModel
/// </summary>
public partial class CodeListSearchResponseModel : BaseO24OpenAPIModel
{
    /// <summary>
    /// code list id
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    /// Code Id
    /// </summary>
    [JsonProperty("codeid")]
    public string CodeId { get; set; }

    /// <summary>
    /// Code Name
    /// </summary>
    [JsonProperty("codename")]
    public string CodeName { get; set; }

    /// <summary>
    /// caption
    /// </summary>
    [JsonProperty("caption")]
    public string Caption { get; set; }

    /// <summary>
    /// Code Group
    /// </summary>
    [JsonProperty("codegroup")]
    public string CodeGroup { get; set; }

    /// <summary>
    /// Code Index
    /// </summary>
    [JsonProperty("codeindex")]
    public int? CodeIndex { get; set; }

    /// <summary>
    /// Code Value
    /// </summary>
    [JsonProperty("codevalue")]
    public string CodeValue { get; set; }

    /// <summary>
    /// Ftag
    /// </summary>
    [JsonProperty("ftag")]
    public string Ftag { get; set; }

    /// <summary>
    /// Visible
    /// </summary>
    [JsonProperty("visible")]
    public bool Visible { get; set; }
}

/// <summary>
/// CodeListViewResponseModel
/// </summary>
public partial class CodeListViewResponseModel : BaseO24OpenAPIModel
{
    /// <summary>
    /// code list id
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    /// Code Id
    /// </summary>
    [JsonProperty("codeid")]
    public string CodeId { get; set; }

    /// <summary>
    /// Code Name
    /// </summary>
    [JsonProperty("codename")]
    public string CodeName { get; set; }

    /// <summary>
    /// caption
    /// </summary>
    [JsonProperty("caption")]
    public string Caption { get; set; }

    /// <summary>
    /// mcaption
    /// </summary>
    [JsonProperty("mcaption")]
    public string MCaption { get; set; }

    /// <summary>
    /// Code Group
    /// </summary>
    [JsonProperty("codegroup")]
    public string CodeGroup { get; set; }

    /// <summary>
    /// Code Index
    /// </summary>
    [JsonProperty("codeindex")]
    public int? CodeIndex { get; set; }

    /// <summary>
    /// Code Value
    /// </summary>
    [JsonProperty("codevalue")]
    public string CodeValue { get; set; }

    /// <summary>
    /// Ftag
    /// </summary>
    [JsonProperty("ftag")]
    public string Ftag { get; set; }

    /// <summary>
    /// Visible
    /// </summary>
    [JsonProperty("visible")]
    public bool Visible { get; set; }
}

/// <summary>
/// codelist update model
/// </summary>
public partial class CodeListUpdateModel : BaseTransactionModel
{
    /// <summary>
    /// codelist update model constructor
    /// </summary>
    public CodeListUpdateModel() { }

    /// <summary>
    /// codelist default id
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    /// Code Id
    /// </summary>
    [JsonProperty("codeid")]
    public string CodeId { get; set; }

    /// <summary>
    /// caption
    /// </summary>
    [JsonProperty("caption")]
    public string Caption { get; set; }

    /// <summary>
    /// mcaption
    /// </summary>
    [JsonProperty("mcaption")]
    public MultiCaption Mcaption { get; set; }

    /// <summary>
    /// Code Index
    /// </summary>
    [JsonProperty("codeindex")]
    public int? CodeIndex { get; set; }

    /// <summary>
    /// Code Value
    /// </summary>
    [JsonProperty("codevalue")]
    public string CodeValue { get; set; }

    /// <summary>
    /// Ftag
    /// </summary>
    [JsonProperty("ftag")]
    public string Ftag { get; set; }

    /// <summary>
    /// Visible
    /// </summary>
    [JsonProperty("visible")]
    public bool Visible { get; set; }
}

public class GetByGroupAndNameRequestModel : BaseTransactionModel
{
    public string CodeGroup { get; set; }

    public string CodeName { get; set; }
}

public class CodeListWithPrimaryKeyModel : BaseTransactionModel
{
    public string CodeGroup { get; set; }

    public string CodeName { get; set; }

    public string CodeId { get; set; }
}

public class CodeListResponseModel : BaseO24OpenAPIModel
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("codeid")]
    public string CodeId { get; set; }

    [JsonProperty("codename")]
    public string CodeName { get; set; }

    [JsonProperty("caption")]
    public string Caption { get; set; }

    [JsonProperty("languagecaption")]
    public string LanguageCaption { get; set; }

    [JsonProperty("codegroup")]
    public string CodeGroup { get; set; }

    [JsonProperty("codeindex")]
    public int? CodeIndex { get; set; }

    [JsonProperty("codevalue")]
    public string CodeValue { get; set; }

    [JsonProperty("ftag")]
    public string Ftag { get; set; }

    [JsonProperty("visible")]
    public bool Visible { get; set; }
}

public class TellerAppCdlist
{
    [Name("CDID")]
    public string CDID { get; set; }

    [Name("CDNAME")]
    public string CDNAME { get; set; }

    [Name("CAPTION")]
    public string CAPTION { get; set; }

    [Name("CDGRP")]
    public string CDGRP { get; set; }

    [Name("CDIDX")]
    [TypeConverter(typeof(CustomIntConverter))] // Sử dụng CustomIntConverter
    public int CDIDX { get; set; }

    [Name("CDVAL")]
    public string CDVAL { get; set; }

    [Name("FTAG")]
    public string FTAG { get; set; }

    [Name("VISIBLE")]
    public int? VISIBLE { get; set; }

    [Name("LAO_LANGUAGE")]
    public string LAO_LANGUAGE { get; set; }
}

public class CodeListWithExcel
{
    public string CDID { get; set; }

    public string CDNAME { get; set; }

    public string CAPTION { get; set; }

    public string CDGRP { get; set; }

    public int CDIDX { get; set; }

    public string CDVAL { get; set; }

    public string FTAG { get; set; }

    public int VISIBLE { get; set; }

    public string LAO_LANGUAGE { get; set; }
}

public class CustomIntConverter : Int32Converter
{
    public override object ConvertFromString(
        string text,
        IReaderRow row,
        MemberMapData memberMapData
    )
    {
        if (!string.IsNullOrWhiteSpace(text))
        {
            text = text.Replace(",", ""); // Xóa dấu phẩy khỏi chuỗi
        }
        return base.ConvertFromString(text, row, memberMapData);
    }
}
