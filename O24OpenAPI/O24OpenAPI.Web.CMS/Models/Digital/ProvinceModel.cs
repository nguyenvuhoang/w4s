namespace O24OpenAPI.Web.CMS.Models.Digital;

public class ProvinceModel: BaseTransactionModel
{
    public int Id { get; set; } // Thêm thuộc tính ProvinceCode
    public string ProvinceCode { get; set; } // Thêm thuộc tính ProvinceCode
    public string ProvinceName { get; set; } // Thêm thuộc tính ProvinceName
    public string SearchCode { get; set; } // Thêm thuộc tính SearchCode
    public decimal ProvinceId { get; set; } // Thêm thuộc tính ProvinceId
    public string Description { get; set; } // Thêm thuộc tính Description
    public decimal Ord { get; set; } // Thêm thuộc tính Ord
    public decimal RegionId { get; set; } // Thêm thuộc tính RegionId
    public string CountryId { get; set; } // Thêm thuộc tính CountryId
    public string Status { get; set; } // Thêm thuộc tính Status
    public string UserCreated { get; set; } // Thêm thuộc tính UserCreated
    public DateTime DateCreated { get; set; } // Thêm thuộc tính DateCreated
    public string UserModified { get; set; } // Thêm thuộc tính UserModified
    public DateTime? LastModified { get; set; } // Thêm thuộc tính LastModified
    public string UserApproved { get; set; } // Thêm thuộc tính UserApproved
    public DateTime? DateApproved { get; set; } // Thêm thuộc tính DateApproved
    public string ProvinceNameMM { get; set; } // Thêm thuộc tính ProvinceNameMM
}
public class DeleteProvinceModel : BaseTransactionModel
{
    public string ProvinceCode { get; set; }
    public List<string> ListProvinceCode { get; set; } = new List<string>();
}
