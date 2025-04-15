using DSMRParser.CRCHandling;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;

namespace DSMRParser.Models;

// Note: this is kind of a huge file but most of it is docblocks / documentation. There's mostly a big bulk of properties and some helper parsing methods in here.

/// <summary>
/// Represents a DSMR telegram.
/// </summary>
[DebuggerDisplay("{Identification,nq}@{TimeStamp,nq}")]
public sealed class Telegram
{
    /// The culture used for parsing values (affecting parsing of values like "1.234,56" vs "1,234.56".
    private static readonly CultureInfo _culture = CultureInfo.InvariantCulture;
    private readonly TimeZoneInfo _timezone;


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
    /// <param name="timeZoneInfo">The <see cref="TimeZoneInfo"/> to use when parsing timestamps.</param>
    internal Telegram(string? identification, IEnumerable<(OBISId obisid, IEnumerable<string?> values)> values, TimeZoneInfo timeZoneInfo)
    {
        Identification = identification ?? throw new ArgumentNullException(nameof(identification));
        Values = values.ToDictionary(i => i.obisid, i => i.values);
        _timezone = timeZoneInfo;
    }

    #region Properties
    /// <summary>Gets the identification of the DSMR meter the telegram originated from.</summary>
    public string? Identification { get; private set; }
    /// <summary>Gets the version of the DSMR telegram.</summary>
    public int? DSMRVersion => ParseInt(OBISRegistry.DSMRVersion);
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
    /// <summary>Tariff indicator electricity.</summary>
    public int? ElectricityTariff => ParseInt(OBISRegistry.ElectricityTariff);
    /// <summary>Actual electricity power delivered.</summary>
    public UnitValue<decimal>? PowerDelivered => ParseDecimalUnit(OBISRegistry.PowerDelivered);
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
    public IEnumerable<TimeStampedValue<TimeSpan>> ElectricityFailureLog
        => ParseTimeStampedValues(OBISRegistry.ElectricityFailureLog, (d, v) => TimeSpan.FromSeconds(ParseLongUnit(d, v)?.Value ?? 0), 2);
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
    public UnitValue<int>? CurrentL1 => ParseIntUnit(OBISRegistry.CurrentL1);
    /// <summary>Instantaneous current L2.</summary>
    public UnitValue<int>? CurrentL2 => ParseIntUnit(OBISRegistry.CurrentL2);
    /// <summary>Instantaneous current L3.</summary>
    public UnitValue<int>? CurrentL3 => ParseIntUnit(OBISRegistry.CurrentL3);
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
    public string? GasEquipmentId => DecodeString(GetByDescriptor(OBISRegistry.GasEquipmentId));
    /// <summary>Gas valve position.</summary>
    public int? GasValvePosition => ParseInt(OBISRegistry.GasValvePosition);
    /// <summary>Gas delivered.</summary>
    public TimeStampedValue<UnitValue<decimal>>? GasDelivered =>
        ParseTimeStampedValues(OBISRegistry.GasDelivered, ParseDecimalUnit).FirstOrDefault();

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
    public IReadOnlyDictionary<OBISId, IEnumerable<string?>> Values { get; private set; }
    #endregion

    /// <summary>
    /// Gets a single value described by the given descriptor.
    /// </summary>
    /// <param name="descriptor">The <see cref="OBISDescriptor"/> describing the value to find in the telegram.</param>
    /// <returns>The value in 'raw string form' if found in the telegram or null otherwise.</returns>
    public string? GetByDescriptor(OBISDescriptor descriptor)
        => GetMultiByDescriptor(descriptor)?.FirstOrDefault();

    /// <summary>
    /// Gets all values described by the given descriptor.
    /// </summary>
    /// <param name="descriptor">The <see cref="OBISDescriptor"/> describing the values to find in the telegram.</param>
    /// <returns>The values in 'raw string form' if found in the telegram or an empty enumerable otherwise.</returns>
    public IEnumerable<string?> GetMultiByDescriptor(OBISDescriptor descriptor)
        => descriptor is null
        ? throw new ArgumentNullException(nameof(descriptor))
        : Values.TryGetValue(descriptor.Id, out var value) ? value : [];

