namespace O24OpenAPI.Web.CMS.Domain;

public partial class UserPassword : BaseEntity
{
    public string UserCode { get; set; } // Mã người dùng
    public string Password { get; set; } // Mật khẩu
    public DateTime LastLogin { get; set; } // Thời gian đăng nhập cuối
    public int FailureCount { get; set; } // Số lần đăng nhập thất bại
    public string PasswordSalt { get; set; } // Muối cho mật khẩu
    public DateTime UpdatedOnUtc { get; set; } // Thời gian cập nhật (UTC)
    public DateTime CreatedOnUtc { get; set; } // Thời gian tạo (UTC)
}
