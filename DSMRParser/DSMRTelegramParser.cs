using DSMRParser.CRCHandling;
using DSMRParser.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DSMRParser;

/// <summary>
/// Initializes a new instance of a <see cref="DSMRTelegramParser"/> with a given <see cref="ICRCVerifier"/>.
/// </summary>
/// <remarks>
/// More information can be found <see href="http://riii.me/p1-5_0_2">here</see> (<see href="hhttp://riii.me/p1-5_0_2-backup">archived</see>)
/// or <see href="https://www.netbeheernederland.nl/veiligheid-en-infrastructuur/slimme-meter">here</see> (in Dutch).
/// </remarks>
/// <remarks>
/// The <see cref="DSMRTelegramParser"/> can be used to parse DSM Telegrams in raw byte form or in string form
/// into <see cref="Telegram"/> objects
/// </remarks>
/// <param name="crcVerifier">The <see cref="ICRCVerifier"/> to use for verifying CRC's.</param>
/// <param name="fixMangledTelegrams">Wether to (try to) fix 'mangled' telegrams (telegrams that don't adhere to the spec)</param>
/// <param name="timeZone">Timezone to use when parsing date/time data for a <see cref="Telegram"/>. When null, "W. Europe Standard Time" is used.</param>
/// <param name="encoding">Encoding to use when parsing telegrams. Default is ASCII.</param>
/// <exception cref="ArgumentNullException">Thrown when the <paramref name="crcVerifier"/> is null.</exception>
public class DSMRTelegramParser(ICRCVerifier crcVerifier, bool fixMangledTelegrams = DSMRTelegramParser._defaultfixmangled, TimeZoneInfo? timeZone = null, Encoding? encoding = null) : IDSMRTelegramParser
{
    private readonly ICRCVerifier _crc = crcVerifier ?? throw new ArgumentNullException(nameof(crcVerifier));
    private readonly TimeZoneInfo _timezone = timeZone ?? TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");
    private readonly bool _fixmangled = fixMangledTelegrams;
    private const bool _defaultignorecrc = false;
    private const bool _defaultfixmangled = false;
    private const string _lineseparator = "\r\n";
    private readonly Encoding _encoding = encoding ?? Encoding.ASCII; // Encoding used for parsing telegrams. ASCII is the default encoding for DSMR telegrams.

    /// <summary>
    /// Initializes a new instance of a <see cref="DSMRTelegramParser"/> with a default <see cref="ICRCVerifier"/>.
    /// </summary>
    /// <param name="fixMangledTelegrams">Wether to (try to) fix 'mangled' telegrams (telegrams that don't adhere to the spec)</param>
    /// <param name="timeZone">Timezone to use when parsing date/time data for a <see cref="Telegram"/>. When null, "W. Europe Standard Time" is used.</param>
    /// <param name="encoding">Encoding to use when parsing telegrams. Default is ASCII.</param>
    public DSMRTelegramParser(bool fixMangledTelegrams = _defaultfixmangled, TimeZoneInfo? timeZone = null, Encoding? encoding = null)
        : this(ICRCVerifier.Default, fixMangledTelegrams, timeZone, encoding) { }

    /// <summary>
    /// Parses a DSMR telegram in raw byte form into a <see cref="Telegram"/>.
    /// </summary>
    /// <param name="telegram">The DSMR telegram in raw byte form.</param>
    /// <returns>A <see cref="Telegram"/> that represents the given <paramref name="telegram"/>.</returns>
    /// <exception cref="TelegramFormatException">Thrown when the given <paramref name="telegram"/> is in an invalid format.</exception>
    /// <exception cref="NullReferenceException">Thrown when the parsed <see cref="Telegram"/> resulted in a null value.</exception>
    public Telegram Parse(Span<byte> telegram)
        => Parse(telegram, _defaultignorecrc);

