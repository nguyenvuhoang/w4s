using ExcelDataReader;
using Newtonsoft.Json.Linq;
using O24OpenAPI.CMS.API.Application.Constants;
using O24OpenAPI.CMS.API.Application.Models;
using O24OpenAPI.Core.Extensions;
using System.Data;
using System.Reflection;
using System.Text.RegularExpressions;

namespace O24OpenAPI.CMS.Utils;

public partial class FileUtils
{
    private static List<T> ReadExcelData<T>(
        string path,
        Func<DataRow, List<DataColumn>, T> createEntity
    )
    {
        List<T> listData = [];

        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

        using (FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read))
        using (IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream))
        {
            var config = new ExcelDataSetConfiguration
            {
                ConfigureDataTable = _ => new ExcelDataTableConfiguration { UseHeaderRow = true },
            };
            DataSet result = reader.AsDataSet(config);
            DataTable table = result.Tables[0];

            var columns = table.Columns.Cast<DataColumn>().ToList();

            foreach (DataRow row in table.Rows)
            {
                T entity = createEntity(row, columns);
                listData.Add(entity);
            }
        }

        return listData;
    }

    public static List<TEntity> ReadExcel<TEntity>(string path)
        where TEntity : BaseEntity
    {
        return ReadExcelData(
            path,
            (row, columns) =>
            {
                TEntity entity = Activator.CreateInstance<TEntity>();
                foreach (PropertyInfo prop in typeof(TEntity).GetProperties())
                {
                    if (
                        columns.Any(c => c.ColumnName == prop.Name)
                        && row[prop.Name] != DBNull.Value
                    )
                    {
                        prop.SetValue(
                            entity,
                            Convert.ChangeType(row[prop.Name], prop.PropertyType)
                        );
                    }
                }
                return entity;
            }
        );
    }

    public static List<C_CODELIST> ReadExcelCodeList(
        string path,
        string channelId = AppCode.TellerApp
    )
    {
        return ReadExcelData(
            path,
            (row, columns) =>
            {
                CodeListWithExcel model = new();
                foreach (PropertyInfo prop in typeof(CodeListWithExcel).GetProperties())
                {
                    if (
                        columns.Any(c => c.ColumnName == prop.Name)
                        && row[prop.Name] != DBNull.Value
                    )
                    {
                        prop.SetValue(model, Convert.ChangeType(row[prop.Name], prop.PropertyType));
                    }
                }
                return new C_CODELIST()
                {
                    CodeId = model.CDID,
                    CodeName = model.CDNAME,
                    CodeGroup = model.CDGRP,
                    Caption = model.CAPTION,
                    CodeIndex = model.CDIDX,
                    CodeValue = model.CDVAL,
                    Ftag = model.FTAG,
                    Visible = model.VISIBLE == 1,
                    MCaption = new JObject()
                    {
                        { "en", model.CAPTION },
                        { "vi", "" },
                        { "lo", model.LAO_LANGUAGE },
                        { "zh", "" },
                    }.ToSerialize(),
                    App = channelId,
                };
            }
        );
    }

    public static string ValidateAndFixJsonString(string input)
    {
        if (input.NullOrEmpty())
        {
            return input;
        }

        try
        {
            // Try parse as JSON first
            JObject.Parse(input);
            return input;
        }
        catch
        {
            var cleanInput = input
                .Replace("\"\"\"", "\"") // Fix triple quotes
                .Replace("\"\"", "\"") // Fix double quotes
                .Trim();
            cleanInput = cleanInput.Replace("\"vi\" : \",", "\"vi\" : \"\",");

            try
            {
                // Validate fixed JSON
                JObject.Parse(cleanInput);
                return cleanInput;
            }
            catch
            {
                // If still invalid, return empty JSON object
                return input;
            }
        }
    }

    public static string TryGetFieldName(string raw)
    {
        try
        {
            var jsonString = ValidateAndFixJsonString(raw);
            JObject jo = JObject.Parse(jsonString);
            var enValue = jo["en"]?.ToString();
            if (enValue.NullOrEmpty())
            {
                return string.Empty;
            }

            return MyRegex().Replace(enValue, "").ToLower();
        }
        catch
        {
            return string.Empty;
        }
    }

    [GeneratedRegex(@"[^a-zA-Z0-9]")]
    private static partial Regex MyRegex();

    public static string GetInputType(string type)
    {
        var result = type switch
        {
            "TET" => "String",
            "TXT" => "String",
            "CBO" => "String",
            "MKT" => "String",
            "MKN" => "Number",
            "CCY" => "Number",
            "MKD" => "DateTime",
            _ => "String",
        };

        return result;
    }
}
