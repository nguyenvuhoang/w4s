using FluentMigrator;
using O24OpenAPI.ControlHub.Constant;
using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Core.Configuration;
using O24OpenAPI.Core.Domain.Localization;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.Data;
using O24OpenAPI.Data.Migrations;

namespace O24OpenAPI.ControlHub.Migrations;

/// <summary>
/// The required data migration class
/// </summary>
/// <seealso cref="AutoReversingMigration"/>
[O24OpenAPIMigration(
    "2025/12/09 10:51:01:0000000",
    "Controlhub data required data error code for DeleteOwnUserError",
    MigrationProcessType.Update
)]
[Environment(EnvironmentType.Dev)]
public class RequiredDataMigration(IO24OpenAPIDataProvider dataProvider) : BaseMigration
{
    /// <summary>
    /// The data provider
    /// </summary>
    private readonly IO24OpenAPIDataProvider _dataProvider = dataProvider;

    /// <summary>
    /// Ups this instance
    /// </summary>
    public override void Up()
    {
        this.InstallInitialData();
        //this.InstallSettings();
    }

    /// <summary>
    /// Installs the initial data
    /// </summary>
    protected void InstallInitialData()
    {
        SeedListData(
                [
                    new LocaleStringResource()
                    {
                        Language = "en",
                        ResourceName = O24CTHResourceCode.Operation.SmartOTPIncorrect,
                        ResourceValue = "EMI - SmartOTP PIN code is incorrect. Please check again.",
                    },
                    new LocaleStringResource()
                    {
                        Language = "vi",
                        ResourceName = O24CTHResourceCode.Operation.SmartOTPIncorrect,
                        ResourceValue = "EMI - Mã PIN SmartOTP không đúng. Vui lòng kiểm tra lại.",
                    },
                    new LocaleStringResource()
                    {
                        Language = "lo",
                        ResourceName = O24CTHResourceCode.Operation.SmartOTPIncorrect,
                        ResourceValue =
                            "EMI - ລະຫັດ PIN ຂອງ SmartOTP ບໍ່ຖືກຕ້ອງ. ກະລຸນາກວດເບິ່ງອີກຄັ້ງ.",
                    },
                    new LocaleStringResource()
                    {
                        Language = "zh",
                        ResourceName = O24CTHResourceCode.Operation.SmartOTPIncorrect,
                        ResourceValue = "EMI - SmartOTP 密码错误，请检查后重试。",
                    },
                    new LocaleStringResource()
                    {
                        Language = "en",
                        ResourceName = O24CTHResourceCode.Operation.UnableDecryptSmartOTP,
                        ResourceValue = "Unable decrypt SmartOTP",
                    },
                    new LocaleStringResource()
                    {
                        Language = "vi",
                        ResourceName = O24CTHResourceCode.Operation.UnableDecryptSmartOTP,
                        ResourceValue = "Không thể giải mã SmartOTP.",
                    },
                    new LocaleStringResource()
                    {
                        Language = "lo",
                        ResourceName = O24CTHResourceCode.Operation.UnableDecryptSmartOTP,
                        ResourceValue = "ບໍ່ສາມາດຖອນລະຫັດ SmartOTP ໄດ້.",
                    },
                    new LocaleStringResource()
                    {
                        Language = "zh",
                        ResourceName = O24CTHResourceCode.Operation.UnableDecryptSmartOTP,
                        ResourceValue = "无法解密 SmartOTP。",
                    },
                    new LocaleStringResource()
                    {
                        Language = "en",
                        ResourceName = O24CTHResourceCode.Operation.SmartOTPExisting,
                        ResourceValue =
                            "This user {0} have been registered!. Please use another user",
                    },
                    new LocaleStringResource()
                    {
                        Language = "vi",
                        ResourceName = O24CTHResourceCode.Operation.SmartOTPExisting,
                        ResourceValue =
                            "Người dùng {0} đã được đăng ký! Vui lòng sử dụng người dùng khác.",
                    },
                    new LocaleStringResource()
                    {
                        Language = "lo",
                        ResourceName = O24CTHResourceCode.Operation.SmartOTPExisting,
                        ResourceValue = "ຜູ້ໃຊ້ {0} ໄດ້ລົງທະບຽນແລ້ວ! ກະລຸນາໃຊ້ຜູ້ໃຊ້ອື່ນ.",
                    },
                    new LocaleStringResource()
                    {
                        Language = "zh",
                        ResourceName = O24CTHResourceCode.Operation.SmartOTPExisting,
                        ResourceValue = "用户 {0} 已经注册！请使用其他用户。",
                    },
                    new LocaleStringResource()
                    {
                        Language = "en",
                        ResourceName = O24CTHResourceCode.Validation.UsernameIsNotExist,
                        ResourceValue =
                            "This user {0} have never register this application. Please use another user",
                    },
                    new LocaleStringResource()
                    {
                        Language = "vi",
                        ResourceName = O24CTHResourceCode.Validation.UsernameIsNotExist,
                        ResourceValue =
                            "Người dùng này {0} chưa từng đăng ký ứng dụng này. Vui lòng sử dụng người dùng khác",
                    },
                    new LocaleStringResource()
                    {
                        Language = "lo",
                        ResourceName = O24CTHResourceCode.Validation.UsernameIsNotExist,
                        ResourceValue =
                            "ຜູ້ໃຊ້ນີ້ {0} ບໍ່ເຄີຍລົງທະບຽນແອັບພລິເຄຊັນນີ້. ກະລຸນາໃຊ້ຜູ້ໃຊ້ອື່ນ",
                    },
                    new LocaleStringResource()
                    {
                        Language = "zh",
                        ResourceName = O24CTHResourceCode.Validation.UsernameIsNotExist,
                        ResourceValue = "此用戶 {0} 從未註冊過此應用程式。請使用其他用戶",
                    },
                    new LocaleStringResource()
                    {
                        Language = "en",
                        ResourceName = O24CTHResourceCode.Validation.PasswordIncorrect,
                        ResourceValue =
                            "Invalid password. You have logged in incorrectly {0} times. If you login incorrectly more than 5 times, your account will be temporarily locked.",
                    },
                    new LocaleStringResource()
                    {
                        Language = "vi",
                        ResourceName = O24CTHResourceCode.Validation.PasswordIncorrect,
                        ResourceValue =
                            "Mật khẩu không hợp lệ. Bạn đã đăng nhập sai {0} lần. Nếu bạn đăng nhập sai hơn 5 lần, tài khoản của bạn sẽ bị khóa tạm thời.",
                    },
                    new LocaleStringResource()
                    {
                        Language = "lo",
                        ResourceName = O24CTHResourceCode.Validation.PasswordIncorrect,
                        ResourceValue =
                            "ລະຫັດຜ່ານບໍ່ຖືກຕ້ອງ. ທ່ານເຂົ້າສູ່ລະບົບບໍ່ຖືກຕ້ອງ {0} ເທື່ອ. ຖ້າເຈົ້າເຂົ້າສູ່ລະບົບຜິດຫຼາຍກວ່າ 5 ເທື່ອ, ບັນຊີຂອງທ່ານຈະຖືກລັອກຊົ່ວຄາວ.",
                    },
                    new LocaleStringResource()
                    {
                        Language = "zh",
                        ResourceName = O24CTHResourceCode.Validation.PasswordIncorrect,
                        ResourceValue =
                            "密碼無效。您已錯誤登入{0}次。如果您登入錯誤超過5次，您的帳戶將被暫時鎖定。",
                    },
                    new LocaleStringResource()
                    {
                        Language = "en",
                        ResourceName = O24CTHResourceCode.Validation.PasswordDonotSetting,
                        ResourceValue = "The password have not set on the system yet",
                    },
                    new LocaleStringResource()
                    {
                        Language = "vi",
                        ResourceName = O24CTHResourceCode.Validation.PasswordDonotSetting,
                        ResourceValue = "Mật khẩu chưa được thiết lập trên hệ thống",
                    },
                    new LocaleStringResource()
                    {
                        Language = "lo",
                        ResourceName = O24CTHResourceCode.Validation.PasswordDonotSetting,
                        ResourceValue = "ລະຫັດຜ່ານຍັງບໍ່ໄດ້ຕັ້ງຢູ່ໃນລະບົບເທື່ອ",
                    },
                    new LocaleStringResource()
                    {
                        Language = "zh",
                        ResourceName = O24CTHResourceCode.Validation.PasswordDonotSetting,
                        ResourceValue = "系統尚未設定密碼",
                    },
                    new LocaleStringResource()
                    {
                        Language = "en",
                        ResourceName = O24CTHResourceCode.Validation.AccountStatusInvalid,
                        ResourceValue =
                            "Your account {0} has been disabled. Please contact Admin for resolution.",
                    },
                    new LocaleStringResource()
                    {
                        Language = "vi",
                        ResourceName = O24CTHResourceCode.Validation.AccountStatusInvalid,
                        ResourceValue =
                            "Tài khoản {0} của bạn đã bị vô hiệu hóa. Vui lòng liên hệ với quản trị viên để giải quyết.",
                    },
                    new LocaleStringResource()
                    {
                        Language = "lo",
                        ResourceName = O24CTHResourceCode.Validation.AccountStatusInvalid,
                        ResourceValue =
                            "ບັນຊີ {0} ຂອງທ່ານຖືກປິດການນຳໃຊ້ແລ້ວ. ກະລຸນາຕິດຕໍ່ Admin ສໍາລັບການແກ້ໄຂ.",
                    },
                    new LocaleStringResource()
                    {
                        Language = "zh",
                        ResourceName = O24CTHResourceCode.Validation.AccountStatusInvalid,
                        ResourceValue = "您的帳戶 {0} 已停用。請聯絡管理員尋求解決方案。",
                    },
                    new LocaleStringResource()
                    {
                        Language = "en",
                        ResourceName = O24CTHResourceCode.Operation.AccountLockedTemporarily,
                        ResourceValue =
                            "Your account {0} has been blocked temporarily in {1} minutes.",
                    },
                    new LocaleStringResource()
                    {
                        Language = "vi",
                        ResourceName = O24CTHResourceCode.Operation.AccountLockedTemporarily,
                        ResourceValue = "Tài khoản {0} của bạn đã bị chặn tạm thời trong {1} phút.",
                    },
                    new LocaleStringResource()
                    {
                        Language = "lo",
                        ResourceName = O24CTHResourceCode.Operation.AccountLockedTemporarily,
                        ResourceValue = "ບັນຊີຂອງທ່ານ {0} ໄດ້ຖືກບລັອກຊົ່ວຄາວໃນ {1} ນາທີ.",
                    },
                    new LocaleStringResource()
                    {
                        Language = "zh",
                        ResourceName = O24CTHResourceCode.Operation.AccountLockedTemporarily,
                        ResourceValue = "您的帳戶 {0} 已於 {1} 分鐘內暫時凍結。",
                    },
                    new LocaleStringResource()
                    {
                        Language = "en",
                        ResourceName = O24CTHResourceCode.Operation.HaveLoggged,
                        ResourceValue =
                            "You have already logged in on another device {0} with User: {1}. Please confirm to continue logging in.",
                    },
                    new LocaleStringResource()
                    {
                        Language = "vi",
                        ResourceName = O24CTHResourceCode.Operation.HaveLoggged,
                        ResourceValue =
                            "Bạn đã đăng nhập trên một thiết bị khác {0} với người dùng: {1}. Vui lòng xác nhận để tiếp tục đăng nhập.",
                    },
                    new LocaleStringResource()
                    {
                        Language = "lo",
                        ResourceName = O24CTHResourceCode.Operation.HaveLoggged,
                        ResourceValue =
                            "ທ່ານໄດ້ເຂົ້າສູ່ລະບົບໃນອຸປະກອນອື່ນແລ້ວ {0} ດ້ວຍຜູ້ໃຊ້: {1}. ກະລຸນາຢືນຢັນເພື່ອສືບຕໍ່ເຂົ້າສູ່ລະບົບ.",
                    },
                    new LocaleStringResource()
                    {
                        Language = "zh",
                        ResourceName = O24CTHResourceCode.Operation.HaveLoggged,
                        ResourceValue =
                            "您已使用使用者 {1} 在另一台裝置 {0} 上登入。請確認繼續登入。",
                    },
                    new LocaleStringResource()
                    {
                        Language = "en",
                        ResourceName = O24CTHResourceCode.Operation.InvalidSessionRefresh,
                        ResourceValue = "Your user {0} have invalid session refresh",
                    },
                    new LocaleStringResource()
                    {
                        Language = "vi",
                        ResourceName = O24CTHResourceCode.Operation.InvalidSessionRefresh,
                        ResourceValue = "Người dùng của bạn {0} có phiên làm mới không hợp lệ",
                    },
                    new LocaleStringResource()
                    {
                        Language = "lo",
                        ResourceName = O24CTHResourceCode.Operation.InvalidSessionRefresh,
                        ResourceValue =
                            "ຜູ້​ໃຊ້ {0} ຂອງ​ທ່ານ​ມີ​ການ​ໂຫຼດ​ຄືນ​ເຊດ​ຊັນ​ທີ່​ບໍ່​ຖືກ​ຕ້ອງ",
                    },
                    new LocaleStringResource()
                    {
                        Language = "zh",
                        ResourceName = O24CTHResourceCode.Operation.InvalidSessionRefresh,
                        ResourceValue = "您的使用者 {0} 的會話刷新無效",
                    },
                    new LocaleStringResource()
                    {
                        Language = "en",
                        ResourceName = O24CTHResourceCode.Operation.InvalidSessionAddress,
                        ResourceValue = "Your user {0} have invalid session ip address",
                    },
                    new LocaleStringResource()
                    {
                        Language = "vi",
                        ResourceName = O24CTHResourceCode.Operation.InvalidSessionAddress,
                        ResourceValue = "Người dùng của bạn {0} có địa chỉ IP phiên không hợp lệ",
                    },
                    new LocaleStringResource()
                    {
                        Language = "lo",
                        ResourceName = O24CTHResourceCode.Operation.InvalidSessionAddress,
                        ResourceValue = "ຜູ້ໃຊ້ຂອງທ່ານ {0} ມີທີ່ຢູ່ ip ເຊດຊັນບໍ່ຖືກຕ້ອງ",
                    },
                    new LocaleStringResource()
                    {
                        Language = "zh",
                        ResourceName = O24CTHResourceCode.Operation.InvalidSessionAddress,
                        ResourceValue = "您的使用者 {0} 的會話 IP 位址無效",
                    },
                    new LocaleStringResource()
                    {
                        Language = "en",
                        ResourceName = O24CTHResourceCode.Operation.InvalidSessionStatus,
                        ResourceValue = "Your user {0} have been revoked session already",
                    },
                    new LocaleStringResource()
                    {
                        Language = "vi",
                        ResourceName = O24CTHResourceCode.Operation.InvalidSessionStatus,
                        ResourceValue = "Người dùng {0} của bạn đã bị thu hồi phiên làm việc rồi",
                    },
                    new LocaleStringResource()
                    {
                        Language = "lo",
                        ResourceName = O24CTHResourceCode.Operation.InvalidSessionStatus,
                        ResourceValue = "ຜູ້ໃຊ້ຂອງທ່ານ {0} ໄດ້ຖືກຖອນເຊດຊັນແລ້ວ",
                    },
                    new LocaleStringResource()
                    {
                        Language = "zh",
                        ResourceName = O24CTHResourceCode.Operation.InvalidSessionStatus,
                        ResourceValue = "您的使用者 {0} 的會話已撤銷",
                    },
                    new LocaleStringResource()
                    {
                        Language = "en",
                        ResourceName = O24CTHResourceCode.Validation.UsernameIsExisting,
                        ResourceValue = "Your user {0} have been existed already",
                    },
                    new LocaleStringResource()
                    {
                        Language = "vi",
                        ResourceName = O24CTHResourceCode.Validation.UsernameIsExisting,
                        ResourceValue = "Người dùng {0} của bạn đã tồn tại",
                    },
                    new LocaleStringResource()
                    {
                        Language = "lo",
                        ResourceName = O24CTHResourceCode.Validation.UsernameIsExisting,
                        ResourceValue = "ຜູ້ໃຊ້ຂອງທ່ານ {0} ມີຢູ່ແລ້ວ",
                    },
                    new LocaleStringResource()
                    {
                        Language = "zh",
                        ResourceName = O24CTHResourceCode.Validation.UsernameIsExisting,
                        ResourceValue = "您的使用者 {0} 已存在",
                    },
                    new LocaleStringResource()
                    {
                        Language = "en",
                        ResourceName = O24CTHResourceCode.Validation.UserNotFoundByRoleId,
                        ResourceValue = "User not found by role id {0}",
                    },
                    new LocaleStringResource()
                    {
                        Language = "vi",
                        ResourceName = O24CTHResourceCode.Validation.UserNotFoundByRoleId,
                        ResourceValue = "Không tìm thấy người dùng theo id vai trò {0}",
                    },
                    new LocaleStringResource()
                    {
                        Language = "lo",
                        ResourceName = O24CTHResourceCode.Validation.UserNotFoundByRoleId,
                        ResourceValue = "ບໍ່ພົບຜູ້ໃຊ້ຕາມ ID ບົດບາດ {0}",
                    },
                    new LocaleStringResource()
                    {
                        Language = "zh",
                        ResourceName = O24CTHResourceCode.Validation.UserNotFoundByRoleId,
                        ResourceValue = "未找到角色 ID {0} 的使用者",
                    },
                    new LocaleStringResource()
                    {
                        Language = "en",
                        ResourceName = O24CTHResourceCode.Validation.PhoneNumberIsExisting,
                        ResourceValue =
                            "The phone number {0} is currently associated with an existing user.",
                    },
                    new LocaleStringResource()
                    {
                        Language = "vi",
                        ResourceName = O24CTHResourceCode.Validation.PhoneNumberIsExisting,
                        ResourceValue =
                            "Số điện thoại {0} hiện đang được liên kết với một người dùng đã tồn tại.",
                    },
                    new LocaleStringResource()
                    {
                        Language = "lo",
                        ResourceName = O24CTHResourceCode.Validation.PhoneNumberIsExisting,
                        ResourceValue = "ເບີໂທ {0} ປະຈຸບັນໄດ້ເຊື່ອມຕໍ່ກັບຜູ້ໃຊ້ທີ່ມີຢູ່ແລ້ວ.",
                    },
                    new LocaleStringResource()
                    {
                        Language = "zh",
                        ResourceName = O24CTHResourceCode.Validation.PhoneNumberIsExisting,
                        ResourceValue = "电话号码 {0} 目前与现有用户关联。",
                    },
                    new LocaleStringResource()
                    {
                        Language = "en",
                        ResourceName = O24CTHResourceCode.Validation.PhoneNumberIsNotExisting,
                        ResourceValue = "The phone number {0} does not currently exist",
                    },
                    new LocaleStringResource()
                    {
                        Language = "vi",
                        ResourceName = O24CTHResourceCode.Validation.PhoneNumberIsNotExisting,
                        ResourceValue = "Số điện thoại {0} hiện không tồn tại",
                    },
                    new LocaleStringResource()
                    {
                        Language = "lo",
                        ResourceName = O24CTHResourceCode.Validation.PhoneNumberIsNotExisting,
                        ResourceValue = "ເບີໂທ {0} ປະຈຸບັນບໍ່ມີຢູ່",
                    },
                    new LocaleStringResource()
                    {
                        Language = "zh",
                        ResourceName = O24CTHResourceCode.Validation.PhoneNumberIsNotExisting,
                        ResourceValue = "电话号码 {0} 目前不存在。",
                    },
                    new LocaleStringResource()
                    {
                        Language = "en",
                        ResourceName = O24CTHResourceCode.Validation.PhoneHaveNoRegisterCustomer,
                        ResourceValue =
                            "The phone number {0} is not registered to any customer yet.",
                    },
                    new LocaleStringResource()
                    {
                        Language = "vi",
                        ResourceName = O24CTHResourceCode.Validation.PhoneHaveNoRegisterCustomer,
                        ResourceValue =
                            "Số điện thoại {0} chưa được đăng ký cho bất kỳ khách hàng nào.",
                    },
                    new LocaleStringResource()
                    {
                        Language = "lo",
                        ResourceName = O24CTHResourceCode.Validation.PhoneHaveNoRegisterCustomer,
                        ResourceValue = "ເບີໂທ {0} ບໍ່ໄດ້ຖືກລົງທະບຽນໃນລາຍຊື່ລູກຄ້າໃດໆ.",
                    },
                    new LocaleStringResource()
                    {
                        Language = "zh",
                        ResourceName = O24CTHResourceCode.Validation.PhoneHaveNoRegisterCustomer,
                        ResourceValue = "电话号码 {0} 尚未注册任何客户。",
                    },
                    new LocaleStringResource()
                    {
                        Language = "en",
                        ResourceName = O24CTHResourceCode.Operation.ChangeDeviceError,
                        ResourceValue = "An error occurred while changing the device.",
                    },
                    new LocaleStringResource()
                    {
                        Language = "vi",
                        ResourceName = O24CTHResourceCode.Operation.ChangeDeviceError,
                        ResourceValue = "Đã xảy ra lỗi khi thay đổi thiết bị.",
                    },
                    new LocaleStringResource()
                    {
                        Language = "lo",
                        ResourceName = O24CTHResourceCode.Operation.ChangeDeviceError,
                        ResourceValue = "ເກີດຂໍ້ຜິດພາດໃນຂະບວນການແລ່ນເຄື່ອງ.",
                    },
                    new LocaleStringResource()
                    {
                        Language = "zh",
                        ResourceName = O24CTHResourceCode.Operation.ChangeDeviceError,
                        ResourceValue = "更換設備時發生錯誤。",
                    },
                    new LocaleStringResource()
                    {
                        Language = "en",
                        ResourceName = O24CTHResourceCode.Validation.ContractNumberIsNotExisting,
                        ResourceValue = "The user {0} have not contract number",
                    },
                    new LocaleStringResource()
                    {
                        Language = "vi",
                        ResourceName = O24CTHResourceCode.Validation.ContractNumberIsNotExisting,
                        ResourceValue = "Người dùng {0} không có mã hợp đồng",
                    },
                    new LocaleStringResource()
                    {
                        Language = "lo",
                        ResourceName = O24CTHResourceCode.Validation.ContractNumberIsNotExisting,
                        ResourceValue = "ຜູ້ໃຊ້ {0} ບໍ່ມີເລກສັນຍາ",
                    },
                    new LocaleStringResource()
                    {
                        Language = "zh",
                        ResourceName = O24CTHResourceCode.Validation.ContractNumberIsNotExisting,
                        ResourceValue = "用户 {0} 没有合同编号",
                    },
                    new LocaleStringResource()
                    {
                        Language = "en",
                        ResourceName = O24CTHResourceCode.Validation.UserNameAndEmailIsRequired,
                        ResourceValue = "The username and email is required",
                    },
                    new LocaleStringResource()
                    {
                        Language = "vi",
                        ResourceName = O24CTHResourceCode.Validation.UserNameAndEmailIsRequired,
                        ResourceValue = "Tên người dùng và email là bắt buộc",
                    },
                    new LocaleStringResource()
                    {
                        Language = "lo",
                        ResourceName = O24CTHResourceCode.Validation.UserNameAndEmailIsRequired,
                        ResourceValue = "ຊື່ຜູ້ໃຊ້ ແລະ ອີເມວ ຈະຕ້ອງເປັນຄວາມຈິງ",
                    },
                    new LocaleStringResource()
                    {
                        Language = "zh",
                        ResourceName = O24CTHResourceCode.Validation.UserNameAndEmailIsRequired,
                        ResourceValue = "用户名和电子邮件是必需的",
                    },
                    new LocaleStringResource()
                    {
                        Language = "en",
                        ResourceName = O24CTHResourceCode.Operation.ChangePasswordError,
                        ResourceValue = "Change password error {0}",
                    },
                    new LocaleStringResource()
                    {
                        Language = "vi",
                        ResourceName = O24CTHResourceCode.Operation.ChangePasswordError,
                        ResourceValue = "Lỗi thay đổi mật khẩu {0}",
                    },
                    new LocaleStringResource()
                    {
                        Language = "lo",
                        ResourceName = O24CTHResourceCode.Operation.ChangePasswordError,
                        ResourceValue = "ຂໍ້ຜິດພາດການປ່ຽນລະຫັດຜ່ານ {0}",
                    },
                    new LocaleStringResource()
                    {
                        Language = "zh",
                        ResourceName = O24CTHResourceCode.Operation.ChangePasswordError,
                        ResourceValue = "更改密碼錯誤 {0}",
                    },
                     new LocaleStringResource()
                    {
                        Language = "en",
                        ResourceName = O24CTHResourceCode.Validation.UserDeviceNotExist,
                        ResourceValue = "User device not found {0}",
                    },
                    new LocaleStringResource()
                    {
                        Language = "vi",
                        ResourceName = O24CTHResourceCode.Validation.UserDeviceNotExist,
                        ResourceValue = "Không tìm thấy thiết bị người dùng {0}",
                    },
                    new LocaleStringResource()
                    {
                        Language = "lo",
                        ResourceName = O24CTHResourceCode.Validation.UserDeviceNotExist,
                        ResourceValue = "ບໍ່ພົບອຸປະກອນຜູ້ໃຊ້ {0}",
                    },
                    new LocaleStringResource()
                    {
                        Language = "zh",
                        ResourceName = O24CTHResourceCode.Validation.UserDeviceNotExist,
                        ResourceValue = "未找到使用者裝置 {0}",
                    },
                    new LocaleStringResource()
                    {
                        Language = "en",
                        ResourceName = O24CTHResourceCode.Operation.DeleteOwnUserError,
                        ResourceValue = "Cannot delete your own account",
                    },
                    new LocaleStringResource()
                    {
                        Language = "vi",
                        ResourceName = O24CTHResourceCode.Operation.DeleteOwnUserError,
                        ResourceValue = "Không thể xóa tài khoản của chính bạn",
                    },
                    new LocaleStringResource()
                    {
                        Language = "lo",
                        ResourceName = O24CTHResourceCode.Operation.DeleteOwnUserError,
                        ResourceValue = "ບໍ່ສາມາດລຶບບັນຊີຂອງທ່ານເອງໄດ້",
                    },
                    new LocaleStringResource()
                    {
                        Language = "zh",
                        ResourceName = O24CTHResourceCode.Operation.DeleteOwnUserError,
                        ResourceValue = "无法删除自己的帐户",
                    },

                ],
                [nameof(LocaleStringResource.Language), nameof(LocaleStringResource.ResourceName)]
            )
            .Wait();
    }

    /// <summary>
    /// Installs the settings
    /// </summary>
    protected void InstallSettings()
    {
        this._dataProvider.BulkInsertEntities([new Setting() { Name = "", Value = "" }])
            .GetAsyncResult();
    }
}
