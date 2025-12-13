using O24OpenAPI.Web.Report.Domain;

namespace O24OpenAPI.Web.Report.Utils;

public static class ReportSanitizer
{
    public static string SafeId(string s)
    {
        if (string.IsNullOrWhiteSpace(s))
        {
            return "_";
        }

        var id = System.Text.RegularExpressions.Regex.Replace(s, @"[^\p{L}\p{Nd}_]", "_");
        if (char.IsDigit(id[0]))
        {
            id = "_" + id;
        }

        return id;
    }

    public static string SafeText(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return string.Empty;
        }

        return text
            .Replace("\\", "\\\\")
            .Replace("\"", "\\\"")
            .Replace("\r", "\\r").Replace("\n", "\\n")
            .Replace("{", "{{").Replace("}", "}}");
    }

    public static void SanitizeDesigns(List<ReportDesign> designs)
    {
        foreach (var d in designs)
        {
            d.ComponentName = SafeId(d.ComponentName);
            d.ComponentParent = SafeId(d.ComponentParent);
            d.BandName = SafeId(d.BandName);
            d.DataSource = SafeId(d.DataSource);
            d.FieldName = SafeId(d.FieldName);
            d.Text = SafeText(d.Text);
        }
    }

    public static void SanitizeDataDefs(List<ReportData> datas)
    {
        foreach (var x in datas)
        {
            x.DataSourceName = SafeId(x.DataSourceName);
            x.DataBand = SafeId(x.DataBand);
            x.ParentDatatable = SafeId(x.ParentDatatable);
            x.DataSource = SafeId(x.DataSource);
        }
    }
}
