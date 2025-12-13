namespace O24OpenAPI.Web.CMS.Services.Interfaces;

public interface ITranslationService
{
    List<TranslationEntry> Load();

    byte[] ExportToExcel(List<TranslationEntry> translations);
    List<TranslationEntry> ImportFromExcel(Stream fileStream);
    Task SaveTranslationsToJsonAsync(List<TranslationEntry> entries);
}
