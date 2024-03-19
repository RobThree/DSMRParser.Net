using DSMRParser.CRCHandling;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;

namespace DSMRParser.Models;

// Note: this is kind of a huge file but most of it is docblocks / documentation. There's mostly a big bulk of properties and some helper parsing methods in here.

/// <summary>
/// Intializes a new instance of a <see cref="Telegram"/> with the given identification and values representing
/// the telegram.
/// </summary>
/// <remarks>
/// This class uses a form of 'lazy parsing' meaning that values are only determined upon actual request; when a property
/// is read the value is resolved in an internal list of values and the value is returned.
/// </remarks>
/// <param name="identification">The identification of the meter the telegram originated from.</param>
/// <param name="values">The (OBIS) values reported by the meter.</param>
[DebuggerDisplay("{Identification,nq}@{TimeStamp,nq}")]
public class Telegram(string? identification, IEnumerable<(OBISId obisid, IEnumerable<string?> values)> values)
{
    /// <summary>The culture used for parsing values (affecting parsing of values like "1.234,56" vs "1,234.56".</summary>
    private static readonly CultureInfo _culture = CultureInfo.InvariantCulture;
    private static readonly TimeZoneInfo _dutchtimezone = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");

    /// <summary>An empty dictionary of OBIS values.</summary>
    protected static readonly IReadOnlyDictionary<OBISId, IEnumerable<string?>> EMPTY = new ReadOnlyDictionary<OBISId, IEnumerable<string?>>(Array.Empty<OBISDescriptor>().ToDictionary(i => i.Id, i => Enumerable.Empty<string?>()));

