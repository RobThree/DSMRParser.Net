namespace DSMRParser.Models;

/// <summary>
/// Extension methods for <see cref="OBISUnit"/>.
/// </summary>
public static class OBISUnitExtensions
{
    /// <summary>
    /// Converts the <see cref="OBISUnit"/> to a string.
    /// </summary>
    /// <param name="unit">The <see cref="OBISUnit"/> to convert.</param>
    /// <returns>Returns a string representing the <see cref="OBISUnit"/>.</returns>
    public static string ToUnitString(this OBISUnit unit)
        => unit switch
        {
            OBISUnit.NONE => string.Empty,
            OBISUnit.m3 => "m³",
            OBISUnit.dm3 => "dm³",
            _ => unit.ToString()
        };
}