using System;

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
    public override string ToString() => $"{DateTime:O}: {Value}";
}