    #region Properties
    /// <summary>Gets the identification of the DSMR meter the telegram originated from.</summary>
    public string? Identification { get; init; } = identification ?? throw new ArgumentNullException(nameof(identification));
    /// <summary>Gets the version of the DSMR telegram.</summary>
    public int? DSMRVersion => ParseInt(OBISRegistry.DSMRVersion) ?? ParseInt(BelgianOBISRegistry.DSMRVersion);
    /// <summary>Gets the timestamp of the DSMR telegram.</summary>
    public DateTimeOffset? TimeStamp => ParseTimeStamp(OBISRegistry.TimeStamp);
    /// <summary>Equipment identifier.</summary>
    public string? EquipmentId => DecodeString(GetByDescriptor(OBISRegistry.EquipmentId));
    /// <summary>Meter Reading electricity delivered to client (Tariff 1).</summary>
    public UnitValue<decimal>? EnergyDeliveredTariff1 => ParseDecimalUnit(OBISRegistry.EnergyDeliveredTariff1);
    /// <summary>Meter Reading electricity delivered to client (Tariff 2).</summary>
    public UnitValue<decimal>? EnergyDeliveredTariff2 => ParseDecimalUnit(OBISRegistry.EnergyDeliveredTariff2);
    /// <summary>Electricity returned (Tariff 1).</summary>
    public UnitValue<decimal>? EnergyReturnedTariff1 => ParseDecimalUnit(OBISRegistry.EnergyReturnedTariff1);
    /// <summary>Electricity returned (Tariff 2).</summary>
    public UnitValue<decimal>? EnergyReturnedTariff2 => ParseDecimalUnit(OBISRegistry.EnergyReturnedTariff2);
    /// <summary>Instantaneous energy delivered max running month.</summary>
    public TimeStampedValue<UnitValue<decimal>>? EnergyDeliveredMaxRunningMonth => ParseTimeStampedValues(BelgianOBISRegistry.PowerDeliveredMaxRunningMonth, ParseDecimalUnit).FirstOrDefault();
    /// <summary>Tariff indicator electricity.</summary>
    public int? ElectricityTariff => ParseInt(OBISRegistry.ElectricityTariff);
    /// <summary>Actual electricity power delivered.</summary>
    public UnitValue<decimal>? PowerDelivered => ParseDecimalUnit(OBISRegistry.PowerDelivered);
    /// <summary>Actual electricity power delivered average.</summary>
    public UnitValue<decimal>? PowerDeliveredCurrentAvg => ParseDecimalUnit(BelgianOBISRegistry.PowerMaxCurrentAverage);
    /// <summary>Actual electricity power returned.</summary>
    public UnitValue<decimal>? PowerReturned => ParseDecimalUnit(OBISRegistry.PowerReturned);
    /// <summary>The actual threshold Electricity.</summary>
    public UnitValue<decimal>? ElectricityThreshold => ParseDecimalUnit(OBISRegistry.ElectricityThreshold);
    /// <summary>Switch position Electricity.</summary>
    public int? ElectricitySwitchPosition => ParseInt(OBISRegistry.ElectricitySwitchPosition);
    /// <summary>Number of power failures in any phase.</summary>
    public int? ElectricityFailures => ParseInt(OBISRegistry.ElectricityFailures);
    /// <summary>Number of long power failures in any phase.</summary>
    public int? ElectricityLongFailures => ParseInt(OBISRegistry.ElectricityLongFailures);
    /// <summary>Power Failure Event Log.</summary>
    public IEnumerable<TimeStampedValue<TimeSpan>> ElectricityFailureLog => ParseTimeStampedValues(OBISRegistry.ElectricityFailureLog, (d, v) => TimeSpan.FromSeconds(ParseLongUnit(d, v)?.Value ?? 0), 2);
    /// <summary>Number of voltage sags in phase L1.</summary>
    public int? ElectricitySagsL1 => ParseInt(OBISRegistry.ElectricitySagsL1);
    /// <summary>Number of voltage sags in phase L2.</summary>
    public int? ElectricitySagsL2 => ParseInt(OBISRegistry.ElectricitySagsL2);
    /// <summary>Number of voltage sags in phase L3.</summary>
    public int? ElectricitySagsL3 => ParseInt(OBISRegistry.ElectricitySagsL3);
    /// <summary>Number of voltage swells in phase L1.</summary>
    public int? ElectricitySwellsL1 => ParseInt(OBISRegistry.ElectricitySwellsL1);
    /// <summary>Number of voltage swells in phase L2.</summary>
    public int? ElectricitySwellsL2 => ParseInt(OBISRegistry.ElectricitySwellsL2);
    /// <summary>Number of voltage swells in phase L3.</summary>
    public int? ElectricitySwellsL3 => ParseInt(OBISRegistry.ElectricitySwellsL3);
    /// <summary>Short text message.</summary>
    public string? MessageShort => DecodeString(GetByDescriptor(OBISRegistry.MessageShort));
    /// <summary>Long text message.</summary>
    public string? MessageLong => DecodeString(GetByDescriptor(OBISRegistry.MessageLong));
    /// <summary>Instantaneous voltage L1.</summary>
    public UnitValue<decimal>? VoltageL1 => ParseDecimalUnit(OBISRegistry.VoltageL1);
    /// <summary>Instantaneous voltage L2.</summary>
    public UnitValue<decimal>? VoltageL2 => ParseDecimalUnit(OBISRegistry.VoltageL2);
    /// <summary>Instantaneous voltage L3.</summary>
    public UnitValue<decimal>? VoltageL3 => ParseDecimalUnit(OBISRegistry.VoltageL3);
    /// <summary>Instantaneous current L1.</summary>
    public UnitValue<decimal>? CurrentL1 => ParseDecimalUnit(OBISRegistry.CurrentL1);
    /// <summary>Instantaneous current L2.</summary>
    public UnitValue<decimal>? CurrentL2 => ParseDecimalUnit(OBISRegistry.CurrentL2);
    /// <summary>Instantaneous current L3.</summary>
    public UnitValue<decimal>? CurrentL3 => ParseDecimalUnit(OBISRegistry.CurrentL3);
    /// <summary>Instantaneous active power L1.</summary>
    public UnitValue<decimal>? PowerDeliveredL1 => ParseDecimalUnit(OBISRegistry.PowerDeliveredL1);
    /// <summary>Instantaneous active power L2.</summary>
    public UnitValue<decimal>? PowerDeliveredL2 => ParseDecimalUnit(OBISRegistry.PowerDeliveredL2);
    /// <summary>Instantaneous active power L3.</summary>
    public UnitValue<decimal>? PowerDeliveredL3 => ParseDecimalUnit(OBISRegistry.PowerDeliveredL3);
    /// <summary>Instantaneous active power returned L1.</summary>
    public UnitValue<decimal>? PowerReturnedL1 => ParseDecimalUnit(OBISRegistry.PowerReturnedL1);
    /// <summary>Instantaneous active power returned L2.</summary>
    public UnitValue<decimal>? PowerReturnedL2 => ParseDecimalUnit(OBISRegistry.PowerReturnedL2);
    /// <summary>Instantaneous active power returned L3.</summary>
    public UnitValue<decimal>? PowerReturnedL3 => ParseDecimalUnit(OBISRegistry.PowerReturnedL3);
    /// <summary>Gas devicetype.</summary>
    public int? GasDeviceType => ParseInt(OBISRegistry.GasDeviceType);
    /// <summary>Gas equipment identifier.</summary>
    public string? GasEquipmentId => DecodeString(GetByDescriptor(OBISRegistry.GasEquipmentId)) ?? DecodeString(GetByDescriptor(BelgianOBISRegistry.GasEquipmentId));
    /// <summary>Gas valve position.</summary>
    public int? GasValvePosition => ParseInt(OBISRegistry.GasValvePosition);

