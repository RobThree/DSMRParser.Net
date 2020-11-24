using System;

namespace DSMRParser
{
    public class InvalidOBISIdException : DSMRParseException
    {
        public InvalidOBISIdException()
            : this("Invalid OBIS ID") { }

        public InvalidOBISIdException(string? message)
            : this(message, null) { }

        public InvalidOBISIdException(string? message, Exception? innerException = null)
            : base(message, innerException) { }
    }
}