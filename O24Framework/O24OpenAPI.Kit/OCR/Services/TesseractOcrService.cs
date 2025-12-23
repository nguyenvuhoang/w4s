using O24OpenAPI.Kit.OCR.Abstractions;
using O24OpenAPI.Kit.OCR.Models;
using O24OpenAPI.Kit.OCR.Options;
using Tesseract;

namespace O24OpenAPI.Kit.OCR.Services;

public sealed class TesseractOcrService : IOcrService
{
    private readonly TesseractOcrOptions _options;

    public TesseractOcrService(TesseractOcrOptions options)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
    }

    public async Task<OcrResult> ReadAsync(
        Stream imageStream,
        OcrRequest? request = null,
        CancellationToken cancellationToken = default)
    {
        if (imageStream is null) throw new ArgumentNullException(nameof(imageStream));

        await using var ms = new MemoryStream();
        await imageStream.CopyToAsync(ms, cancellationToken).ConfigureAwait(false);

        return await ReadAsync(ms.ToArray(), request, cancellationToken).ConfigureAwait(false);
    }

    public Task<OcrResult> ReadAsync(
        byte[] imageBytes,
        OcrRequest? request = null,
        CancellationToken cancellationToken = default)
    {
        if (imageBytes is null) throw new ArgumentNullException(nameof(imageBytes));
        if (imageBytes.Length == 0) throw new ArgumentException("Empty imageBytes", nameof(imageBytes));

        cancellationToken.ThrowIfCancellationRequested();

        var docType = request?.DocumentType ?? OcrDocumentType.Auto;

        var lang = NormalizeLang(
            string.IsNullOrWhiteSpace(request?.Language) ? _options.DefaultLanguage : request!.Language!
        );

        EnsureTessdataFolder(_options.TessdataPath);
        EnsureTraineddataExists(_options.TessdataPath, lang);

        using var engine = new TesseractEngine(_options.TessdataPath, lang, _options.EngineMode);

        // PSM: IdCard thường là nhiều dòng rải rác -> SparseText đọc ổn hơn SingleBlock
        engine.DefaultPageSegMode = docType == OcrDocumentType.IdCard
            ? PageSegMode.SparseText
            : _options.PageSegMode;

        // Gợi ý chung
        engine.SetVariable("user_defined_dpi", "300");
        engine.SetVariable("preserve_interword_spaces", "1");

        foreach (var kv in _options.EngineVariables)
            engine.SetVariable(kv.Key, kv.Value);

        Console.WriteLine($"[OCR] lang='{lang}', docType={docType}, psm={engine.DefaultPageSegMode}");

        // ===== Preprocess =====
        var debugDir = @"D:\O24\ocr_debug";
        Directory.CreateDirectory(debugDir);

        var workingBytes = imageBytes;

        // Chỉ receipt-preprocess cho hóa đơn
        if (docType == OcrDocumentType.Invoice)
        {
            workingBytes = OcrImagePreprocessor.PreprocessReceipt(workingBytes);
            File.WriteAllBytes(Path.Combine(debugDir, "preprocessed_receipt.png"), workingBytes);
        }

        workingBytes = OcrImagePreprocessor.Preprocess(workingBytes, docType);
        File.WriteAllBytes(Path.Combine(debugDir, "preprocessed_final.png"), workingBytes);

        using var pix = Pix.LoadFromMemory(workingBytes);
        using var page = engine.Process(pix);

        var text = page.GetText() ?? string.Empty;

        if (request?.CleanText != false)
            text = OcrTextCleaner.Clean(text);

        text = text.Trim();

        // MeanConfidence có thể “ảo” khi gần như không có chữ
        var mean = page.GetMeanConfidence();
        if (string.IsNullOrWhiteSpace(text))
            mean = 0;

        var result = new OcrResult
        {
            Text = text,
            MeanConfidence = mean,
            Language = lang,
            Words = request?.CollectWords == true ? ExtractWords(page) : Array.Empty<OcrWord>()
        };

        return Task.FromResult(result);
    }

    private static string NormalizeLang(string lang)
    {
        lang = (lang ?? "").Trim();

        // chống vụ '+' bị decode thành space
        if (lang.Contains(' ') && !lang.Contains('+'))
            lang = lang.Replace(" ", "+");

        // dọn lang kiểu "lao++eng"
        while (lang.Contains("++"))
            lang = lang.Replace("++", "+");

        return lang;
    }

    private static void EnsureTraineddataExists(string tessdataPath, string lang)
    {
        // lang có thể là "lao+eng"
        var parts = lang.Split('+', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        foreach (var p in parts)
        {
            var file = Path.Combine(tessdataPath, $"{p}.traineddata");
            if (!File.Exists(file))
            {
                throw new FileNotFoundException(
                    $"Missing traineddata: '{file}'. " +
                    $"(lang='{lang}')"
                );
            }
        }
    }

    private static void EnsureTessdataFolder(string tessdataPath)
    {
        if (!Directory.Exists(tessdataPath))
        {
            throw new DirectoryNotFoundException(
                $"Tessdata folder not found: '{tessdataPath}'. " +
                "Hãy tạo folder này và copy *.traineddata vào trong."
            );
        }
    }

    private static IReadOnlyList<OcrWord> ExtractWords(Page page)
    {
        var words = new List<OcrWord>();

        using var iterator = page.GetIterator();
        iterator.Begin();

        do
        {
            var wordText = iterator.GetText(PageIteratorLevel.Word);
            if (string.IsNullOrWhiteSpace(wordText))
                continue;

            var conf = iterator.GetConfidence(PageIteratorLevel.Word);

            if (iterator.TryGetBoundingBox(PageIteratorLevel.Word, out var rect))
            {
                words.Add(new OcrWord
                {
                    Text = wordText.Trim(),
                    Confidence = conf,
                    BoundingBox = new OcrBoundingBox
                    {
                        X = rect.X1,
                        Y = rect.Y1,
                        Width = rect.Width,
                        Height = rect.Height
                    }
                });
            }
            else
            {
                words.Add(new OcrWord
                {
                    Text = wordText.Trim(),
                    Confidence = conf
                });
            }
        }
        while (iterator.Next(PageIteratorLevel.Word));

        return words;
    }
}
