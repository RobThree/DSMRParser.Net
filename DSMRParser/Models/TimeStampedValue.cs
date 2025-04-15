using System;
using System.Globalization;

namespace DSMRParser.Models;

/// <summary>
/// Represents a value at a given point in time.
/// </summary>
/// <typeparam name="T">The type of the value.</typeparam>
public record TimeStampedValue<T>
{
    /// <summary>
    /// Gets the time at wich the value was determined/measured.
    /// </summary>
    public DateTimeOffset? DateTime { get; init; }
    /// <summary>
    /// Gets the value.
    /// </summary>
    public T? Value { get; init; }

    /// <summary>
    /// Initializes a new instance of a <see cref="TimeStampedValue{T}"/>.
    /// </summary>
    /// <param name="dateTimeOffset">The date/time at which the value was determined/measured.</param>
    /// <param name="value">The value.</param>
    public TimeStampedValue(DateTimeOffset? dateTimeOffset, T? value)
    {
        DateTime = dateTimeOffset;
        Value = value;
    }

    /// <summary>
    /// Converts a <see cref="TimeStampedValue{T}"/> to a string.
    /// </summary>
    /// <returns>Returns a string representing a <see cref="TimeStampedValue{T}"/>.</returns>
    public override string ToString()
        => ToString(null, null, null);

    /// <summary>
    /// Converts a <see cref="TimeStampedValue{T}"/> to a string.
    /// </summary>
    /// <param name="valueFormat">The format to use when formatting the value -or- a null
    /// reference to use the default format defined for the type of the <see cref="IFormattable"/>
    /// implementation.</param>
    /// <param name="formatProvider">The provider to use to format the value.</param>
    /// <returns>Returns a string representing a <see cref="TimeStampedValue{T}"/>.</returns>
    public string ToString(string? valueFormat, IFormatProvider? formatProvider)
        => ToString(valueFormat, null, formatProvider);

    /// <summary>
    /// Converts a <see cref="TimeStampedValue{T}"/> to a string.
    /// </summary>
    /// <param name="formatProvider">The provider to use to format the value.</param>
    /// <returns>Returns a string representing a <see cref="TimeStampedValue{T}"/>.</returns>
    public string ToString(IFormatProvider? formatProvider)
        => ToString(null, null, formatProvider);

    /// <summary>
    /// Converts a <see cref="TimeStampedValue{T}"/> to a string.
    /// </summary>
    /// <param name="valueFormat">The format to use when formatting the value -or- a null
    /// reference to use the default format defined for the type of the <see cref="IFormattable"/>
    /// implementation.</param>
    /// <param name="dateFormat">The format to use when formatting the datetime -or- a null
    /// reference to use the default format defined for the type of the <see cref="IFormattable"/>
    /// implementation.</param>
    /// <param name="formatProvider">The provider to use to format the value.</param>
    /// <returns>Returns a string representing a <see cref="TimeStampedValue{T}"/>.</returns>
    public string ToString(string? valueFormat, string? dateFormat, IFormatProvider? formatProvider)
    {
        var date = GetFormattedDate(dateFormat, formatProvider);
        var value = GetFormattedValue(valueFormat, formatProvider);

        return !string.IsNullOrEmpty(date) && !string.IsNullOrEmpty(value)
            ? $"{date}: {value}"
            : !string.IsNullOrEmpty(date)
            ? date
            : !string.IsNullOrEmpty(value)
            ? value : string.Empty;
    }

    private string GetFormattedDate(string? dateFormat, IFormatProvider? formatProvider)
        => DateTime switch
        {
            null => string.Empty,
            IFormattable formattable => formattable.ToString(dateFormat ?? "O", formatProvider ?? CultureInfo.InvariantCulture)
        };

    private string GetFormattedValue(string? valueFormat, IFormatProvider? formatProvider)
        => Value switch
        {
            null => string.Empty,
            IUnitValue unitValue => unitValue.ToString(valueFormat, formatProvider),
            IFormattable formattable => formattable.ToString(valueFormat, formatProvider ?? CultureInfo.InvariantCulture),
            _ => Value.ToString() ?? string.Empty
        };
}