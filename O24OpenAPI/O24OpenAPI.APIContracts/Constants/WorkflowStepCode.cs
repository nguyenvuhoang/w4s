namespace O24OpenAPI.APIContracts.Constants;

public static class WorkflowStepCode
{
    #region CTH
    public static class CTH
    {
        public const string WF_STEP_BO_GET_USER_BY_ROLE = "WF_STEP_BO_GET_USER_BY_ROLE";
        public const string WF_STEP_CTH_APP_INFO = "WF_STEP_CTH_APP_INFO";
        public const string WF_STEP_CTH_CERATE_USER = "WF_STEP_CTH_CERATE_USER";
        public const string WF_STEP_CTH_CHANGE_DEVICE = "WF_STEP_CTH_CHANGE_DEVICE";
        public const string WF_STEP_CTH_CHANGE_OWNER_PW = "WF_STEP_CTH_CHANGE_OWNER_PW";
        public const string WF_STEP_CTH_CHANGE_PASSWORD = "WF_STEP_CTH_CHANGE_PASSWORD";
        public const string WF_STEP_CTH_CHECK_EMAIL = "WF_STEP_CTH_CHECK_EMAIL";
        public const string WF_STEP_CTH_CHECK_USER_PHONE_NUMBER =
            "WF_STEP_CTH_CHECK_USER_PHONE_NUMBER";
        public const string WF_STEP_CTH_CREATE_ROLE = "WF_STEP_CTH_CREATE_ROLE";
        public const string WF_STEP_CTH_CREATE_USER = "WF_STEP_CTH_CREATE_USER";
        public const string WF_STEP_CTH_DEACTIVE_USER_AUTHEN = "WF_STEP_CTH_DEACTIVE_USER_AUTHEN";
        public const string WF_STEP_CTH_DELETE_USER = "WF_STEP_CTH_DELETE_USER";
        public const string WF_STEP_CTH_GET_USER = "WF_STEP_CTH_GET_USER";
        public const string WF_STEP_CTH_GET_USER_BY_PHONE = "WF_STEP_CTH_GET_USER_BY_PHONE";
        public const string WF_STEP_CTH_GET_USER_STATUS_LOGIN = "WF_STEP_CTH_GET_USER_STATUS_LOGIN";
        public const string WF_STEP_CTH_GET_VISIBLE_TRANS = "WF_STEP_CTH_GET_VISIBLE_TRANS";
        public const string WF_STEP_CTH_LOAD_MENU = "WF_STEP_CTH_LOAD_MENU";
        public const string WF_STEP_CTH_LOAD_OPERATION = "WF_STEP_CTH_LOAD_OPERATION";
        public const string WF_STEP_CTH_LOAD_USERAGREEMENT = "WF_STEP_CTH_LOAD_USERAGREEMENT";
        public const string WF_STEP_CTH_LOGIN = "WF_STEP_CTH_LOGIN";
        public const string WF_STEP_CTH_LOGOUT = "WF_STEP_CTH_LOGOUT";
        public const string WF_STEP_CTH_REFRESH_TOKEN = "WF_STEP_CTH_REFRESH_TOKEN";
        public const string WF_STEP_CTH_REGISTER_USER_AUTHEN = "WF_STEP_CTH_REGISTER_USER_AUTHEN";
        public const string WF_STEP_CTH_RESET_PASSWORD = "WF_STEP_CTH_RESET_PASSWORD";
        public const string WF_STEP_CTH_RETRIEVE_CALENDAR = "WF_STEP_CTH_RETRIEVE_CALENDAR";
        public const string WF_STEP_CTH_SYNC_USER_INFO = "WF_STEP_CTH_SYNC_USER_INFO";
        public const string WF_STEP_CTH_TEST_REVERT = "WF_STEP_CTH_TEST_REVERT";
        public const string WF_STEP_CTH_TRANSITION_USER_STATUS =
            "WF_STEP_CTH_TRANSITION_USER_STATUS";
        public const string WF_STEP_CTH_UNBLOCK_USER = "WF_STEP_CTH_UNBLOCK_USER";
        public const string WF_STEP_CTH_UPDATE_USER = "WF_STEP_CTH_UPDATE_USER";
        public const string WF_STEP_CTH_UPDATE_USER_BANNER = "WF_STEP_CTH_UPDATE_USER_BANNER";
        public const string WF_STEP_CTH_VALIDATE_USER = "WF_STEP_CTH_VALIDATE_USER";
        public const string WF_STEP_CTH_VERIFY_PASSWORD = "WF_STEP_CTH_VERIFY_PASSWORD";
        public const string WF_STEP_CTH_VERIFY_SMARTOTP_PINCODE =
            "WF_STEP_CTH_VERIFY_SMARTOTP_PINCODE";
        public const string WF_STEP_CTH_VERIFY_USER = "WF_STEP_CTH_VERIFY_USER";
        public const string WF_STEP_UMG_REFRESH_TOKEN = "WF_STEP_UMG_REFRESH_TOKEN";
        public const string WF_STEP_CTH_UPDATE_USER_AVATAR = "WF_STEP_CTH_UPDATE_USER_AVATAR";

