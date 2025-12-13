using O24OpenAPI.Data.Configuration;

namespace O24OpenAPI.Web.CMS.Utils;

public class DirUtils
{
    public static string GetPathScriptFunction()
    {
        return $"Migrations/Scripts/{Singleton<DataConfig>.Instance.DataProvider}/Functions";
    }

    public static string GetPathScriptStored()
    {
        {
            return $"Migrations/Scripts/{Singleton<DataConfig>.Instance.DataProvider}/StoredProcedures";
        }
    }

    public static string[] GetPaths(string folderPath, string fileType = null)
    {
        if (!Directory.Exists(folderPath))
        {
            return [];
        }

        if (!string.IsNullOrEmpty(fileType))
        {
            return Directory.GetFiles(folderPath, fileType);
        }
        else
        {
            return Directory.GetFiles(folderPath);
        }
    }
}
