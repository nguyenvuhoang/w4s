using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;

[Auditable]
public partial class UserRole : BaseEntity
{
    public int RoleId { get; set; } // ID của vai trò
    public string? RoleName { get; set; } // Tên vai trò
    public string? RoleDescription { get; set; } // Mô tả vai trò
    public string? UserType { get; set; } // Loại người dùng
    public string? ContractNo { get; set; } // Số hợp đồng
    public string? UserCreated { get; set; } // Người tạo
    public DateTime? DateCreated { get; set; } // Ngày tạo
    public string? UserModified { get; set; } // Người sửa đổi
    public DateTime? DateModified { get; set; } // Ngày sửa đổi
    public string? ServiceID { get; set; } // ID dịch vụ
    public string? RoleType { get; set; } // Loại vai trò
    public string? Status { get; set; } // Trạng thái
    public string? IsShow { get; set; } // Hiển thị hay không
    public int SortOrder { get; set; } // Thứ tự
}
