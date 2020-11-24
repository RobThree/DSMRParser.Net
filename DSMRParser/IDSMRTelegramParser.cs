using DSMRParser.Models;
using System;
using System.Diagnostics.CodeAnalysis;

namespace DSMRParser
{
    public interface IDSMRTelegramParser
    {
        Telegram Parse(Span<byte> data);
        Telegram Parse(string data);
        bool TryParse(Span<byte> data, [NotNullWhen(true)] out Telegram? result);
        bool TryParse(string data, [NotNullWhen(true)] out Telegram? result);
    }
}