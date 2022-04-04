using System;

namespace DSMRParser.CRCHandling;

/// <summary>
/// Provides the base interface for CRC calculation
/// </summary>
public interface ICRCVerifier
{
    /// <summary>
    /// Extracts the CRC of the given Telegram data and verifies it against the calculated CRC.
    /// </summary>
    /// <param name="rawTelegram">The Telegram in raw byte form.</param>
    /// <param name="exception">
    /// When this method returns, contains the <see cref="CRCException"/> containing information on why the CRC failed,
    /// if the CRC failed, or null if the CRC is correct. This parameter is passed uninitialized; any value originally
    /// supplied in result will be overwritten.
    /// </param>
    /// <returns>true if value was converted successfully, false otherwise.</returns>
    bool TryVerify(Span<byte> rawTelegram, out CRCException? exception);

    /// <summary>
    /// Calculates the CRC of the given Telegram data.
    /// </summary>
    /// <param name="rawTelegram">The Telegram in raw byte form.</param>
    /// <returns>Returns the CRC of the given Telegram.</returns>
    ushort CalculateCRC(Span<byte> rawTelegram);

    /// <summary>
    /// Returns the default CRC verifier.
    /// </summary>
    static ICRCVerifier Default => new CRC16Verifier();
}