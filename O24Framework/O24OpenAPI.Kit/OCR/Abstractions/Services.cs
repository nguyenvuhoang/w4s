using O24OpenAPI.Kit.OCR.Models;

namespace O24OpenAPI.Kit.OCR.Abstractions;

public interface IOcrService
{
    Task<OcrResult> ReadAsync(
        Stream imageStream,
        OcrRequest? request = null,
        CancellationToken cancellationToken = default
    );

    Task<OcrResult> ReadAsync(
        byte[] imageBytes,
        OcrRequest? request = null,
        CancellationToken cancellationToken = default
    );
}
