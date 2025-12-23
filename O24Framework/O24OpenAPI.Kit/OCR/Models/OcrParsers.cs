using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace O24OpenAPI.Kit.OCR.Parsing;

// ========= 1) Common models =========

public sealed class OcrField<T>
{
    public T? Value { get; set; }
    public double? Confidence { get; set; } // heuristic 0..1 (not OCR engine confidence)
    public string? Source { get; set; }      // where we got it from (line matched)
    public string? Method { get; set; }      // e.g. "regex", "keyword-next-line"
}

public abstract class OcrExtractionBase
{
    public string DocType { get; set; } = "Unknown";   // "IdCardVn", "IdCardLao", "Invoice"
    public string? RawLanguage { get; set; }
    public OcrField<string> DocumentNumber { get; set; } = new(); // CCCD/ID/InvoiceNo if desired
}

public sealed class IdCardVnExtraction : OcrExtractionBase
{
    public OcrField<string> IdNumber { get; set; } = new();
    public OcrField<string> FullName { get; set; } = new();
    public OcrField<DateTime> Dob { get; set; } = new();
    public OcrField<string> Sex { get; set; } = new();
    public OcrField<string> Nationality { get; set; } = new();
    public OcrField<string> PlaceOfOrigin { get; set; } = new();
    public OcrField<string> PlaceOfResidence { get; set; } = new();
    public OcrField<DateTime> Expiry { get; set; } = new();
}

public sealed class IdCardLaoExtraction : OcrExtractionBase
{
    public OcrField<string> IdNumber { get; set; } = new();
    public OcrField<string> FullName { get; set; } = new();
    public OcrField<DateTime> Dob { get; set; } = new();

    public OcrField<string> CurrentAddress { get; set; } = new();
    public OcrField<string> Ethnicity { get; set; } = new();
    public OcrField<string> Nationality { get; set; } = new();

    public OcrField<DateTime> IssueDate { get; set; } = new();
    public OcrField<DateTime> ExpiryDate { get; set; } = new();

    // giữ tương thích ngược nếu client cũ đang đọc Address
    public OcrField<string> Address { get; set; } = new();
}

public sealed class InvoiceExtraction : OcrExtractionBase
{
    public OcrField<string> InvoiceNo { get; set; } = new();
    public OcrField<DateTime> InvoiceDate { get; set; } = new();

    public OcrField<string> SellerName { get; set; } = new();
    public OcrField<string> SellerTaxCode { get; set; } = new();

    public OcrField<string> BuyerName { get; set; } = new();
    public OcrField<string> BuyerTaxCode { get; set; } = new();

    public OcrField<decimal> SubTotal { get; set; } = new();
    public OcrField<decimal> VatRate { get; set; } = new();   // e.g. 8
    public OcrField<decimal> VatAmount { get; set; } = new();
    public OcrField<decimal> Total { get; set; } = new();
}

public sealed class OcrParsedResult
{
    public string RawText { get; set; } = "";
    public string? Language { get; set; }
    public float MeanConfidence { get; set; }

    // exactly one of these is usually filled
    public IdCardVnExtraction? IdCardVn { get; set; }
    public IdCardLaoExtraction? IdCardLao { get; set; }
    public InvoiceExtraction? Invoice { get; set; }
}

// ========= 2) Utilities =========

internal static class OcrTextUtil
{
    private static readonly Regex RxSlashes = new(@"\s*[/\\]{2,}\s*", RegexOptions.Compiled);
    private static readonly Regex RxSpaces = new(@"\s+", RegexOptions.Compiled);
    private static readonly Regex RxCommaSpaces = new(@"\s*,\s*", RegexOptions.Compiled);

    private static readonly HashSet<string> LaoAddrAbbrev = new(StringComparer.Ordinal)
{
"ບ", "ມ", "ຂ", "ນ", "ຕ",
"ບ.", "ມ.", "ຂ.", "ນ.", "ຕ."
};

    public static string NormalizeLaoAddress(string? s)
    {
        if (string.IsNullOrWhiteSpace(s)) return "";

        // 1) bỏ // (hoặc \\) nhưng giữ phần chữ sau đó
        s = RxSlashes.Replace(s, " ");

        // 2) chuẩn hoá dấu phẩy + khoảng trắng
        s = RxCommaSpaces.Replace(s, ", ");

        // 3) gom spaces
        s = RxSpaces.Replace(s, " ").Trim();

        // 4) drop tail noise kiểu ", ເງ" (token ngắn mà không phải ký hiệu địa chỉ)
        var toks = s.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();
        while (toks.Count > 0)
        {
            var last = toks[^1].Trim(',', '.', ';', ':');
            if (last.Length <= 2 && !LaoAddrAbbrev.Contains(last))
                toks.RemoveAt(toks.Count - 1);
            else
                break;
        }

        return string.Join(' ', toks).Trim();
    }
    public static string Normalize(string s)
    {
        if (string.IsNullOrWhiteSpace(s)) return string.Empty;
        s = s.Replace("\r\n", "\n").Replace("\r", "\n");
        s = s.Normalize(NormalizationForm.FormKC);

        // normalize weird OCR spaces
        s = Regex.Replace(s, @"[ \t]+", " ");
        return s.Trim();
    }

