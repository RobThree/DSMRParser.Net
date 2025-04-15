using System;

namespace DSMRParser.Models;

internal interface IUnitValue
{
    string ToString(string? format, IFormatProvider? provider);
}