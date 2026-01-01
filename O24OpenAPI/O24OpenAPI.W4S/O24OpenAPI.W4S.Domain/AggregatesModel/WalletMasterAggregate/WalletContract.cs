using O24OpenAPI.Core;
using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.W4S.Domain.AggregatesModel.WalletMasterAggregate
{
    [Auditable]
    public partial class WalletContract : BaseEntity
    {
        // ===== Identity =====
        public string ContractNumber { get; set; } = default!;

        // ===== Contract Info =====
        public WalletContractType ContractType { get; set; }
        public WalletTier WalletTier { get; set; }
        public WalletUserType UserType { get; set; }
        public WalletUserLevel? UserLevel { get; set; }

        // ===== Policy =====
        public string PolicyCode { get; set; } = default!;

        // ===== Customer =====
        public string CustomerCode { get; set; } = default!;
        public string FullName { get; set; } = default!;
        public string Phone { get; set; } = default!;
        public string? Email { get; set; }

        // ===== Lifecycle =====
        public WalletContractStatus Status { get; set; }
        public DateTime OpenDateUtc { get; set; }
        public DateTime? CloseDateUtc { get; set; }

        // ===== Channel =====
        public WalletChannel Channel { get; set; }

        public WalletContract() { }

        // ===== Factory =====
        public static WalletContract Create(
            string contractNumber,
            WalletContractType contractType,
            WalletTier walletTier,
            WalletUserType userType,
            WalletUserLevel? userLevel,
            string policyCode,
            string customerCode,
            string fullName,
            string phone,
            string email,
            WalletChannel channel
        )
        {
            if (string.IsNullOrWhiteSpace(contractNumber))
                throw new ArgumentException("ContractNumber is required.");

            if (string.IsNullOrWhiteSpace(customerCode))
                throw new ArgumentException("CustomerCode is required.");

            if (string.IsNullOrWhiteSpace(policyCode))
                throw new ArgumentException("PolicyCode is required.");

            return new WalletContract
            {
                ContractNumber = contractNumber.Trim(),
                ContractType = contractType,
                WalletTier = walletTier,
                UserType = userType,
                UserLevel = userLevel,
                PolicyCode = policyCode.Trim(),
                CustomerCode = customerCode.Trim(),
                FullName = fullName.Trim(),
                Phone = phone.Trim(),
                Email = email.Trim(),
                Status = WalletContractStatus.Active,
                OpenDateUtc = DateTime.UtcNow,
                Channel = channel,
                CreatedOnUtc = DateTime.UtcNow
            };
        }

        // ===== Domain Behaviors =====
        public void UpgradeTier(WalletTier newTier)
        {
            if (Status != WalletContractStatus.Active)
                throw new O24OpenAPIException("Only active contract can be upgraded.");

            if (newTier <= WalletTier)
                throw new O24OpenAPIException("Invalid wallet tier upgrade.");

            WalletTier = newTier;
            UpdatedOnUtc = DateTime.UtcNow;
        }

        public void Suspend()
        {
            if (Status != WalletContractStatus.Active)
                throw new O24OpenAPIException("Contract is not active.");

            Status = WalletContractStatus.Suspended;
            UpdatedOnUtc = DateTime.UtcNow;
        }

        public void Close()
        {
            if (Status == WalletContractStatus.Closed)
                return;

            Status = WalletContractStatus.Closed;
            CloseDateUtc = DateTime.UtcNow;
            UpdatedOnUtc = DateTime.UtcNow;
        }
    }
    public enum WalletContractStatus
    {
        Active = 1,
        Suspended = 2,
        Locked = 3,
        Closed = 4
    }
    public enum WalletTier
    {
        Basic = 1,
        Standard = 2,
        Premium = 3
    }
    public enum WalletUserType
    {
        Individual = 1,   // 0502
        Agent = 2,
        Merchant = 3
    }
    public enum WalletUserLevel
    {
        L0 = 0,
        L1 = 1,
        L2 = 2,
        L3 = 3
    }
    public enum WalletChannel
    {
        MB = 1,      // Mobile Banking
        CMS = 2,     // Admin
        API = 3
    }
    public enum WalletContractType
    {
        Wallet = 1
    }
}
