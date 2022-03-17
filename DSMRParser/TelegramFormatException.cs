using DSMRParser.Models;
using System;

namespace DSMRParser;

/// <summary>
/// The exception thrown when parsing a DSMR <see cref="Telegram"/> is formatted incorrectly.
/// </summary>
public class TelegramFormatException : Exception
{
    /// <summary>
    /// Initializes a new instance of a <see cref="TelegramFormatException"/>.
    /// </summary>
    public TelegramFormatException()
        : this("Invalid format") { }

    /// <summary>
    /// Initializes a new instance of an <see cref="TelegramFormatException"/> with a given message.
    /// </summary>
    /// <param name="message">The exception message.</param>
    public TelegramFormatException(string message)
        : base(message) { }


    /// <summary>
    /// Initializes a new instance of an <see cref="TelegramFormatException"/> with a given message and
    /// a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="innerException">
    /// The exception that is the cause of the current exception, or a null reference if no inner exception is specified.
    /// </param>
    public TelegramFormatException(string message, Exception innerException)
        : base(message, innerException) { }
}