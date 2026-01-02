using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.W4S.Domain.AggregatesModel.WalletMasterAggregate
{
    [Auditable]
    public partial class WalletCategoryDefault : BaseEntity
    {
        /// <summary>
        /// Stable business code (EXP_FOOD, EXP_BILL_ELECT, INC_SALARY...)
        /// </summary>
        public string CategoryCode { get; set; } = default!;

        /// <summary>
        /// Parent code for building tree. Empty = root.
        /// </summary>
        public string ParentCategoryCode { get; set; } = string.Empty;

        /// <summary>
        /// Group for UI tabs (EXPENSE / INCOME / LOAN)
        /// </summary>
        public string CategoryGroup { get; set; } = string.Empty;

        /// <summary>
        /// Category type (EXPENSE / INCOME / LOAN)
        /// </summary>
        public string CategoryType { get; set; } = default!;

        /// <summary>
        /// Display name (Vietnamese by default)
        /// </summary>
        public string CategoryName { get; set; } = default!;

        /// <summary>
        /// Icon key for mobile/web mapping (food, bill, taxi...)
        /// </summary>
        public string Icon { get; set; } = string.Empty;

        /// <summary>
        /// Hex color string (#FF9800)
        /// </summary>
        public string Color { get; set; } = string.Empty;

        /// <summary>
        /// Sorting order within same level
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// Master enable/disable (do not delete to keep history/versioning)
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Optional: language code (vi, en, lo...)
        /// Leave null if you store only one language in master.
        /// </summary>
        public string Language { get; set; } = string.Empty;
    }
}
