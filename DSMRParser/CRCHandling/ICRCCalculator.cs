using System;

namespace DSMRParser.CRCHandling
{
    /// <summary>
    /// Provides the base interface for CRC calculation
    /// </summary>
    public interface ICRCCalculator
    {
        /// <summary>
        /// Extracts the CRC of the given Telegram data and verifies it against the calculated CRC.
        /// </summary>
        /// <param name="rawTelegram">The Telegram in raw byte form.</param>
        /// <returns>Returns a <see cref="CRCException"/> when the CRC doesn't match, null otherwise.</returns>
        Exception? Verify(Span<byte> rawTelegram);

        /// <summary>
        /// Calculates the CRC of the given Telegram data.
        /// </summary>
        /// <param name="rawTelegram">The Telegram in raw byte form.</param>
        /// <returns>Returns the CRC of the given Telegram.</returns>
        ushort CalculateCRC(Span<byte> rawTelegram);

        /// <summary>
        /// Returns the default CRC calculator.
        /// </summary>
        static ICRCCalculator Default => new CRC16Calculator();
    }
}