using Newtonsoft.Json;
using System.Text.Json;

namespace O24OpenAPI.Web.CMS.Models.Digital;


public class CardUserSearchResponseModel : BaseO24OpenAPIModel
{
    /// <summary>
    /// SimpleSearchResponseModel
    /// </summary>
    public CardUserSearchResponseModel()
    {
        this.PageSize = int.MaxValue;
    }

    public int Id { get; set; }

    [JsonProperty("usercode")]
    public string UserCode { get; set; }

    [JsonProperty("cardcode")]
    public string CardCode { get; set; }

    [JsonProperty("cardnumber")]
    public string CardNumber { get; set; }

    [JsonProperty("cardholdername")]
    public string CardHolderName { get; set; }

    [JsonProperty("cardlimit")]
    public decimal CardLimit { get; set; }

    [JsonProperty("availablelimit")]
    public decimal AvailableLimit { get; set; }

    [JsonProperty("balance")]

    public decimal Balance { get; set;}

    [JsonProperty("status")]
    public string Status { get; set; }
    [JsonProperty("isprimary")]
    public bool IsPrimary { get; set; }

    [JsonProperty("cardexpirydate")]
    public DateTime CardExpiryDate { get; set; }

    [JsonProperty("linkagedaccount")]
    public string LinkagedAccount { get; set; }

    /// <summary>Page size</summary>
    public int PageSize { get; set; } = int.MaxValue;

    /// <summary>Page index</summary>
    public int PageIndex { get; set; } = 0;

}

public class CardUserAdvancedSearchRequestModel : BaseTransactionModel
{
    /// <summary>
    /// SimpleSearchResponseModel
    /// </summary>
    public CardUserAdvancedSearchRequestModel()
    {
        this.PageSize = int.MaxValue;
    }

    [JsonProperty("usercode")]
    public string UserCode { get; set; }

    [JsonProperty("cardcode")]
    public string CardCode { get; set; }

    [JsonProperty("cardnumber")]
    public string CardNumber { get; set; }

    [JsonProperty("cardholdername")]
    public string CardHolderName { get; set; }

    [JsonProperty("cardlimit")]
    public decimal CardLimit { get; set; }

    [JsonProperty("availablelimit")]
    public decimal AvailableLimit { get; set; }

    [JsonProperty("balance")]

    public decimal Balance { get; set;}

    [JsonProperty("status")]
    public string Status { get; set; }
    [JsonProperty("isprimary")]
    public bool IsPrimary { get; set; }

    [JsonProperty("cardexpirydate")]
    public string CardExpiryDate { get; set; }

    [JsonProperty("linkagedaccount")]
    public string LinkagedAccount { get; set; }

    /// <summary>Page size</summary>
    public int PageSize { get; set; } = int.MaxValue;

    /// <summary>Page index</summary>
    public int PageIndex { get; set; } = 0;

}



public class CardUserInsertModel : BaseTransactionModel
{
    /// <summary>
    /// SimpleSearchResponseModel
    /// </summary>
    public CardUserInsertModel(){}

    [JsonProperty("usercode")]
    public string UserCode { get; set; }

    [JsonProperty("cardcode")]
    public string CardCode { get; set; }

    [JsonProperty("cardnumber")]
    public string CardNumber { get; set; }

    [JsonProperty("cardholdername")]
    public string CardHolderName { get; set; }

    [JsonProperty("cardlimit")]
    public decimal CardLimit { get; set; }

    [JsonProperty("availablelimit")]
    public decimal AvailableLimit { get; set; }

    [JsonProperty("balance")]

    public decimal Balance { get; set;}

    [JsonProperty("status")]
    public string Status { get; set; }
    [JsonProperty("isprimary")]
    public bool IsPrimary { get; set; }

    [JsonProperty("cardexpirydate")]
    public DateTime CardExpiryDate { get; set; }

    [JsonProperty("linkagedaccount")]
    public string LinkagedAccount { get; set; }

}


public class CardUserUpdateModel : BaseTransactionModel
{
    /// <summary>
    /// SimpleSearchResponseModel
    /// </summary>
    public CardUserUpdateModel() { }

    public int Id { get; set; }

    [JsonProperty("cardholdername")]
    public string CardHolderName { get; set; }

    [JsonProperty("cardlimit")]
    public decimal CardLimit { get; set; }

    [JsonProperty("availablelimit")]
    public decimal AvailableLimit { get; set; }

    [JsonProperty("balance")]
    public decimal Balance { get; set; }

    [JsonProperty("status")]
    public string Status { get; set; }

    [JsonProperty("isprimary")]
    public bool IsPrimary { get; set; }

    [JsonProperty("linkagedaccount")]
    public string LinkagedAccount { get; set; }

}

public class CardUserViewResponseModel : BaseO24OpenAPIModel
{
    /// <summary>
    /// SimpleSearchResponseModel
    /// </summary>
    public CardUserViewResponseModel() { }

    public int Id { get; set; }

    [JsonProperty("usercode")]
    public string UserCode { get; set; }

    [JsonProperty("cardcode")]
    public string CardCode { get; set; }

    [JsonProperty("cardnumber")]
    public string CardNumber { get; set; }

    [JsonProperty("cardlogo")]
    public string CardLogo { get; set; }

    [JsonProperty("cardname")]
    public string CardName { get; set; }

    [JsonProperty("cardtype")]
    public string CardType { get; set; }

    [JsonProperty("cardholdername")]
    public string CardHolderName { get; set; }

    [JsonProperty("cardlimit")]
    public decimal CardLimit { get; set; }

    [JsonProperty("availablelimit")]
    public decimal AvailableLimit { get; set; }

    [JsonProperty("balance")]

    public decimal Balance { get; set;}

    [JsonProperty("status")]
    public string Status { get; set; }
    [JsonProperty("isprimary")]
    public bool IsPrimary { get; set; }

    [JsonProperty("cardexpirydate")]
    public DateTime CardExpiryDate { get; set; }

    [JsonProperty("linkagedaccount")]
    public string LinkagedAccount { get; set; }

}


public class GetCardInformationModel : BaseTransactionModel
{
    /// <summary>
    /// SimpleSearchResponseModel
    /// </summary>
    public GetCardInformationModel(){}

    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("cardcode")]
    public string CardCode { get; set; }

    [JsonProperty("cardnumber")]
    public string CardNumber { get; set; }

    [JsonProperty("cardtype")]
    public string CardType { get; set; }

    [JsonProperty("cardlogo")]
    public string CardLogo { get; set; }

    [JsonProperty("cardname")]
    public string CardName { get; set; }

    [JsonProperty("cardexpirydate")]
    public DateTime CardExpiryDate { get; set; }
}

public class ModelWithUserCode : BaseTransactionModel
{
    [JsonProperty("usercode")]
    public string UserCode { get; set; }
}
