using O24OpenAPI.Framework.Domain;

namespace O24OpenAPI.W4S.Domain.AggregatesModel.WalletMasterAggregate
{
    public partial class WalletAccountGLs : BaseMasterGL
    {
        /// <summary>
        /// Master Account
        /// </summary>
        public string? WalletAccount { get; set; }


        /// <summary>
        /// Catalog code
        /// </summary>
        public string? CatalogCode { get; set; }
    }
}
