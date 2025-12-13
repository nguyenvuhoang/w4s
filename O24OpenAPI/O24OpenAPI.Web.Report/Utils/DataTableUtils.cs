using System.Data;
using System.Reflection;

namespace O24OpenAPI.Web.Report.Utils;

public static class DataTableUtils
{
    public static DataTable ToDataTable(IEnumerable<object> items)
    {
        var dt = new DataTable();
        bool columnsCreated = false;

        foreach (var item in items)
        {
            if (item == null)
            {
                continue;
            }

            var type = item.GetType();
            var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            if (!columnsCreated)
            {
                foreach (var p in props)
                {
                    var colType = Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType;
                    dt.Columns.Add(p.Name, colType);
                }
                columnsCreated = true;
            }

            var row = dt.NewRow();
            foreach (var p in props)
            {
                row[p.Name] = p.GetValue(item) ?? DBNull.Value;
            }
            dt.Rows.Add(row);
        }

        return dt;
    }
}
