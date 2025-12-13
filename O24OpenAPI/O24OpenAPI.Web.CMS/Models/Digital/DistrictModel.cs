namespace O24OpenAPI.Web.CMS.Models.Digital;

public class DistrictModel:BaseTransactionModel
{
    public int Id { get; set; } // Thêm thuộc tính Id

    public string DistCode { get; set; } // Thêm thuộc tính DistCode
    public string DistName { get; set; } // Thêm thuộc tính DistName
    public string CityCode { get; set; } // Thêm thuộc tính CityCode
    public string SearchCode { get; set; } // Thêm thuộc tính SearchCode
    public string DistNameMM { get; set; } // Thêm thuộc tính DistNameMM
}
public class DeleteDistrictModel : BaseTransactionModel
{
    public string DistrictCode { get; set; }
    public List<string> ListDistrictCode { get; set; } = new List<string>();
}
