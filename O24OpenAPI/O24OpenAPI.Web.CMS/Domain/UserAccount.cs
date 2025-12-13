namespace O24OpenAPI.Web.CMS.Domain;

public class UserAccount : BaseEntity
{
    public string UserCode { get; set; } // Mã người dùng
    public string OldUserCode { get; set; } // Mã người dùng cũ
    public string UserName { get; set; } // Tên người dùng
    public string LoginName { get; set; } // Tên đăng nhập
    public string BranchCode { get; set; } // Mã chi nhánh
    public string DepartmentCode { get; set; } // Mã phòng ban
    public string Position { get; set; } // Chức vụ
    public string MainLanguage { get; set; } // Ngôn ngữ chính
    public string UserPhone { get; set; } // Số điện thoại người dùng
    public string PhoneNumber { get; set; } // Số điện thoại
    public string Remark { get; set; } // Ghi chú
    public string UserAccountStatus { get; set; } // Trạng thái tài khoản
    public string UserCanChangePassword { get; set; } // Có thể thay đổi mật khẩu
    public string PasswordNeverExpire { get; set; } // Mật khẩu không bao giờ hết hạn
    public string ChangePasswordWhenLogin { get; set; } // Thay đổi mật khẩu khi đăng nhập
    public long EnforcePasswordHistory { get; set; } // Lịch sử mật khẩu
    public long MaximumPasswordAge { get; set; } // Tuổi thọ tối đa của mật khẩu
    public long MinimumPasswordAge { get; set; } // Tuổi thọ tối thiểu của mật khẩu
    public long MinimumPasswordLength { get; set; } // Độ dài tối thiểu của mật khẩu
    public string PasswordComplexityRequirements { get; set; } // Yêu cầu độ phức tạp mật khẩu
    public decimal TimeZone { get; set; } // Múi giờ
    public string ThousandSeparateCharacter { get; set; } // Ký tự phân tách hàng nghìn
    public string DecimalSeparateCharacter { get; set; } // Ký tự phân tách thập phân
    public string DateFormat { get; set; } // Định dạng ngày
    public string LongDateFormat { get; set; } // Định dạng ngày dài
    public string TimeFormat { get; set; } // Định dạng thời gian
    public long LockoutDur { get; set; } // Thời gian khóa
    public long LockoutTthrs { get; set; } // Số lần khóa
    public long ResetLockout { get; set; } // Đặt lại khóa
    public long PolicyId { get; set; } // ID chính sách
    public DateTime ExpireDate { get; set; } // Ngày hết hạn
    public string Email { get; set; } // Email
    public string IsOnline { get; set; } // Trạng thái trực tuyến
    public string Avatar { get; set; } // Hình đại diện
    public DateTime? UpdatedOnUtc { get; set; } // Thời gian cập nhật (UTC)
    public DateTime? CreatedOnUtc { get; set; } // Thời gian tạo (UTC)
}
