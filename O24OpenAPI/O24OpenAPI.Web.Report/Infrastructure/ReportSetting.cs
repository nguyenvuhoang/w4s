using O24OpenAPI.Core.Configuration;

namespace O24OpenAPI.Web.Report.Infrastructure;

/// <summary>
/// Bank setting
/// </summary>
public class ReportSetting : ISettings
{
    /// <summary>
    /// BackupDirectory
    /// </summary>
    /// <value></value>
    public string BackupDirectory { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <value></value>

    public string AdminUrl { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public string LogoBank { get; set; }
    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public string BankName { get; set; }
    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public double TimeZone { get; set; }
    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public string MoMoneyBankAccountNumber { get; set; }
    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public string ModusOperandiBankAccountNumber { get; set; }
    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public string ReportAttachmentDirectory { get; set; }
    /// <summary>
    /// PortalUser
    /// </summary>
    /// <value></value>
    public string PortalUser { get; set; } = "neptune";

    /// <summary>
    /// PortalPassword
    /// </summary>
    /// <value></value>
    public string PortalPassword { get; set; } = "HwfKP+W3ra0mxTKkzOxC4Q==";
    /// <summary>
    ///
    /// </summary>
    public string ReportExportDirectory { get; set; } = "ExportData/Report";
    /// <summary>
    ///
    /// </summary>
    public int ArchiveRetentionDays { get; set; } = 3;
    /// <summary>
    ///
    /// </summary>
    public long TimeElapsedMonitoring { get; set; } = 5000;

    /// <summary>
    ///
    /// </summary>
    public string FxRevaluationGoliveDate { get; set; }
}
