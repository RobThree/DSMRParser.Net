using System;
using System.Globalization;
using System.Text;

namespace DSMRParser.CRCHandling
{
    public class CRC16Calculator : ICRCCalculator
    {
        private readonly ushort[] _lookup = new ushort[256];
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

        public Exception? Verify(Span<byte> data)
        {
            // Find where the CRC starts
            var crcstart = data.LastIndexOf((byte)'!') + 1;
            // Calculate the CRC
            var calculatedcrc = CalculateCRC(data[0..crcstart]);
            // Get the CRC claimed by the telegram
            var crcstring = Encoding.ASCII.GetString(data[(crcstart)..(crcstart + 4)]);
            // Compare CRCs
            if (int.TryParse(crcstring, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var claimedcrc) && claimedcrc == calculatedcrc)
                return null;
            return new CRCException(calculatedcrc, claimedcrc);
        }

        public ushort CalculateCRC(Span<byte> bytes)
        {
            ushort crc = 0;
            for (var i = 0; i < bytes.Length; ++i)
                crc = (ushort)((crc >> 8) ^ _lookup[(byte)(crc ^ bytes[i])]);
            return crc;
        }
    }
}