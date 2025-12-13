namespace O24OpenAPI.Web.CMS.Models;

public partial class SettingCreateModel : BaseTransactionModel
{
    /// <summary>
    ///
    /// </summary>
    public SettingCreateModel() { }

    /// <summary>
    ///
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string Value { get; set; }

    /// <summary>
    ///
    /// </summary>
    public int OrganizationId { get; set; }
}

public partial class SettingSearchModel : BaseTransactionModel
{
    /// <summary>
    ///
    /// </summary>
    public SettingSearchModel() { }

    /// <summary>
    ///
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string Value { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string OrganizationId { get; set; }

    /// <summary>
    ///
    /// </summary>
    public int PageIndex { get; set; } = 0;

    /// <summary>
    ///
    /// </summary>
    public int PageSize { get; set; } = int.MaxValue;
}

public partial class SettingUpdateModel : BaseTransactionModel
{
    /// <summary>
    ///
    /// </summary>
    public SettingUpdateModel() { }

    /// <summary>
    ///
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string Value { get; set; }

    /// <summary>
    ///
    /// </summary>
    public int OrganizationId { get; set; }
}
