namespace O24OpenAPI.Kit.OCR.Models;

public sealed class OcrResult
{
    public string Text { get; init; } = string.Empty;

    /// <summary>0..1</summary>
    public float MeanConfidence { get; init; }

    public string Language { get; init; } = string.Empty;

    public IReadOnlyList<OcrWord> Words { get; init; } = Array.Empty<OcrWord>();
}