    /// <summary>
    /// Parses a DSMR telegram in raw byte form into a <see cref="Telegram"/>.
    /// </summary>
    /// <param name="telegram">The DSMR telegram in raw byte form.</param>
    /// <param name="ignoreCrc">Ignore CRC errors</param>
    /// <returns>A <see cref="Telegram"/> that represents the given <paramref name="telegram"/>.</returns>
    /// <exception cref="TelegramFormatException">Thrown when the given <paramref name="telegram"/> is in an invalid format.</exception>
    /// <exception cref="NullReferenceException">Thrown when the parsed <see cref="Telegram"/> resulted in a null value.</exception>
    public Telegram Parse(Span<byte> telegram, bool ignoreCrc = _defaultignorecrc)
    {
        var ex = TryParseCore(telegram, ignoreCrc, out var result);
        return ex is not null ? throw ex : result ?? throw new NullReferenceException($"{nameof(TryParseCore)} returned null");
    }

    /// <summary>
    /// Parses a DSMR telegram in string form into a <see cref="Telegram"/>.
    /// </summary>
    /// <param name="telegram">The DSMR telegram in string form.</param>
    /// <returns>A <see cref="Telegram"/> that represents the given <paramref name="telegram"/>.</returns>
    /// <exception cref="TelegramFormatException">Thrown when the given <paramref name="telegram"/> is in an invalid format.</exception>
    /// <exception cref="NullReferenceException">Thrown when the parsed <see cref="Telegram"/> resulted in a null value.</exception>
    public Telegram Parse(string telegram)
        => Parse(_encoding.GetBytes(telegram), _defaultignorecrc);

    /// <summary>
    /// Parses a DSMR telegram in string form into a <see cref="Telegram"/>.
    /// </summary>
    /// <param name="telegram">The DSMR telegram in string form.</param>
    /// <param name="ignoreCrc">Ignore CRC errors</param>
    /// <returns>A <see cref="Telegram"/> that represents the given <paramref name="telegram"/>.</returns>
    /// <exception cref="TelegramFormatException">Thrown when the given <paramref name="telegram"/> is in an invalid format.</exception>
    /// <exception cref="NullReferenceException">Thrown when the parsed <see cref="Telegram"/> resulted in a null value.</exception>
    public Telegram Parse(string telegram, bool ignoreCrc = _defaultignorecrc)
        => Parse(_encoding.GetBytes(telegram), ignoreCrc);

    /// <summary>
    /// Attempts to parse a DSMR telegram in raw byte form into a <see cref="Telegram"/>.
    /// </summary>
    /// <param name="telegram">The DSMR telegram in raw byte form.</param>
    /// <param name="result">
    /// A <see cref="Telegram"/> that represents the given <paramref name="telegram"/>. If the method returns true,
    /// <paramref name="result"/> contains a valid <see cref="Telegram"/> or null when the method returns false.
    /// </param>
    /// <returns>True if the parse operation was successful; otherwise, false.</returns>
    public bool TryParse(Span<byte> telegram, [NotNullWhen(true)] out Telegram? result)
        => TryParseCore(telegram, _defaultignorecrc, out result) is null;

    /// <summary>
    /// Attempts to parse a DSMR telegram in raw byte form into a <see cref="Telegram"/>.
    /// </summary>
    /// <param name="telegram">The DSMR telegram in raw byte form.</param>
    /// <param name="ignoreCrc">Ignore CRC errors</param>
    /// <param name="result">
    /// A <see cref="Telegram"/> that represents the given <paramref name="telegram"/>. If the method returns true,
    /// <paramref name="result"/> contains a valid <see cref="Telegram"/> or null when the method returns false.
    /// </param>
    /// <returns>True if the parse operation was successful; otherwise, false.</returns>
    public bool TryParse(Span<byte> telegram, bool ignoreCrc, [NotNullWhen(true)] out Telegram? result)
        => TryParseCore(telegram, ignoreCrc, out result) is null;

    /// <summary>
    /// Attempts to parse a DSMR telegram in string form into a <see cref="Telegram"/>.
    /// </summary>
    /// <param name="telegram">The DSMR telegram in string form.</param>
    /// <param name="result">
    /// A <see cref="Telegram"/> that represents the given <paramref name="telegram"/>. If the method returns true,
    /// <paramref name="result"/> contains a valid <see cref="Telegram"/> or null when the method returns false.
    /// </param>
    /// <returns>True if the parse operation was successful; otherwise, false.</returns>
    public bool TryParse(string telegram, [NotNullWhen(true)] out Telegram? result)
        => TryParse(_encoding.GetBytes(telegram), _defaultignorecrc, out result);

