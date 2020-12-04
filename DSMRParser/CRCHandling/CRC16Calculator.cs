using System;
using System.Globalization;
using System.Text;

namespace DSMRParser.CRCHandling
{
    /// <summary>
    /// Implements a CRC-16-IBM CRC.
    /// </summary>
    /// <remarks>See https://en.wikipedia.org/wiki/Cyclic_redundancy_check</remarks>
    public class CRC16Calculator : ICRCCalculator
    {
        /// <summary>
        /// Internal lookup table
        /// </summary>
        private readonly ushort[] _lookup = new ushort[256];

        /// <summary>
        /// Initializes a new instance of a <see cref="CRC16Calculator" /> with the given polynomial (defaults to a reversed polynomial 0XA001).
        /// </summary>
        /// <param name="polynomial">The polynomial this </param>
        public CRC16Calculator(ushort polynomial = 0xA001)
        {
            ushort value;
            ushort temp;
            // Prepare lookup table
            for (ushort i = 0; i < _lookup.Length; ++i)
            {
                value = 0;
                temp = i;
                for (byte j = 0; j < 8; ++j)
                {
                    value = (((value ^ temp) & 0x0001) != 0) ? (ushort)((value >> 1) ^ polynomial) : (ushort)(value >> 1);
                    temp >>= 1;
                }
                _lookup[i] = value;
            }
        }

        /// <summary>
        /// Extracts the CRC-16-IBM of the given Telegram data and verifies it against the calculated CRC-16-IBM.
        /// </summary>
        /// <param name="rawTelegram">The Telegram in raw byte form.</param>
        /// <returns>Returns a <see cref="CRCException"/> when the CRC-16-IBM doesn't match, null otherwise.</returns>
        public Exception? Verify(Span<byte> rawTelegram)
        {
            // Find where the CRC starts
            var crcstart = rawTelegram.LastIndexOf((byte)'!') + 1;
            // Calculate the CRC
            var calculatedcrc = CalculateCRC(rawTelegram[0..crcstart]);

            if (crcstart != rawTelegram.Length - 6)
                return new CRCException("CRC must be 4 bytes");

            // Get the CRC claimed by the telegram
            var crcstring = Encoding.ASCII.GetString(rawTelegram[(crcstart)..(crcstart + 4)]);
            // Compare CRCs
            if (int.TryParse(crcstring, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var claimedcrc) && claimedcrc == calculatedcrc)
                return null;
            return new CRCException(calculatedcrc, claimedcrc);
        }

        /// <summary>
        /// Calculates the CRC-16-IBM of the given Telegram data.
        /// </summary>
        /// <param name="rawTelegram">The Telegram in raw byte form.</param>
        /// <returns>Returns the CRC-16-IBM of the given Telegram.</returns>
        public ushort CalculateCRC(Span<byte> rawTelegram)
        {
            ushort crc = 0;
            for (var i = 0; i < rawTelegram.Length; ++i)
                crc = (ushort)((crc >> 8) ^ _lookup[(byte)(crc ^ rawTelegram[i])]);
            return crc;
        }
    }
}