    public static List<string> Lines(string raw)
    {
        var t = Normalize(raw);
        return t.Split('\n')
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();
    }

    public static bool ContainsAny(string s, params string[] keys)
    {
        var low = s.ToLowerInvariant();
        foreach (var k in keys)
        {
            if (string.IsNullOrWhiteSpace(k)) continue;
            if (low.Contains(k.ToLowerInvariant())) return true;
        }
        return false;
    }

    public static string CleanupValue(string s)
    {
        if (string.IsNullOrWhiteSpace(s)) return "";
        s = s.Trim();
        s = Regex.Replace(s, @"\s{2,}", " ");
        s = s.Trim('|', ':', '—', '-', '–', ' ');
        return s.Trim();
    }

    // dd/MM/yyyy or dd-MM-yyyy or dd.MM.yyyy
    private static readonly Regex RxDate = new(@"\b(\d{1,2})[\/\-.](\d{1,2})[\/\-.](\d{4})\b",
        RegexOptions.Compiled);

    // OCR merge ddMM/yyyy  (vd: 1007/2019 -> 10/07/2019)
    private static readonly Regex RxDateDdmmyyyy = new(@"\b(\d{2})(\d{2})[\/\-.](\d{4})\b",
        RegexOptions.Compiled);

    // OCR merge dd/MMYYYY  (vd: 10/072019 -> 10/07/2019)
    private static readonly Regex RxDateDdMmYyyy = new(@"\b(\d{1,2})[\/\-.](\d{2})(\d{4})\b",
        RegexOptions.Compiled);

    public static List<(DateTime dt, string source)> FindDates(string raw)
    {
        var list = new List<(DateTime, string)>();

        void AddMatches(Regex rx)
        {
            foreach (Match m in rx.Matches(raw))
                if (TryParseDate(m, out var dt))
                    list.Add((dt, m.Value));
        }

        AddMatches(RxDate);
        AddMatches(RxDateDdmmyyyy);
        AddMatches(RxDateDdMmYyyy);

        return list;
    }

    public static bool TryParseDate(Match m, out DateTime dt)
    {
        dt = default;
        if (m.Groups.Count < 4) return false;

        if (!int.TryParse(m.Groups[1].Value, out var d)) return false;
        if (!int.TryParse(m.Groups[2].Value, out var mo)) return false;
        if (!int.TryParse(m.Groups[3].Value, out var y)) return false;

        try { dt = new DateTime(y, mo, d); return true; } catch { return false; }
    }

    // numbers with separators: 6.602.304 / 6,602,304 / 6602304
    private static readonly Regex RxMoney = new(@"\b\d{1,3}([.,]\d{3})+(?:[.,]\d+)?\b|\b\d+(?:[.,]\d+)?\b",
        RegexOptions.Compiled);

    public static decimal? TryParseMoney(string s)
    {
        if (string.IsNullOrWhiteSpace(s)) return null;

        // grab first money-like token
        var m = RxMoney.Match(s);
        if (!m.Success) return null;

        var token = m.Value.Trim();

        // If has thousand separators, normalize by removing them
        // Heuristic: treat last separator as decimal only if there are 1-2 digits after it
        // For VND typically integer => remove all '.' ',' then parse.
        var justDigits = Regex.Replace(token, @"[^\d]", "");
        if (decimal.TryParse(justDigits, NumberStyles.Number, CultureInfo.InvariantCulture, out var v))
            return v;

        return null;
    }

    public static int BestLineIndexByKeyword(List<string> lines, params string[] keys)
    {
        for (int i = 0; i < lines.Count; i++)
            if (ContainsAny(lines[i], keys)) return i;
        return -1;
    }

    public static string? TakeValueSameLineAfterColon(string line)
    {
        // e.g. "Họ và tên | Full name: NGO HOANG VIET"
        var idx = line.IndexOf(':');
        if (idx < 0) return null;
        var val = line[(idx + 1)..];
        val = CleanupValue(val);
        return string.IsNullOrWhiteSpace(val) ? null : val;
    }

    public static string? TakeNextNonEmpty(List<string> lines, int startIndex)
    {
        for (int i = startIndex; i < lines.Count; i++)
        {
            var v = CleanupValue(lines[i]);
            if (!string.IsNullOrWhiteSpace(v)) return v;
        }
        return null;
    }

