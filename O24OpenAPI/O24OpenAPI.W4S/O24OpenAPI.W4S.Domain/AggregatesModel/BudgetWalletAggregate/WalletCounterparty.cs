using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate
{
    public partial class WalletCounterparty : BaseEntity
    {
        public WalletCounterparty() { }

        // ===== Scope =====
        public int WalletId { get; private set; }

        // ===== Identity / Display =====
        public string DisplayName { get; private set; } = string.Empty;
        public string? Phone { get; private set; }
        public string? Email { get; private set; }
        public string? AvatarUrl { get; private set; }

        // ===== Type / Tags =====
        public WalletCounterpartyType CounterpartyType { get; private set; } = WalletCounterpartyType.Person;
        public string? Note { get; private set; }

        // ===== UX / Suggestion =====
        public bool IsFavorite { get; private set; }
        public int UseCount { get; private set; }
        public DateTime? LastUsedOnUtc { get; private set; }

        // ===== Soft flags =====
        public bool IsActive { get; private set; } = true;

        // ===== Optional: quick matching =====
        public string? SearchKey { get; private set; } // normalized key for search/suggest (name/phone/email)

        /// <summary>
        /// Factory create a counterparty used for "from/to" in transactions.
        /// </summary>
        public static WalletCounterparty Create(
            int walletId,
            string displayName,
            string? phone = null,
            string? email = null,
            WalletCounterpartyType counterpartyType = WalletCounterpartyType.Person,
            string? avatarUrl = null,
            string? note = null,
            bool isFavorite = false,
            DateTime? nowUtc = null
        )
        {
            if (walletId <= 0)
                throw new ArgumentOutOfRangeException(nameof(walletId));

            displayName = (displayName ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(displayName))
                throw new ArgumentException("DisplayName is required", nameof(displayName));

            phone = NormalizePhone(phone);
            email = NormalizeEmail(email);

            var entity = new WalletCounterparty
            {
                WalletId = walletId,
                DisplayName = displayName,
                Phone = phone,
                Email = email,
                AvatarUrl = string.IsNullOrWhiteSpace(avatarUrl) ? null : avatarUrl.Trim(),
                Note = string.IsNullOrWhiteSpace(note) ? null : note.Trim(),
                CounterpartyType = counterpartyType,
                IsFavorite = isFavorite,
                UseCount = 0,
                LastUsedOnUtc = null,
                IsActive = true
            };

            entity.SearchKey = BuildSearchKey(entity.DisplayName, entity.Phone, entity.Email);
            // If you want to set CreatedOnUtc here, do it via BaseEntity/audit pipeline.

            return entity;
        }

        /// <summary>
        /// Mark as used for suggestion ranking.
        /// Call this after creating a transaction/statement with this counterparty.
        /// </summary>
        public void Touch(DateTime? nowUtc = null)
        {
            var now = nowUtc ?? DateTime.UtcNow;
            UseCount = checked(UseCount + 1);
            LastUsedOnUtc = now;
        }

        public void SetFavorite(bool value)
        {
            IsFavorite = value;
        }

        public void UpdateProfile(
            string? displayName = null,
            string? phone = null,
            string? email = null,
            string? avatarUrl = null,
            string? note = null,
            WalletCounterpartyType? counterpartyType = null
        )
        {
            if (!string.IsNullOrWhiteSpace(displayName))
                DisplayName = displayName.Trim();

            Phone = NormalizePhone(phone) ?? Phone;
            Email = NormalizeEmail(email) ?? Email;

            if (avatarUrl != null)
                AvatarUrl = string.IsNullOrWhiteSpace(avatarUrl) ? null : avatarUrl.Trim();

            if (note != null)
                Note = string.IsNullOrWhiteSpace(note) ? null : note.Trim();

            if (counterpartyType.HasValue)
                CounterpartyType = counterpartyType.Value;

            SearchKey = BuildSearchKey(DisplayName, Phone, Email);
        }

        public void SetActive(bool value)
        {
            IsActive = value;
        }

        // ===== Helpers =====

        private static string? NormalizePhone(string? phone)
        {
            if (string.IsNullOrWhiteSpace(phone)) return null;

            // Keep digits and leading +
            phone = phone.Trim();
            var chars = phone.Where(c => char.IsDigit(c) || c == '+').ToArray();
            var normalized = new string(chars);

            // If it's only "+", treat as null
            if (normalized == "+") return null;
            return string.IsNullOrWhiteSpace(normalized) ? null : normalized;
        }

        private static string? NormalizeEmail(string? email)
        {
            if (string.IsNullOrWhiteSpace(email)) return null;
            email = email.Trim().ToLowerInvariant();
            return string.IsNullOrWhiteSpace(email) ? null : email;
        }

        private static string BuildSearchKey(string displayName, string? phone, string? email)
        {
            // simple normalized key; you can upgrade to remove diacritics if needed
            var parts = new List<string>(3)
            {
                displayName.Trim().ToLowerInvariant()
            };

            if (!string.IsNullOrWhiteSpace(phone)) parts.Add(phone.Trim().ToLowerInvariant());
            if (!string.IsNullOrWhiteSpace(email)) parts.Add(email.Trim().ToLowerInvariant());

            return string.Join(" | ", parts);
        }
    }

    public enum WalletCounterpartyType
    {
        Person = 1,
        Merchant = 2,
        Organization = 3,
        Other = 9
    }
}
