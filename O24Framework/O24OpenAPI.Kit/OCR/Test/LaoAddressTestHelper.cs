using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace O24OpenAPI.Kit.OCR.Test
{
    public static class LaoAddressTestHelper
    {
        // keyword tìm dòng địa chỉ
        private static readonly string[] KwCurrentAddr = { "ທີ່ຢູ່ປັດຈຸບັນ", "ປັດຈຸບັນ", "ຈຸບັນ" };

        // keyword để dừng (để khỏi kéo rác xuống dưới)
        private static readonly string[] StopKws =
        {
        "ເຊື້ອຊາດ", "ສັນຊາດ", "ອອກໃຫ້", "ອອກໃຫ້ວັນທີ", "ຫົດກໍານົດ", "ເລກທີ"
    };

        private static readonly Regex RxSlashes = new(@"\s*[/\\]{2,}\s*", RegexOptions.Compiled);
        private static readonly Regex RxSpaces = new(@"\s+", RegexOptions.Compiled);
        private static readonly Regex RxCommaSpaces = new(@"\s*,\s*", RegexOptions.Compiled);

        private static readonly HashSet<string> LaoAddrAbbrev = new(StringComparer.Ordinal)
    {
        "ບ","ມ","ຂ","ນ","ຕ","ບ.","ມ.","ຂ.","ນ.","ຕ."
    };

        public static string ExtractAddressOnly(string rawText)
        {
            var lines = rawText
                .Split('\n', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();

            int dobIdx = BestLineIndexByKeyword(lines, new[] { "ວັນເດືອນປີເກີດ", "ເກີດ" });
            int ethIdx = BestLineIndexByKeyword(lines, new[] { "ເຊື້ອຊາດ" });

            if (ethIdx < 0) ethIdx = lines.Count;
            int start = (dobIdx >= 0 ? dobIdx + 1 : 0);
            if (start >= ethIdx) return "";

            // gom các dòng giữa DOB và Ethnicity, ưu tiên dòng có pattern địa chỉ
            var candidates = new List<string>();
            for (int i = start; i < ethIdx; i++)
            {
                var l = lines[i];

                // bỏ rác quá ngắn
                if (l.Length <= 1) continue;

                // chỉ giữ những dòng “giống địa chỉ”
                if (l.Contains("ບ ") || l.Contains("ມ ") || l.Contains("ຂ "))
                    candidates.Add(l);
            }

            // nếu OCR nát quá không bắt được dòng có ບ/ມ/ຂ thì fallback lấy tất cả giữa 2 mốc
            string addr = candidates.Count > 0
                ? string.Join(' ', candidates)
                : string.Join(' ', lines.Skip(start).Take(ethIdx - start));

            addr = NormalizeLaoAddress(addr);
            return addr;
        }

        // ===== helpers thêm =====
        private static int FindWindowIndexByKeyword(List<string> lines, string[] kws, int window)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                var merged = "";
                for (int j = 0; j < window && i + j < lines.Count; j++)
                    merged += lines[i + j].Replace(" ", "");

                foreach (var kw in kws)
                {
                    var k = kw.Replace(" ", "");
                    if (merged.IndexOf(k, StringComparison.OrdinalIgnoreCase) >= 0)
                        return i;
                }
            }
            return -1;
        }

        private static int BestLineIndexByKeyword(List<string> lines, string[] kws)
        {
            for (int i = 0; i < lines.Count; i++)
                if (LineContainsAny(lines[i], kws))
                    return i;
            return -1;
        }

        private static bool LineContainsAny(string line, string[] kws)
        {
            foreach (var kw in kws)
                if (line.IndexOf(kw, StringComparison.OrdinalIgnoreCase) >= 0)
                    return true;
            return false;
        }

        private static string RemoveLeadingKeyword(string s, string[] kws)
        {
            foreach (var kw in kws)
            {
                var p = s.IndexOf(kw, StringComparison.OrdinalIgnoreCase);
                if (p >= 0)
                {
                    var tail = s[(p + kw.Length)..].Trim(' ', ':', '-', '–', '—', '|');
                    if (!string.IsNullOrWhiteSpace(tail)) return tail;
                }
            }
            return s;
        }

        private static string NormalizeLaoAddress(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return "";

            s = RxSlashes.Replace(s, " ");
            s = RxCommaSpaces.Replace(s, ", ");
            s = RxSpaces.Replace(s, " ").Trim();

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
    }
}
