using O24OpenAPI.CMS.Domain.AggregateModels;

namespace O24OpenAPI.CMS.API.Application.Services.Interfaces;

public interface ITranslationService
{
    List<TranslationEntry> Load();

    //byte[] ExportToExcel(List<TranslationEntry> translations);
    //List<TranslationEntry> ImportFromExcel(Stream fileStream);
    Task SaveTranslationsToJsonAsync(List<TranslationEntry> entries);
}
