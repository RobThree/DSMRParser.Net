using System;

namespace DSMRParser
{
    public abstract class DSMRParseException : Exception
    {
        public DSMRParseException()
            : this("DSMR Parse error") { }

        public DSMRParseException(string? message)
            : this(message, null) { }

        public DSMRParseException(string? message, Exception? innerException = null)
            : base(message, innerException) { }
    }
}