    public static bool LooksLikeMostlyLao(string s)
    {
        if (string.IsNullOrWhiteSpace(s)) return false;
        int lao = 0, letters = 0;

        foreach (var ch in s)
        {
            if (char.IsWhiteSpace(ch) || char.IsPunctuation(ch)) continue;
            if (char.IsLetter(ch)) letters++;
            if (ch >= '\u0E80' && ch <= '\u0EFF') lao++; // Lao range
        }

        if (letters == 0) return false;
        return (double)lao / letters >= 0.55;
    }

    public static string StripWeirdOcrNoise(string s)
    {
        if (string.IsNullOrWhiteSpace(s)) return "";
        // remove obvious junk-only lines (but keep Lao letters)
        s = Regex.Replace(s, @"[<>{}\[\]_=~^`]+", " ");
        s = Regex.Replace(s, @"\s{2,}", " ");
        return s.Trim();
    }
}

// ========= 3) VN CCCD Parser =========

public static class IdCardVnParser
{
    private static readonly Regex RxId = new(@"\b\d{9,12}\b", RegexOptions.Compiled);

    public static IdCardVnExtraction Parse(string rawText, string? lang = null)
    {
        var lines = OcrTextUtil.Lines(rawText);
        var ex = new IdCardVnExtraction { DocType = "IdCardVn", RawLanguage = lang };

        // ID number: best effort (first 9-12 digits found)
        {
            var m = RxId.Match(rawText);
            if (m.Success)
            {
                ex.IdNumber.Value = m.Value;
                ex.IdNumber.Method = "regex";
                ex.IdNumber.Confidence = 0.85;
                ex.IdNumber.Source = m.Value;
                ex.DocumentNumber.Value = m.Value;
            }
        }

        // Full name
        {
            int idx = OcrTextUtil.BestLineIndexByKeyword(lines, "họ và tên", "full name");
            if (idx >= 0)
            {
                var sameLine = OcrTextUtil.TakeValueSameLineAfterColon(lines[idx]);
                if (!string.IsNullOrWhiteSpace(sameLine))
                {
                    ex.FullName.Value = sameLine;
                    ex.FullName.Method = "keyword-same-line";
                    ex.FullName.Confidence = 0.80;
                    ex.FullName.Source = lines[idx];
                }
                else
                {
                    var next = OcrTextUtil.TakeNextNonEmpty(lines, idx + 1);
                    if (!string.IsNullOrWhiteSpace(next))
                    {
                        ex.FullName.Value = next;
                        ex.FullName.Method = "keyword-next-line";
                        ex.FullName.Confidence = 0.75;
                        ex.FullName.Source = lines[idx] + " | " + next;
                    }
                }
            }
        }

        // DOB
        {
            int idx = OcrTextUtil.BestLineIndexByKeyword(lines, "ngày sinh", "date of birth", "dob");
            if (idx >= 0)
            {
                var scan = lines[idx] + "\n" + (idx + 1 < lines.Count ? lines[idx + 1] : "");
                var dates = OcrTextUtil.FindDates(scan);
                if (dates.Count > 0)
                {
                    ex.Dob.Value = dates[0].dt;
                    ex.Dob.Method = "keyword-nearby-date";
                    ex.Dob.Confidence = 0.80;
                    ex.Dob.Source = scan;
                }
            }
            else
            {
                // fallback: first date in doc is often DOB on CCCD
                var dates = OcrTextUtil.FindDates(rawText);
                if (dates.Count > 0)
                {
                    ex.Dob.Value = dates[0].dt;
                    ex.Dob.Method = "fallback-first-date";
                    ex.Dob.Confidence = 0.50;
                    ex.Dob.Source = dates[0].source;
                }
            }
        }

        // Expiry
        {
            int idx = OcrTextUtil.BestLineIndexByKeyword(lines, "có giá trị đến", "date of expiry", "expiry");
            if (idx >= 0)
            {
                var scan = lines[idx] + "\n" + (idx + 1 < lines.Count ? lines[idx + 1] : "");
                var dates = OcrTextUtil.FindDates(scan);
                if (dates.Count > 0)
                {
                    ex.Expiry.Value = dates[0].dt;
                    ex.Expiry.Method = "keyword-nearby-date";
                    ex.Expiry.Confidence = 0.75;
                    ex.Expiry.Source = scan;
                }
            }
        }

        // Sex
        {
            int idx = OcrTextUtil.BestLineIndexByKeyword(lines, "giới tính", "sex");
            if (idx >= 0)
            {
                var v = OcrTextUtil.TakeValueSameLineAfterColon(lines[idx]) ?? (idx + 1 < lines.Count ? lines[idx + 1] : "");
                v = OcrTextUtil.CleanupValue(v);
                if (!string.IsNullOrWhiteSpace(v))
                {
                    ex.Sex.Value = v;
                    ex.Sex.Method = "keyword-nearby";
                    ex.Sex.Confidence = 0.60;
                    ex.Sex.Source = lines[idx] + (idx + 1 < lines.Count ? " | " + lines[idx + 1] : "");
                }
            }
        }

        // Nationality
        {
            int idx = OcrTextUtil.BestLineIndexByKeyword(lines, "quốc tịch", "nationality");
            if (idx >= 0)
            {
                var v = OcrTextUtil.TakeValueSameLineAfterColon(lines[idx]) ?? (idx + 1 < lines.Count ? lines[idx + 1] : "");
                v = OcrTextUtil.CleanupValue(v);
                if (!string.IsNullOrWhiteSpace(v))
                {
                    ex.Nationality.Value = v;
                    ex.Nationality.Method = "keyword-nearby";
                    ex.Nationality.Confidence = 0.60;
                    ex.Nationality.Source = lines[idx] + (idx + 1 < lines.Count ? " | " + lines[idx + 1] : "");
                }
            }
        }

        // Place of origin
        {
            int idx = OcrTextUtil.BestLineIndexByKeyword(lines, "quê quán", "place of origin");
            if (idx >= 0)
            {
                var v = OcrTextUtil.TakeValueSameLineAfterColon(lines[idx]) ?? OcrTextUtil.TakeNextNonEmpty(lines, idx + 1);
                v = OcrTextUtil.CleanupValue(v ?? "");
                if (!string.IsNullOrWhiteSpace(v))
                {
                    ex.PlaceOfOrigin.Value = v;
                    ex.PlaceOfOrigin.Method = "keyword-next";
                    ex.PlaceOfOrigin.Confidence = 0.55;
                    ex.PlaceOfOrigin.Source = lines[idx] + " | " + v;
                }
            }
        }

        // Place of residence
        {
            int idx = OcrTextUtil.BestLineIndexByKeyword(lines, "nơi thường trú", "place of residence");
            if (idx >= 0)
            {
                var v = OcrTextUtil.TakeValueSameLineAfterColon(lines[idx]) ?? OcrTextUtil.TakeNextNonEmpty(lines, idx + 1);
                v = OcrTextUtil.CleanupValue(v ?? "");
                if (!string.IsNullOrWhiteSpace(v))
                {
                    ex.PlaceOfResidence.Value = v;
                    ex.PlaceOfResidence.Method = "keyword-next";
                    ex.PlaceOfResidence.Confidence = 0.55;
                    ex.PlaceOfResidence.Source = lines[idx] + " | " + v;
                }
            }
        }

        return ex;
    }
}