    /// <summary>Gas delivered.</summary>
    public TimeStampedValue<UnitValue<decimal>>? GasDelivered =>
        ParseTimeStampedValues(OBISRegistry.GasDelivered, ParseDecimalUnit).FirstOrDefault() ??
        ParseTimeStampedValues(BelgianOBISRegistry.GasDelivered, ParseDecimalUnit).FirstOrDefault();

    /// <summary>Gas delivered - OLD (pre-V4).</summary>
    public TimeStampedValue<UnitValue<decimal>>? GasDeliveredOld =>
        ParseTimeStampedValues(OBISRegistry.GasDeliveredOld, ParseDecimalUnit).FirstOrDefault();

    /// <summary>Thermal devicetype.</summary>
    public int? ThermalDeviceType => ParseInt(OBISRegistry.ThermalDeviceType);
    /// <summary>Thermal equipment identifier.</summary>
    public string? ThermalEquipmentId => DecodeString(GetByDescriptor(OBISRegistry.ThermalEquipmentId));
    /// <summary>Thermal valve position.</summary>
    public int? ThermalValvePosition => ParseInt(OBISRegistry.ThermalValvePosition);
    /// <summary>Thermal energy delivered.</summary>
    public TimeStampedValue<UnitValue<decimal>>? ThermalDelivered =>
        //TODO: Check unit/factor
        ParseTimeStampedValues(OBISRegistry.ThermalDelivered, ParseDecimalUnit).FirstOrDefault();
    /// <summary>Water devicetype.</summary>
    public int? WaterDeviceType => ParseInt(OBISRegistry.WaterDeviceType);
    /// <summary>Water equipment identifier.</summary>
    public string? WaterEquipmentId => DecodeString(GetByDescriptor(OBISRegistry.WaterEquipmentId));
    /// <summary>Water valve position.</summary>
    public int? WaterValvePosition => ParseInt(OBISRegistry.WaterValvePosition);
    /// <summary>Water delivered.</summary>
    public TimeStampedValue<UnitValue<decimal>>? WaterDelivered =>
        //TODO: Check unit/factor
        ParseTimeStampedValues(OBISRegistry.WaterDelivered, ParseDecimalUnit).FirstOrDefault();
    /// <summary>Slave devicetype.</summary>
    public int? SlaveDeviceType => ParseInt(OBISRegistry.SlaveDeviceType);
    /// <summary>Slave equipment identifier.</summary>
    public string? SlaveEquipmentId => DecodeString(GetByDescriptor(OBISRegistry.SlaveEquipmentId));
    /// <summary>Slave valve position.</summary>
    public int? SlaveValvePosition => ParseInt(OBISRegistry.SlaveValvePosition);
    /// <summary>Slave delivered value.</summary>
    public TimeStampedValue<UnitValue<decimal>>? SlaveDelivered =>
        //TODO: Check unit/factor
        ParseTimeStampedValues(OBISRegistry.SlaveDelivered, ParseDecimalUnit).FirstOrDefault();

