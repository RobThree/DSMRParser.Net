using DSMRParser.CRCHandling;
using DSMRParser.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace DSMRParser
{
    public class DSMRTelegramParser : IDSMRTelegramParser
    {
        // https://www.netbeheernederland.nl/_upload/Files/Slimme_meter_15_a727fce1f1.pdf

        private readonly ICRCCalculator _crc;

        public DSMRTelegramParser()
            : this(ICRCCalculator.Default) { }

        public DSMRTelegramParser(ICRCCalculator crc) => _crc = crc ?? throw new ArgumentNullException(nameof(crc));

        public Telegram Parse(string data) => Parse(Encoding.ASCII.GetBytes(data));

        public Telegram Parse(Span<byte> data)
        {
            var ex = TryParseCore(data, out var result);
            if (ex != null)
                throw ex;
            return result ?? throw new NullReferenceException($"{nameof(TryParseCore)} returned null");
        }

        public bool TryParse(string data, [NotNullWhen(true)] out Telegram? result) => TryParse(Encoding.ASCII.GetBytes(data), out result);

        public bool TryParse(Span<byte> data, [NotNullWhen(true)] out Telegram? result) => TryParseCore(data, out result) == null;

        private Exception? TryParseCore(Span<byte> data, [NotNullWhen(true)] out Telegram? result)
        {
            result = null;
            // Make sure data starts with identification ("/") and that the start of the CRC is at least 4 bytes before the end
            if (data[0] == (byte)'/')
            {
                var lines = Encoding.ASCII.GetString(data).Split("\r\n");

                // TODO: Do CRC check only for V4/5
                var crcex = _crc.Verify(data);
                if (crcex != null)
                    return crcex;

                result = new Telegram(
                                lines[0].TrimStart('/'),
                                lines.Skip(2)
                                    .Where(l => !string.IsNullOrEmpty(l) && char.IsDigit(l[0]))
                                    .Select(l => (OBISId.FromString(l.Substring(0, Math.Max(0, l.IndexOf("(", StringComparison.Ordinal)))), GetValues(l)))
                );
                return null;
            }
            return new FormatException("Invalid format");
        }

        private static IEnumerable<string?> GetValues(string value) => value[value.IndexOf("(", StringComparison.Ordinal)..].TrimStart('(').TrimEnd(')').Split(")(");
    }
}