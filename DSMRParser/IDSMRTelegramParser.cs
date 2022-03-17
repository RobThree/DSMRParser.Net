using DSMRParser.Models;
using System;

namespace DSMRParser;

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
}