        //New
        public const string WF_STEP_CTH_CREATE_SADMIN = "WF_STEP_CTH_CREATE_SADMIN";
        public const string WF_STEP_CTH_UPDATE_RIGHT = "WF_STEP_CTH_UPDATE_RIGHT";
        public const string WF_STEP_CTH_GET_USER_BY_ROLE = "WF_STEP_CTH_GET_USER_BY_ROLE";
        public const string WF_STEP_CTH_UPDATE_USER_ROLE_ASSIGNMENT =
            "WF_STEP_CTH_UPDATE_USER_ROLE_ASSIGNMENT";
        public const string WF_STEP_CTH_USER_RESET_PASSWORD = "WF_STEP_CTH_USER_RESET_PASSWORD";
        public const string WF_STEP_CTH_BO_CHANGE_PASSWORD = "WF_STEP_CTH_BO_CHANGE_PASSWORD";
        public const string WF_STEP_CTH_REGISTER_SIGNATURE_KEY =
            "WF_STEP_CTH_REGISTER_SIGNATURE_KEY";
        public const string WF_STEP_CTH_UMG_REFRESH_TOKEN = "WF_STEP_CTH_UMG_REFRESH_TOKEN";
        public const string WF_STEP_CTH_DELETE_USER_CONTRACT = "WF_STEP_CTH_DELETE_USER_CONTRACT";
        public const string WF_STEP_CTH_VERIFICATION_USER_CONTRACT =
            "WF_STEP_CTH_VERIFICATION_USER_CONTRACT";
        public const string WF_STEP_CTH_GET_USER_CODE_BY_CONTRACT_NUMBER =
            "WF_STEP_CTH_GET_USER_CODE_BY_CONTRACT_NUMBER";
        public const string WF_STEP_CTH_VERIFY_TRANSACTION = "WF_STEP_CTH_VERIFY_TRANSACTION";
        public const string WF_STEP_CTH_SNOWFLAKE_TRANSACTIONNUMBER_GENERATOR =
            "WF_STEP_CTH_SNOWFLAKE_TRANSACTIONNUMBER_GENERATOR";
        public const string WF_STEP_CTH_CHECK_WEEKEND = "WF_STEP_CTH_CHECK_WEEKEND";
        public const string WF_STEP_CTH_RETRIEVE_CHANNEL = "WF_STEP_CTH_RETRIEVE_CHANNEL";
        public const string WF_STEP_CTH_UPDATE_CHANNEL_STATUS = "WF_STEP_CTH_UPDATE_CHANNEL_STATUS";
        public const string WF_STEP_CTH_VERIFY_CHANNEL_STATUS = "WF_STEP_CTH_VERIFY_CHANNEL_STATUS";
        public const string WF_STEP_CTH_CHANGE_AVATAR = "WF_STEP_CTH_CHANGE_AVATAR";
    }
    #endregion

