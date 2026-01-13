using O24OpenAPI.W4S.Domain.Constants;

namespace O24OpenAPI.W4S.API.Application.Helpers
{
    public static class WalletAccountTypeHelper
    {
        public static int GetSign(string accountType) => accountType switch
        {
            WalletAccountType.Income => 1, // Income
            WalletAccountType.Expense => -1, // Expense
            WalletAccountType.Loan => -1, // Loan
            _ => 0
        };
        public static string GetDisplayName(string accountType) => accountType switch
        {
            WalletAccountType.Income => "Income", // Income
            WalletAccountType.Expense => "Expense", // Expense
            WalletAccountType.Loan => "Loan", // Loan
            _ => "Unknown"
        };
    }
}
