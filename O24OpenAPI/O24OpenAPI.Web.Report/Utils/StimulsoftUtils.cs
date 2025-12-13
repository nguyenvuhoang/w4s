using Newtonsoft.Json;
using O24OpenAPI.Core.Configuration;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Web.Framework.DBContext;
using O24OpenAPI.Web.Framework.Extensions;
using O24OpenAPI.Web.Framework.Services.Mapping;
using O24OpenAPI.Web.Report.Domain;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Drawing.Text;
using Stimulsoft.Report;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Table;
using Stimulsoft.Report.Dictionary;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;

namespace O24OpenAPI.Web.Report.Utils;

/// <summary>
///
/// </summary>
public static class StimulsoftUtils
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="report"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static string GetValue(this StiReport report, string key)
    {
        if (report.Dictionary.Variables == null)
        {
            return string.Empty;
        }

        if (report.Dictionary.Variables.Contains(key))
        {
            var stiVariable = report.Dictionary.Variables[key];
            if (stiVariable != null)
            {
                return report.Dictionary.Variables[key].Value;
            }
        }

        return string.Empty;
    }

    /// <summary>
    /// InitReport
    ///
    public static StiReport Init(ReportConfig reportConfig, ReportTemplate templateReport)
    {

        StiReport stiReport = StiReport.CreateNewReport();
        StiOptions.Engine.HideExceptions = false;
        StiOptions.Engine.HideMessages = true;
        StiOptions.Engine.HideRenderingProgress = true;
        StiOptions.Engine.ShowReportRenderingMessages = false;

        StiOptions.Engine.UseGarbageCollectorForImageCache = true;
        StiOptions.Engine.ImageCache.Enabled = false;
        StiOptions.Engine.ReportCache.AllowGCCollect = true;
        StiOptions.Engine.ReportCache.AmountOfProcessedPagesForStartGCCollect = 1;
        StiOptions.Engine.ReportCache.LimitForStartUsingCache = Int32.MaxValue;
        StiOptions.Engine.RtfCache.AllowGCCollect = true;
        StiOptions.Engine.RtfCache.Enabled = false;

        stiReport.ReportName = reportConfig.Code;
        stiReport.ReportAlias = reportConfig.Description;
        stiReport.ReportAuthor = reportConfig.OrganizationId;
        stiReport.ReportDescription = reportConfig.Description;

        stiReport.ReportVersion = templateReport.Version;
        stiReport.ReportUnit = templateReport.ReportUnit.Contains("Inches") ? StiReportUnitType.Inches : StiReportUnitType.Centimeters;

        stiReport.Pages[0].Name = reportConfig.Name;
        byte[] bytesWatermark = Convert.FromBase64String(templateReport.Watermark);
        stiReport.Pages[0].Watermark.ImageBytes = bytesWatermark;
        stiReport.Pages[0].Watermark.Angle = 60;
        stiReport.Pages[0].Watermark.Font = new Stimulsoft.Drawing.Font(templateReport.Font ?? "Quicksand", 50, FontStyle.Regular);
        stiReport.Pages[0].Watermark.ImageTransparency = 160;
        stiReport.Pages[0].UnlimitedBreakable = false;
        stiReport.Pages[0].CanGrow = true;
        stiReport.Pages[0].PaperSize = LoadPaperKind(templateReport.PaperSize);
        stiReport.Pages[0].PageWidth = (double)Convert.ToSingle(templateReport.PageWidth);
        stiReport.Pages[0].PageHeight = (double)Convert.ToSingle(templateReport.PageHeight);
        stiReport.Pages[0].Margins = ParseMargin(templateReport.Margins);
        stiReport.Pages[0].Report = stiReport;
        return stiReport;
    }


    public static void LoadCustomFonts()
    {
        string fontsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "fonts");
        PrivateFontCollection privateFonts = new();

        foreach (var file in Directory.GetFiles(fontsPath, "*.ttf"))
        {
            privateFonts.AddFontFile(file);
        }

        foreach (var font in privateFonts.Families)
        {
            Console.WriteLine($"Loaded font: {font.Name}");
        }
    }


    /// <summary>
    /// ParseRectangleD
    /// </summary>
    /// <param name="clientRectangle"></param>
    /// <returns></returns>
    private static RectangleD? ParseRectangleD(string clientRectangle)
    {
        if (string.IsNullOrEmpty(clientRectangle))
        {
            return null;
        }

        var values = clientRectangle.Split(',')
                                    .Select(v => double.TryParse(v.Trim(), out double result) ? result : 0)
                                    .ToArray();

        if (values.Length != 4)
        {
            return null;
        }

        return new RectangleD(values[0], values[1], values[2], values[3]);
    }

    /// <summary>
    /// ParseMargin
    /// </summary>
    /// <param name="clientRectangle"></param>
    /// <returns></returns>
    private static StiMargins ParseMargin(string margin)
    {
        if (string.IsNullOrEmpty(margin))
        {
            return null;
        }

        var values = margin.Split(',').Select(v => double.TryParse(v.Trim(), out double result) ? result : 0).ToArray();

        if (values.Length != 4)
        {
            return null;
        }

        return new StiMargins(values[0], values[1], values[2], values[3]);
    }


    /// <summary>
    /// GenerateStiVariables
    /// </summary>
    /// <param name="report"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>

    public static void GenerateStiVariables(StiReport report, List<ReportParam> parameters)
    {
        if (parameters == null || parameters.Count == 0)
        {
            return;
        }

        foreach (var param in parameters)
        {

            var variable = new StiVariable("", param.ParamName, param.ControlName, param.Value ?? "")
            {
                RequestFromUser = true,
                Type = TypeUtils.GetVariableType(param.Tag)
            };

            if (param.ControlType == "DropDownList" && !string.IsNullOrEmpty(param.Store))
            {
                try
                {
                    var dbContext = new ServiceDBContext();
                    var dropdownValues = dbContext.CallServiceStoredProcedure(param.Store, null, Singleton<O24OpenAPIConfiguration>.Instance.DWHSchema).GetAwaiter().GetResult();

                    if (string.IsNullOrEmpty(dropdownValues?.ToString()))
                    {
                        Console.WriteLine($"⚠ Warning:  {param.Store} dropdownValues is null or empty.");
                        return;
                    }

                    var parsedData = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(dropdownValues.ToString());

                    if (parsedData == null || parsedData.Count == 0)
                    {
                        Console.WriteLine($"⚠ Warning: {param.Store}  parsedData is null or empty.");
                        return;
                    }

                    var dropdownData = parsedData
                    .Select(row => new
                    {
                        Key = row.ContainsKey(param.Key) ? row[param.Key]?.ToString() ?? "" : "",
                        Value = row.ContainsKey(param.Text) ? row[param.Text]?.ToString() ?? "" : ""
                    })
                    .ToList();

                    string dataSource = $"{param.ReportCode}.{param.ParamName}";
                    var dataTable = new DataTable(dataSource);
                    dataTable.Columns.AddRange(
                    [
                        new DataColumn("Key", typeof(string)),
                        new DataColumn("Value", typeof(string))
                    ]);

                    dropdownData?.ForEach(item => dataTable.Rows.Add(item.Key, item.Value));

                    report.RegData(dataSource, dataTable);

                    if (dataTable.Rows.Count > 0)
                    {
                        variable.DialogInfo = new StiDialogInfo(
                            StiDateTimeType.Date,
                            dataSource,
                            false,
                            dataTable.AsEnumerable().Select(row => row["Key"].ToString()).ToArray(),
                            dataTable.AsEnumerable().Select(row => row["Value"].ToString()).ToArray()
                        );
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error fetching dropdown values for {param.ParamName}: {ex.Message}");
                    _ = ex.LogErrorAsync();
                }
            }

            report.Dictionary.Variables.Add(variable);
        }
    }


    /// <summary>
    /// GenerateReportBands
    /// </summary>
    /// <param name="report"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    #region GenerateReportBands
    public static void GenerateReportBands(StiReport report, List<ReportComponent> reportComponents)
    {

        foreach (var component in reportComponents)
        {
            if (report.Pages[0].Components.OfType<StiBand>().Any(b => b.Name == component.ComponentName))
            {
                continue;
            }

            var band = report.Pages[0].Components.OfType<StiBand>().FirstOrDefault(b => b.Name == component.ComponentName);

            if (band != null)
            {
                return;
            }

            StiBand newBand = component.ComponentType switch
            {
                "Header" => new StiHeaderBand { Name = component.ComponentName, Height = (double)Convert.ToSingle(component.Height), PrintOn = StiPrintOnType.OnlyFirstPage },
                "Footer" => new StiFooterBand { Name = component.ComponentName, Height = (double)Convert.ToSingle(component.Height) },
                "GroupHeader" => new StiGroupHeaderBand { Name = component.ComponentName, Height = (double)Convert.ToSingle(component.Height) },
                "GroupFooter" => new StiGroupFooterBand { Name = component.ComponentName, Height = (double)Convert.ToSingle(component.Height) },
                "PageHeader" => new StiPageHeaderBand { Name = component.ComponentName, Height = (double)Convert.ToSingle(component.Height), PrintOn = StiPrintOnType.OnlyFirstPage },
                "PageFooter" => new StiPageFooterBand { Name = component.ComponentName, Height = (double)Convert.ToSingle(component.Height) },
                "Data" => new StiDataBand { Name = component.ComponentName, Height = (double)Convert.ToSingle(component.Height) },
                _ => null
            };

            if (newBand != null)
            {
                newBand.Brush = new StiSolidBrush(Color.Transparent);
                report.Pages[0].Components.Add(newBand);
                var rectangle = ParseRectangleD(component.ClientRectangle);
                if (rectangle != null)
                {
                    newBand.ClientRectangle = rectangle.Value;
                }
                newBand.CanShrink = true;
                newBand.CanBreak = true;
            }

        }
    }
    #endregion


    private static List<StiText> GenerateStiText(StiReport report, List<ReportDesign> textDesign)
    {
        StiBrush hatchBrush = new StiHatchBrush(
            HatchStyle.BackwardDiagonal,
            System.Drawing.Color.FromArgb(50, 50, 50),
            System.Drawing.Color.FromArgb(100, 100, 100)
        );

        List<StiText> textComponents = [];

        foreach (var design in textDesign)
        {

            Stimulsoft.Drawing.FontFamily fontFamily;
            try
            {
                fontFamily = new Stimulsoft.Drawing.FontFamily(design.FontFamily ?? "Century Gothic");
            }
            catch
            {
                fontFamily = new Stimulsoft.Drawing.FontFamily("Century Gothic");
            }


            StiText stiText = new()
            {
                Name = design.ComponentName,
                Text = design.Text ?? $"{{{design.DataSource}.{design.FieldName}}}",
                Font = new Stimulsoft.Drawing.Font(fontFamily, design.FontSize ?? 12, design.FontStyle == "Bold" ? FontStyle.Bold : design.FontStyle == "Italic" ? FontStyle.Italic : FontStyle.Regular),
                Width = (double)Convert.ToSingle(design.Width),
                Height = (double)Convert.ToSingle(design.Height),
                Left = (double)Convert.ToSingle(design.PositionX),
                Top = (double)Convert.ToSingle(design.PositionY),
                Brush = design.BackgroundColor != null ? new StiSolidBrush(ColorTranslator.FromHtml(design.BackgroundColor ?? "#FFFFFF")) : new StiSolidBrush(System.Drawing.Color.Transparent),
                TextBrush = design.TextBrush != null ? new StiSolidBrush(design.TextBrush) : hatchBrush,
                HorAlignment = design.HorAlignment switch
                {
                    "Center" => StiTextHorAlignment.Center,
                    "Right" => StiTextHorAlignment.Right,
                    _ => StiTextHorAlignment.Left
                },

                VertAlignment = design.VertAlignment switch
                {
                    "Center" => StiVertAlignment.Center,
                    "Bottom" => StiVertAlignment.Bottom,
                    _ => StiVertAlignment.Top // Default
                },
                CanGrow = true,
                GrowToHeight = design.GrowToHeight,
                WordWrap = true,
                Parent = report.Pages[0].Components.OfType<StiBand>().FirstOrDefault(b => b.Name == design.BandName)
            };

            if (!string.IsNullOrEmpty(design.BorderColor))
            {
                stiText.Border = new StiBorder(StiBorderSides.All,
                                               ColorTranslator.FromHtml(design.BorderColor),
                                               (float)Convert.ToSingle(design.BorderThickness),
                                               StiPenStyle.Solid);
            }

            textComponents.Add(stiText);
        }

        return textComponents;
    }


    #region GenerateStiTable
    public static List<StiTable> GenerateStiTable(StiReport report, List<ReportDesign> tableDesigns, List<ReportDesign> columnDesigns)
    {
        List<StiTable> tableList = new();
        List<ReportDesign> tableColumnMapping;

        if (tableDesigns == null || tableDesigns.Count == 0)
        {
            return tableList;
        }

        foreach (var tableDesign in tableDesigns)
        {
            tableColumnMapping = columnDesigns.Where(d => d.ComponentParent == tableDesign.ComponentName).ToList();

            StiTable stiTable = new()
            {
                Name = tableDesign.ComponentName,
                Width = (double)Convert.ToSingle(tableDesign.Width),
                Height = (double)Convert.ToSingle(tableDesign.Height),
                Left = (double)Convert.ToSingle(tableDesign.PositionX),
                Top = (double)Convert.ToSingle(tableDesign.PositionY),
                ColumnCount = tableColumnMapping.Count,
                RowCount = 1,
                Parent = report.Pages[0].Components.OfType<StiBand>().FirstOrDefault(b => b.Name == tableDesign.BandName),
                DataSourceName = tableDesign.DataSource,
                CanGrow = true
            };

            if (tableColumnMapping.Count > 0)
            {
                Stimulsoft.Drawing.Font fontFamily;
                try
                {
                    fontFamily = new Stimulsoft.Drawing.Font(tableDesign.FontFamily ?? "Century Gothic", 10, FontStyle.Regular);
                }
                catch
                {
                    fontFamily = new Stimulsoft.Drawing.Font("Century Gothic", 10, FontStyle.Regular);
                }
                foreach (var column in tableColumnMapping)
                {
                    int i = 0;
                    var stiCell = new StiTableCell
                    {

                        ID = i++,
                        Name = column.ComponentName,
                        Text = column.Text ?? $"{{{column.DataSource}.{column.FieldName}}}",
                        Width = (double)Convert.ToSingle(column.Width),
                        Height = (double)Convert.ToSingle(column.Height),
                        Left = (double)Convert.ToSingle(column.PositionX),
                        Top = (double)Convert.ToSingle(column.PositionY),
                        HorAlignment = column.HorAlignment switch
                        {
                            "Center" => StiTextHorAlignment.Center,
                            "Right" => StiTextHorAlignment.Right,
                            _ => StiTextHorAlignment.Left
                        },
                        VertAlignment = column.VertAlignment switch
                        {
                            "Center" => StiVertAlignment.Center,
                            "Bottom" => StiVertAlignment.Bottom,
                            _ => StiVertAlignment.Top
                        },
                        Restrictions = Stimulsoft.Report.Components.StiRestrictions.AllowResize,
                        //Parent = stiTable,
                        WordWrap = true,
                        CanGrow = column.GrowToHeight,
                        GrowToHeight = column.GrowToHeight,
                        Font = fontFamily
                    };


                    if (!string.IsNullOrEmpty(column.BorderColor))
                    {
                        stiCell.Border = new StiBorder(StiBorderSides.All,
                        ColorTranslator.FromHtml(column.BorderColor),
                                                       (float)Convert.ToSingle(column.BorderThickness),
                                                       StiPenStyle.Solid);
                    }

                    stiTable.Components.Add(stiCell);
                }
            }

            tableList.Add(stiTable);
        }

        return tableList;
    }


    #endregion

    #region GenerateStiImage

    public static List<StiImage> GenerateStiImage(StiReport report, List<ReportDesign> imageDesigns)
    {
        List<StiImage> imageList = new();

        if (imageDesigns == null || imageDesigns.Count == 0)
        {
            return imageList;
        }

        foreach (var design in imageDesigns)
        {
            var stiImage = new StiImage
            {
                Name = design.ComponentName,
                Width = (double)Convert.ToSingle(design.Width),
                Height = (double)Convert.ToSingle(design.Height),
                Left = (double)Convert.ToSingle(design.PositionX),
                Top = (double)Convert.ToSingle(design.PositionY),
                Stretch = true,
                Parent = report.Pages[0].Components.OfType<StiBand>().FirstOrDefault(b => b.Name == design.BandName)
            };

            if (!string.IsNullOrEmpty(design.TargetValue))
            {
                stiImage.DataColumn = $"{design.DataSource}.{design.TargetValue}";
            }
            else
            {
                byte[] bytes = Convert.FromBase64String(design.Image);
                stiImage.ImageBytes = bytes;
            }

            imageList.Add(stiImage);
        }

        return imageList;
    }
    #endregion

    /// <summary>
    /// AddDynamicLabel
    /// </summary>
    /// <param name="report"></param>
    /// <param name="labelText"></param>
    /// <returns></returns>

    public static int IndexOfInvariant(this string str, string value)
    {
        return str.IndexOf(value, StringComparison.InvariantCulture);
    }
    public static void GenerateDynamicReport(StiReport report, List<ReportComponent> reportComponents, List<ReportDesign> reportDesigns)
    {
        if (reportComponents == null || reportComponents.Count == 0 || reportDesigns == null || reportDesigns.Count == 0)
        {
            return;
        }

        GenerateReportBands(report, reportComponents);

        List<StiBand> reportBands = report.Pages[0].Components.OfType<StiBand>().ToList();


        List<StiText> stiTexts = GenerateStiText(report, reportDesigns.Where(d => d.ComponentType == "Text").ToList());
        List<StiImage> stiImages = GenerateStiImage(report, reportDesigns.Where(d => d.ComponentType == "Image").ToList());
        List<StiTable> stiTables = GenerateStiTable(report, reportDesigns.Where(d => d.ComponentType == "DataTable").ToList(), reportDesigns.Where(d => d.ComponentType == "DataColumn").ToList());

        Dictionary<string, StiBand> bandLookup = reportBands.ToDictionary(b => b.Name);

        foreach (var text in stiTexts)
        {
            if (!bandLookup.TryGetValue(text.Parent?.Name ?? "", out var band))
            {
                continue;
            }

            var existingText = band.Components.OfType<StiText>().FirstOrDefault(t => t.Name == text.Name);
            if (existingText != null)
            {
                existingText.Text = text.Text;
            }
            else
            {
                band.Components.Add(text);
            }
        }

        foreach (var image in stiImages)
        {
            if (!bandLookup.TryGetValue(image.Parent?.Name ?? "", out var band))
            {
                continue;
            }

            if (!band.Components.OfType<StiImage>().Any(t => t.Name == image.Name))
            {
                band.Components.Add(image);
            }
        }

        foreach (var table in stiTables)
        {
            if (!bandLookup.TryGetValue(table.Parent?.Name ?? "", out var band))
            {
                continue;
            }

            if (!band.Components.OfType<StiTable>().Any(t => t.Name == table.Name))
            {
                band.Components.Add(table);
            }
        }
    }


    /// <summary>
    /// ExtractReportVariables
    /// </summary>
    /// <param name="report"></param>
    /// <returns></returns>
    public static Dictionary<string, object> ExtractReportVariables(StiReport report)
    {
        var reportVariables = report.Dictionary.Variables.ToList();
        Dictionary<string, object> reportParameters = new();

        foreach (var variable in reportVariables)
        {
            object value = report.GetValue(variable.Name);
            reportParameters[variable.Name] = value ?? DBNull.Value;
        }

        return reportParameters;
    }


    /// <summary>
    /// GenerateDynamicExpressions
    /// </summary>
    /// <param name="report"></param>
    /// <returns></returns>
    #region GenerateDynamicExpressions
    public static void GenerateDynamicExpressions(StiReport report, List<ReportDesign> reportDesigns, Dictionary<string, object> userInput, IDataMappingService dataMappingService)
    {
        if (reportDesigns == null || reportDesigns.Count == 0 || userInput == null)
        {
            return;
        }

        // Cache lưu lại kết quả datasource theo config+userinput
        var dataSourceCache = new Dictionary<string, object>();

        StiBrush hatchBrush = new StiHatchBrush(
            HatchStyle.BackwardDiagonal,
            System.Drawing.Color.FromArgb(50, 50, 50),
            System.Drawing.Color.FromArgb(100, 100, 100)
        );

        var expressionFields = reportDesigns
            .Where(r => r.ComponentType == "Expression")
            .ToList();

        var queryHeader = new List<Dictionary<string, object>>();

        string dataSourceName = string.Empty;

        foreach (var field in expressionFields)
        {
            dataSourceName = field.DataSource;

            object value = string.Empty;

            if (!string.IsNullOrEmpty(field.DataSource))
            {
                value = DataSourceProcessor.ProcessDataSource(dataSourceName, userInput, field.TargetValue, dataMappingService, true, dataSourceCache).Result;
            }
            else if (userInput.TryGetValue(field.FieldName, out var inputValue))
            {
                value = O24OpenAPI.Core.Utils.StringUtils.ApplyMask(inputValue, field.Mask);
            }

            if (value != null)
            {
                string bandName = field.BandName ?? "PageHeader";
                string bandType = field.BandType ?? "Header";

                var band = report.Pages[0].Components.OfType<StiBand>().FirstOrDefault(b => b.Name == field.BandName);

                if (band == null)
                {
                    band = bandType switch
                    {
                        "Header" => new StiHeaderBand { Name = bandName },
                        "Footer" => new StiFooterBand { Name = bandName },
                        "GroupHeader" => new StiGroupHeaderBand { Name = bandName },
                        "GroupFooter" => new StiGroupFooterBand { Name = bandName },
                        "PageHeader" => new StiPageHeaderBand { Name = bandName },
                        "PageFooter" => new StiPageFooterBand { Name = bandName },
                        "Data" => new StiDataBand { Name = bandName },
                        _ => new StiPageHeaderBand { Name = bandName }
                    };

                    band.Brush = new StiSolidBrush(System.Drawing.Color.Transparent);
                    report.Pages[0].Components.Add(band);
                }

                var existingTextComponent = band.Components.OfType<StiText>().FirstOrDefault(text => text.Name == field.ComponentName);

                if (existingTextComponent != null)
                {
                    existingTextComponent.Text = value.ToString() ?? string.Empty;
                }
                else
                {

                    Stimulsoft.Drawing.FontFamily fontFamily;
                    try
                    {
                        fontFamily = new Stimulsoft.Drawing.FontFamily(field.FontFamily ?? "Century Gothic");
                    }
                    catch
                    {
                        fontFamily = new Stimulsoft.Drawing.FontFamily("Century Gothic");
                    }

                    StiText stiText = new()
                    {
                        Name = field.ComponentName,
                        Text = value.ToString() ?? string.Empty,
                        Font = new Stimulsoft.Drawing.Font(fontFamily, field.FontSize ?? 12, field.FontStyle == "Bold" ? FontStyle.Bold : field.FontStyle == "Italic" ? FontStyle.Italic : FontStyle.Regular),
                        Width = (float)Convert.ToSingle(field.Width),
                        Height = (float)Convert.ToSingle(field.Height),
                        Left = (float)Convert.ToSingle(field.PositionX),
                        Top = (float)Convert.ToSingle(field.PositionY),
                        Brush = field.BackgroundColor != null ? new StiSolidBrush(ColorTranslator.FromHtml(field.BackgroundColor ?? "#FFFFFF")) : new StiSolidBrush(System.Drawing.Color.Transparent),
                        TextBrush = field.TextBrush != null ? new StiSolidBrush(field.TextBrush) : hatchBrush,
                        HorAlignment = field.HorAlignment switch
                        {
                            "Center" => StiTextHorAlignment.Center,
                            "Right" => StiTextHorAlignment.Right,
                            _ => StiTextHorAlignment.Left
                        },

                        VertAlignment = field.VertAlignment switch
                        {
                            "Center" => StiVertAlignment.Center,
                            "Bottom" => StiVertAlignment.Bottom,
                            _ => StiVertAlignment.Top // Default
                        }

                    };

                    if (!string.IsNullOrEmpty(field.BorderColor))
                    {
                        stiText.Border = new StiBorder(StiBorderSides.All,
                                                       ColorTranslator.FromHtml(field.BorderColor),
                                                       (float)Convert.ToSingle(field.BorderThickness),
                                                       StiPenStyle.Solid);
                    }
                    band.Components.Add(stiText);
                }

            }
        }

    }

    #endregion




    /// <summary>
    /// Create DataSource Dynamic From List Dictionary
    /// Register Stimulsoft Report.
    /// </summary>
    #region GenerateDynamicDataSource
    public static void GenerateDynamicDataSource(StiReport report, List<ReportData> listReportData, List<ReportDesign> reportDesigns)
    {
        if (listReportData == null || listReportData.Count == 0 || reportDesigns == null || reportDesigns.Count == 0)
        {
            return;
        }

        foreach (var reportData in listReportData)
        {
            string dataBandName = reportData.DataBand;

            if (string.IsNullOrEmpty(reportData.DataSourceName) || string.IsNullOrEmpty(dataBandName))
            {
                continue;
            }

            var dataBindFields = reportDesigns
                .Where(r => r.ComponentType == "DataColumn" && r.DataSource == reportData.DataSourceName && r.ComponentParent == reportData.ParentDatatable)
                .ToList();

            if (!dataBindFields.Any())
            {
                continue;
            }

            var dataTable = new DataTable(reportData.DataSourceName);
            foreach (var field in dataBindFields)
            {
                if (!dataTable.Columns.Contains(field.FieldName))
                {
                    dataTable.Columns.Add(field.FieldName, typeof(string));
                }
            }

            report.RegData(reportData.DataSourceName, dataTable);
            report.Dictionary.Synchronize();

            var dataBand = report.Pages[0].Components.OfType<StiDataBand>().FirstOrDefault(b => b.Name == dataBandName);

            if (dataBand == null)
            {
                dataBand = new StiDataBand
                {
                    Name = dataBandName,
                    DataSourceName = reportData.DataSourceName
                };
                report.Pages[0].Components.Add(dataBand);
            }
            else
            {
                dataBand.DataSourceName = reportData.DataSourceName;
            }

        }
    }
    #endregion

    /// <summary>
    /// GenerateDynamicDatabind
    /// </summary>
    /// <param name="report"></param>
    /// <param name="dataSourceName"></param>
    /// <param name="jsonConfig"></param>
    /// <param name="userInput"></param>
    /// <returns></returns>
    #region GenerateDynamicDatabind
    public static void GenerateDynamicDatabind(StiReport report,
                                                     List<ReportData> listReportData,
                                                     List<ReportDesign> reportDesigns,
                                                     Dictionary<string, object> userInput,
                                                     IDataMappingService dataMappingService)
    {

        DataTable dataTable = new();

        if (listReportData == null || listReportData.Count == 0 || reportDesigns == null || reportDesigns.Count == 0)
        {
            return;
        }

        foreach (var reportData in listReportData)
        {
            string datasource = reportData.DataSource;
            string dataSourceName = reportData.DataSourceName;
            if (string.IsNullOrEmpty(dataSourceName) || string.IsNullOrEmpty(reportData.DataBand))
            {
                return;
            }

            var resultData = DataSourceProcessor.ProcessDataSource(datasource, userInput, dataSourceName, dataMappingService).Result;

            if (resultData == null)
            {
                Console.WriteLine("⚠ Warning: No data retrieved from DataSource.");
                return;
            }

            if (resultData is string jsonString)
            {
                dataTable = ConvertJsonToDataTable(jsonString);
            }

            try
            {
                report.RegData(dataSourceName, dataTable);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error registering data source: {ex.Message}");
            }

        }


    }
    #endregion

    #region ConvertJsonToDataTable
    private static DataTable ConvertJsonToDataTable(string jsonString)
    {
        try
        {
            DataTable dataTable = new DataTable();
            var dataList = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(jsonString);

            if (dataList != null && dataList.Count > 0)
            {
                foreach (var key in dataList.First().Keys)
                {
                    dataTable.Columns.Add(key, typeof(string));
                }

                foreach (var dict in dataList)
                {
                    var row = dataTable.NewRow();
                    foreach (var key in dict.Keys)
                    {
                        row[key] = dict[key]?.ToString() ?? DBNull.Value.ToString();
                    }
                    dataTable.Rows.Add(row);
                }
            }

            return dataTable;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error converting JSON to DataTable: {ex.Message}");
            return null;
        }
    }
    #endregion

    #region LoadPaperKind
    public static PaperKind LoadPaperKind(string paperKind)
    {
        return paperKind switch
        {
            "A4" => PaperKind.A4,
            "A3" => PaperKind.A3,
            "A5" => PaperKind.A5,
            "Letter" => PaperKind.Letter,
            "Legal" => PaperKind.Legal,
            "Tabloid" => PaperKind.Tabloid,
            _ => PaperKind.Letter
        };
    }
    #endregion

}
