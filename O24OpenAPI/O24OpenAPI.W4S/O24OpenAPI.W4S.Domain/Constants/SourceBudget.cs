namespace O24OpenAPI.W4S.Domain.Constants
{
    /// <summary>
    /// Indicates where the budget amount comes from
    /// </summary>
    public static class SourceBudget
    {
        /// <summary>
        /// System creates budget automatically from default categories
        /// </summary>
        public const string DefaultCategory = "DEFAULT_CATEGORY";

        /// <summary>
        /// User manually sets or edits budget
        /// </summary>
        public const string UserManual = "USER_MANUAL";

        /// <summary>
        /// Budget copied from previous period (rollover)
        /// </summary>
        public const string CopyPreviousPeriod = "COPY_PREVIOUS_PERIOD";

        /// <summary>
        /// System suggests budget based on rules
        /// </summary>
        public const string SystemSuggest = "SYSTEM_SUGGEST";

        /// <summary>
        /// AI/ML suggests budget
        /// </summary>
        public const string AiSuggest = "AI_SUGGEST";

        /// <summary>
        /// Budget imported from external source (Excel, API)
        /// </summary>
        public const string Import = "IMPORT";

        /// <summary>
        /// Budget enforced by policy (enterprise / corporate)
        /// </summary>
        public const string Policy = "POLICY";
    }
}
