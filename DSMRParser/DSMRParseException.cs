using System;

namespace DSMRParser;

/// <summary>
/// The exception thrown when handling DSMR telegrams fails.
/// </summary>
public abstract class DSMRParserException : Exception
{
    /// <summary>
    /// Initializes a new instance of a <see cref="DSMRParserException"/>.
    /// </summary>
    public DSMRParserException()
        : this("DSMR Parse error") { }

    /// <summary>
    /// Initializes a new instance of a <see cref="DSMRParserException"/> with a given message.
    /// </summary>
    /// <param name="message">The exception message.</param>
    public DSMRParserException(string? message)
        : this(message, null) { }

    /// <summary>
    /// Initializes a new instance of a <see cref="DSMRParserException"/> with a given message and
    /// a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="innerException">
    /// The exception that is the cause of the current exception, or a null reference if no inner exception is specified.
    /// </param>
    public DSMRParserException(string? message, Exception? innerException = null)
        : base(message, innerException) { }
}