    /// <summary>
    /// Gets a single value described by the given OBIS ID.
    /// </summary>
    /// <param name="obisId">The <see cref="OBISId"/> to find in the telegram.</param>
    /// <returns>The value in 'raw string form' if found in the telegram or null otherwise.</returns>
    public string? GetByObisID(OBISId obisId)
        => GetByDescriptor(obisId);

    /// <summary>
    /// Gets all values described by the given OBIS ID.
    /// </summary>
    /// <param name="obisId">The <see cref="OBISId"/> to find in the telegram.</param>
    /// <returns>The values in 'raw string form' if found in the telegram or an empty enumerable otherwise.</returns>
    public IEnumerable<string?> GetMultiByObisID(OBISId obisId)
        => GetMultiByDescriptor(obisId);

    private bool TryParseDateTimeOffsetCore(string? value, out DateTimeOffset result)
    {
        if (DateTime.TryParseExact(value?.TrimEnd('W', 'S'), "yyMMddHHmmss", _culture, DateTimeStyles.None, out var dt))
        {
            result = new DateTimeOffset(dt, _timezone.GetUtcOffset(dt));
            return true;
        }
        result = default;
        return false;
    }

    private static bool TryParseHexByteCore(string? value, out byte result)
        => byte.TryParse(value, NumberStyles.HexNumber, _culture, out result);

    private static bool TryParseIntCore(string? value, out int result)
        => int.TryParse(value, NumberStyles.Integer, _culture, out result);

    private static bool TryParseLongCore(string? value, out long result)
        => long.TryParse(value, NumberStyles.Integer, _culture, out result);

    private static bool TryParseDecimalCore(string? value, out decimal result)
        => decimal.TryParse(value, NumberStyles.AllowDecimalPoint, _culture, out result);

    private static bool TryParseEnumCore<T>(string? value, out T result)
        where T : struct => Enum.TryParse<T>(value, out result);

    private DateTimeOffset? ParseTimeStamp(string? value)
        => TryParseDateTimeOffsetCore(value, out var result) ? result : null;

    private DateTimeOffset? ParseTimeStamp(OBISDescriptor descriptor)
        => ParseTimeStamp(GetByDescriptor(descriptor));

    private UnitValue<decimal>? ParseDecimalUnit(OBISDescriptor descriptor)
        => ParseDecimalUnit(descriptor, GetByDescriptor(descriptor));

    private static UnitValue<decimal>? ParseDecimalUnit(OBISDescriptor descriptor, string? obisValue)
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

    private int? ParseInt(OBISDescriptor descriptor)
        => descriptor is null
        ? throw new ArgumentNullException(nameof(descriptor))
        : !TryParseIntCore(GetByDescriptor(descriptor), out var result) ? null : (int)(result * descriptor.Factor);

    private UnitValue<int>? ParseIntUnit(OBISDescriptor descriptor)
        => ParseIntUnit(descriptor, GetByDescriptor(descriptor));

    private static UnitValue<int>? ParseIntUnit(OBISDescriptor descriptor, string? obisValue)
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

    private static UnitValue<long>? ParseLongUnit(OBISDescriptor descriptor, string? obisValue)
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

    private static bool IsCorrectUnit(OBISUnit expected, string? actual)
        => TryParseEnumCore<OBISUnit>(actual, out var result) && expected == result;

    private IEnumerable<TimeStampedValue<T>> ParseTimeStampedValues<T>(OBISDescriptor descriptor, Func<OBISDescriptor, string?, T?> valueSelector, int skip = 0)
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

    private static (string? value, string? unit) SplitValues(string? value, char unitSeparator = '*')
    {
        if (string.IsNullOrEmpty(value))
        {
            return (null, null);
        }

        var d = value.LastIndexOf(unitSeparator);
        return d >= 0 ? ((string? value, string? unit))(value[0..d], value[(d + 1)..]) : ((string? value, string? unit))(value, null);
    }

    private static string? DecodeString(string? value)
        => (!string.IsNullOrEmpty(value) && value.Length % 2 == 0) ? Encoding.ASCII.GetString([.. DecodeHexString(value)]) : value;

    private static IEnumerable<byte> DecodeHexString(string? value)
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