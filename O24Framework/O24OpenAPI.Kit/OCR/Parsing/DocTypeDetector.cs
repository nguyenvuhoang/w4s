using System.Text.RegularExpressions;

namespace O24OpenAPI.Kit.OCR.Parsing
{
    public enum DetectedDocType
    {
        Unknown = 0,
        IdCard = 1,
        Invoice = 2
        // sau này thêm Passport, DriverLicense...
    }

    public sealed class DocDetectResult
    {
        public DetectedDocType Type { get; set; }
        public double Confidence { get; set; } // 0..1
        public Dictionary<DetectedDocType, double> Scores { get; set; } = new();
        public string? Reason { get; set; }
    }

    public static class DocTypeDetector
    {
        // CCCD/ID signals (VN + Lao chung)
        private static readonly string[] IdKw =
        {
            // Lao ID
            "ບັດປະຈໍາຕົວ", "ເລກທີ", "ວັນເດືອນປີເກີດ", "ອອກໃຫ້ວັນທີ", "ຫົດກໍານົດ",
            "ເຊື້ອຊາດ", "ສັນຊາດ",

            // VN ID (tuỳ code bổ sung thêm)
            "CĂN CƯỚC CÔNG DÂN", "CHỨNG MINH NHÂN DÂN", "HỌ VÀ TÊN", "NGÀY SINH", "QUỐC TỊCH"
        };

        private static readonly string[] InvoiceKw =
        {
            "INVOICE", "HÓA ĐƠN", "VAT", "MST", "TỔNG TIỀN", "THÀNH TIỀN", "SỐ HÓA ĐƠN"
        };

        // Lao ID number kiểu 04-19 001381
        private static readonly Regex RxLaoId = new(@"(?<!\d)(\d{2})\s*[-–—]?\s*(\d{2})\s*[-–—\s]?\s*(\d{6})(?!\d)",
            RegexOptions.Compiled);

        public static DocDetectResult Detect(string rawText)
        {
            rawText ??= "";

            double idScore = ScoreKeywords(rawText, IdKw) + (RxLaoId.IsMatch(rawText) ? 3.0 : 0.0);
            double invScore = ScoreKeywords(rawText, InvoiceKw);

            var scores = new Dictionary<DetectedDocType, double>
            {
                [DetectedDocType.IdCard] = idScore,
                [DetectedDocType.Invoice] = invScore,
                [DetectedDocType.Unknown] = 0
            };

            var best = scores.Where(kv => kv.Key != DetectedDocType.Unknown)
                             .OrderByDescending(kv => kv.Value).First();
            var second = scores.Where(kv => kv.Key != DetectedDocType.Unknown)
                               .OrderByDescending(kv => kv.Value).Skip(1).First();

            // chống đoán bừa
            if (best.Value < 2.5 || (best.Value - second.Value) < 1.0)
            {
                return new DocDetectResult
                {
                    Type = DetectedDocType.Unknown,
                    Confidence = 0,
                    Scores = scores,
                    Reason = $"ambiguous(best={best.Value:0.##}, second={second.Value:0.##})"
                };
            }

            var conf = Math.Clamp(best.Value / 8.0, 0, 1);
            return new DocDetectResult
            {
                Type = best.Key,
                Confidence = conf,
                Scores = scores,
                Reason = $"best={best.Key} score={best.Value:0.##}"
            };
        }

        private static double ScoreKeywords(string raw, string[] kws)
        {
            double s = 0;
            foreach (var kw in kws)
                if (raw.IndexOf(kw, StringComparison.OrdinalIgnoreCase) >= 0)
                    s += 1.0;
            return s;
        }
    }
}
