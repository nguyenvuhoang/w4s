using System.Text.RegularExpressions;

namespace O24OpenAPI.Kit.OCR.Services;

public static class OcrTextCleaner
{
    // Chỉ gồm whitespace + ký tự phân cách / gạch ngang (kể cả unicode dash)
    private static readonly Regex JunkCharsOnly =
        new(@"^[\s\-= _|`~^:;,.\\/–—−]+$", RegexOptions.Compiled);

    // Pattern lặp hay gặp do line/border/noise
    private static readonly Regex RepeatedNoise =
        new(@"(n{6,}|z{6,}|Z{6,}|={6,}|_{6,}|\|{6,}|x{6,}|X{6,}|m{6,}|M{6,}|h{6,}|H{6,})",
            RegexOptions.Compiled);

    // 1 ký tự lặp quá dài: "mmmmmmmm", "------", "======"
    private static readonly Regex LongRunSameChar =
        new(@"(.)\1{7,}", RegexOptions.Compiled);

    // Lặp token ngắn: "mm mm mm mm ..." hoặc "HH HH HH ..."
    private static readonly Regex RepeatedShortToken =
        new(@"\b(\w{1,3})\b(?:\s+\1){6,}", RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public static string Clean(string text)
    {
        if (string.IsNullOrWhiteSpace(text)) return string.Empty;

        var lines = text.Replace("\r\n", "\n").Replace("\r", "\n").Split('\n');
        var kept = new List<string>(lines.Length);

        foreach (var raw in lines)
        {
            var line = raw.Trim();
            if (line.Length == 0) continue;

            if (IsJunkLine(line)) continue;

            kept.Add(line);
        }

        return string.Join("\n", kept);
    }
    private static readonly Regex HasLao = new(@"\p{IsLao}", RegexOptions.Compiled);

    private static bool IsJunkLine(string line)
    {
        if (JunkCharsOnly.IsMatch(line)) return true;
        if (RepeatedNoise.IsMatch(line)) return true;
        if (LongRunSameChar.IsMatch(line)) return true;
        if (RepeatedShortToken.IsMatch(line)) return true;

        if (JunkCharsOnly.IsMatch(line)) return true;
        if (RepeatedNoise.IsMatch(line)) return true;

        // ✅ Nếu có chữ Lào thì đừng áp các heuristic "entropy/alnum ratio" quá gắt
        if (HasLao.IsMatch(line)) return false;

        // Token-based: nếu đa số token rất ngắn và bị lặp => rác (đặc biệt trên hóa đơn/bảng)
        var tokens = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (tokens.Length >= 10)
        {
            var shortTokens = tokens.Where(t => t.Length <= 3).ToArray();
            if (shortTokens.Length >= tokens.Length * 0.8)
            {
                var distinct = shortTokens.Distinct(StringComparer.OrdinalIgnoreCase).Count();
                if (distinct <= 2) return true;
            }
        }

        // Tính thống kê ký tự (bỏ whitespace)
        var freq = new Dictionary<char, int>();
        int nonWs = 0;
        int letters = 0, digits = 0;

        foreach (var ch in line)
        {
            if (char.IsWhiteSpace(ch)) continue;
            nonWs++;

            if (char.IsLetter(ch)) letters++;
            if (char.IsDigit(ch)) digits++;

            if (freq.TryGetValue(ch, out var c)) freq[ch] = c + 1;
            else freq[ch] = 1;
        }

        // 1) Dòng chứa quá nhiều m/H (hay gặp khi OCR nhầm line/border)
        if (line.Length >= 25)
        {
            int mH = line.Count(ch => ch is 'm' or 'M' or 'h' or 'H');
            digits = line.Count(char.IsDigit);
            if (mH >= 12 && digits == 0) return true;
        }

        // 2) Dòng có quá nhiều ký tự “dị” kiểu Ô/Ổ/Ư/Ù/Ä... (thường là noise block)
        if (line.Length >= 25)
        {
            int weird = line.Count(ch => "ÔỔỘƠỜỚỢÕƯỪỨỰŨÙẤẦẨẪẬÄ".Contains(ch));
            if (weird >= 6) return true;
        }

        // Dòng dài mà ít ký tự khác nhau => thường là noise
        if (nonWs >= 30)
        {
            if (freq.Count <= 12) return true;

            var max = freq.Values.Max();
            if (max / (double)nonWs >= 0.65) return true;

            // thêm: toàn chữ nhưng không có số, mà diversity thấp => hay là "mm HH mm..."
            if (digits == 0 && letters >= nonWs * 0.7 && freq.Count <= 18) return true;
        }

        // Tỉ lệ chữ/số quá thấp cũng là rác (ký tự lạ)
        int alnum = letters + digits;
        if (line.Length >= 12)
        {
            var ratio = (double)alnum / line.Length;
            if (ratio < 0.25) return true;
        }

        // Dòng rất dài nhưng ít chữ/số => rác
        if (line.Length > 60 && alnum < 8) return true;

        return false;
    }
}
