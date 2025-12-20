namespace O24OpenAPI.CMS.API.Application.Models;

/// <summary>
/// FileModel
/// </summary>
public partial class FileModel : BaseO24OpenAPIModel
{
    /// <summary>
    /// FileModel
    /// </summary>
    public FileModel() { }

    /// <summary>
    /// /// FileName
    /// </summary>
    public string FileName { get; set; }

    /// <summary>
    /// ContentType
    /// </summary>
    public string ContentType { get; set; }

    /// <summary>
    /// FileContent
    /// </summary>
    public string FileContent { get; set; }
}

/// <summary>
/// BoExportDataModel
/// </summary>
public partial class BoExportDataModel : BaseO24OpenAPIModel
{
    /// <summary>
    /// Txcode
    /// </summary>
    public string Txcode { get; set; }

    /// <summary>
    /// App
    /// </summary>
    /// <value></value>
    public string App { get; set; }
}

/// <summary>
/// FoExportDataModel
/// </summary>
public partial class FoExportDataModel : BaseO24OpenAPIModel
{
    /// <summary>
    /// Txcode
    /// </summary>
    public string Txcode { get; set; }

    /// <summary>
    /// App
    /// </summary>
    /// <value></value>
    public string App { get; set; }
}

/// <summary>
/// AppExportDataModel
/// </summary>
public partial class AppExportDataModel : BaseO24OpenAPIModel
{
    /// <summary>
    /// ListApplicationId
    /// </summary>
    public string ListApplicationId { get; set; }
}

/// <summary>
/// AppExportDataModel
/// </summary>
public partial class AppOfRoleExportDataModel : BaseO24OpenAPIModel
{
    /// <summary>
    /// RoleId
    /// </summary>
    public int? RoleId { get; set; }
}

/// <summary>
/// DesignGroupExportDataModel
/// </summary>
public partial class DesignGroupExportDataModel : BaseO24OpenAPIModel
{
    /// <summary>
    /// GroupId
    /// </summary>
    /// <value></value>
    public string GroupId { get; set; }
}

/// <summary>
/// DesignItemExportDataModel
/// </summary>
public partial class DesignItemExportDataModel : BaseO24OpenAPIModel
{
    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public string AttId { get; set; }
}

/// <summary>
/// FormExportDataModel
/// </summary>
public partial class FormExportDataModel : BaseO24OpenAPIModel
{
    /// <summary>
    ///
    /// </summary>
    public string FormId { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string App { get; set; } = "ncbsCbs";
}

/// <summary>
/// LangExportDataModel
/// </summary>
public partial class LangExportDataModel : BaseO24OpenAPIModel
{
    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public string App { get; set; }
}

/// <summary>
/// LearApiExportDataModel
/// </summary>
public partial class LearnApiExportDataModel : BaseO24OpenAPIModel
{
    /// <summary>
    /// LearnApiId
    /// </summary>
    public string LearnApiId { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public string App { get; set; } = "ncbsCbs";
}

/// <summary>
/// LearApiExportDataModel
/// </summary>
public partial class WorkflowExportDataModel : BaseO24OpenAPIModel
{
    /// <summary>
    /// LearnApiId
    /// </summary>
    public string WorkflowId { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public int? Status { get; set; }
}

/// <summary>
/// LearApiExportDataModel
/// </summary>
public partial class UserCommandsExportDataModel : BaseO24OpenAPIModel
{
    /// <summary>
    /// Application code
    /// </summary>
    public string ApplicationCode { get; set; }

    /// <summary>
    /// Command Id
    /// </summary>
    public string CommandId { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string ParentId { get; set; }
}

/// <summary>
/// ParaServerExportDataModel
/// </summary>
public partial class ParaServerExportDataModel : BaseO24OpenAPIModel
{
    /// <summary>
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string App { get; set; }
}

/// <summary>
/// TemplateVoucherExportDataModel
/// </summary>
public partial class TemplateVoucherExportDataModel : BaseO24OpenAPIModel
{
    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public string Code { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public string App { get; set; }
}

/// <summary>
/// TemplateVoucherExportDataModel
/// </summary>
public partial class CdlistExportDataModel : BaseO24OpenAPIModel
{
    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public string Cdgrp { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public string Cdname { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public string Cdid { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public string App { get; set; }
}

/// <summary>
/// TemplateReportExportDataModel
/// </summary>
public partial class TemplateReportExportDataModel : BaseO24OpenAPIModel
{
    /// <summary>
    /// FileModel
    /// </summary>
    public TemplateReportExportDataModel() { }

    /// <summary>
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public string OrganizationId { get; set; }
}
