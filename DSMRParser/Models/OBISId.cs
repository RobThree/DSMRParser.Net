using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;

namespace DSMRParser.Models
{
    /// <summary>
    /// Represents an OBIS ID.
    /// </summary>
    [DebuggerDisplay("{ToString()}")]
    public record OBISId
    {
        private static readonly char[] SPLITCHARS = new char[] { '-', ':', '.' };

        /// <summary>
        /// A read-only instance of the <see cref="OBISId"/> structure whose value is all 255's.
        /// </summary>
        public static readonly OBISId NONE = new OBISId();

        /// <summary>Part A of the OBIS ID (from the form A-B:C.D.E.F).</summary>
        public byte A { get; init; } = 255;
        /// <summary>Part B of the OBIS ID (from the form A-B:C.D.E.F).</summary>
        public byte B { get; init; } = 255;
        /// <summary>Part C of the OBIS ID (from the form A-B:C.D.E.F).</summary>
        public byte C { get; init; } = 255;
        /// <summary>Part D of the OBIS ID (from the form A-B:C.D.E.F).</summary>
        public byte D { get; init; } = 255;
        /// <summary>Part E of the OBIS ID (from the form A-B:C.D.E.F).</summary>
        public byte E { get; init; } = 255;
        /// <summary>Part F of the OBIS ID (from the form A-B:C.D.E.F).</summary>
        public byte F { get; init; } = 255;

        /// <summary>
        /// Initializes a new <see cref="OBISId"/> with the given parts.
        /// </summary>
        /// <param name="parts">The individual parts of an OBIS ID.</param>
        /// <exception cref="InvalidOBISIdException">Thrown when too many parts are specified (>6).</exception>
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

        /// <summary>
        /// Initializes a new <see cref="OBISId"/> from the given string representation of an OBIS ID.
        /// </summary>
        /// <param name="id">The OBIS ID in string representation (e.g. "1-2:3.4.5.6")</param>
        /// <seealso cref="FromString(string)"/>
        /// <exception cref="InvalidOBISIdException">
        /// Thrown when <paramref name="id"/> is null or empty or any of the parts contain an invalid value.
        /// </exception>
        public OBISId(string id)
            : this(string.IsNullOrEmpty(id) ? throw new InvalidOBISIdException("Null or empty OBIS ID") : GetParts(id).ToArray()) { }

        /// <summary>
        /// Creates an <see cref="OBISId"/> from a string representation of an OBIS ID.
        /// </summary>
        /// <param name="id">The OBIS ID in string representation (e.g. "1-2:3.4.5.6")</param>
        /// <seealso cref="OBISId(string)"/>
        /// <exception cref="InvalidOBISIdException">
        /// Thrown when <paramref name="id"/> is null or empty or any of the parts contain an invalid value.
        /// </exception>
        /// <returns>Returns an <see cref="OBISId"/> when the given string can be parsed as such.</returns>
        public static OBISId FromString(string id) => new OBISId(id);

        /// <summary>
        /// Returns the string representation of an <see cref="OBISId"/>.
        /// </summary>
        /// <returns>The string representation of an <see cref="OBISId"/>.</returns>
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

        /// <summary>
        /// Creates an OBISId from a string
        /// </summary>
        /// <param name="id">The OBIS ID to create from.</param>
        public static implicit operator OBISId(string id) => FromString(id);

        /// <summary>
        /// Splits a given string up into the byte-values of each of the parts.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>
        /// This methods splits on a few given chars but doesn't care about their order. This means an OBIS ID of
        /// "1-2:3.4.5.6" is considered just as valid as an OBIS ID of "1.2-3:4.5-6" or "1-2-3-4-5-6" or "1.2.3:4:5:6".
        /// </remarks>
        private static IEnumerable<byte> GetParts(string id)
        {
            // Split on defined separators, return Max+1 parts so we can throw when too many parts are given, convert
            // each part to a 0..255 value.
            foreach (var v in id.Split(SPLITCHARS, 7))
            {
                if (byte.TryParse(v, NumberStyles.None, CultureInfo.InvariantCulture, out var result))
                    yield return result;
                else
                    throw new InvalidOBISIdException($"Invalid value '{v}' in string '{id}'");
            }
        }
    }
}