using System.Text;

namespace O24OpenAPI.ControlHub.Utils;

public static class Base32Helper
{
    private const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";

    public static string Base32Encode(byte[] data)
    {
        if (data == null || data.Length == 0)
        {
            return string.Empty;
        }

        int outputLength = ((data.Length * 8) + 4) / 5;
        StringBuilder result = new StringBuilder((outputLength + 7) / 8 * 8);

        int buffer = data[0];
        int next = 1;
        int bitsLeft = 8;

        while (bitsLeft > 0 || next < data.Length)
        {
            if (bitsLeft < 5)
            {
                if (next < data.Length)
                {
                    buffer <<= 8;
                    buffer |= data[next++] & 0xFF;
                    bitsLeft += 8;
                }
                else
                {
                    int pad = 5 - bitsLeft;
                    buffer <<= pad;
                    bitsLeft += pad;
                }
            }

            int index = (buffer >> (bitsLeft - 5)) & 0x1F;
            bitsLeft -= 5;
            result.Append(Alphabet[index]);
        }

        return result.ToString(); // no padding (hi-base32 strips '=' too)
    }

    public static byte[] Base32Decode(string base32)
    {
        if (string.IsNullOrWhiteSpace(base32))
        {
            return Array.Empty<byte>();
        }

        base32 = base32.TrimEnd('=').ToUpperInvariant();
        int buffer = 0;
        int bitsLeft = 0;
        List<byte> result = new List<byte>();

        foreach (char c in base32)
        {
            if (!Alphabet.Contains(c))
            {
                throw new FormatException($"Invalid Base32 character '{c}'.");
            }

            buffer <<= 5;
            buffer |= Alphabet.IndexOf(c);
            bitsLeft += 5;

            if (bitsLeft >= 8)
            {
                bitsLeft -= 8;
                result.Add((byte)((buffer >> bitsLeft) & 0xFF));
            }
        }

        return result.ToArray();
    }
}
