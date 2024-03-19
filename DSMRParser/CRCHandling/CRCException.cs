using System;

namespace DSMRParser.CRCHandling;

/// <summary>
/// The exception thrown when a calculated CRC doesn't match an expected (or 'claimed') CRC.
/// </summary>
/// <remarks>
/// Initializes a new instance of a <see cref="CRCException"/> with the given message, claimed and
/// calculated CRC and a reference to the inner exception that is the cause of this exception.
/// </remarks>
/// <param name="message">The exception message.</param>
/// <param name="calculated">The calculated CRC.</param>
/// <param name="claimed">The expected, or 'claimed', CRC.</param>
/// <param name="innerException">
/// The exception that is the cause of the current exception, or a null reference if no inner exception is specified.
/// </param>
public class CRCException(string? message, int calculated, int claimed, Exception? innerException = null) : DSMRParserException(message ?? $"CRC mismatch; expected '{calculated:X4}', found '{claimed:X4}'", innerException)
{
    /// <summary>
    /// Gets the calculated CRC.
    /// </summary>
    public int Calculated { get; private set; } = calculated;
    /// <summary>
    /// Gets the claimed CRC.
    /// </summary>
    public int Claimed { get; private set; } = claimed;

    /// <summary>
    /// Initializes a new instance of a <see cref="CRCException"/>.
    /// </summary>
    public CRCException()
        : this("CRC error") { }

    /// <summary>
    /// Initializes a new instance of a <see cref="CRCException"/> with the given message
    /// </summary>
    /// <param name="message">The exception message.</param>
    public CRCException(string? message)
        : this(message, null) { }

    /// <summary>
    /// Initializes a new instance of a <see cref="CRCException"/> with the given message and a reference to the inner
    /// exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="innerException">
    /// The exception that is the cause of the current exception, or a null reference if no inner exception is specified.
    /// </param>
    public CRCException(string? message, Exception? innerException)
        : this(message, 0, 0, innerException) { }

    /// <summary>
    /// Initializes a new instance of a <see cref="CRCException"/> with the given claimed and calculated CRC.
    /// </summary>
    /// <param name="calculated">The calculated CRC.</param>
    /// <param name="claimed">The expected, or 'claimed', CRC.</param>
    public CRCException(int calculated, int claimed)
        : this(null, calculated, claimed) { }

    /// <summary>
    /// Initializes a new instance of a <see cref="CRCException"/> with the given message, claimed and
    /// calculated CRC.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="calculated">The calculated CRC.</param>
    /// <param name="claimed">The expected, or 'claimed', CRC.</param>
    public CRCException(string? message, int calculated, int claimed)
        : this(message, calculated, claimed, null) { }
}