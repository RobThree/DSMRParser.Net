namespace DSMRParser.Models;

/// <summary>
/// Represents a value in a given unit.
/// </summary>
/// <typeparam name="T">The type of the value.</typeparam>
public record UnitValue<T>
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
    /// Converts a <see cref="TimeStampedValue{T}"/> to a string.
    /// </summary>
    /// <returns>Returns a string representing a <see cref="TimeStampedValue{T}"/>.</returns>
    public override string ToString() => $"{Value}{Unit}";
}