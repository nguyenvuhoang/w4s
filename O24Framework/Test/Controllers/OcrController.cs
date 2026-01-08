using Microsoft.AspNetCore.Mvc;
using O24OpenAPI.Kit.OCR.Abstractions;
using O24OpenAPI.Kit.OCR.Models;
using O24OpenAPI.Kit.OCR.Parsing; // parser dispatcher
using O24OpenAPI.Kit.OCR.Test;

namespace Test.Controllers;

[ApiController]
[Route("api/ocr")]
[ApiExplorerSettings(IgnoreApi = true)] // Ẩn khỏi Swagger
public sealed class OcrController : ControllerBase
{
    private readonly IOcrService _ocr;

    public OcrController(IOcrService ocr)
    {
        _ocr = ocr ?? throw new ArgumentNullException(nameof(ocr));
    }

    // =========================
    // 1) OCR from multipart image
    // =========================
    // Postman: form-data => key "file" type File
    // Example:
    // POST /api/ocr/image?language=vie&documentType=Invoice&collectWords=false&cleanText=true
    [HttpPost("image")]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(40_000_000)]
    public async Task<IActionResult> ReadImage(
        [FromForm] IFormFile file,
        [FromQuery] string? language = null,
        [FromQuery] bool collectWords = false,
        [FromQuery] string? documentType = null,
        [FromQuery] bool cleanText = true,
        CancellationToken ct = default)
    {
        if (file == null || file.Length == 0)
            return BadRequest("file is required");

        var dt = ParseDocType(documentType);

        var req = new OcrRequest
        {
            Language = language,
            CollectWords = collectWords,
            DocumentType = dt,
            CleanText = cleanText
        };

        await using var stream = file.OpenReadStream();
        var result = await _ocr.ReadAsync(stream, req, ct);

        // Return parsed + raw
        var parsed = OcrParserDispatcher.Parse(
            docType: MapDocType(dt),
            rawText: result.Text,
            language: result.Language,
            meanConfidence: result.MeanConfidence
        );

        return Ok(parsed);

        // Nếu muốn giữ output cũ:
        // return Ok(result);
    }

    // =========================
    // 2) OCR from JSON base64
    // =========================
    // POST /api/ocr/base64
    // Body: { "base64": "...", "language":"vie", "collectWords":false, "documentType":"Invoice", "cleanText":true }
    [HttpPost("base64")]
    [Consumes("application/json")]
    [RequestSizeLimit(40_000_000)]
    public async Task<IActionResult> ReadBase64(
        [FromBody] OcrBase64Request body,
        CancellationToken ct = default)
    {
        if (body is null) return BadRequest("Body is required");
        if (string.IsNullOrWhiteSpace(body.Base64)) return BadRequest("Base64 is required");

        byte[] bytes;
        try
        {
            bytes = Base64ImageDecoder.DecodeToBytes(body.Base64);

            // LOG magic bytes + length để biết format gì
            var head = BitConverter.ToString(bytes.Take(16).ToArray());
            var tail = BitConverter.ToString(bytes.TakeLast(16).ToArray());
            Console.WriteLine($"[OCR base64] len={bytes.Length}, head={head}");
            Console.WriteLine($"[OCR base64] tail={tail}");

            // (optional) debug write
            // System.IO.File.WriteAllBytes("debug_input.bin", bytes);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        var dt = ParseDocType(body.DocumentType);

        var req = new OcrRequest
        {
            Language = body.Language,
            CollectWords = body.CollectWords,
            DocumentType = dt,
            CleanText = body.CleanText
        };

        var result = await _ocr.ReadAsync(bytes, req, ct);

        var parsed = OcrParserDispatcher.Parse(
            docType: MapDocType(dt),
            rawText: result.Text,
            language: result.Language,
            meanConfidence: result.MeanConfidence
        );

        return Ok(parsed);

        // Nếu muốn giữ output cũ:
        // return Ok(result);
    }

    // =========================
    // helpers
    // =========================
    private static OcrDocumentType ParseDocType(string? docType)
    {
        if (!string.IsNullOrWhiteSpace(docType) &&
            Enum.TryParse<OcrDocumentType>(docType, true, out var parsed))
            return parsed;

        return OcrDocumentType.Auto;
    }

    private static SimpleDocType MapDocType(OcrDocumentType dt)
    {
        return dt switch
        {
            OcrDocumentType.Invoice => SimpleDocType.Invoice,
            OcrDocumentType.IdCard => SimpleDocType.IdCard,
            _ => SimpleDocType.Auto
        };
    }


    //Test
    [HttpPost("lao/address")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> TestLaoAddress(
[FromForm] IFormFile file,
[FromQuery] string? language = "lao+eng",
CancellationToken ct = default)
    {
        if (file == null || file.Length == 0) return BadRequest("file is required");

        var req = new OcrRequest
        {
            Language = language,
            CollectWords = false,
            DocumentType = OcrDocumentType.IdCard,
            CleanText = true
        };

        await using var stream = file.OpenReadStream();
        var result = await _ocr.ReadAsync(stream, req, ct);

        var addr = LaoAddressTestHelper.ExtractAddressOnly(result.Text);

        return Ok(new
        {
            address = addr,
            meanConfidence = result.MeanConfidence,
            language = result.Language,
            rawText = result.Text
        });
    }

}
