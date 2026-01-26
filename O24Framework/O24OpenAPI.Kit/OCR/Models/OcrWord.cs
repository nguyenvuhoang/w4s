namespace O24OpenAPI.Kit.OCR.Models;

public sealed class OcrWord
{
    public string Text { get; init; } = string.Empty;

    /// <summary>0..100 (tùy iterator trả về)</summary>
    public float Confidence { get; init; }

    public OcrBoundingBox? BoundingBox { get; init; }
}

public sealed class OcrBoundingBox
{
    public int X { get; init; }
    public int Y { get; init; }
    public int Width { get; init; }
    public int Height { get; init; }
}
