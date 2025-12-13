using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.Data.Utils;

/// <summary>
/// The file utils class
/// </summary>
public class FileUtils
{
    /// <summary>
    /// Reads the csv using the specified path
    /// </summary>
    /// <typeparam name="TEntity">The entity</typeparam>
    /// <param name="path">The path</param>
    /// <param name="delimiter">The delimiter</param>
    /// <returns>The list data</returns>
    public static async Task<List<TEntity>> ReadCSV<TEntity>(
        string path,
        string delimiter = ","
    )
        where TEntity : BaseEntity
    {
        var listData = new List<TEntity>();

        using (var reader = new StreamReader(path))
        using (
            var csv = new CsvReader(
                reader,
                new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = delimiter,
                    MissingFieldFound = null,
                    HeaderValidated = null,
                }
            )
        )
        {
            await csv.ReadAsync();
            csv.ReadHeader();

            while (await csv.ReadAsync())
            {
                var entity = csv.GetRecord<TEntity>();
                listData.Add(entity);
            }
        }

        return listData;
    }

    /// <summary>
    /// Reads the json using the specified path
    /// </summary>
    /// <typeparam name="TEntity">The entity</typeparam>
    /// <param name="path">The path</param>
    /// <returns>A task containing a list of t entity</returns>
    public static async Task<List<TEntity>> ReadJson<TEntity>(string path)
        where TEntity : BaseEntity
    {
        try
        {
            JArray jArray = JArray.Parse(File.ReadAllText(path));

            var jObject = jArray.Children<JObject>().FirstOrDefault(o => o["data"] != null);
            if (jObject is not null)
            {
                var data = jObject.Value<JArray>("data");
                var strData = JsonConvert.SerializeObject(data);

                var listData = JsonConvert.DeserializeObject<List<TEntity>>(strData);
                await Task.CompletedTask;
                return listData;
            }

            return new List<TEntity>();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new List<TEntity>();
        }
    }

    public static void CreateDirectoryIfNotExist(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }

    public static bool FileWriter(string fullPath, string mediaData)
    {
        try
        {
            File.WriteAllText(fullPath, mediaData);
            Console.WriteLine("Media Insert Done at: " + fullPath);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception: " + ex);
            throw;
        }
    }
}