    /// <summary>
    /// Attempts to parse a DSMR telegram in string form into a <see cref="Telegram"/>.
    /// </summary>
    /// <param name="telegram">The DSMR telegram in string form.</param>
    /// <param name="ignoreCrc">Ignore CRC errors</param>
    /// <param name="result">
    /// A <see cref="Telegram"/> that represents the given <paramref name="telegram"/>. If the method returns true,
    /// <paramref name="result"/> contains a valid <see cref="Telegram"/> or null when the method returns false.
    /// </param>
    /// <returns>True if the parse operation was successful; otherwise, false.</returns>
    public bool TryParse(string telegram, bool ignoreCrc, [NotNullWhen(true)] out Telegram? result)
        => TryParse(_encoding.GetBytes(telegram), ignoreCrc, out result);

    /// <summary>
    /// Attempts to parse a DSMR telegram in raw byte form into a <see cref="Telegram"/>.
    /// </summary>
    /// <param name="telegram">The DSMR telegram in raw byte form.</param>
    /// <param name="ignoreCrc">Wether to ignore the CRC (if any)./</param>
    /// <param name="result">
    /// A <see cref="Telegram"/> that represents the given <paramref name="telegram"/>. If the method returns null,
    /// <paramref name="result"/> contains a valid <see cref="Telegram"/> or null when the method returns an exception.
    /// </param>
    /// <returns>An exception indicating a reason for the failure to parse the telegram or null when successful.</returns>
    private Exception? TryParseCore(Span<byte> telegram, bool ignoreCrc, [NotNullWhen(true)] out Telegram? result)
    {
        // Initialize result
        result = null;

        // Make sure data starts with identification ("/") and that the start of the CRC is at least 4 bytes before the end
        if (telegram.Length > 0 && telegram[0] == (byte)'/')
        {
            // Get individual lines
            var lines = _encoding.GetString(telegram).Split(_lineseparator, StringSplitOptions.RemoveEmptyEntries);

            // Do we have a CRC, then check it unless ignored specifically
            if (!ignoreCrc && lines[^1][0] == '!' && lines[^1].Length > 1)
            {
                if (!_crc.TryVerify(telegram, out var crcex))  // CRC failed?
                {
                    return crcex;   // Return exception from CRCVerifier
                }
            }

            // Check to see if we need to 'fix' a 'mangled' telegram
            if (_fixmangled && lines.Any(l => l.StartsWith('(')))
            {
                lines = FixLines(lines);
            }

            result = new Telegram(
                lines[0].TrimStart('/'),    // Get identification (part after the "/" of the first line)
                lines.Skip(1)               // Skip identification (mandatory empty line has already been removed by Split method)
                    .Where(l => !string.IsNullOrEmpty(l) && char.IsDigit(l[0])) // Only process lines starting with a digit
                    .Select(l => (          // Parse values from telegram data
                        OBISId.FromString(l[..Math.Max(0, l.IndexOf('(', StringComparison.Ordinal))]), GetValues(l)
                    )),
                _timezone
            );

            return null;    // No exceptions, all went well!
        }
        // If we reache this point it must be an invalid format.
        return new TelegramFormatException();
    }

    /// <summary>
    /// Extracts values like "(a)(b)(c)" and returns these as a string array.
    /// </summary>
    private static IEnumerable<string?> GetValues(string value)
        => value[value.IndexOf('(', StringComparison.Ordinal)..].TrimStart('(').TrimEnd(')').Split(")(");

    private static readonly Regex _fixgasvalue = new(@"(0-1:24.3.0)\((\d+)\)(.*?)\n\(([\d\.]+)\)", RegexOptions.CultureInvariant | RegexOptions.Compiled);
    private static string[] FixLines(string[] lines)
    {
        var tmp = string.Join(_lineseparator, lines);
        tmp = _fixgasvalue.Replace(tmp, "$1($2)($4*m3)");
        return tmp.Split(_lineseparator);
    }
}