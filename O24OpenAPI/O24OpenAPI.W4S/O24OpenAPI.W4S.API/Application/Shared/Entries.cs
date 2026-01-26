namespace O24OpenAPI.W4S.API.Application.Shared
{
    using O24OpenAPI.W4S.Domain.AggregatesModel.WalletMasterAggregate;

    /// <summary>
    /// Defines the <see cref="Entries" />
    /// </summary>
    public static class Entries
    {
        /// <summary>
        /// The OpeningEntries
        /// </summary>
        /// <param name="transactionId">The transactionId<see cref="string"/></param>
        /// <param name="currency">The currency<see cref="string"/></param>
        /// <param name="amount">The amount<see cref="decimal"/></param>
        /// <param name="postingUtc">The postingUtc<see cref="DateTime"/></param>
        /// <param name="virtualSourceGl">The virtualSourceGl<see cref="string"/></param>
        /// <param name="walletAccountGl">The walletAccountGl<see cref="string"/></param>
        /// <param name="virtualSourceEntryType">The virtualSourceEntryType<see cref="string"/></param>
        /// <param name="walletAccountEntryType">The walletAccountEntryType<see cref="string"/></param>
        /// <returns>The <see cref="List{WalletLedgerEntry}"/></returns>


        public static List<WalletLedgerEntry> OpeningEntries(
            string transactionId,
            string currency,
            decimal amount,
            DateTime postingUtc,
            string virtualSourceGl,
            string walletAccountGl,
            string virtualSourceEntryType,
            string walletAccountEntryType)
        {
            return
            [
                // Debit: Wallet Account (Asset increases)
                new()
                {
                    TRANSACTIONID = transactionId,
                    AccountNumber = walletAccountGl,
                    Currency = currency,
                    Group = 1,
                    Index = 1,
                    DrCr = DrCr.D,
                    Amount = amount,
                    EntryType = walletAccountEntryType,
                    PostingDateUtc = postingUtc
                },

                // Credit: Virtual Source (Funding/Equity)
                new()
                {
                    TRANSACTIONID = transactionId,
                    AccountNumber = virtualSourceGl,
                    Currency = currency,
                    Group = 1,
                    Index = 2,
                    DrCr = DrCr.C,
                    Amount = amount,
                    EntryType = virtualSourceEntryType,
                    PostingDateUtc = postingUtc
                }
            ];
        }


        /// <summary>
        /// The IncomeEntry
        /// </summary>
        /// <param name="transactionId">The transactionId<see cref="string"/></param>
        /// <param name="currency">The currency<see cref="string"/></param>
        /// <param name="amount">The amount<see cref="decimal"/></param>
        /// <param name="postingUtc">The postingUtc<see cref="DateTime"/></param>
        /// <param name="incomeSourceGl">The incomeSourceGl<see cref="string"/></param>
        /// <param name="walletAccountGl">The walletAccountGl<see cref="string"/></param>
        /// <param name="incomeSourceEntryType">The incomeSourceEntryType<see cref="string"/></param>
        /// <param name="walletAccountEntryType">The walletAccountEntryType<see cref="string"/></param>
        /// <returns>The <see cref="List{WalletLedgerEntry}"/></returns>
        public static List<WalletLedgerEntry> IncomeEntry(
            string transactionId,
            string currency,
            decimal amount,
            DateTime postingUtc,
            string incomeSourceGl,
            string walletAccountGl,
            string incomeSourceEntryType,
            string walletAccountEntryType)
        {
            return
            [
                // Debit: Wallet Account
                new()
                {
                    TRANSACTIONID = transactionId,
                    AccountNumber = walletAccountGl,
                    Currency = currency,
                    Group = 1,
                    Index = 1,
                    DrCr = DrCr.D,
                    Amount = amount,
                    EntryType = walletAccountEntryType,
                    PostingDateUtc = postingUtc
                },

                // Credit: Income Source
                new()
                {
                    TRANSACTIONID = transactionId,
                    AccountNumber = incomeSourceGl,
                    Currency = currency,
                    Group = 1,
                    Index = 2,
                    DrCr = DrCr.C,
                    Amount = amount,
                    EntryType = incomeSourceEntryType,
                    PostingDateUtc = postingUtc
                }
            ];
        }

        /// <summary>
        /// The ExpenseEntry
        /// </summary>
        /// <param name="transactionId">The transactionId<see cref="string"/></param>
        /// <param name="currency">The currency<see cref="string"/></param>
        /// <param name="amount">The amount<see cref="decimal"/></param>
        /// <param name="postingUtc">The postingUtc<see cref="DateTime"/></param>
        /// <param name="expenseTargetGl">The expenseTargetGl<see cref="string"/></param>
        /// <param name="walletAccountGl">The walletAccountGl<see cref="string"/></param>
        /// <param name="expenseTargetEntryType">The expenseTargetEntryType<see cref="string"/></param>
        /// <param name="walletAccountEntryType">The walletAccountEntryType<see cref="string"/></param>
        /// <returns>The <see cref="List{WalletLedgerEntry}"/></returns>
        public static List<WalletLedgerEntry> ExpenseEntry(
            string transactionId,
            string currency,
            decimal amount,
            DateTime postingUtc,
            string expenseTargetGl,
            string walletAccountGl,
            string expenseTargetEntryType,
            string walletAccountEntryType)
        {
            return [
                    // Debit: Expense Target
            new()
                {
                    TRANSACTIONID = transactionId,
                    AccountNumber = expenseTargetGl,
                    Currency = currency,
                    Group = 1,
                    Index = 1,
                    DrCr = DrCr.D,
                    Amount = amount,
                    EntryType = expenseTargetEntryType,
                    PostingDateUtc = postingUtc
                },

                // Credit: Wallet Account
                new()
                {
                    TRANSACTIONID = transactionId,
                    AccountNumber = walletAccountGl,
                    Currency = currency,
                    Group = 1,
                    Index = 2,
                    DrCr = DrCr.C,
                    Amount = amount,
                    EntryType = walletAccountEntryType,
                    PostingDateUtc = postingUtc
                }
            ];
        }

        /// <summary>
        /// The LoanEntry
        /// </summary>
        /// <param name="transactionId">The transactionId<see cref="string"/></param>
        /// <param name="currency">The currency<see cref="string"/></param>
        /// <param name="amount">The amount<see cref="decimal"/></param>
        /// <param name="postingUtc">The postingUtc<see cref="DateTime"/></param>
        /// <param name="loanLiabilityGl">The loanLiabilityGl<see cref="string"/></param>
        /// <param name="walletAccountGl">The walletAccountGl<see cref="string"/></param>
        /// <param name="loanLiabilityEntryType">The loanLiabilityEntryType<see cref="string"/></param>
        /// <param name="walletAccountEntryType">The walletAccountEntryType<see cref="string"/></param>
        /// <returns>The <see cref="List{WalletLedgerEntry}"/></returns>
        public static List<WalletLedgerEntry> LoanEntry(
            string transactionId,
            string currency,
            decimal amount,
            DateTime postingUtc,
            string loanLiabilityGl,
            string walletAccountGl,
            string loanLiabilityEntryType,
            string walletAccountEntryType)
        {
            return
            [
                // Debit: Wallet Account
                new()
                {
                    TRANSACTIONID = transactionId,
                    AccountNumber = walletAccountGl,
                    Currency = currency,
                    Group = 1,
                    Index = 1,
                    DrCr = DrCr.D,
                    Amount = amount,
                    EntryType = walletAccountEntryType,
                    PostingDateUtc = postingUtc
                },

                // Credit: Loan Liability
                new()
                {
                    TRANSACTIONID = transactionId,
                    AccountNumber = loanLiabilityGl,
                    Currency = currency,
                    Group = 1,
                    Index = 2,
                    DrCr = DrCr.C,
                    Amount = amount,
                    EntryType = loanLiabilityEntryType,
                    PostingDateUtc = postingUtc
                }
            ];
        }
    }
}