    /// <summary>Gets all values reported by the DSMR meter.</summary>
    public IReadOnlyDictionary<OBISId, IEnumerable<string?>> Values { get; init; } = new ReadOnlyDictionary<OBISId, IEnumerable<string?>>(values.ToDictionary(i => i.obisid, i => i.values));

    #endregion

    /// <summary>
    /// Gets a single value described by the given descriptor.
    /// </summary>
    /// <param name="descriptor">The <see cref="OBISDescriptor"/> describing the value to find in the telegram.</param>
    /// <returns>The value in 'raw string form' if found in the telegram or null otherwise.</returns>
    public string? GetByDescriptor(OBISDescriptor descriptor) => GetMultiByDescriptor(descriptor)?.FirstOrDefault();

    /// <summary>
    /// Gets all values described by the given descriptor.
    /// </summary>
    /// <param name="descriptor">The <see cref="OBISDescriptor"/> describing the values to find in the telegram.</param>
    /// <returns>The values in 'raw string form' if found in the telegram or an empty enumerable otherwise.</returns>
    public IEnumerable<string?> GetMultiByDescriptor(OBISDescriptor descriptor) => descriptor is null
            ? throw new ArgumentNullException(nameof(descriptor))
            : Values.TryGetValue(descriptor.Id, out var value) ? value : [];

    /// <summary>
    /// Gets a single value described by the given OBIS ID.
    /// </summary>
    /// <param name="obisId">The <see cref="OBISId"/> to find in the telegram.</param>
    /// <returns>The value in 'raw string form' if found in the telegram or null otherwise.</returns>
    public string? GetByObisID(OBISId obisId) => GetByDescriptor(obisId);

    /// <summary>
    /// Gets all values described by the given OBIS ID.
    /// </summary>
    /// <param name="obisId">The <see cref="OBISId"/> to find in the telegram.</param>
    /// <returns>The values in 'raw string form' if found in the telegram or an empty enumerable otherwise.</returns>
    public IEnumerable<string?> GetMultiByObisID(OBISId obisId) => GetMultiByDescriptor(obisId);

    /// <summary>
    /// Attempts to parse a timestamp to a datetimeoffset.
    /// </summary>
    /// <param name="value">The value of the timestamp.</param>
    /// <param name="result">
    /// A <see cref="DateTimeOffset"/> that represents the given <paramref name="value"/>. If the method returns true,
    /// <paramref name="result"/> contains a valid <see cref="DateTimeOffset"/> or null when the method returns false.
    /// </param>
    /// <returns>True if the parse operation was successful; otherwise, false.</returns>
    protected static bool TryParseDateTimeOffsetCore(string? value, out DateTimeOffset result)
    {
        if (DateTime.TryParseExact(value?.TrimEnd('W', 'S'), "yyMMddHHmmss", _culture, DateTimeStyles.None, out var dt))
        {
            result = new DateTimeOffset(dt, _dutchtimezone.GetUtcOffset(dt));
            return true;
        }
        result = default;
        return false;
    }

    /// <summary>
    /// Attempts to parse a byte in hexadecimal format to a byte value.
    /// </summary>
    /// <param name="value">The hexadecimal value.</param>
    /// <param name="result">
    /// A <see cref="byte"/> that represents the given <paramref name="value"/>. If the method returns true,
    /// <paramref name="result"/> contains a valid <see cref="byte"/> or null when the method returns false.
    /// </param>
    /// <returns>True if the parse operation was successful; otherwise, false.</returns>
    protected static bool TryParseHexByteCore(string? value, out byte result) => byte.TryParse(value, NumberStyles.HexNumber, _culture, out result);

