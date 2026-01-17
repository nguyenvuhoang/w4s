using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.W4S.Domain.AggregatesModel.WalletMasterAggregate;

/// <summary>
/// Domain Stock Transaction
/// </summary>
public partial class WalletCatalogGLs : BaseEntity
{
    /// <summary>
    /// Mã catalogue
    /// </summary>
    public string CatalogCode { get; set; }

    /// <summary>
    /// Tên của GL account được sử dụng trong khai báo Posting, cấp product (DEPOSIT, CREDIT0...)
    /// </summary>
    public string SysAccountName { get; set; }

    /// <summary>
    /// mã GL cấp cuối cùng trên Account Chart (cấp cha của nút lá, thường là GL cấp 6)
    /// </summary>
    public string COAAccount { get; set; }

    /// <summary>
    /// Alias của account. Bao gồm các thành phần chưa xác định và đã xác định trong cấu trúc tài khoản GL. (---208723$$223: thiếu thông tin chi nhánh, Customer Sector. Lúc mở tài khoản sẽ xác định và được thay thế).
    /// </summary>
    public string AccountAlias { get; set; }
}
