namespace O24OpenAPI.CTH.API.Application.Constants;

public partial class O24CTHResourceCode
{
    public partial class Operation
    {
        public const string SmartOTPExisting = "operation.smartotp.existing";
        public const string SmartOTPIncorrect = "operation.smartotp.incorrect";
        public const string UnableDecryptSmartOTP = "operation.smartotp.unabledecrypt";
        public const string AccountLockedTemporarily = "operation.account.locked.temporarily";
        public const string HaveLoggged = "operation.user.have.logged";
        public const string InvalidSessionRefresh = "operation.user.invalid.sessionrefresh";
        public const string InvalidSessionAddress = "operation.user.invalid.sessionaddress";
        public const string InvalidSessionStatus = "operation.user.invalid.sessionstatus";
        public const string ChangeDeviceError = "change.device.error";
        public const string SmartOTPNotFound = "operation.smartotp.notfound";
        public const string ChangePasswordError = "operation.change.password.error";
        public const string DeleteOwnUserError = "operation.delete.ownuser.error";
    }
    public partial class Validation
    {
        public const string UsernameIsExisting = "validation.username.isexisting";
        public const string UsernameIsNotExist = "validation.username.notexist";
        public const string PasswordIncorrect = "validation.password.incorrect";
        public const string PasswordDonotSetting = "validation.password.donot.setting";
        public const string AccountStatusInvalid = "validation.account.status.invalid";
        public const string TransacionSmartOTPInvalid = "validation.transaction.smartotp.fail";
        public const string UserNotFoundByRoleId = "validation.user.notfound.byroleid";
        public const string PhoneNumberIsExisting = "validation.user.phonenumber.isexisting";
        public const string PhoneNumberIsNotExisting = "validation.user.phonenumber.isnotexisting";
        public const string PhoneHaveNoRegisterCustomer = "validation.user.phonenumber.haveno.register.customer";
        public const string ContractNumberIsNotExisting = "validation.user.contractnumber.isnotexisting";
        public const string UserNameAndEmailIsRequired = "validation.user.username.email.required";
        public const string UserDeviceNotExist = "validation.user.device.notexist";
    }
}