// ========= 4) Lao ID Card Parser (best-effort) =========
// Note: Lao OCR quality depends heavily on image + traineddata + psm.
// This parser tries:
// - Find dates (DOB / issue / expiry) by Lao keywords or by order
// - Find a plausible Lao name line (mostly Lao letters, not digits)
public static class IdCardLaoParser
{
    private static readonly string[] KwId = { "ເລກທີ", "ເລກທີ່", "ເລກທິ", "ເລກບັດ", "ເລກ" };
    private static readonly string[] KwName = { "ຊື່", "ຊື", "ຊື້", "ຊື່-ນາມ" };
    private static readonly string[] KwDob = { "ວັນເດືອນປີເກີດ", "ເກີດ", "ວັນເກີດ" };
    private static readonly string[] KwCurrentAddr = { "ທີ່ຢູ່ປັດຈຸບັນ", "ປັດຈຸບັນ", "ຈຸບັນ" };
    private static readonly string[] KwEthnicity = { "ເຊື້ອຊາດ" };
    private static readonly string[] KwNationality = { "ສັນຊາດ" };
    private static readonly string[] KwIssue = { "ອອກໃຫ້ວັນທີ", "ອອກໃຫ້", "ວັນອອກ", "ອອກ" };
    private static readonly string[] KwExpiry = { "ຫົດກໍານົດ", "ຫມົດກໍານົດ", "ໝົດອາຍຸ", "expiry", "expire" };

    private static readonly Regex RxDigits = new(@"\d", RegexOptions.Compiled);

    // 04-19 001381 / 04-19-001381 / OCR variant
    private static readonly Regex RxLaoId = new(@"(?<!\d)(\d{2})\s*[-–—]?\s*(\d{2})\s*[-–—\s]?\s*(\d{6})(?!\d)",
        RegexOptions.Compiled);

