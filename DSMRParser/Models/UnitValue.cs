using System;
using System.Globalization;

namespace DSMRParser.Models;

/// <summary>
/// Represents a value in a given unit.
/// </summary>
/// <typeparam name="T">The type of the value.</typeparam>
public record UnitValue<T> : IUnitValue
{
    /// <summary>
    /// Gets the value.
    /// </summary>
    public T? Value { get; init; }
    /// <summary>
    /// Gets the <see cref="OBISUnit"/> of the value.
    /// </summary>
    public OBISUnit Unit { get; init; }

    /// <summary>
    /// Initializes a new instance of a <see cref="UnitValue{T}"/>.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="unit">The <see cref="OBISUnit"/> of the value.</param>
    public UnitValue(T? value, OBISUnit unit)
    {
        Value = value;
        Unit = unit;
    }

    /// <summary>
    /// Converts a <see cref="UnitValue{T}"/> to a string.
    /// </summary>
    /// <returns>Returns a string representing a <see cref="UnitValue{T}"/>.</returns>
    public override string ToString()
        => ToString(null, null);

    /// <summary>
    /// Converts a <see cref="UnitValue{T}"/> to a string.
    /// </summary>
    /// <param name="format">The format to use -or- a null reference to use the default format
    /// defined for the type of the <see cref="IFormattable"/> implementation.</param>
    /// <returns>Returns a string representing a <see cref="UnitValue{T}"/>.</returns>
    public string ToString(string? format)
        => ToString(format, null);

    /// <summary>
    /// Converts a <see cref="UnitValue{T}"/> to a string.
    /// </summary>
    /// <param name="formatProvider">The provider to use to format the value.</param>
    /// <returns>Returns a string representing a <see cref="UnitValue{T}"/>.</returns>
    public string ToString(IFormatProvider formatProvider)
        => ToString(null, formatProvider);

    /// <summary>
    /// Converts a <see cref="UnitValue{T}"/> to a string.
    /// </summary>
    /// <param name="format">The format to use -or- a null reference to use the default format
    /// defined for the type of the <see cref="IFormattable"/> implementation.</param>
    /// <param name="formatProvider">The provider to use to format the value.</param>
    /// <returns>Returns a string representing a <see cref="UnitValue{T}"/>.</returns>
    public string ToString(string? format, IFormatProvider? formatProvider)
        => Value switch
        {
            null => string.Empty,
            IFormattable formattable => formattable.ToString(format, formatProvider ?? CultureInfo.InvariantCulture),
            _ => Value.ToString() ?? string.Empty
        } + Unit.ToUnitString();
}
