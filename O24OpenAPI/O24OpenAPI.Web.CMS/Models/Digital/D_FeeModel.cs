using System.Text.Json;
using Newtonsoft.Json;

namespace O24OpenAPI.Web.CMS.Models.Digital;

/// <summary>

/// The bankmodel class
/// </summary>
/// <seealso cref="BaseEntity"/>

public class FeeSearchAdvanceModel : BaseTransactionModel
{
    /// <summary>
    /// Deposit Account Search advance model constructor
    /// </summary>
    public FeeSearchAdvanceModel()
    {
        this.PageSize = int.MaxValue;
    }

    /// <summary>
    ///
    /// </summary>
    public string FeeID { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string FeeName { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string FeeType { get; set; }

    /// <summary>
    ///
    /// </summary>
    public decimal? FixAmount { get; set; }

    /// <summary>
    ///
    /// </summary>
    public bool? IsLadder { get; set; }

    /// <summary>
    ///
    /// </summary>
    public bool? IsRegionFee { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string ChargeLater { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string CCYID { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string UserCreated { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string DateCreated { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string UserModified { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string DateModified { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string Branchid { get; set; }

    /// <summary>
    ///
    /// </summary>
    public bool? IsBillPaymentFee { get; set; }
    public int PageSize { get; set; } = int.MaxValue;

    /// <summary>Page index</summary>
    public int PageIndex { get; set; } = 0;
}

/// <summary>

/// The bankmodel class
/// </summary>
/// <seealso cref="BaseEntity"/>

public class FeeSearchSimpleResponseModel : BaseO24OpenAPIModel
{
    /// <summary>
    ///
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("feeid")]
    public string FeeID { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("feename")]
    public string FeeName { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("feetype")]
    public string FeeType { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("fixamount")]
    public decimal FixAmount { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("isladder")]
    public bool IsLadder { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("isregionfee")]
    public bool IsRegionFee { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("chargelater")]
    public string ChargeLater { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("ccyid")]
    public string CCYID { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("usercreated")]
    public string UserCreated { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("datecreated")]
    public DateTime DateCreated { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("usermodified")]
    public string UserModified { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("datemodified")]
    public DateTime DateModified { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("isbillpaymentfee")]
    public bool IsBillPaymentFee { get; set; }

}

/// <summary>

/// The bankmodel class
/// </summary>
/// <seealso cref="BaseEntity"/>

public class FeeSearchAdvanceResponseModel : BaseO24OpenAPIModel
{
    /// <summary>
    ///
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("feeid")]
    public string FeeID { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("feename")]
    public string FeeName { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("feetype")]
    public string FeeType { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("fixamount")]
    public decimal FixAmount { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("isladder")]
    public bool IsLadder { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("isregionfee")]
    public bool IsRegionFee { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("chargelater")]
    public string ChargeLater { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("ccyid")]
    public string CCYID { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("usercreated")]
    public string UserCreated { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("datecreated")]
    public DateTime? DateCreated { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("usermodified")]
    public string UserModified { get; set; }


    /// <summary>
    ///
    /// </summary>
    [JsonProperty("isbillpaymentfee")]
    public bool IsBillPaymentFee { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("datemodified")]
    public DateTime? DateModified { get; set; }

}

public class FeeInsertModel : BaseTransactionModel
{
    /// <summary>
    ///
    /// </summary>
    public FeeInsertModel() { }

    /// <summary>
    ///
    /// </summary>
    ///
    public string FeeID { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string FeeName { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string FeeType { get; set; }

    /// <summary>
    ///
    /// </summary>
    public decimal FixAmount { get; set; }

    /// <summary>
    ///
    /// </summary>
    public bool IsLadder { get; set; }

    /// <summary>
    ///
    /// </summary>
    public bool IsRegionFee { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string ChargeLater { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string CCYID { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string UserCreated { get; set; }

    /// <summary>
    ///
    /// </summary>
    public DateTime? DateCreated { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string UserModified { get; set; }


    /// <summary>
    ///
    /// </summary>
    public bool IsBillPaymentFee { get; set; }
    /// <summary>
    ///
    /// </summary>
    public DateTime? DateModified { get; set; }
}

public class FeeViewModel : BaseO24OpenAPIModel
{
    /// <summary>
    ///
    /// </summary>
    public FeeViewModel() { }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("feeid")]
    public string FeeID { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("feename")]
    public string FeeName { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("feetype")]
    public string FeeType { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("fixamount")]
    public decimal FixAmount { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("isladder")]
    public bool IsLadder { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("isregionfee")]
    public bool IsRegionFee { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("chargelater")]
    public string ChargeLater { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("ccyid")]
    public string CCYID { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("usercreated")]
    public string UserCreated { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("datecreated")]
    public DateTime? DateCreated { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("usermodified")]
    public string UserModified { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("isbillpaymentfee")]
    public bool IsBillPaymentFee { get; set; }
    /// <summary>
    ///
    /// </summary>

    public DateTime? DateModified { get; set; }
}

public class FeeUpdateModel : BaseTransactionModel
{
    /// <summary>
    ///
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string FeeID { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string FeeName { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string FeeType { get; set; }

    /// <summary>
    ///
    /// </summary>
    public decimal FixAmount { get; set; }

    /// <summary>
    ///
    /// </summary>
    public bool IsLadder { get; set; }

    /// <summary>
    ///
    /// </summary>
    public bool IsRegionFee { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string ChargeLater { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string CCYID { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string UserCreated { get; set; }

    /// <summary>
    ///
    /// </summary>
    public DateTime? DateCreated { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string UserModified { get; set; }


    /// <summary>
    ///
    /// </summary>
    public bool IsBillPaymentFee { get; set; }
    /// <summary>
    ///
    /// </summary>
    public DateTime? DateModified { get; set; }
}