    #region NCH
    public static class NCH
    {
        public const string WF_STEP_NCH_SEND_NOTIFICATION = "WF_STEP_NCH_SEND_NOTIFICATION";
        public const string WF_STEP_NCH_PUSH_FIREBASE_NOTIFICATION =
            "WF_STEP_NCH_PUSH_FIREBASE_NOTIFICATION";
        public const string WF_STEP_NCH_SMS_GENERATE_OTP = "WF_STEP_NCH_SMS_GENERATE_OTP";
        public const string WF_STEP_NCH_SMS_VERIFY_OTP = "WF_STEP_NCH_SMS_VERIFY_OTP";
        public const string WF_STEP_NCH_SEND_EMAIL_ASYNC = "WF_STEP_NCH_SEND_EMAIL_ASYNC";
        public const string WF_STEP_NCH_SEARCH_MAIL_CONFIG = "WF_STEP_NCH_SEARCH_MAIL_CONFIG";
        public const string WF_STEP_NCH_UPDATE_MAIL_CONFIG = "WF_STEP_NCH_UPDATE_MAIL_CONFIG";
        public const string WF_STEP_NCH_DELETE_MAIL_CONFIG = "WF_STEP_NCH_DELETE_MAIL_CONFIG";
        public const string WF_STEP_NCH_CREATE_MAIL_CONFIG = "WF_STEP_NCH_CREATE_MAIL_CONFIG";
        public const string WF_STEP_NCH_SEARCH_MAIL_TEMPLATE = "WF_STEP_NCH_SEARCH_MAIL_TEMPLATE";
        public const string WF_STEP_NCH_SEND_TEST_EMAIL_ASYNC = "WF_STEP_NCH_SEND_TEST_EMAIL_ASYNC";
        public const string WF_STEP_NCH_GET_NOTIFICATIONS = "WF_STEP_NCH_GET_NOTIFICATIONS";
        public const string WF_STEP_NCH_SEND_SMS = "WF_STEP_NCH_SEND_SMS";
        public const string WF_STEP_NCH_UPDATE_SMSPROVIDER = "WF_STEP_NCH_UPDATE_SMSPROVIDER";
        public const string WF_STEP_NCH_SMS_ANALSYS = "WF_STEP_NCH_SMS_ANALSYS";
        public const string WF_STEP_NCH_GET_UNREAD_COUNT = "WF_STEP_NCH_GET_UNREAD_COUNT";
        public const string WF_STEP_NCH_SEND_SMS_ASYNC = "WF_STEP_NCH_SEND_SMS_ASYNC";
        public const string WF_STEP_NCH_CREATE_USER_NOTIFICATIONS =
            "WF_STEP_NCH_CREATE_USER_NOTIFICATIONS";
        public const string WF_STEP_NCH_INSERT_SMS_TEMPLATE = "WF_STEP_NCH_INSERT_SMS_TEMPLATE";
        public const string WF_STEP_NCH_UPDATE_SMS_TEMPLATE = "WF_STEP_NCH_UPDATE_SMS_TEMPLATE";
        public const string WF_STEP_NCH_DEVICE_SEND = "WF_STEP_NCH_DEVICE_SEND";
    }
    #endregion

    #region ACT
    public static class ACT
    {
        public const string WF_STEP_ACT_CREATE_ACCOUNTCHART = "WF_STEP_ACT_CREATE_ACCOUNTCHART";
        public const string WF_STEP_ACT_DELETE_ACCOUNTCHART = "WF_STEP_ACT_DELETE_ACCOUNTCHART";
        public const string WF_STEP_ACT_EXECUTE_POSTING = "WF_STEP_ACT_EXECUTE_POSTING";
    }
    #endregion

    #region CMS
    public static class CMS { }
    #endregion

    #region W4S
    public static class W4S
    {
        public const string WF_STEP_W4S_CREATE_WALLET = "WF_STEP_W4S_CREATE_WALLET";
        public const string WF_STEP_W4S_RETRIEVE_WALLET_CATEGORY =
            "WF_STEP_W4S_RETRIEVE_WALLET_CATEGORY";
        public const string WF_STEP_W4S_CREATE_WALLET_BUDGET = "WF_STEP_W4S_CREATE_WALLET_BUDGET";
        public const string WF_STEP_W4S_GET_WALLET_BUDGETS = "WF_STEP_W4S_GET_WALLET_BUDGETS";
    }
    #endregion
}
