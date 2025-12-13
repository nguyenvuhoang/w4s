using System.Text.Json;
using System.Text.Json.Nodes;

namespace O24OpenAPI.Core.Json;

public static class JsonNodePathAccessor
{
    public static bool TryGetValueByPath<T>(
        this JsonNode node,
        ReadOnlySpan<char> path,
        out T result
    )
    {
        result = default;
        var current = node;
        var reader = new PathReader(path);

        while (reader.MoveNext())
        {
            var segment = reader.Current;

            if (segment.IsProperty)
            {
                if (current is JsonObject obj)
                {
                    if (!obj.TryGetPropertyValue(segment.Property, out current))
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else // array index
            {
                if (
                    current is JsonArray arr
                    && segment.ArrayIndex >= 0
                    && segment.ArrayIndex < arr.Count
                )
                {
                    current = arr[segment.ArrayIndex];
                }
                else
                {
                    return false;
                }

                // Xử lý các array lồng nhau như [1][2][3]
                while (reader.PeekNextIsArrayIndex)
                {
                    reader.MoveNext();
                    segment = reader.Current;

                    if (
                        current is JsonArray nestedArr
                        && segment.ArrayIndex >= 0
                        && segment.ArrayIndex < nestedArr.Count
                    )
                    {
                        current = nestedArr[segment.ArrayIndex];
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        if (current is JsonValue val)
        {
            if (val.TryGetValue<T>(out var value))
            {
                result = value;
                return true;
            }
            else
            {
                try
                {
                    result = val.Deserialize<T>();
                    return result != null;
                }
                catch
                {
                    return false;
                }
            }
        }
        else if (current is T casted)
        {
            result = casted;
            return true;
        }
        else
        {
            try
            {
                result = current.Deserialize<T>();
                return result != null;
            }
            catch
            {
                return false;
            }
        }
    }

    // --------------------------
    // Path Reader & Segment
    // --------------------------
    private ref struct PathReader
    {
        private ReadOnlySpan<char> _path;
        private int _pos;
        public PathSegment Current;

        public PathReader(ReadOnlySpan<char> path)
        {
            _path = path;
            _pos = 0;
            Current = default;
        }

        public bool MoveNext()
        {
            SkipDot();

            if (_pos >= _path.Length)
            {
                return false;
            }

            if (_path[_pos] == '[')
            {
                return TryParseArraySegment(out Current);
            }

            return TryParsePropertySegment(out Current);
        }

        public bool PeekNextIsArrayIndex
        {
            get
            {
                int temp = _pos;
                while (temp < _path.Length && _path[temp] == '.')
                {
                    temp++;
                }

                return temp < _path.Length && _path[temp] == '[';
            }
        }

        private void SkipDot()
        {
            while (_pos < _path.Length && _path[_pos] == '.')
            {
                _pos++;
            }
        }

        private bool TryParsePropertySegment(out PathSegment segment)
        {
            int start = _pos;
            while (_pos < _path.Length && _path[_pos] != '.' && _path[_pos] != '[')
            {
                _pos++;
            }

            if (start == _pos)
            {
                segment = default;
                return false;
            }

            segment = new PathSegment(_path.Slice(start, _pos - start));
            return true;
        }

        private bool TryParseArraySegment(out PathSegment segment)
        {
            _pos++; // skip '['

            int indexStart = _pos;
            while (_pos < _path.Length && char.IsDigit(_path[_pos]))
            {
                _pos++;
            }

            if (_pos >= _path.Length || _path[_pos] != ']')
            {
                segment = default;
                return false;
            }

            if (!int.TryParse(_path.Slice(indexStart, _pos - indexStart), out int index))
            {
                segment = default;
                return false;
            }

            _pos++; // skip ']'
            segment = new PathSegment(index);
            return true;
        }
    }

    private readonly struct PathSegment
    {
        public string Property { get; }
        public int ArrayIndex { get; }
        public bool IsProperty { get; }

        public PathSegment(ReadOnlySpan<char> property)
        {
            Property = property.ToString();
            ArrayIndex = -1;
            IsProperty = true;
        }

        public PathSegment(int index)
        {
            Property = string.Empty;
            ArrayIndex = index;
            IsProperty = false;
        }
    }
}
