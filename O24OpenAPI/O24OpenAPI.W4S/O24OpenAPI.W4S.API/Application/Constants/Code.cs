namespace O24OpenAPI.W4S.API.Application.Constants;

public partial class Code
{
    public partial class WalletType
    {
        public const string TWDR = "TWDR";
        public const string TWCR = "TWCR";
        public const string FIAT = "FIAT";
        public const string DEFI = "DEFI";
    }
    public partial class WalletTransactionType
    {
        public const string INCOME = "INCOME";
        public const string EXPENSE = "EXPENSE";
        public const string TRANSFER = "TRANSFER";
    }
    public partial class WalletTransactionCategory
    {
        public const string FOOD = "FOOD";
        public const string TRANSPORT = "TRANSPORT";
        public const string ENTERTAINMENT = "ENTERTAINMENT";
        public const string UTILITIES = "UTILITIES";
        public const string HEALTHCARE = "HEALTHCARE";
        public const string EDUCATION = "EDUCATION";
        public const string SHOPPING = "SHOPPING";
        public const string TRAVEL = "TRAVEL";
        public const string OTHER = "OTHER";
    }
    public partial class Status
    {
        public const string ACTIVE = "A";
        public const string INACTIVE = "I";
        public const string NORMAL = "N";

    }
    public partial class WalletTranCode
    {
        public const string WALLET_OPENING = "WALLET_OPENING";
    }
}
