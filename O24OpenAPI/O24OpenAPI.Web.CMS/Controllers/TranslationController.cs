using Microsoft.AspNetCore.Mvc;
using O24OpenAPI.Web.CMS.Services.Interfaces;
using O24OpenAPI.Framework.Controllers;

namespace O24OpenAPI.Web.CMS.Controllers;

public class TranslationController(ITranslationService translationService) : BaseController
{
    private readonly ITranslationService _translationService = translationService;
    [HttpGet]
    public IActionResult ExportExcel()
    {
        var translations = _translationService.Load();
        var fileBytes = _translationService.ExportToExcel(translations);
        var fileName = $"translations_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

        return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
    }

    [HttpPost]
    public async Task<IActionResult> ImportExcel(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("File is empty");
        }

        try
        {
            using var stream = file.OpenReadStream();
            var entries = _translationService.ImportFromExcel(stream);
            await _translationService.SaveTranslationsToJsonAsync(entries);

            return Ok("Import success");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Import failed: {ex.Message}");
        }
    }


}