    /// <summary>
    /// Attempts to parse an integer value.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="result">
    /// An <see cref="int"/> that represents the given <paramref name="value"/>. If the method returns true,
    /// <paramref name="result"/> contains a valid <see cref="int"/> or null when the method returns false.
    /// </param>
    /// <returns>True if the parse operation was successful; otherwise, false.</returns>
    protected static bool TryParseIntCore(string? value, out int result) => int.TryParse(value, NumberStyles.Integer, _culture, out result);

    /// <summary>
    /// Attempts to parse a long value.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="result">
    /// A <see cref="long"/> that represents the given <paramref name="value"/>. If the method returns true,
    /// <paramref name="result"/> contains a valid <see cref="long"/> or null when the method returns false.
    /// </param>
    /// <returns>True if the parse operation was successful; otherwise, false.</returns>
    protected static bool TryParseLongCore(string? value, out long result) => long.TryParse(value, NumberStyles.Integer, _culture, out result);

    /// <summary>
    /// Attempts to parse a decimal value.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="result">
    /// A <see cref="decimal"/> that represents the given <paramref name="value"/>. If the method returns true,
    /// <paramref name="result"/> contains a valid <see cref="decimal"/> or null when the method returns false.
    /// </param>
    /// <returns>True if the parse operation was successful; otherwise, false.</returns>
    protected static bool TryParseDecimalCore(string? value, out decimal result) => decimal.TryParse(value, NumberStyles.AllowDecimalPoint, _culture, out result);

    /// <summary>
    /// Attempts to parse an enum value.
    /// </summary>
    /// <typeparam name="T">The type of the enum.</typeparam>
    /// <param name="value">The string representation of an enum value.</param>
    /// <param name="result">
    /// The enum value that represents the given <paramref name="value"/>. If the method returns true,
    /// <paramref name="result"/> contains a valid enum value or null when the method returns false.
    /// </param>
    /// <returns>True if the parse operation was successful; otherwise, false.</returns>
    protected static bool TryParseEnumCore<T>(string? value, out T result) where T : struct => Enum.TryParse<T>(value, out result);

    /// <summary>
    /// Parses a timestamp in yyMMddHHmmss to a <see cref="DateTimeOffset"/>.
    /// </summary>
    /// <param name="value">The value to parse.</param>
    /// <returns>Returns a <see cref="DateTimeOffset"/> when parsing the given <paramref name="value"/> succeeded, null otherwise.</returns>
    protected static DateTimeOffset? ParseTimeStamp(string? value) => TryParseDateTimeOffsetCore(value, out var result) ? result : null;
    /// <summary>
    /// Parses a timestamp for a given <see cref="OBISDescriptor"/> to a <see cref="DateTimeOffset"/>.
    /// </summary>
    /// <param name="descriptor">
    /// The <see cref="OBISDescriptor"/> specifying the value to find in the telegram and parse as <see cref="DateTimeOffset"/>
    /// </param>
    /// <returns>Returns a <see cref="DateTimeOffset"/> when parsing the given <paramref name="descriptor"/> succeeded, null otherwise.</returns>
    protected DateTimeOffset? ParseTimeStamp(OBISDescriptor descriptor) => ParseTimeStamp(GetByDescriptor(descriptor));

