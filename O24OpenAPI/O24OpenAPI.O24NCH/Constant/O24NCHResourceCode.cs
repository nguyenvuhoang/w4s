namespace O24OpenAPI;

public partial class O24NCHResourceCode
{
    public partial class Validation
    {
        public const string ExistOTP = "validation.invalid.existotp";
        public const string InvalidOTP = "validation.invalid.otp";
        public const string UsedOTP = "validation.invalid.usedotp";
        public const string ExpiredOTP = "validation.invalid.expiredotp";
        public const string FCMTokenIsNotExist = "validation.fcmtoken.isnotexist";
        public const string MailConfigNotExist = "validation.mailconfig.isnotexist";
        public const string MailTemplateNotExist = "validation.mailtemplate.isnotexist";
        public const string MailReceiverNotFound = "validation.mailreceiver.is.notfound";
        public const string SMSProviderIsExisting = "validation.smsprovider.is.existing";
    }
    public partial class Error
    {
        public const string SendEmailFailed = "error.sendemail.failed";
        public const string SendSMSFailed = "error.sendsms.failed";
        public const string GenerateOTPFailed = "error.generateotp.failed";
        public const string VerifyOTPFailed = "error.verifyotp.failed";
    }

}
