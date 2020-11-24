using System;

namespace DSMRParser.CRCHandling
{
    public class CRCException : DSMRParseException
    {
        public int Calculated { get; private set; }
        public int Claimed { get; private set; }

        public CRCException()
            : this("CRC error") { }

        public CRCException(string? message)
            : this(message, null) { }

        public CRCException(string? message, Exception? innerException)
            : this(message, 0, 0, innerException) { }

        public CRCException(int calculated, int claimed)
            : this(null, calculated, claimed) { }

        public CRCException(string? message, int calculated, int claimed)
            : this(message, calculated, claimed, null) { }

        public CRCException(string? message, int calculated, int claimed, Exception? innerException = null)
            : base(message ?? $"CRC mismatch; expected '{calculated:X4}', found '{claimed}'", innerException)
        {
            Calculated = calculated;
            Claimed = claimed;
        }
    }
}