    /// <summary>
    /// Parses a decimal value with unit from a given <see cref="OBISDescriptor"/>.
    /// </summary>
    /// <param name="descriptor">
    /// The <see cref="OBISDescriptor"/> specifying the value to find in the telegram and parse as <see cref="UnitValue{T}"/>
    /// </param>
    /// <returns>Returns a <see cref="UnitValue{T}"/> when parsing the given <paramref name="descriptor"/> succeeded, null otherwise.</returns>
    protected UnitValue<decimal>? ParseDecimalUnit(OBISDescriptor descriptor) => ParseDecimalUnit(descriptor, GetByDescriptor(descriptor));
    /// <summary>
    /// Attempts to parse a given value with unit from a given <see cref="OBISDescriptor"/>.
    /// </summary>
    /// <param name="descriptor">
    /// The <see cref="OBISDescriptor"/> specifying the value to find in the telegram and parse as <see cref="UnitValue{T}"/>
    /// </param>
    /// <param name="obisValue">The value (including any units) to parse.</param>
    /// <returns>A <see cref="UnitValue{T}"/> representing the value parsed.</returns>
    /// <remarks>This method assumes values are specified in "value*unit" format (i.e. "1.23*kW").</remarks>
    /// <exception cref="ArgumentNullException">Thrown when the given <paramref name="descriptor"/> is null.</exception>
    protected static UnitValue<decimal>? ParseDecimalUnit(OBISDescriptor descriptor, string? obisValue)
    {
        if (descriptor is null)
        {
            throw new ArgumentNullException(nameof(descriptor));
        }

        var (value, unit) = SplitValues(obisValue);
        return TryParseDecimalCore(value, out var parsed) && IsCorrectUnit(descriptor.Unit, unit)
            ? new UnitValue<decimal>(parsed * descriptor.Factor, descriptor.Unit)
            : null;
    }

    /// <summary>
    /// Parses the value from the given <see cref="OBISDescriptor"/> and returns the integer value, corrected by and
    /// optional factor given by the <see cref="OBISDescriptor"/> or null when the value fails to parse or doesn't exist.
    /// </summary>
    /// <param name="descriptor">
    /// The <see cref="OBISDescriptor"/> specifying the value to find in the telegram and parse as <see cref="int"/>
    /// </param>
    /// <returns>
    /// The, corrected by factor where applicable, integer value of the value specified by the <see cref="OBISDescriptor"/> in
    /// <paramref name="descriptor"/> or null when the value fails to parse or doesn't exist.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="descriptor"/> is null.</exception>
    protected int? ParseInt(OBISDescriptor descriptor) => descriptor is null
            ? throw new ArgumentNullException(nameof(descriptor))
            : !TryParseIntCore(GetByDescriptor(descriptor), out var result) ? null : (int)(result * descriptor.Factor);

    /// <summary>
    /// Parses an integer value with unit from a given <see cref="OBISDescriptor"/>.
    /// </summary>
    /// <param name="descriptor">
    /// The <see cref="OBISDescriptor"/> specifying the value to find in the telegram and parse as <see cref="UnitValue{T}"/>
    /// </param>
    /// <returns>Returns a <see cref="UnitValue{T}"/> when parsing the given <paramref name="descriptor"/> succeeded, null otherwise.</returns>
    protected UnitValue<int>? ParseIntUnit(OBISDescriptor descriptor) => ParseIntUnit(descriptor, GetByDescriptor(descriptor));
    /// <summary>
    /// Attempts to parse a given value with unit from a given <see cref="OBISDescriptor"/>.
    /// </summary>
    /// <param name="descriptor">
    /// The <see cref="OBISDescriptor"/> specifying the value to find in the telegram and parse as <see cref="UnitValue{T}"/>
    /// </param>
    /// <param name="obisValue">The value (including any units) to parse.</param>
    /// <returns>A <see cref="UnitValue{T}"/> representing the value parsed.</returns>
    /// <remarks>This method assumes values are specified in "value*unit" format (i.e. "123*A").</remarks>
    /// <exception cref="ArgumentNullException">Thrown when the given <paramref name="descriptor"/> is null.</exception>
    protected static UnitValue<int>? ParseIntUnit(OBISDescriptor descriptor, string? obisValue)
    {
        if (descriptor is null)
        {
            throw new ArgumentNullException(nameof(descriptor));
        }

        var (value, unit) = SplitValues(obisValue);
        return TryParseIntCore(value, out var parsed) && IsCorrectUnit(descriptor.Unit, unit)
            ? new UnitValue<int>((int)(parsed * descriptor.Factor), descriptor.Unit)
            : null;
    }

