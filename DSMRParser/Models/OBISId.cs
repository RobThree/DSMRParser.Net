using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;

namespace DSMRParser.Models
{
    [DebuggerDisplay("{ToString()}")]
    public record OBISId
    {
        private static readonly char[] SPLITCHARS = new char[] { '-', ':', '.' };
        public static readonly OBISId NONE = new OBISId(255, 255, 255, 255, 255);

        public byte A { get; init; } = 255;
        public byte B { get; init; } = 255;
        public byte C { get; init; } = 255;
        public byte D { get; init; } = 255;
        public byte E { get; init; } = 255;
        public byte F { get; init; } = 255;

        public OBISId(params byte[] parts)
        {
            if (parts.Length > 6)
                throw new InvalidOBISIdException("Too many parts");

            for (var i = 0; i < parts.Length && i < 6; i++)
                switch (i)
                {
                    case 0: A = parts[i]; break;
                    case 1: B = parts[i]; break;
                    case 2: C = parts[i]; break;
                    case 3: D = parts[i]; break;
                    case 4: E = parts[i]; break;
                    case 5: F = parts[i]; break;
                }
        }

        public OBISId(string value)
            : this(string.IsNullOrEmpty(value) ? throw new InvalidOBISIdException("Null or empty OBIS ID") : GetParts(value).ToArray()) { }


        private static IEnumerable<byte> GetParts(string value)
        {
            // Split on defined separators, return Max+1 parts so we can throw when too many parts are given, convert
            // each part to a 0..255 value.
            foreach (var v in value.Split(SPLITCHARS, 7))
            {
                if (byte.TryParse(v, NumberStyles.None, CultureInfo.InvariantCulture, out var result))
                    yield return result;
                else
                    throw new InvalidOBISIdException($"Invalid value '{v}' in string '{value}'");
            }
        }

        public static OBISId FromString(string value) => new OBISId(value);

        public override string ToString()
        {
            var parts = new byte[] { A, B, C, D, E, F };
            var i = 0;
            var result = new StringBuilder();
            while (i < parts.Length && parts[i] < 255)
            {
                switch (i)
                {
                    case 0: break;
                    case 1: result.Append('-'); break;
                    case 2: result.Append(':'); break;
                    default: result.Append('.'); break;
                }
                result.Append(parts[i]);
                i++;
            }
            return result.ToString();
        }

        public static explicit operator OBISId(string value) => FromString(value);
    }
}