using System;

namespace DSMRParser.CRCHandling
{
    public interface ICRCCalculator
    {
        Exception? Verify(Span<byte> data);
        ushort CalculateCRC(Span<byte> bytes);

        static ICRCCalculator Default => new CRC16Calculator();
    }
}