    /// <summary>
    /// Parses the value from the given <see cref="OBISDescriptor"/> and returns the long value, corrected by and
    /// optional factor given by the <see cref="OBISDescriptor"/> or null when the value fails to parse or doesn't exist.
    /// </summary>
    /// <param name="descriptor">
    /// The <see cref="OBISDescriptor"/> specifying the value to find in the telegram and parse as <see cref="long"/>
    /// </param>
    /// <returns>
    /// The, corrected by factor where applicable, long value of the value specified by the <see cref="OBISDescriptor"/> in
    /// <paramref name="descriptor"/> or null when the value fails to parse or doesn't exist.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="descriptor"/> is null.</exception>
    protected long? ParseLong(OBISDescriptor descriptor) => descriptor is null
            ? throw new ArgumentNullException(nameof(descriptor))
            : !TryParseLongCore(GetByDescriptor(descriptor), out var result) ? null : (long)(result * descriptor.Factor);

    /// <summary>
    /// Parses a long value with unit from a given <see cref="OBISDescriptor"/>.
    /// </summary>
    /// <param name="descriptor">
    /// The <see cref="OBISDescriptor"/> specifying the value to find in the telegram and parse as <see cref="UnitValue{T}"/>
    /// </param>
    /// <returns>Returns a <see cref="UnitValue{T}"/> when parsing the given <paramref name="descriptor"/> succeeded, null otherwise.</returns>
    protected UnitValue<long>? ParseLongUnit(OBISDescriptor descriptor) => ParseLongUnit(descriptor, GetByDescriptor(descriptor));
    /// <summary>
    /// Attempts to parse a given value with unit from a given <see cref="OBISDescriptor"/>.
    /// </summary>
    /// <param name="descriptor">
    /// The <see cref="OBISDescriptor"/> specifying the value to find in the telegram and parse as <see cref="UnitValue{T}"/>
    /// </param>
    /// <param name="obisValue">The value (including any units) to parse.</param>
    /// <returns>A <see cref="UnitValue{T}"/> representing the value parsed.</returns>
    /// <remarks>This method assumes values are specified in "value*unit" format (i.e. "123*A").</remarks>
    /// <exception cref="ArgumentNullException">Thrown when the given <paramref name="descriptor"/> is null.</exception>
    protected static UnitValue<long>? ParseLongUnit(OBISDescriptor descriptor, string? obisValue)
    {
        if (descriptor is null)
        {
            throw new ArgumentNullException(nameof(descriptor));
        }

        var (value, unit) = SplitValues(obisValue);
        return TryParseLongCore(value, out var parsed) && IsCorrectUnit(descriptor.Unit, unit)
            ? new UnitValue<long>((long)(parsed * descriptor.Factor), descriptor.Unit)
            : null;
    }

    /// <summary>
    /// Verifies a given unit in string representation matches the expected <see cref="OBISUnit"/>
    /// </summary>
    /// <param name="expected">The expected <see cref="OBISUnit"/> the value should represent.</param>
    /// <param name="actual">The actual unit the value represents.</param>
    /// <returns>True when the given string matches the given <see cref="OBISUnit"/>, false otherwise.</returns>
    protected static bool IsCorrectUnit(OBISUnit expected, string? actual) => TryParseEnumCore<OBISUnit>(actual, out var result) && expected == result;

