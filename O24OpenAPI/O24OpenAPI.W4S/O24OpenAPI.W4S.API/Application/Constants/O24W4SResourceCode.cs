namespace O24OpenAPI.W4S.API.Application.Constants;

public partial class O24W4SResourceCode
{
    public partial class Validation
    {
        public const string WalletContractNotFound = "validation.walletcontract.notfound";
        public const string WalletContractAlreadyExists = "validation.walletcontract.alreadyexists";
        public const string WalletContractInvalidStatus = "validation.walletcontract.invalidstatus";
        public const string WalletContractCannotBeClosed = "validation.walletcontract.cannotbeclosed";
        public const string ContractPhoneExists = "validation.contract.phone.exists";
        public const string WalletCategoryNotFound = "validation.walletcategory.notfound";
    }
    public partial class Accounting
    {
        public const string AccountNumberIsNotDefined = "account.number.is.not.defined";
        public const string CurrencyIsNotDefined = "currency.code.is.not.defined";
    }
}
