using System;

namespace DSMRParser.Models
{
    /// <summary>
    /// The exception thrown when parsing an <see cref="OBISId"/> fails.
    /// </summary>
    public class InvalidOBISIdException : DSMRParserException
    {
        /// <summary>
        /// Initializes a new instance of an <see cref="InvalidOBISIdException"/>.
        /// </summary>
        public InvalidOBISIdException()
            : this("Invalid OBIS ID") { }

        /// <summary>
        /// Initializes a new instance of an <see cref="InvalidOBISIdException"/> with a given message.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public InvalidOBISIdException(string? message)
            : this(message, null) { }

        /// <summary>
        /// Initializes a new instance of an <see cref="InvalidOBISIdException"/> with a given message and
        /// a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception, or a null reference if no inner exception is specified.
        /// </param>
        public InvalidOBISIdException(string? message, Exception? innerException = null)
            : base(message, innerException) { }
    }
}