    /// <summary>
    /// Parses values in (date)(value)(date)(value)... format to an <see cref="TimeStampedValue{T}"/> enumerable.
    /// </summary>
    /// <typeparam name="T">The type of the values.</typeparam>
    /// <param name="descriptor">
    /// The <see cref="OBISDescriptor"/> specifying the values to find in the telegram and parse as <see cref="TimeStampedValue{T}"/> enumerable.
    /// </param>
    /// <param name="valueSelector">A function that handles the parsing of the individual values.</param>
    /// <param name="skip">Optional number of values to skip (when values are in (xxx)(yyy)(date)(value)(date)(value)... format).</param>
    /// <returns>Returns parsed values as <see cref="TimeStampedValue{T}"/> enumerable.</returns>
    /// <exception cref="ArgumentNullException">Thrown when no <paramref name="descriptor"/> or <paramref name="valueSelector"/> is given.</exception>
    /// <remarks>
    /// The <paramref name="skip"/> argument specifies the number of individual values to skip, not date/value pairs. So when a skip of 3 is given with
    /// a value of "(xxx)(yyy)(zzz)(date)(value)(date)(value)..." this method will skip (xxx), (yyy) and (zzz).
    /// </remarks>
    protected IEnumerable<TimeStampedValue<T>> ParseTimeStampedValues<T>(OBISDescriptor descriptor, Func<OBISDescriptor, string?, T?> valueSelector, int skip = 0)
    {
        if (valueSelector is null)
        {
            throw new ArgumentNullException(nameof(valueSelector));
        }

        var values = GetMultiByDescriptor(descriptor).Skip(skip).ToArray();
        if (values.Length % 2 == 0)  // We should have an even number of values
        {
            for (var i = 0; i < values.Length; i += 2)
            {
                yield return new TimeStampedValue<T>(ParseTimeStamp(values[i]), valueSelector(descriptor, values[i + 1]));
            }
        }
    }

    /// <summary>
    /// Splits a "value*unit" string into it's value and unit pair.
    /// </summary>
    /// <param name="value">The value/unit to split.</param>
    /// <param name="unitSeparator">The character that separates value/unit pairs (defaults to '*').</param>
    /// <returns>Returns the value/unit pair.</returns>
    protected static (string? value, string? unit) SplitValues(string? value, char unitSeparator = '*')
    {
        if (string.IsNullOrEmpty(value))
        {
            return (null, null);
        }

        var d = value.LastIndexOf(unitSeparator);
        return d >= 0 ? ((string? value, string? unit))(value[0..d], value[(d + 1)..]) : ((string? value, string? unit))(value, null);
    }

    /// <summary>
    /// Decodes a string in "hex byte format" to it's string counterpart.
    /// </summary>
    /// <param name="value">The "hex byte format string".</param>
    /// <returns>The string the "hex byte string" represents.</returns>
    protected static string? DecodeString(string? value) => (!string.IsNullOrEmpty(value) && value.Length % 2 == 0) ? Encoding.ASCII.GetString(DecodeHexString(value).ToArray()) : value;
    /// <summary>
    /// Decodes a string in "hex byte format" to it's byte values.
    /// </summary>
    /// <param name="value">The "hex byte format string".</param>
    /// <returns>The byte values from the given string.</returns>
    protected static IEnumerable<byte> DecodeHexString(string? value)
    {
        if (!string.IsNullOrEmpty(value) && value.Length % 2 == 0)
        {
            for (var i = 0; i < value.Length; i += 2)
            {
                if (TryParseHexByteCore(value.Substring(i, 2), out var result))
                {
                    yield return result;
                }
            }
        }
    }

    /// <summary>
    /// Gets the string representation of this <see cref="Telegram"/>.
    /// </summary>
    /// <param name="crcVerifier">
    /// The <see cref="ICRCVerifier"/> to use when determining the CRC for the telegram. Defaults to <see cref="ICRCVerifier.Default"/>.
    /// </param>
    /// <returns>The string representation of this <see cref="Telegram"/>.</returns>
    public string AsString(ICRCVerifier? crcVerifier = null)
    {
        var values = string.Join("\r\n", Values.Select(kv => $"{kv.Key}({string.Join(")(", kv.Value)})"));
        var telegram = $"/{Identification}\r\n\r\n{values}\r\n!";
        var crc = (crcVerifier ?? ICRCVerifier.Default).CalculateCRC(Encoding.ASCII.GetBytes(telegram));
        return $"{telegram}{crc:X4}\r\n";
    }
}