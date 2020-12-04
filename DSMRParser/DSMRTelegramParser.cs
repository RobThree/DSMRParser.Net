using DSMRParser.CRCHandling;
using DSMRParser.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace DSMRParser
{
    /// <summary>
    /// The <see cref="DSMRTelegramParser"/> can be used to parse DSM Telegrams in raw byte form or in ASCII string form
    /// into <see cref="Telegram"/> objects
    /// </summary>
    /// <remarks>
    /// More information can be found here: https://www.netbeheernederland.nl/_upload/Files/Slimme_meter_15_a727fce1f1.pdf
    /// </remarks>
    public class DSMRTelegramParser : IDSMRTelegramParser
    {
        private readonly ICRCCalculator _crc;
        private const bool DEFAULTIGNORECRC = false;

        /// <summary>
        /// Initializes a new instance of a <see cref="DSMRTelegramParser"/> with a default <see cref="ICRCCalculator"/>.
        /// </summary>
        public DSMRTelegramParser()
            : this(ICRCCalculator.Default) { }

        /// <summary>
        /// Initializes a new instance of a <see cref="DSMRTelegramParser"/> with a given <see cref="ICRCCalculator"/>.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>Thrown when the <param name="crcCalculator"/> is null.
        public DSMRTelegramParser(ICRCCalculator crcCalculator) => _crc = crcCalculator ?? throw new ArgumentNullException(nameof(crcCalculator));

        /// <summary>
        /// Parses a DSMR telegram in raw byte form into a <see cref="Telegram"/>.
        /// </summary>
        /// <param name="telegram">The DSMR telegram in raw byte form.</param>
        /// <returns>A <see cref="Telegram"/> that represents the given <paramref name="telegram"/>.</returns>
        /// <exception cref="TelegramFormatException">Thrown when the given <paramref name="telegram"/> is in an invalid format.</exception>
        /// <exception cref="NullReferenceException">Thrown when the parsed <see cref="Telegram"/> resulted in a null value.</exception>
        public Telegram Parse(Span<byte> telegram) => Parse(telegram, DEFAULTIGNORECRC);

        /// <summary>
        /// Parses a DSMR telegram in raw byte form into a <see cref="Telegram"/>.
        /// </summary>
        /// <param name="telegram">The DSMR telegram in raw byte form.</param>
        /// <param name="ignoreCrc">Ignore CRC errors</param>
        /// <returns>A <see cref="Telegram"/> that represents the given <paramref name="telegram"/>.</returns>
        /// <exception cref="TelegramFormatException">Thrown when the given <paramref name="telegram"/> is in an invalid format.</exception>
        /// <exception cref="NullReferenceException">Thrown when the parsed <see cref="Telegram"/> resulted in a null value.</exception>
        public Telegram Parse(Span<byte> telegram, bool ignoreCrc = DEFAULTIGNORECRC)
        {
            var ex = TryParseCore(telegram, ignoreCrc, out var result);
            if (ex != null)
                throw ex;
            return result ?? throw new NullReferenceException($"{nameof(TryParseCore)} returned null");
        }

        /// <summary>
        /// Parses a DSMR telegram in ASCII string form into a <see cref="Telegram"/>.
        /// </summary>
        /// <param name="telegram">The DSMR telegram in ASCII string form.</param>
        /// <returns>A <see cref="Telegram"/> that represents the given <paramref name="telegram"/>.</returns>
        /// <exception cref="TelegramFormatException">Thrown when the given <paramref name="telegram"/> is in an invalid format.</exception>
        /// <exception cref="NullReferenceException">Thrown when the parsed <see cref="Telegram"/> resulted in a null value.</exception>
        public Telegram Parse(string telegram) => Parse(Encoding.ASCII.GetBytes(telegram), DEFAULTIGNORECRC);

        /// <summary>
        /// Parses a DSMR telegram in ASCII string form into a <see cref="Telegram"/>.
        /// </summary>
        /// <param name="telegram">The DSMR telegram in ASCII string form.</param>
        /// <param name="ignoreCrc">Ignore CRC errors</param>
        /// <returns>A <see cref="Telegram"/> that represents the given <paramref name="telegram"/>.</returns>
        /// <exception cref="TelegramFormatException">Thrown when the given <paramref name="telegram"/> is in an invalid format.</exception>
        /// <exception cref="NullReferenceException">Thrown when the parsed <see cref="Telegram"/> resulted in a null value.</exception>
        public Telegram Parse(string telegram, bool ignoreCrc = DEFAULTIGNORECRC) => Parse(Encoding.ASCII.GetBytes(telegram), ignoreCrc);

        /// <summary>
        /// Attempts to parse a DSMR telegram in raw byte form into a <see cref="Telegram"/>.
        /// </summary>
        /// <param name="telegram">The DSMR telegram in raw byte form.</param>
        /// <param name="result">
        /// A <see cref="Telegram"/> that represents the given <paramref name="telegram"/>. If the method returns true,
        /// <paramref name="result"/> contains a valid <see cref="Telegram"/> or null when the method returns false.
        /// </param>
        /// <returns>True if the parse operation was successful; otherwise, false.</returns>
        public bool TryParse(Span<byte> telegram, [NotNullWhen(true)] out Telegram? result) => TryParseCore(telegram, DEFAULTIGNORECRC, out result) == null;

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
        public bool TryParse(Span<byte> telegram, bool ignoreCrc, [NotNullWhen(true)] out Telegram? result) => TryParseCore(telegram, ignoreCrc, out result) == null;

        /// <summary>
        /// Attempts to parse a DSMR telegram in ASCII string form into a <see cref="Telegram"/>.
        /// </summary>
        /// <param name="telegram">The DSMR telegram in ASCII string form.</param>
        /// <param name="result">
        /// A <see cref="Telegram"/> that represents the given <paramref name="telegram"/>. If the method returns true,
        /// <paramref name="result"/> contains a valid <see cref="Telegram"/> or null when the method returns false.
        /// </param>
        /// <returns>True if the parse operation was successful; otherwise, false.</returns>
        public bool TryParse(string telegram, [NotNullWhen(true)] out Telegram? result) => TryParse(Encoding.ASCII.GetBytes(telegram), DEFAULTIGNORECRC, out result);

        /// <summary>
        /// Attempts to parse a DSMR telegram in ASCII string form into a <see cref="Telegram"/>.
        /// </summary>
        /// <param name="telegram">The DSMR telegram in ASCII string form.</param>
        /// <param name="ignoreCrc">Ignore CRC errors</param>
        /// <param name="result">
        /// A <see cref="Telegram"/> that represents the given <paramref name="telegram"/>. If the method returns true,
        /// <paramref name="result"/> contains a valid <see cref="Telegram"/> or null when the method returns false.
        /// </param>
        /// <returns>True if the parse operation was successful; otherwise, false.</returns>
        public bool TryParse(string telegram, bool ignoreCrc, [NotNullWhen(true)] out Telegram? result) => TryParse(Encoding.ASCII.GetBytes(telegram), ignoreCrc, out result);

        /// <summary>
        /// Attempts to parse a DSMR telegram in raw byte form into a <see cref="Telegram"/>.
        /// </summary>
        /// <param name="telegram">The DSMR telegram in raw byte form.</param>
        /// <param name="result">
        /// A <see cref="Telegram"/> that represents the given <paramref name="telegram"/>. If the method returns null,
        /// <paramref name="result"/> contains a valid <see cref="Telegram"/> or null when the method returns an exception.
        /// </param>
        /// <returns>An exception indicating a reason for the failure to parse the telegram or null when successful.</returns>
        protected virtual Exception? TryParseCore(Span<byte> telegram, bool ignoreCrc, [NotNullWhen(true)] out Telegram? result)
        {
            // Initialize result
            result = null;

            // Make sure data starts with identification ("/") and that the start of the CRC is at least 4 bytes before the end
            if (telegram[0] == (byte)'/')
            {
                // Get individual lines
                var lines = Encoding.ASCII.GetString(telegram).Split("\r\n");

                result = new Telegram(
                                lines[0].TrimStart('/'),    // Get identification (part after the "/" of the first line)
                                lines.Skip(2)               // Skip identification and mandatory empty line
                                    .Where(l => !string.IsNullOrEmpty(l) && char.IsDigit(l[0])) // Only process lines starting with a digit
                                    .Select(l => (          // Parse values from telegram data
                                        OBISId.FromString(l.Substring(0, Math.Max(0, l.IndexOf("(", StringComparison.Ordinal)))), GetValues(l)
                                    ))
                );

                if (!ignoreCrc && result.DSMRVersion >= 4)
                {
                    var crcex = _crc.Verify(telegram);
                    if (crcex != null)  // CRC failed?
                        return crcex;   // Return exception froom CRCCalculator
                }

                return null;    // No exceptions, all went well!
            }
            // If we reache this point it must be an invalid format.
            return new TelegramFormatException();
        }

        /// <summary>
        /// Extracts values like "(a)(b)(c)" and returns these as a string array.
        /// </summary>
        private static IEnumerable<string?> GetValues(string value) => value[value.IndexOf("(", StringComparison.Ordinal)..].TrimStart('(').TrimEnd(')').Split(")(");
    }
}