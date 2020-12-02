using DSMRParser.Models;
using System;
using System.Diagnostics.CodeAnalysis;

namespace DSMRParser
{
    /// <summary>
    /// Represents a DSMR <see cref="Telegram"/> parser.
    /// </summary>
    public interface IDSMRTelegramParser
    {
        /// <summary>
        /// Parses a DSMR telegram in raw byte form into a <see cref="Telegram"/>.
        /// </summary>
        /// <param name="telegram">The DSMR telegram in raw byte form.</param>
        /// <returns>A <see cref="Telegram"/> that represents the given <paramref name="telegram"/>.</returns>
        Telegram Parse(Span<byte> telegram);

        /// <summary>
        /// Parses a DSMR telegram in ASCII string form into a <see cref="Telegram"/>.
        /// </summary>
        /// <param name="telegram">The DSMR telegram in ASCII string form.</param>
        /// <returns>A <see cref="Telegram"/> that represents the given <paramref name="telegram"/>.</returns>
        Telegram Parse(string telegram);

        /// <summary>
        /// Attempts to parse a DSMR telegram in raw byte form into a <see cref="Telegram"/>.
        /// </summary>
        /// <param name="telegram">The DSMR telegram in raw byte form.</param>
        /// <param name="result">
        /// A <see cref="Telegram"/> that represents the given <paramref name="telegram"/>. If the method returns true,
        /// <paramref name="result"/> contains a valid <see cref="Telegram"/> or null when the method returns false.
        /// </param>
        /// <returns>True if the parse operation was successful; otherwise, false.</returns>
        bool TryParse(Span<byte> telegram, [NotNullWhen(true)] out Telegram? result);

        /// <summary>
        /// Attempts to parse a DSMR telegram in ASCII string form into a <see cref="Telegram"/>.
        /// </summary>
        /// <param name="telegram">The DSMR telegram in ASCII string form.</param>
        /// <param name="result">
        /// A <see cref="Telegram"/> that represents the given <paramref name="telegram"/>. If the method returns true,
        /// <paramref name="result"/> contains a valid <see cref="Telegram"/> or null when the method returns false.
        /// </param>
        /// <returns>True if the parse operation was successful; otherwise, false.</returns>
        bool TryParse(string telegram, [NotNullWhen(true)] out Telegram? result);
    }
}