    public static IdCardLaoExtraction Parse(string rawText, string? lang = null)
    {
        var lines = OcrTextUtil.Lines(rawText)
            .Select(OcrTextUtil.StripWeirdOcrNoise)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToList();

        var ex = new IdCardLaoExtraction { DocType = "IdCardLao", RawLanguage = lang };

        // 1) IdNumber
        {
            var id = FindIdNumber(rawText, lines, out var src, out var method);
            if (!string.IsNullOrWhiteSpace(id))
            {
                ex.IdNumber.Value = id;
                ex.IdNumber.Method = method;
                ex.IdNumber.Source = src;
                ex.IdNumber.Confidence = 0.70;

                ex.DocumentNumber.Value = id;
            }
        }

        // 2) FullName (lấy substring sau keyword, không phụ thuộc ':')
        {
            int idx = OcrTextUtil.BestLineIndexByKeyword(lines, KwName);
            var candidate = idx >= 0 ? ExtractAfterAnyKeyword(lines[idx], KwName) : null;
            if (string.IsNullOrWhiteSpace(candidate) && idx >= 0)
                candidate = OcrTextUtil.TakeNextNonEmpty(lines, idx + 1);

            candidate = OcrTextUtil.CleanupValue(candidate ?? "");

            // CLEAN NGAY TẠI ĐÂY (trước khi set vào ex)
            candidate = candidate.Replace("້", "").Replace("່", "").Trim();
            candidate = Regex.Replace(candidate, @"^\p{M}+", "");
            candidate = candidate.Trim();

            if (string.IsNullOrWhiteSpace(candidate))
            {
                candidate = lines
                    .Where(l => OcrTextUtil.LooksLikeMostlyLao(l))
                    .Where(l => !RxDigits.IsMatch(l))
                    .OrderByDescending(l => l.Length)
                    .FirstOrDefault();
            }

            if (!string.IsNullOrWhiteSpace(candidate))
            {
                ex.FullName.Value = candidate;
                ex.FullName.Method = idx >= 0 ? "keyword-after-v2" : "fallback-longest-lao-line-v2";
                ex.FullName.Confidence = idx >= 0 ? 0.65 : 0.35;
                ex.FullName.Source = idx >= 0 ? lines[idx] : candidate;
            }
        }

        // 3) DOB / IssueDate / ExpiryDate (đã bắt thêm pattern 1007/2019 ở FindDates)
        FillDate(lines, KwDob, ex.Dob, "dob");
        FillDate(lines, KwIssue, ex.IssueDate, "issue");
        FillDate(lines, KwExpiry, ex.ExpiryDate, "expiry");

        // 4) Current Address (multi-line)
        {
            int idx = OcrTextUtil.BestLineIndexByKeyword(lines, KwCurrentAddr);
            if (idx >= 0)
            {
                var addr = ExtractMultiLineValue(lines, idx, maxLines: 4);
                addr = RemoveLeadingKeyword(addr, KwCurrentAddr);
                addr = OcrTextUtil.NormalizeLaoAddress(addr);

                if (!string.IsNullOrWhiteSpace(addr))
                {
                    ex.CurrentAddress.Value = addr;
                    ex.CurrentAddress.Method = "keyword-multiline+addr-normalize";
                    ex.CurrentAddress.Confidence = 0.55;

                    ex.Address.Value = addr; // compat
                }
            }
        }

        // 5) Ethnicity + Nationality
        {
            int idx = OcrTextUtil.BestLineIndexByKeyword(
    lines,
    KwEthnicity.Concat(KwNationality).ToArray()
);

            if (idx >= 0)
            {
                var scan = lines[idx]; // <-- CHỈ DÙNG 1 LINE

                var eth = ExtractBetween(scan, "ເຊື້ອຊາດ", "ສັນຊາດ");
                var nat = ExtractAfter(scan, "ສັນຊາດ");

                if (!string.IsNullOrWhiteSpace(eth))
                {
                    eth = OcrTextUtil.CleanupValue(eth);
                    ex.Ethnicity.Value = eth.Split(' ', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault() ?? eth;
                    ex.Ethnicity.Method = "keyword-inline-v2";
                    ex.Ethnicity.Confidence = 0.55;
                    ex.Ethnicity.Source = scan;
                }

                if (!string.IsNullOrWhiteSpace(nat))
                {
                    nat = OcrTextUtil.CleanupValue(nat);
                    nat = nat.Split(' ', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault() ?? nat;
                    ex.Nationality.Value = nat;
                    ex.Nationality.Method = "keyword-inline-v2";
                    ex.Nationality.Confidence = 0.55;
                    ex.Nationality.Source = scan;
                }
            }
        }

        return ex;
    }

    private static string? FindIdNumber(string rawText, List<string> lines, out string? source, out string? method)
    {
        source = null; method = null;

        int idx = OcrTextUtil.BestLineIndexByKeyword(lines, KwId);
        if (idx >= 0)
        {
            var scan = lines[idx] + " " + (idx + 1 < lines.Count ? lines[idx + 1] : "");
            var m = RxLaoId.Match(scan);
            if (m.Success)
            {
                source = scan;
                method = "keyword+regex";
                return $"{m.Groups[1].Value}-{m.Groups[2].Value}-{m.Groups[3].Value}";
            }
        }

        var m2 = RxLaoId.Match(rawText);
        if (m2.Success)
        {
            source = m2.Value;
            method = "regex";
            return $"{m2.Groups[1].Value}-{m2.Groups[2].Value}-{m2.Groups[3].Value}";
        }

        return null;
    }

    private static void FillDate(List<string> lines, string[] kws, OcrField<DateTime> field, string tag)
    {
        int idx = OcrTextUtil.BestLineIndexByKeyword(lines, kws);
        if (idx < 0) return;

        var scan = lines[idx] + "\n" + (idx + 1 < lines.Count ? lines[idx + 1] : "");
        var dates = OcrTextUtil.FindDates(scan);
        if (dates.Count == 0) return;

        field.Value = dates[0].dt;
        field.Method = $"keyword-nearby-date:{tag}";
        field.Confidence = 0.60;
        field.Source = scan;
    }

    private static string ExtractMultiLineValue(List<string> lines, int startIndex, int maxLines)
    {
        var sb = new StringBuilder();
        for (int i = startIndex; i < lines.Count && i < startIndex + maxLines; i++)
        {
            if (sb.Length > 0) sb.Append(' ');
            sb.Append(lines[i]);
        }
        return sb.ToString();
    }

    private static string? ExtractAfterAnyKeyword(string line, string[] keywords)
    {
        int best = -1; string bestKw = "";
        foreach (var kw in keywords)
        {
            var idx = line.IndexOf(kw, StringComparison.OrdinalIgnoreCase);
            if (idx >= 0 && (best < 0 || idx < best)) { best = idx; bestKw = kw; }
        }
        if (best < 0) return null;

        var s = line[(best + bestKw.Length)..].Trim(' ', ':', '-', '–', '—', '|');
        return string.IsNullOrWhiteSpace(s) ? null : s;
    }

    private static string RemoveLeadingKeyword(string line, string[] kws) => ExtractAfterAnyKeyword(line, kws) ?? line;

    private static string? ExtractBetween(string text, string leftKw, string rightKw)
    {
        var i1 = text.IndexOf(leftKw, StringComparison.OrdinalIgnoreCase);
        if (i1 < 0) return null;
        i1 += leftKw.Length;

        var i2 = text.IndexOf(rightKw, i1, StringComparison.OrdinalIgnoreCase);
        var s = i2 >= 0 ? text[i1..i2] : text[i1..];
        s = s.Trim(' ', ':', '-', '–', '—', '|');
        return string.IsNullOrWhiteSpace(s) ? null : s;
    }

    private static string? ExtractAfter(string text, string kw)
    {
        var i1 = text.IndexOf(kw, StringComparison.OrdinalIgnoreCase);
        if (i1 < 0) return null;
        var s = text[(i1 + kw.Length)..].Trim(' ', ':', '-', '–', '—', '|');
        return string.IsNullOrWhiteSpace(s) ? null : s;
    }
}

// ========= 5) Invoice Parser =========

public static class InvoiceParser
{
    private static readonly Regex RxTaxCode = new(@"\b\d{10}(\d{3})?\b", RegexOptions.Compiled);
    private static readonly Regex RxInvoiceNo = new(@"\b\d{6,}\b", RegexOptions.Compiled); // e.g. 00000014

    public static InvoiceExtraction Parse(string rawText, string? lang = null)
    {
        var lines = OcrTextUtil.Lines(rawText);
        var ex = new InvoiceExtraction { DocType = "Invoice", RawLanguage = lang };

        // InvoiceNo: prefer line contains "Invoice No" or "Số"
        {
            int idx = OcrTextUtil.BestLineIndexByKeyword(lines, "invoice no", "số (invoice", "số (wvoi", "số (inv");
            string scan = idx >= 0 ? lines[idx] : rawText;

            var m = RxInvoiceNo.Match(scan);
            if (!m.Success && idx >= 0 && idx + 1 < lines.Count) m = RxInvoiceNo.Match(lines[idx + 1]);

            if (m.Success)
            {
                ex.InvoiceNo.Value = m.Value;
                ex.InvoiceNo.Method = idx >= 0 ? "keyword+regex" : "regex";
                ex.InvoiceNo.Confidence = 0.80;
                ex.InvoiceNo.Source = scan;
                ex.DocumentNumber.Value = m.Value;
            }
        }

        // InvoiceDate: prefer line contains "Ngày (day)"
        {
            int idx = OcrTextUtil.BestLineIndexByKeyword(lines, "ngày (day)", "ngày", "date");
            if (idx >= 0)
            {
                var scan = lines[idx] + "\n" + (idx + 1 < lines.Count ? lines[idx + 1] : "");
                var dates = OcrTextUtil.FindDates(scan);
                if (dates.Count > 0)
                {
                    ex.InvoiceDate.Value = dates[0].dt;
                    ex.InvoiceDate.Method = "keyword-nearby-date";
                    ex.InvoiceDate.Confidence = 0.75;
                    ex.InvoiceDate.Source = scan;
                }
            }
            else
            {
                var dates = OcrTextUtil.FindDates(rawText);
                if (dates.Count > 0)
                {
                    ex.InvoiceDate.Value = dates[0].dt;
                    ex.InvoiceDate.Method = "fallback-first-date";
                    ex.InvoiceDate.Confidence = 0.40;
                    ex.InvoiceDate.Source = dates[0].source;
                }
            }
        }

        // SellerName: line after "Đơn vị bán" / "Seller"
        {
            int idx = OcrTextUtil.BestLineIndexByKeyword(lines, "đơn vị bán", "seller");
            if (idx >= 0)
            {
                var v = OcrTextUtil.TakeValueSameLineAfterColon(lines[idx]) ?? OcrTextUtil.TakeNextNonEmpty(lines, idx + 1);
                v = OcrTextUtil.CleanupValue(v ?? "");
                if (!string.IsNullOrWhiteSpace(v))
                {
                    ex.SellerName.Value = v;
                    ex.SellerName.Method = "keyword-next";
                    ex.SellerName.Confidence = 0.65;
                    ex.SellerName.Source = lines[idx] + " | " + v;
                }
            }
        }

        // SellerTaxCode: pick first MST near seller area, else first MST in doc
        {
            // near "Mã số thuế" close to seller name
            int idx = OcrTextUtil.BestLineIndexByKeyword(lines, "mã số thuế", "tax code");
            if (idx >= 0)
            {
                var window = string.Join("\n", lines.Skip(Math.Max(0, idx - 2)).Take(6));
                var m = RxTaxCode.Match(window);
                if (m.Success)
                {
                    ex.SellerTaxCode.Value = m.Value;
                    ex.SellerTaxCode.Method = "keyword-window-regex";
                    ex.SellerTaxCode.Confidence = 0.70;
                    ex.SellerTaxCode.Source = window;
                }
            }
            if (string.IsNullOrWhiteSpace(ex.SellerTaxCode.Value))
            {
                var m = RxTaxCode.Match(rawText);
                if (m.Success)
                {
                    ex.SellerTaxCode.Value = m.Value;
                    ex.SellerTaxCode.Method = "fallback-first-taxcode";
                    ex.SellerTaxCode.Confidence = 0.40;
                    ex.SellerTaxCode.Source = m.Value;
                }
            }
        }

        // BuyerName: after "Người mua" / "Đơn vị (Company name)"
        {
            int idx = OcrTextUtil.BestLineIndexByKeyword(lines, "người mua", "buyer", "company name");
            if (idx >= 0)
            {
                // Often appears: "Đơn vị (Company name): XXX"
                var same = OcrTextUtil.TakeValueSameLineAfterColon(lines[idx]);
                if (!string.IsNullOrWhiteSpace(same))
                {
                    ex.BuyerName.Value = same;
                    ex.BuyerName.Method = "keyword-same-line";
                    ex.BuyerName.Confidence = 0.65;
                    ex.BuyerName.Source = lines[idx];
                }
                else
                {
                    var next = OcrTextUtil.TakeNextNonEmpty(lines, idx + 1);
                    if (!string.IsNullOrWhiteSpace(next))
                    {
                        ex.BuyerName.Value = next;
                        ex.BuyerName.Method = "keyword-next";
                        ex.BuyerName.Confidence = 0.55;
                        ex.BuyerName.Source = lines[idx] + " | " + next;
                    }
                }
            }
        }

        // BuyerTaxCode: find next taxcode after buyer section
        {
            int idx = OcrTextUtil.BestLineIndexByKeyword(lines, "người mua", "buyer");
            if (idx >= 0)
            {
                var window = string.Join("\n", lines.Skip(idx).Take(12));
                var ms = RxTaxCode.Matches(window).Cast<Match>().Select(m => m.Value).Distinct().ToList();
                // Heuristic: second tax code often buyer if first is seller
                if (ms.Count >= 1)
                {
                    // If sellerTaxCode equals first, take second; else take first
                    var pick = ms.FirstOrDefault(x => !string.Equals(x, ex.SellerTaxCode.Value, StringComparison.OrdinalIgnoreCase))
                               ?? ms[0];
                    ex.BuyerTaxCode.Value = pick;
                    ex.BuyerTaxCode.Method = "buyer-window-taxcode";
                    ex.BuyerTaxCode.Confidence = 0.60;
                    ex.BuyerTaxCode.Source = window;
                }
            }
            if (string.IsNullOrWhiteSpace(ex.BuyerTaxCode.Value))
            {
                // fallback: if doc has >=2 taxcodes, take one different from seller
                var all = RxTaxCode.Matches(rawText).Cast<Match>().Select(m => m.Value).Distinct().ToList();
                var pick = all.FirstOrDefault(x => !string.Equals(x, ex.SellerTaxCode.Value, StringComparison.OrdinalIgnoreCase));
                if (!string.IsNullOrWhiteSpace(pick))
                {
                    ex.BuyerTaxCode.Value = pick;
                    ex.BuyerTaxCode.Method = "fallback-taxcode-different";
                    ex.BuyerTaxCode.Confidence = 0.35;
                    ex.BuyerTaxCode.Source = pick;
                }
            }
        }

        // SubTotal / VAT rate / VAT amount / Total
        ParseMoneyByKeyword(lines, ex.SubTotal, "cộng tiền hàng", "sub total", "subtotal");
        ParseVatRate(lines, ex.VatRate);
        ParseMoneyByKeyword(lines, ex.VatAmount, "tiền thuế", "vat amount");
        ParseMoneyByKeyword(lines, ex.Total, "tổng cộng tiền thanh toán", "total payment", "total");

        return ex;
    }

    private static void ParseMoneyByKeyword(List<string> lines, OcrField<decimal> field, params string[] keys)
    {
        int idx = OcrTextUtil.BestLineIndexByKeyword(lines, keys);
        if (idx < 0) return;

        // scan current + next line (sometimes value is on next)
        var scan = lines[idx] + "\n" + (idx + 1 < lines.Count ? lines[idx + 1] : "");
        var v = OcrTextUtil.TryParseMoney(scan);
        if (v != null)
        {
            field.Value = v.Value;
            field.Method = "keyword+money";
            field.Confidence = 0.65;
            field.Source = scan;
        }
    }

    private static void ParseVatRate(List<string> lines, OcrField<decimal> field)
    {
        int idx = OcrTextUtil.BestLineIndexByKeyword(lines, "thuế suất", "tax rate", "vat rate");
        if (idx < 0) return;

        var scan = lines[idx] + "\n" + (idx + 1 < lines.Count ? lines[idx + 1] : "");
        var m = Regex.Match(scan, @"(\d{1,2})\s*%", RegexOptions.Compiled);
        if (m.Success && decimal.TryParse(m.Groups[1].Value, NumberStyles.Number, CultureInfo.InvariantCulture, out var r))
        {
            field.Value = r;
            field.Method = "keyword+percent";
            field.Confidence = 0.70;
            field.Source = scan;
        }
    }
}

// ========= 6) Dispatcher (call 1 function, get parsed object) =========

public enum SimpleDocType
{
    Auto = 0,
    IdCard = 1,
    Invoice = 2,
    // add more if you want
}

public static class OcrParserDispatcher
{
    // For IdCard: it will decide VN vs LAO by language + script
    public static OcrParsedResult Parse(SimpleDocType docType, string rawText, string? language, float meanConfidence)
    {
        rawText = rawText ?? "";
        var res = new OcrParsedResult
        {
            RawText = rawText,
            Language = language,
            MeanConfidence = meanConfidence
        };

        if (docType == SimpleDocType.Invoice)
        {
            res.Invoice = InvoiceParser.Parse(rawText, language);
            return res;
        }

        if (docType == SimpleDocType.IdCard)
        {
            // decide Lao vs VN
            var looksLao = (language?.ToLowerInvariant().Contains("lao") ?? false)
                           || OcrTextUtil.LooksLikeMostlyLao(rawText);

            if (looksLao)
                res.IdCardLao = IdCardLaoParser.Parse(rawText, language);
            else
                res.IdCardVn = IdCardVnParser.Parse(rawText, language);

            return res;
        }

        // Auto: try invoice first if has "HÓA ĐƠN" or "VAT INVOICE"
        if (Regex.IsMatch(rawText, @"\bHÓA\s*ĐƠN\b|\bVAT\s*INVOICE\b", RegexOptions.IgnoreCase))
        {
            res.Invoice = InvoiceParser.Parse(rawText, language);
            return res;
        }

        // else treat as IdCard (VN vs Lao)
        var looksLaoAuto = (language?.ToLowerInvariant().Contains("lao") ?? false)
                           || OcrTextUtil.LooksLikeMostlyLao(rawText);

        if (looksLaoAuto)
            res.IdCardLao = IdCardLaoParser.Parse(rawText, language);
        else
            res.IdCardVn = IdCardVnParser.Parse(rawText, language);

        return res;
    }
}
