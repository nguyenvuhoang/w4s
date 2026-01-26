namespace O24OpenAPI.Kit.OCR.Models;

public sealed class OcrRequest
{
    public string? Language { get; init; }
    public bool CollectWords { get; init; } = false;

    //
    public OcrDocumentType DocumentType { get; init; } = OcrDocumentType.Auto;

    // clean text output
    public bool CleanText { get; init; } = true;

    // (tuỳ chọn) lọc word theo confidence
    public float? MinWordConfidence { get; init; } = null;
}
