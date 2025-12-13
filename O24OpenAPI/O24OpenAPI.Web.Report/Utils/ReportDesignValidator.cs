using O24OpenAPI.Web.Report.Domain;

namespace O24OpenAPI.Web.Report.Utils;

public static class ReportDesignValidator
{
    // Kiểm tra tên identifier hợp lệ cho C#
    private static bool IsValidIdentifier(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return false;
        }

        if (!char.IsLetter(name[0]) && name[0] != '_')
        {
            return false;
        }

        for (int i = 1; i < name.Length; i++)
        {
            char c = name[i];
            if (!char.IsLetterOrDigit(c) && c != '_')
            {
                return false;
            }
        }
        return true;
    }

    // Kiểm tra text có ký tự nguy hiểm
    private static bool HasDangerousChars(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return false;
        }

        return text.Contains("\"") || text.Contains("\r") || text.Contains("\n");
    }

    public static void ValidateDesigns(List<ReportDesign> designs)
    {
        foreach (var d in designs)
        {
            // Check ComponentName
            if (!IsValidIdentifier(d.ComponentName))
            {
                Console.WriteLine($"⚠ ComponentName không hợp lệ: '{d.ComponentName}'");
            }

            // Check FieldName
            if (!string.IsNullOrEmpty(d.FieldName) && !IsValidIdentifier(d.FieldName))
            {
                Console.WriteLine($"⚠ FieldName không hợp lệ: '{d.FieldName}'");
            }

            // Check Text
            if (HasDangerousChars(d.Text))
            {
                Console.WriteLine($"⚠ Text có ký tự đặc biệt: '{d.Text}'");
            }

            // Check dấu ngoặc nhọn
            if (!string.IsNullOrEmpty(d.Text) && d.Text.Contains("{") && !d.Text.Contains("{{"))
            {
                Console.WriteLine($"⚠ Text có thể cần escape dấu {{ }}: '{d.Text}'");
            }
        }
    }
}
