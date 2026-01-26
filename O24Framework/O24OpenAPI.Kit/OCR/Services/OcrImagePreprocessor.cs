using O24OpenAPI.Kit.OCR.Models;
using OpenCvSharp;

namespace O24OpenAPI.Kit.OCR.Services;

public static class OcrImagePreprocessor
{
    public static byte[] Preprocess(byte[] input, OcrDocumentType docType)
    => docType switch
    {
        OcrDocumentType.Receipt => PreprocessReceipt(input),
        OcrDocumentType.Invoice => PreprocessInvoice(input),
        _ => input
    };

    public static byte[] PreprocessReceipt(byte[] input)
    {
        using var src = Cv2.ImDecode(input, ImreadModes.Color);
        if (src.Empty()) return input;

        using var gray = new Mat();
        Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);

        // 1) Tạo mask vùng không-đen để cắt bỏ viền đen (receipt hay có background đen)
        using var mask = new Mat();
        Cv2.Threshold(gray, mask, 15, 255, ThresholdTypes.Binary);

        // FindNonZero dạng OutputArray (không return)
        using var nonZero = new Mat();
        Cv2.FindNonZero(mask, nonZero);

        if (nonZero.Empty())
            return input;

        // BoundingRect nhận InputArray points
        var rect = Cv2.BoundingRect(nonZero);

        // Nới rect cho an toàn
        rect.X = Math.Max(0, rect.X - 5);
        rect.Y = Math.Max(0, rect.Y - 5);
        rect.Width = Math.Min(gray.Width - rect.X, rect.Width + 10);
        rect.Height = Math.Min(gray.Height - rect.Y, rect.Height + 10);

        using var cropped = new Mat(gray, rect);

        // 2) Phóng to chữ (receipt chữ nhỏ)
        using var up = new Mat();
        Cv2.Resize(cropped, up, new Size(), 3.0, 3.0, InterpolationFlags.Cubic);

        // 3) Otsu threshold để chữ nổi rõ
        using var bin = new Mat();
        Cv2.Threshold(up, bin, 0, 255, ThresholdTypes.Binary | ThresholdTypes.Otsu);

        // Nếu ảnh ra chữ trắng nền đen thì bật dòng này:
        // Cv2.BitwiseNot(bin, bin);

        Cv2.ImEncode(".png", bin, out var buf);

        return buf.ToArray();
    }
    // invoice: crop viền + remove lines bảng + upscale + threshold
    public static byte[] PreprocessInvoice(byte[] input)
    {
        using var src = Cv2.ImDecode(input, ImreadModes.Color);
        if (src.Empty()) return input;

        using var gray = new Mat();
        Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);

        // 1) Crop nhẹ để bỏ viền trang trí (2%)
        int mx = (int)(gray.Width * 0.02);
        int my = (int)(gray.Height * 0.02);
        var rect = new Rect(mx, my, gray.Width - 2 * mx, gray.Height - 2 * my);
        rect.Width = Math.Max(1, rect.Width);
        rect.Height = Math.Max(1, rect.Height);

        using var cropped = new Mat(gray, rect);

        // 2) Upscale để chữ rõ hơn
        using var up = new Mat();
        Cv2.Resize(cropped, up, new Size(), 2.0, 2.0, InterpolationFlags.Cubic);

        // 3) Binary INV (text + lines sẽ trắng trên nền đen)
        using var binInv = new Mat();
        Cv2.Threshold(up, binInv, 0, 255, ThresholdTypes.BinaryInv | ThresholdTypes.Otsu);

        // 4) Tách line ngang
        using var horizontal = new Mat();
        binInv.CopyTo(horizontal);
        var hSize = Math.Max(40, up.Width / 30);
        using var hKernel = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(hSize, 1));
        Cv2.Erode(horizontal, horizontal, hKernel);
        Cv2.Dilate(horizontal, horizontal, hKernel);

        // 5) Tách line dọc
        using var vertical = new Mat();
        binInv.CopyTo(vertical);
        var vSize = Math.Max(40, up.Height / 30);
        using var vKernel = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(1, vSize));
        Cv2.Erode(vertical, vertical, vKernel);
        Cv2.Dilate(vertical, vertical, vKernel);

        // 6) Combine lines rồi remove khỏi ảnh
        using var lines = new Mat();
        Cv2.BitwiseOr(horizontal, vertical, lines);

        using var noLines = new Mat();
        Cv2.Subtract(binInv, lines, noLines);
        // Remove noise nhỏ sau khi trừ lines
        using var small = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(2, 2));
        Cv2.MorphologyEx(noLines, noLines, MorphTypes.Open, small);

        // 7) Đảo lại về chữ đen nền trắng (Tesseract thường thích vậy)
        using var outBin = new Mat();
        Cv2.BitwiseNot(noLines, outBin);

        Cv2.ImEncode(".png", outBin, out var buf);
        return buf.ToArray();
    }
}
